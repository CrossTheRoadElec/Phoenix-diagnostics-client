using System;
using System.Text;
using System.Threading;
using BE = CTRE.Phoenix.Diagnostics.BackEnd.BackEnd;

namespace CTRE.Phoenix.Diagnostics
{
    public class UnitTesting
    {
        private static UnitTesting _instance;
        public static UnitTesting Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UnitTesting();
                return _instance;
            }
        }

        private const uint CHECK_PROCESS_RUNNING = 0x1;
        private const uint CHECK_VERSION_NUMBER = 0x2;
        private const uint CHECK_NUM_DEVICES = 0x4;
        private const uint CHECK_FIRMWARE_FLASH = 0x8;
        private const uint REBOOT_RIO = 0x10;

        private const string VERSION_CHECK = "1.0.0.0 - PreRelease";

        private dotNET.Thread _runningThread;
        private bool _actionFinished = true;
        private Status _actionStatus = Status.Ok;
        private BackEnd.Action _failedAction;
        private uint _tests;
        private StringBuilder _sb = new StringBuilder();
        private object _lock = new object();
        private bool _running = false;

        public UnitTesting()
        {
        }

        public bool Done()
        {
            return !_running;
        }

        public Status Dispose()
        {
            if (_running)
            {
                _running = false;
                bool success = _runningThread.Shutdown(2000);
                if (!success)
                {
                    _runningThread.Abort();
                    return Status.Aborted;
                }
            }
            return Status.Ok;
        }

        public string GetLog()
        {
            string ret = _sb.ToString();
            _sb.Clear();
            return ret;
        }

        public Status StartTesting(uint tests)
        {
            _tests = tests;
            if (_runningThread == null)
            {
                dotNET.Thread t = new dotNET.Thread(TestThread);
                t.Start();
                _runningThread = t;
                return Status.Ok;
            }
            else
            {
                return Status.Busy;
            }
        }

        private void TestThread()
        {
            _actionStatus = Status.Ok;
            _running = true;
            uint testLoops = 0;
            while (_actionStatus == Status.Ok && _running)
            {
                if (_actionStatus == Status.Ok && _running)
                {
                    UpdateRio();
                }

                if ((_tests & CHECK_PROCESS_RUNNING) != CHECK_PROCESS_RUNNING)
                {
                    //Don't execute Check Process
                }
                else if (_actionStatus == Status.Ok && _running)
                {
                    CheckProcess();
                }

                if ((_tests & CHECK_VERSION_NUMBER) != CHECK_VERSION_NUMBER)
                {
                    //Don't execute Check Version Number
                }
                else if (_actionStatus == Status.Ok && _running)
                {
                    CheckVersionNumber();
                }

                if((_tests & CHECK_NUM_DEVICES) != CHECK_NUM_DEVICES)
                {
                    //Don't execute Check Num Devices
                }
                else if(_actionStatus == Status.Ok && _running)
                {
                    CheckNumDevices(1);
                }

                /*------------ This must be at end ------------*/
                if((_tests & REBOOT_RIO) != REBOOT_RIO)
                {
                    //Don't reboot rio
                }
                else if(_actionStatus == Status.Ok && _running)
                {
                    RebootRio();
                }
                ++testLoops;
            }
            if (_actionStatus == Status.Ok)
            {
                Log("Ending test, test is successful.");
                Log("Number of test loops was " + testLoops);
            }
            else if (_failedAction != null)
            {
                Log("Ending test, failure was: " + _failedAction.type.ToString());
                Log("Number of test loops was " + testLoops);
            }
            else
            {
                Log("Ending test, failure is unknown");
                Log("Number of test loops was " + testLoops);
            }

            _running = false;
            _runningThread = null;
        }

        private void UpdateRio()
        {
            lock (_lock)
            {
                _actionFinished = false;
            }

            
            _actionStatus = BE.Instance.UpdateRIO(new BackEnd.Action.CallBack(CallBack));
            Log("------------------------Updating RIO--------------------------");
            WaitUntilRIOFinished();
            Log("Files sent");
            WaitUntilActionFinished();
            Log("Callback Received");
            WaitUntilGoodConnection();
            Log("Regained Comms");

            Log(_actionStatus);
        }

        private void CheckProcess()
        {
            lock (_lock)
            {
                _actionFinished = false;
            }
            _actionStatus = BE.Instance.CheckRIOProcess(new BackEnd.Action.CallBack(CallBack));
            Log("----------------Checking process is running-------------------");
            WaitUntilRIOFinished();
            Log("Command Sent");
            WaitUntilActionFinished();
            Log("Callback Received");
            Log(_actionStatus);
        }

        private void CheckVersionNumber()
        {
            lock (_lock)
            {
                _actionFinished = false;
            }
            _actionStatus = BE.Instance.GetNewConnectionVersion(new BackEnd.Action.CallBack(CallBack), VERSION_CHECK);
            Log("-------------Checking Version String from Server-------------");
            WaitUntilActionFinished();
            Log("Callback Received");
            Log(_actionStatus);
        }

        private void CheckNumDevices(uint numberToExceed)
        {
            lock(_lock)
            {
                _actionFinished = false;
            }
            _actionStatus = BE.Instance.GetNumberOfDevices(new BackEnd.Action.CallBack(CallBack), numberToExceed);
            Log("-----------Checking Number Of Devices exceeds " + numberToExceed + "--------------");
            WaitUntilActionFinished();
            Log("Callback Received");
            Log(_actionStatus);
        }

        private void RebootRio()
        {
            lock (_lock)
            {
                _actionFinished = false;
            }
            _actionStatus = BE.Instance.RebootRio(new BackEnd.Action.CallBack(CallBack));
            Log("-----------------------Rebooting RIO-------------------------");

            DateTime beforeReboot = DateTime.UtcNow;
            /* Wait states include:
             *  Waiting until _rioUpdater is finished
             *  Waiting until callback is called
             *  Waiting until we lost comms
             *  Waiting until we get comms back
             */
            WaitUntilRIOFinished();
            Log("Command Sent");
            WaitUntilActionFinished();
            Log("Callback Received");
            WaitUntilLostComms();
            Log("Lost Comms");
            WaitUntilGoodConnection();
            Log("Regained Comms");

            if (DateTime.UtcNow - beforeReboot > TimeSpan.FromSeconds(80))
                _actionStatus = Status.RebootTookTooLong;
            if (DateTime.UtcNow - beforeReboot < TimeSpan.FromSeconds(20))
                _actionStatus = Status.RebootTooFast;

            Log(_actionStatus);
        }

        private void WaitUntilRIOFinished()
        {
            bool done = false;
            while (!done && _running)
            {
                string updateStatus = BE.Instance.GetRioUpdateStatus(ref done);
                if (updateStatus.Trim() != "")
                    Log(updateStatus);
                Thread.Sleep(100);
            }
        }

        private void WaitUntilActionFinished()
        {
            while(_running)
            {
                lock (_lock)
                {
                    if (_actionFinished)
                    {
                        break;
                    }
                }
                Thread.Sleep(100);
            }
            Log("Action finished");
            Log(_actionStatus);
        }

        private void WaitUntilLostComms()
        {
            string mes, mesCol, hoverMsg;
            while (_running)
            {
                BE.Instance.GetStatus(out mes, out mesCol, out hoverMsg);
                if (mesCol[0] != 'G') //Waiting on connection or lost connection, break out
                    break;
                Thread.Sleep(100);
            }
        }

        private void WaitUntilGoodConnection()
        {
            string mes, mesCol, hoverMsg;
            DateTime timeSinceLastGood = DateTime.UtcNow;
            /* There's a timeout to ensure we aren't flipping betwen good and bad */
            while(_running && (timeSinceLastGood + TimeSpan.FromSeconds(2) > DateTime.UtcNow))
            {
                BE.Instance.GetStatus(out mes, out mesCol, out hoverMsg);
                if (mesCol[0] == 'G')
                    continue;
                timeSinceLastGood = DateTime.UtcNow;
                Thread.Sleep(100);
            }
        }

        private void CallBack(BackEnd.Action action, Status err)
        {
            lock (_lock)
            {
                _actionFinished = true;
            }
            _actionStatus = err;
            if (err != Status.Ok)
                _failedAction = action;
        }

        private void Log(string message)
        {
            _sb.Append(message + "\r\n");
        }
        private void Log(Status err)
        {
            _sb.Append("Status is: " + err.ToString() + "\r\n");
        }
    }
}
