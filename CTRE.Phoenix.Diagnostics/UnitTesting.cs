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

        private const uint SFTP_BINARY_ONTO_RIO = 0x1;
        private const uint CHECK_PROCESS_RUNNING = 0x2;
        private const uint CHECK_VERSION_NUMBER = 0x4;
        private const uint CHECK_NUM_DEVICES = 0x8;
        private const uint CHECK_FIRMWARE_FLASH = 0x10;
        private const uint SAVE_CONFIGS_TO_DEVICE = 0x20;
        private const uint REBOOT_RIO = 0x40;

        private const string VERSION_CHECK = "0.3.0.0 - PreRelease";

        private dotNET.Thread _runningThread;
        private bool _actionFinished = true;
        private Status _actionStatus = Status.Ok;
        private BackEnd.Action _failedAction;
        private StringBuilder _sb = new StringBuilder();
        private object _lock = new object();
        private bool _running = false;

        private uint _tests;
        private DeviceDescrip _device;
        private string _firmwarePath;
        private string _configData;

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

        public Status StartTesting(uint tests, DeviceDescrip deviceToUse, string firmwareFilePath, string paramData)
        {
            _tests = tests;
            _device = deviceToUse;
            _firmwarePath = firmwareFilePath;
            _configData = paramData;
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
                /* Put binaries onto RIO */
                if((_tests & SFTP_BINARY_ONTO_RIO) != SFTP_BINARY_ONTO_RIO)
                {
                    //Don't executre Check Process
                }
                else if(_actionStatus == Status.Ok && _running)
                {
                    UpdateRio();

                }

                /* Check if process is running */
                if ((_tests & CHECK_PROCESS_RUNNING) != CHECK_PROCESS_RUNNING)
                {
                    //Don't execute Check Process
                }
                else if (_actionStatus == Status.Ok && _running)
                {
                    CheckProcess();
                }

                /* Check version is correct */
                if ((_tests & CHECK_VERSION_NUMBER) != CHECK_VERSION_NUMBER)
                {
                    //Don't execute Check Version Number
                }
                else if (_actionStatus == Status.Ok && _running)
                {
                    CheckVersionNumber();
                }

                /* Check minimum number of devices */
                if((_tests & CHECK_NUM_DEVICES) != CHECK_NUM_DEVICES)
                {
                    //Don't execute Check Num Devices
                }
                else if(_actionStatus == Status.Ok && _running)
                {
                    CheckNumDevices(1);
                }

                /* Check Firmware flash succeeds */
                if((_tests & CHECK_FIRMWARE_FLASH) != CHECK_FIRMWARE_FLASH)
                {
                    //Don't execute Check Firmware Flash
                }
                else if(_actionStatus == Status.Ok && _running)
                {
                    CheckFirmwareFlash(_device, _firmwarePath);
                }

                /* Save configs to device */
                if((_tests & SAVE_CONFIGS_TO_DEVICE) != SAVE_CONFIGS_TO_DEVICE)
                {
                    //Don't execute Save Configs To Device
                }
                else if(_actionStatus == Status.Ok && _running)
                {
                    SaveConfigsToDevice(_device, _configData);
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

        private void CheckFirmwareFlash(DeviceDescrip device, string firmwareFilePath)
        {
            lock(_lock)
            {
                _actionFinished = false;
            }
            _actionStatus = BE.Instance.RequestFieldUpgrade(device, firmwareFilePath, new BackEnd.Action.CallBack(CallBack));
            Log("--------------Updating firmware on " + device.jsonStrings.Name + " -------------------");
            WaitUntilActionFinished();
            Log("Callback Received");
            Log(_actionStatus);
        }

        private void SaveConfigsToDevice(DeviceDescrip device, string saveParamData)
        {
            lock(_lock)
            {
                _actionFinished = false;
            }
            _actionStatus = BE.Instance.SaveConfigs(device, saveParamData, new BackEnd.Action.CallBack(CallBack));
            Log("---------------- Saving configs for device " + device.jsonStrings.Name + "------------");
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
