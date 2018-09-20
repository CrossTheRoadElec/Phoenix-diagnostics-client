using CTRE.Phoenix.Diagnostics.HTTP;
using CTRE.Phoenix.Diagnostics.JSON.Serializers;
using CTRE.Phoenix.dotNET;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CTRE.Phoenix.Diagnostics.BackEnd
{
    public partial class BackEnd
    {
        /// <summary>
        /// Back End state (machine).
        /// </summary>
        public enum State
        {
            /* starting up */
            Connecting,
            /* tasks when idle */
            Polling,
            /* individual tasks */
            ExecAction,
            /* error handling */
            LostComm,
            /* shuting down*/
            Disposing,
            Disposed,
        }
        /* -------------------------- private variables to manage/hold info from Phoenix Diagnostic Server ------ */
        private HostNameAndPort _hostName = new HostNameAndPort();
        private DeviceDescriptors _descriptors = new DeviceDescriptors();
        private State _state = State.Connecting;
        private Object _lock = new Object();
        private Thread _thread;
        private string _ConnectionStatus = String.Empty;
        private Action _action;
        private uint _timeSincePollingMs = uint.MaxValue / 2;
        private uint _timeSinceSomeElsesToolTransmit = uint.MaxValue / 2;
        private WebExchange _WebServerScripts = new WebExchange();
        private AsyncWebExchange _asyncWebExchange = null; //!< Only used for field upgrades
        private string _serverVersion = "Unknown";
        private DateTime _lastPoll = DateTime.UtcNow;
        private RioUpdater _rioUpdater;
        private int _refreshRequest = 0;
        private bool _usingPost = false;


        private const string RIO_SEARCH_DIRECTORY = "/usr/local/frc/bin/";

        public DateTime GetLastPoll()
        {
            return _lastPoll;
        }

        public string GetVersionNumbers()
        {
            return _serverVersion;
        }

        public struct FirmwareStatus
        {
            public uint progressPerc;
            public String message;
        }
        public FirmwareStatus _firmwareStatus;

        //---------------------------- Shutdown interface, GUI needs to dispose of the Backend when closing app --------------------//
        public void Dispose()
        {
            /* turn off polling so that state machine doesn't spent too long in the task loop */
            DisableRefreshing();

            /* just in case we are in the middle of field-upgrading */
            if (_asyncWebExchange != null) { _asyncWebExchange.Dispose(); }

            /* start disposing */
            Debug.Print("BackEnd", "Requesting Disposing State");
            bool setSuccess = SetStateWithTimeout(State.Disposing, 1000);
            if (setSuccess)
                Debug.Print("BackEnd", "Entered Disposing State");
            else
                Debug.Print("BackEnd", "Could not enter Disposing State, stuck in: " + GetState_NoLock());

            /* wait for clean up to finish */
            if (setSuccess)
            {
                Debug.Print("BackEnd", "Waiting for Disposed confirmation.");
                for (int i = 0; i < 20; ++i)
                {
                    /* yield */
                    System.Threading.Thread.Sleep(50);
                    /* check if disposal is done */
                    if (GetState_NoLock() == State.Disposed) { break; }
                }
            }

            /* note if the final state transition took place */
            bool stateMachineFinSuccess = (GetState_NoLock() == State.Disposed);
            Debug.Print("BackEnd", stateMachineFinSuccess ? "Disposed confirmation received" : "Disposed confirmation not received");

            /* now shutdown the thread using our signal mecanism*/
            bool shutdownSuccess = _thread.Shutdown(100);
            Debug.Print("BackEnd", shutdownSuccess ? "Thread shutdown complete" : "Thread shutdown timed out.");

            /* if something goes wrong hard kill the thread */
            if (shutdownSuccess == false)
            {
                Debug.Print("BackEnd", "Thread aborted.");
                _thread.Abort();
            }
        }

        //---------------------------- private implem for polling state machine and registering overlapped actions to execute from form. ------------------//
        private Status PushAction(Action action, bool bCheckForIdleState = true)
        {
            //lock (_lock) /* don't appear to need this - and removal prevents lockups*/
            {
                if (_action != null) { return Status.Busy; }

                if (bCheckForIdleState == false)
                {
                    /* caller does not care what the state machine is doing, force this action */
                }
                else
                {
                    /* only accept action request if state machine is ready */
                    if (IsIdle_NoLock() == false) { return Status.Busy; }
                }

                _action = action;   /* save the action, Loop will eventually react */
                return Status.Ok;
            }
        }
        private void AbortAction()
        {
            /* callback to GUI */
            _action.Error = Status.Aborted;
            _action.callback(_action, Status.Aborted);
            /* free it */
            _action = null;
        }
        /// <summary>
        /// Service _action and update _action.Error.
        /// </summary>
        private Status PerformAction(out bool setStateToConnecting)
        {
            setStateToConnecting = false;
            /* get info on this device */
            byte devID = _action.deviceID;
            var model = _action.model;
            /* lookup device descriptor */
            DeviceDescrip ddRef;
            bool foundOk = _descriptors.Get(model, devID, out ddRef);

            /* track the error status */
            Status retval = Status.Ok;
            /* temp for catching JSON responses */
            string response = string.Empty;
            string fileName = string.Empty;

            switch (_action.type)
            {
                case ActionType.Blink:
                    /* make sure device was found in our collection */
                    if (retval == Status.Ok)
                        retval = (foundOk) ? Status.Ok : Status.DeviceNotFound;
                    /* perform http exchange */
                    if (retval == Status.Ok)
                        retval = _WebServerScripts.HttpGet(_hostName, ddRef.model, ddRef.deviceID, ActionType.Blink, out response);
                    /* parse resp */
                    if (retval == Status.Ok)
                    {
                        BlinkReturn jsonDeser = JsonConvert.DeserializeObject<BlinkReturn>(response);
                        retval = (Status)jsonDeser.GeneralReturn.Error;
                    }
                    break;

                case ActionType.SelfTest:
                    /* make sure device was found in our collection */
                    if (retval == Status.Ok)
                        retval = (foundOk) ? Status.Ok : Status.DeviceNotFound;
                    /* perform http exchange */
                    if (retval == Status.Ok)
                        retval = _WebServerScripts.HttpGet(_hostName, ddRef.model, ddRef.deviceID, ActionType.SelfTest, out response, "", 5000);
                    /* parse resp */
                    if (retval == Status.Ok)
                    {
                        SelfTestReturn jsonDeser = JsonConvert.DeserializeObject<SelfTestReturn>(response);
                        retval = (Status)jsonDeser.GeneralReturn.Error;

                        if (retval == Status.Ok)
                        {
                            _action.selfTestResults = jsonDeser.SelfTest;
                        }
                        else
                        {
                            _action.selfTestResults = "Self-Test Failed";
                        }
                    }
                    break;

                case ActionType.SetDeviceName:
                    /* get params for this action */
                    String newName = _action.stringParam;
                    /* make sure device was found in our collection */
                    if (retval == Status.Ok)
                        retval = (foundOk) ? Status.Ok : Status.DeviceNotFound;
                    /* perform http exchange */
                    if (retval == Status.Ok)
                        retval = _WebServerScripts.HttpGet(_hostName, ddRef.model, ddRef.deviceID, ActionType.SetDeviceName, out response, "&newname=" + newName, 2000);
                    /* parse resp */
                    if (retval == Status.Ok)
                    {
                        NameReturn jsonDeser = JsonConvert.DeserializeObject<NameReturn>(response);
                        retval = (Status)jsonDeser.GeneralReturn.Error;
                    }
                    break;

                case ActionType.SetID:
                    /* get params for this action */
                    byte newId = (byte)_action.newID;
                    /* make sure device was found in our collection */
                    if (retval == Status.Ok)
                        retval = (foundOk) ? Status.Ok : Status.DeviceNotFound;
                    /* perform http exchange */
                    if (retval == Status.Ok)
                        retval = _WebServerScripts.HttpGet(_hostName, ddRef.model, ddRef.deviceID, ActionType.SetID, out response, "&newid=" + newId.ToString(), 2000);
                    /* parse resp */
                    if (retval == Status.Ok)
                    {
                        IDReturn jsonDeser = JsonConvert.DeserializeObject<IDReturn>(response);
                        retval = (Status)jsonDeser.GeneralReturn.Error;
                    }
                    /* if ID change was successful, update our local device list */
                    if (retval == Status.Ok)
                        _descriptors.ChangeID(ddRef, newId);
                    break;

                case ActionType.SetConfig:
                    /* Specify the filename so it's somewhat unique */
                    fileName = (ddRef.model + ": " + ddRef.deviceID + ".json").ToLower(); //ToLower Everything to make it parsable in URL

                    /* get params for this action */
                    string serializedData = _action.stringParam;
                    /* Get content payload from action */
                    byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(serializedData);

                    /* If we're using GET we need to send the file up */
                    if (_usingPost == false)
                    {
                        _rioUpdater = new RioUpdater(_hostName);
                        _rioUpdater.SendFileContents(dataBytes, RIO_SEARCH_DIRECTORY + fileName);
                        System.Threading.Thread.Sleep(250); //Wait a bit to make sure file got onto the RIO
                    }

                    /* make sure device was found in our collection */
                    if (retval == Status.Ok)
                        retval = (foundOk) ? Status.Ok : Status.DeviceNotFound;
                    /* perform http exchange */
                    if (retval == Status.Ok)
                    {
                        /* Decide if we're posting or getting from the _usingPost flag */
                        if (_usingPost)
                        {
                            retval = _WebServerScripts.HttpPost(_hostName, ddRef.model, ddRef.deviceID, ActionType.SetConfig, dataBytes, out response, 5000);
                        }
                        else
                        {
                            retval = _WebServerScripts.HttpGet(_hostName, ddRef.model, ddRef.deviceID, ActionType.SetConfig, out response, "&file=" + fileName, 5000);
                        }
                    }
                    /* parse resp */
                    if (retval == Status.Ok)
                    {
                        SetConfigReturn jsonDeser = JsonConvert.DeserializeObject<SetConfigReturn>(response);
                        retval = (Status)jsonDeser.GeneralReturn.Error;
                    }
                    break;

                case ActionType.FieldUpgradeDevice:
                    /* Specify file name as generic so it gets overwritten every time */
                    fileName = "firmwarefile.crf";

                    /* If we're using POST, we don't need to send the file up */
                    if (_usingPost == false)
                    {
                        /* Create a RioFile to be sent to the server */
                        RioFile file = new RioFile(_action.filePath, RIO_SEARCH_DIRECTORY + fileName);
                        /* First put the files onto the RIO */
                        _rioUpdater = new RioUpdater(_hostName);
                        _rioUpdater.SendFile(file);
                    }

                    /* make a new web exchange that allows for overlapped operation */
                    _asyncWebExchange = new AsyncWebExchange();
                    /* make sure device was found in our collection */
                    if (retval == Status.Ok)
                        retval = (foundOk) ? Status.Ok : Status.DeviceNotFound;
                    if (retval == Status.Ok)
                        retval = ExecuteFieldUpgrade(ddRef, _asyncWebExchange, fileName, _usingPost);
                    /* free resouces */
                    _asyncWebExchange.Dispose();
                    break;

                case ActionType.InstallDiagServerToRobotController:
                    _rioUpdater = new RioUpdater(_hostName);
                    _rioUpdater.StartUpdate();
                    retval = Status.Ok;
                    setStateToConnecting = true;
                    break;

                case ActionType.UninstallDiagServerToRobotController:
                    _rioUpdater = new RioUpdater(_hostName);
                    _rioUpdater.StartRevert();
                    retval = Status.Ok;
                    setStateToConnecting = true;
                    break;

                case ActionType.StartServer:
                    _rioUpdater = new RioUpdater(_hostName);
                    retval = _rioUpdater.StartServer();
                    setStateToConnecting = true;
                    break;

                case ActionType.StopServer:
                    _rioUpdater = new RioUpdater(_hostName);
                    retval = _rioUpdater.StopServer();
                    setStateToConnecting = true;
                    break;

                /* Unit Testing Cases */

                case ActionType.GetVersion: //Used for Unit Testing
                    VersionReturn responseClass = null;
                    if (retval == Status.Ok)
                        retval = _WebServerScripts.HttpGet(_hostName, "", 0, ActionType.GetVersion, out response);
                    if (retval == Status.Ok)
                    {
                        responseClass = JsonConvert.DeserializeObject<VersionReturn>(response);
                        retval = responseClass.Version.Equals(_action.stringParam) ? Status.Ok : Status.VersionDoesNotMatch;
                    }
                    break;

                case ActionType.GetNumOfDevices:
                    GetDevicesReturn numDeviceReturn = null;
                    if (retval == Status.Ok)
                        retval = _WebServerScripts.HttpGet(_hostName, "", 0, ActionType.GetDeviceList, out response);
                    if (retval == Status.Ok)
                    {
                        numDeviceReturn = JsonConvert.DeserializeObject<GetDevicesReturn>(response);
                        retval = numDeviceReturn.DeviceArray.Length >= _action.param ? Status.Ok : Status.NotEnoughDevices;
                    }
                    break;

                case ActionType.RebootRio:
                    _rioUpdater = new RioUpdater(_hostName);
                    retval = _rioUpdater.RebootRio();
                    setStateToConnecting = true;
                    break;

                case ActionType.CheckProcess:
                    _rioUpdater = new RioUpdater(_hostName);
                    retval = _rioUpdater.CheckProcessStarted();
                    break;

                default:
                    retval = Status.UnsupportedAction;
                    break;
            }

            /* callback to GUI */
            _action.Error = retval;
            _action.callback(_action, retval);
            /* free it */
            _action = null;

            /* pass error to caller */
            return retval;
        }

        private Status ExecuteApiVersion()
        {
            /* attempt requesting an update */
            string response;
            Status retval = _WebServerScripts.HttpGet(_hostName, "", 0, ActionType.GetVersion, out response, String.Empty, 1000);

            /* attempt parsing */
            VersionReturn general = null;
            if (retval == Status.Ok)
            {
                try
                {
                    general = Newtonsoft.Json.JsonConvert.DeserializeObject<VersionReturn>(response);

                    if (general != null)
                    {
                        /* decoded okay */
                        _serverVersion = general.Version;
                    }
                    else
                    {
                        retval = Status.CouldNotParseJson;
                    }
                }
                catch (Exception)
                {
                    retval = Status.CouldNotParseJson;
                }
            }

            return retval;
        }

        private Status ExecutePollDevices()
        {
            /* attempt requesting an update */
            string response;
            Status retval = _WebServerScripts.HttpGet(_hostName, "", 0, ActionType.GetDeviceList, out response, String.Empty, 2000);

            /* attempt parsing */
            GetDevicesReturn deviceStatus = null;
            if (retval == Status.Ok)
            {
                try
                {
                    deviceStatus = Newtonsoft.Json.JsonConvert.DeserializeObject<GetDevicesReturn>(response);

                    if (deviceStatus != null)
                    {
                        /* decoded okay */
                        retval = (Status)deviceStatus.GeneralReturn.Error;
                    }
                    else
                    {
                        retval = Status.CouldNotParseJson;
                    }
                }
                catch (Exception)
                {
                    retval = Status.CouldNotParseJson;
                }
            }

            /* roll thru decoded json objects */
            if (retval == Status.Ok)
            {
                foreach (JsonDevice jd in deviceStatus.DeviceArray)
                {
                    DeviceDescrip dd = CTRE.Phoenix.Diagnostics.JSON.Adapter.Convert(jd);

                    _descriptors.Insert(dd);
                }
            }
            _lastPoll = DateTime.UtcNow;
            return retval;
        }

        private Status ExecuteGetConfigs(DeviceDescrip dd, out GetConfigsReturn configs)
        {
            /* init the outputs */
            configs = null;
            Status retval = Status.Ok;

            /* all state info to track */
            string response = string.Empty;

            /* do the http exchange */
            if (retval == Status.Ok)
            {
                retval = _WebServerScripts.HttpGet(_hostName, dd.model, dd.deviceID, ActionType.GetConfig, out response);
            }

            /* parse the served response */
            if (retval == Status.Ok)
            {
                configs = JsonConvert.DeserializeObject<GetConfigsReturn>(response);

                if (configs == null) /* what does this mean */
                {
                    retval = Status.GeneralError;
                }
                else if (configs.Device == null) /* what does this mean? */
                {
                    retval = Status.GeneralError;
                }
                else if (configs.Device.Configs == null) /* what does this mean? */
                {
                    retval = Status.GeneralError;
                }
            }

            return retval;
        }

        /// <summary>
        /// Main thread of the BackEnd
        /// </summary>
        private void Loop()
        {
            /* part 1 of lopo time measurement */
            long lastTime = DateTime.Now.Ticks;

            /* check if we need to start shutting down */
            while (_thread.ShuttingDown() == false)
            {
                Status err = Status.Ok;

                /* yield for a bit */
                System.Threading.Thread.Sleep(10);

                /* measure time of each loop */
                long nowTime = DateTime.Now.Ticks;
                TimeSpan elapsedSpan = new TimeSpan(nowTime - lastTime);
                uint loopTimeMs = (uint)elapsedSpan.TotalMilliseconds;
                lastTime = nowTime;

                /* track timeouts and counters */
                _timeSincePollingMs += loopTimeMs;
                _timeSinceSomeElsesToolTransmit += loopTimeMs;

                /* track refresh loops - decrement without underflow */
                if (_refreshRequest > 0) { --_refreshRequest; }

                /* reset state machine */
                if (_hostName.HasChanged())
                {
                    /* move to connecting, and _hostName has internally updated in HasChanged */
                    SetState(State.Connecting);
                }

                /* lock */
                lock (_lock)
                {
                    /* state machine */
                    var state = GetState();
                    switch (state)
                    {
                        case State.LostComm:
                        //fall thru...
                        case State.Connecting:

                            /* abort action if one is occuring */
                            if (_action != null)
                            {
                                if (ActionIsAbortable(_action.type))
                                {
                                    AbortAction();
                                    _action = null;
                                }
                            }

                            /* attempt to get version from server*/
                            err = ExecuteApiVersion();
                            if (err == Status.Ok)
                                SetState(State.Polling);
                            else
                                SetState(State.LostComm);

                            if (_action != null)
                                SetState(State.ExecAction);

                            break;

                        case State.Polling:

                            if (_action != null)
                            {
                                /* process the action */
                                SetState(State.ExecAction);
                            }

                            if (err == Status.Ok)
                            {
                                if (ShouldUpdateDevices() && (_timeSincePollingMs > 500))
                                {
                                    /* reset timeout */
                                    _timeSincePollingMs = 0;

                                    /* attempt a fresh copy of devices */
                                    err = ExecutePollDevices();

                                    if (err == Status.Ok)
                                    {
                                        /* produce a copy of the device list */
                                        var list = _descriptors.ToArray();

                                        /*roll thru the copy of list */
                                        foreach (var dd in list)
                                        {
                                            /* attempt a fresh copy of configs */
                                            GetConfigsReturn configs;
                                            Status innerLoopErr = ExecuteGetConfigs(dd, out configs);

                                            /* if transactoin was successful, update the cache */
                                            if (innerLoopErr == Status.Ok)
                                            {
                                                /* form will get this on next poll */
                                                dd.configCache = configs;
                                            }
                                            else
                                            {
                                                /* leave the last cache alone, just because 
                                                 * the connectoin was lost, don't drop anything */
                                            }

                                            /* let any failure code float up to the outer loop */
                                            if (err != Status.Ok)
                                                err = innerLoopErr;

                                            /* frontend has queued an action, take a break from this inner loop */
                                            if (_action != null) { break; }
                                            /* if polling is disabled, leave immedietely */
                                            if (ShouldUpdateDevices() == false) { break; }
                                        }
                                    }
                                }
                            }
                            /* if any transaction failed in the tool, something is wrong */
                            if (err != Status.Ok)
                            {
                                SetState(State.Connecting);
                            }
                            break;

                        case State.ExecAction:
                            /* run calling applications requested action */
                            bool setStateToConnecting;
                            if (PerformAction(out setStateToConnecting) != Status.Ok)
                            {
                                SetState(State.LostComm);
                            }
                            else
                            {
                                if(setStateToConnecting)
                                {
                                    SetState(State.Connecting);
                                }
                                else
                                {
                                    SetState(State.Polling);
                                }
                            }
                            break;

                        case State.Disposing:
                            /* free any system resources here */
                            SetState(State.Disposed); /* go to an end state and wait for thread shutdown */
                            break;
                        case State.Disposed:
                            /* wait for thread shutdown */
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private bool ActionIsAbortable(ActionType action)
        {
            return !(action == ActionType.InstallDiagServerToRobotController || action == ActionType.CheckProcess);
        }

        //------------------------------- Routines for getting status of BackEnd, these routines must not block -------------------------------------//
        public String GetConnectionStatus()
        {
            /* return the last rendered tool status from our backEnd Thread */
            return _ConnectionStatus;
        }
        public State GetStatus(out String message, out String messageColor, out String hoverMsg)
        {
            State retval = GetState_NoLock();

            message = "";
            hoverMsg = "";

            /* set color based on state, so we can override it below */
            switch (retval)
            {
                case BackEnd.State.Connecting:
                    messageColor = "ORANGE";
                    break;
                case BackEnd.State.LostComm:
                    messageColor = "RED";
                    break;
                default:
                    messageColor = "GREEN";
                    break;
            }

            switch (retval)
            {
                case State.Connecting: message = "Connecting to " + _hostName; break;

                case State.Polling:
                    if (ShouldUpdateDevices() == false)
                    {
                        message = "Idle (Auto Refresh Disabled)";
                    }
                    else
                    {
                        message = "Updating " + _hostName + " CTRE Devices...";
                    }
                    break;

                case State.ExecAction:
                    message = "Performing Action...";
                    break;

                case State.LostComm: message = "Lost Comm, trying to reconnect to " + _hostName; break;
                case State.Disposing: message = "Cleaning up..."; break;
                case State.Disposed: message = "Disconnected."; break;

            }

            return retval;
        }
        public uint FieldUpgradeProgressPerc
        {
            get { return _firmwareStatus.progressPerc; }
        }
        public string FieldUpgradeStatus
        {
            get { return _firmwareStatus.message; }
        }
        private void SetFieldUpgradeStatus(string message, uint perc)
        {
            _firmwareStatus.message = message;
            _firmwareStatus.progressPerc = perc;
            System.Threading.Thread.Sleep(100);
        }
        private void SetFieldUpgradeStatus(ProgressReturn jsonResp)
        {
            Status err = (Status)jsonResp.GeneralReturn.Error;
            uint prog = (uint)(jsonResp.progress);

            _firmwareStatus.progressPerc = prog;
            if (err == Status.Ok)
                _firmwareStatus.message = "Updating firmware... " + "Percent : " + prog;
            else
                _firmwareStatus.message = "Updating firmware... Eror:" + err;
        }
        public string GetRioUpdateStatus(ref bool done)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            done = false;
            if (_rioUpdater == null)
            {
                return "";
            }

            int error = 0;
            done = _rioUpdater.Poll(ref sb, ref error);
            if (done)
            {
                _rioUpdater = null;
            }
            return sb.ToString();
        }
        public bool NewIdConflicts(DeviceDescrip dd, uint newID)
        {
            foreach (DeviceDescrip dev in GetDeviceDescriptors())
            {
                if (dev == dd) continue; //We're looking at our own device
                if (dev.model == dd.model)
                {
                    if (dev.deviceID == newID)
                        return true;
                }
            }
            return false;
        }
        //------------------- Private state accessors with various locking patterns -----------------//
        private bool IsIdle_NoLock()
        {
            switch (GetState_NoLock())
            {
                case State.Polling:
                    return true;
            }
            return false;
        }
        private void SetState(State newState)
        {
            lock (_lock)
            {
                /* save new state */
                _state = newState;

                /* rearm timeSince so we update immedietely */
                _timeSincePollingMs = uint.MaxValue / 2;

                /* telemetry */
                Debug.Print("BackEnd", newState.ToString());
            }
        }
        private State GetState()
        {
            State retval;
            lock (_lock)
            {
                retval = _state;
            }
            return retval;
        }
        private State GetState_NoLock()
        {
            /* anything that requires polling state, but not changing it does not need a lock */
            State retval;
            retval = _state;
            return retval;
        }
        private bool SetStateWithTimeout(State newState, uint timeoutMs)
        {
            bool retval = false;
            if (System.Threading.Monitor.TryEnter(_lock, (int)timeoutMs))
            {
                try
                {
                    _state = newState;
                    retval = true;
                }
                finally
                {
                    System.Threading.Monitor.Exit(_lock);
                }
            }
            return retval;
        }
        //----------------- Public functional interface for manipulating devices ------------------------//
        /// <summary>
        /// Must be called once to start the BackEnd. 
        /// BackEnd.Instance.Start() is the proper means.
        /// </summary>
        public void Start()
        {
            _thread.Start();
        }

        /// <summary>
        /// Calling application can turn off the auto-refreshing of devices, this is helpful for
        /// pausing the web exchanges during advanced debugging.
        /// </summary>
        public bool EnableAutoRefresh { set; get; } = true;
 
        /// <summary>
        /// Queues a few rounds of updates/pollings for device and config values.
        /// Typically useful if EnableAutoRefresh is false, but calling app
        /// wants to update device caches.
        /// </summary>
        public void ManualRefresh()
        {
			/* do a few rounds of polling/updating */
            _refreshRequest = 5;
        }
        /// <returns>True if backend thread should poll/update
        /// devices on next loop </returns>
        private bool ShouldUpdateDevices()
        {
            if (_refreshRequest > 0)
                return true;
            if (EnableAutoRefresh)
                return true;
            return false;
        }
        /// <summary>
        /// Turns off polling features in backend thread.
        /// </summary>
        private void DisableRefreshing()
        {
            _refreshRequest = 0;
            EnableAutoRefresh = false;
        }
        /// <returns>  A copy of all devices found with latest info. </returns>
        public IEnumerable<DeviceDescrip> GetDeviceDescriptors()
        {
            List<DeviceDescrip> retval = new List<DeviceDescrip>();

            /* return a flat copy of what's been discovered so far */
            List<DeviceDescrip> temp = _descriptors.ToList();
            for (int i = 0; i < temp.Count; ++i)
            {
                bool ok = true;
                /* checks */
                if (ok)
                    ok = (temp[i].configCache != null); //Will be useful once every device has a config
                                                        //Currently not useful at all, however
                                                        /* if okay, insert to callers return */
                if (ok) { retval.Add(temp[i]); }
            }
            return temp;
        }

        public Status RequestBlink(DeviceDescrip dd, Action.CallBack callback)
        {
            Action ac = new Action(callback, dd, ActionType.Blink);
            return PushAction(ac);
        }
        public Status RequestSelfTest(DeviceDescrip dd, Action.CallBack callback)
        {
            Action ac = new Action(callback, dd, ActionType.SelfTest);
            return PushAction(ac);
        }
        public Status RequestChangeName(DeviceDescrip dd, String newName, Action.CallBack callback)
        {
            Action ac = new Action(callback, dd, ActionType.SetDeviceName);
            ac.stringParam = newName;
            return PushAction(ac);
        }
        public Status RequestChangeDeviceID(DeviceDescrip dd, byte newID, Action.CallBack callback)
        {
            Action ac = new Action(callback, dd, ActionType.SetID);
            ac.newID = (uint)newID;
            return PushAction(ac);
        }
        public Status RequestFieldUpgrade(DeviceDescrip dd, string crfPath, Action.CallBack callback)
        {
            Action ac = new Action(callback, dd, ActionType.FieldUpgradeDevice);
            ac.filePath = crfPath;
            return PushAction(ac);
        }
        public Status SaveConfigs(DeviceDescrip dd, string serializedData, Action.CallBack callback)
        {
            Action ac = new Action(callback, dd, ActionType.SetConfig);
            ac.stringParam = serializedData;
            return PushAction(ac);
        }

        //------------- Robot Controller Commands --------//
        public Status UpdateRIO(Action.CallBack callback)
        {
            Action ac = new Action(callback, ActionType.InstallDiagServerToRobotController);
            return PushAction(ac, false); //Server may not be on RIO yet, so we must force this actino past connection
        }
        public Status RevertRIO(Action.CallBack callback)
        {
            Action ac = new Action(callback, ActionType.UninstallDiagServerToRobotController);
            return PushAction(ac, false); //Server may not be on RIO yet, so we must force this actino past connection
        }
        public Status StartServer(Action.CallBack callBack)
        {
            Action ac = new Action(callBack, ActionType.StartServer);
            return PushAction(ac, false);
        }
        public Status StopServer(Action.CallBack callBack)
        {
            Action ac = new Action(callBack, ActionType.StopServer);
            return PushAction(ac, false);
        }
        //------------ Unit Testing Commands --------//
        public Status CheckRIOProcess(Action.CallBack callback)
        {
            Action ac = new Action(callback, ActionType.CheckProcess);
            return PushAction(ac); //Server may not be up yet, so we must force past connection
        }
        public Status GetNewConnectionVersion(Action.CallBack callback, string version)
        {
            Action ac = new Action(callback, ActionType.GetVersion);
            ac.stringParam = version;
            return PushAction(ac); //Must wait until we are connected before calling this
        }
        public Status GetNumberOfDevices(Action.CallBack callback, uint numberToExceed)
        {
            Action ac = new Action(callback, ActionType.GetNumOfDevices);
            ac.param = numberToExceed;
            return PushAction(ac);
        }
        public Status RebootRio(Action.CallBack callback)
        {
            Action ac = new Action(callback, ActionType.RebootRio);
            return PushAction(ac);
        }
        //------------ HOST selection ---------------//
        public Status SetHostName(String hostNewName, String hostPort)
        {
            _hostName.Set(hostNewName, hostPort);
            return Status.Ok;
        }
        //------------------- Getting raw HTTP exchanges -----------------//
        public List<Telemetry.ExchangeLog> GetExchanges()
        {
            return HTTP.Telemetry.GetInstance().Pop();
        }
        //------------ Singleton pattern ---------------//
        private BackEnd()
        {
            _thread = new Thread(Loop);
        }

        public static BackEnd Instance
        {
            get
            {
                if (_instance == null) { _instance = new BackEnd(); }
                return _instance;
            }
        }
        private static BackEnd _instance = null;
    }
}
