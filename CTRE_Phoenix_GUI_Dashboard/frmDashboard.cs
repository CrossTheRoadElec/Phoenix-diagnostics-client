using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CTRE.Phoenix.Diagnostics;
using CTRE.Phoenix.Diagnostics.BackEnd;
using CTRE.Phoenix.Diagnostics.JSON.Serializers;
using CTRE.Phoenix.dotNET;
using CTRE.Phoenix.dotNET.Form;
using BackEndAction = CTRE.Phoenix.Diagnostics.BackEnd.Action;
using System.IO;
using System.IO.Compression;

namespace CTRE_Phoenix_GUI_Dashboard {
    public partial class frmDashboard : Form {
        
        /// <summary>
        /// GUI state (machine)
        /// </summary>
        enum GuiState {
            Enabled, //!< User can press buttons
            Disabled_WaitForAction, //!<Waiting for action to finish
            Disabled_WaitForSelfTest, //!< Waiting for selt-test to finish
            Disabled_waitForReflash, //!< Waiting for field-upgrade to finish
            Disabled_WaitForInstallIntoRobotController,  //!< Waiting for installing into RIO/RaspPi/etc.
            Disabled_WaitForUnitTest,
        }

        /* ----------- state variables -------- */
        GuiState _guiState;
        private BottomStatusBar _BottomStatusBar;
        private DeviceListContainer _deviceListContainer;
        int _guiTimeoutMs = 0;
        private Object _lock = new Object();
        private struct ActionResponse {
            public BackEndAction action;
            public Status error;
        }
        ActionResponse _actionResponse;
        private bool _bAutoSaveGuiChanges = false;
        private DeviceDescrip _deviceUsedForConfigs = null;
        private TabPage _tabPageSelfTest = null;
        private ToggleButton _toggleWebPaused = null;
        private ToggleButton _toggleWebJumpToBtm = null;
        /* ----------- Constants -------- */
        private readonly String kAppName = "Dashboard";

        public frmDashboard()
        {
            InitializeComponent();
            /* update form caption */
            Text = FormRoutines.MakeVersionString(kAppName);
            /* GUI components */
            bottomStrip_R.Text = kAppName;
            _deviceListContainer = new DeviceListContainer(lstDevices);
            _deviceListContainer.SelectionChanged += new DeviceListContainer.SelectionChangedEventHandler(ListChanged);
            _BottomStatusBar = new BottomStatusBar(statusStrip1, bottomStrip_L, bottomStrip_ML, bottomStrip_MR, bottomStrip_R);
            _toggleWebPaused = new ToggleButton(btnPauseTelem, false, "Press to Pause", "Paused");
            _toggleWebJumpToBtm = new ToggleButton(btnJumpToBottom, true, "Auto-scroll Disabled", "Auto-scroll Enabled");
            /* setup start machine */
            SetGuiState(GuiState.Enabled, 0);
            /* allow user to press buttons */
            EnableDisableEntireGui(true);
            /* load the persistent form settings */
            LoadStickySettings();
            /* init any easter egg logic */
            EasterEgg_Init();

            /* setup and start the backend */
            UpdateHostNameAndPort();
            BackEnd.Instance.Start();

            /* more GUI rendering updates */
            PaintEnablePollingCheckbox();
        }

        //--------------- Rendering GUI elements ---------------------------------//
        Color DecodeMessageColor(String colorString)
        {
            switch (colorString[0]) {
                case 'G':
                    return Color.DarkGreen;
                case 'R':
                    return Color.Red;
                case 'O':
                    return Color.OrangeRed;
            }
            return Color.Black;
        }

        /// <summary>
        /// Caller wants to enable or disable the entire GUI.
        /// </summary>
        /// 
        /// This is primarily to prevent the user from pressing buttons
        /// when the BackEnd is busy servicing the previous button press.
        /// 
        /// This also signals to the user that the BackEnd is busy.
        /// 
        /// This is called...
        /// - to disable, immedietely after an action is started,
        /// - then called again to enable after action finishes.
        /// <param name="enabled"></param>
        void EnableDisableEntireGui(bool enabled)
        {
            /* disabling the outer tab control is adequeate */
            tabControl.Enabled = enabled;

            if (enabled) {
                RefreshAllDataEntry();
            } else {
                /* anything special to do when disabled? */
            }
        }
        /// <summary>
        /// Refresh the panel enables and any data entry GUI components so they match 
        /// what's current in the device.
        /// 
        /// This is called...
        /// - immedietely after an action is started when user presses a button,
        /// - again after action finishes.
        /// - if the device selection changes.
        /// </summary>
        void RefreshAllDataEntry()
        {
            var selected = _deviceListContainer.SelectedDeviceDescriptor;

            bool bDeviceIsSelected = (selected != null);

            if (bDeviceIsSelected)
            {
                /* make sure the stuff on the left are filled in with device's latest info */
                PaintDeviceIdLocaleAndBitrateEntry(selected);
            }

            /* this always leads to text becoming black because we just rendered it */
            PaintBlueUnappliedSettings();

            /* config rendering */
            if (bDeviceIsSelected == false)
            {
                _deviceUsedForConfigs = null;
                ClearSelfTestAndConfigParamsTabs();
            }
            else if (_deviceUsedForConfigs != selected)
            {
                _deviceUsedForConfigs = selected;
                PaintSelfTestAndConfigParamsTabs(_deviceUsedForConfigs);
            }
            else
            {
                /* we haven't actually change the device, 
                 * don't update anything */
            }
        }

        void PaintEnablePollingCheckbox()
        {
            bool bEnableAutoRefresh = BackEnd.Instance.EnableAutoRefresh;
            enableAutoRefreshMenuItem1.Checked = bEnableAutoRefresh;
            disableAutoRefreshMenuItem1.Checked = !bEnableAutoRefresh;
            btnRefreshDevices.Visible = !bEnableAutoRefresh;
        }
        /// Caller must ensure dd is not null
        /// <param name="dd"></param>
        void PaintDeviceIdLocaleAndBitrateEntry(DeviceDescrip dd)
        {
            /* update all the data entry related to this */
            numNewDevId.Value = dd.deviceID;
            txtDeviceNewName.Text = dd.jsonStrings.Name;
        }
        /// <summary>
        /// Change any unapplied settings BLUE so user knows to save settings.
        /// Or put them back to BLACK if settings match latest update.
        /// </summary>
        private void PaintBlueUnappliedSettings()
        {
            Color col;
            /* get selected device */
            var selected = _deviceListContainer.SelectedDeviceDescriptor;

            // --------------------- Device ID ---------------------- //
            /* color blue if the user choice is different than what's current */
            byte deviceId = (byte)numNewDevId.Value;
            col = Color.Black;
            if (selected == null)
            {
                /* no device selected, stay black */
            }
            else if (selected.deviceID == deviceId)
            {
                /* setting matches, stay black */
            }
            else
            {
                col = Color.Blue;
            }
            /* paint blue if setting does not match whats current */
            btnChangeDevId.ForeColor = col;
            numNewDevId.ForeColor = col;

            // --------------------- Device Name ---------------------- //
            /* color blue if the user choice is different than what's current */
            string newName = txtDeviceNewName.Text;
            col = Color.Black;
            if (selected == null)
            {
                /* no device selected, stay black */
            }
            else if (selected.jsonStrings.Name == newName)
            {
                /* setting matches, stay black */
            }
            else
            {
                /* setting does not match */
                col = Color.Blue;
            }
            btnNameChange.ForeColor = col; /* paint blue if setting does not match whats current */
            txtDeviceNewName.ForeColor = col;
        }

        void RenderHTTPBrowser(int RowIndex)
        {
            if (RowIndex < 0)
            {
                /* user pressed on column */
            }
            else
            {
                /* get the row object */
                DataGridViewRow rowToDisplay = gridDiagnosticLog.Rows[RowIndex];

                /* pull the strings out of grid row */
                string request = (string)rowToDisplay.Cells[CTRE.Phoenix.Diagnostics.HTTP.Telemetry.ExchangeLog.kStringArrayIndex_Req].Value;
                string response = (string)rowToDisplay.Cells[CTRE.Phoenix.Diagnostics.HTTP.Telemetry.ExchangeLog.kStringArrayIndex_Resp].Value;
                string type = (string)rowToDisplay.Cells[CTRE.Phoenix.Diagnostics.HTTP.Telemetry.ExchangeLog.kStringArrayIndex_Type].Value;

                /* render the html */
                string textBox;
                textBox = "<!DOCTYPE html><html>";
                textBox += "    <p><font size =\"" + 0.3 + "\">" + "URL Request is:<br>" + request + "</font></p> ";
                textBox += "    <p><font size =\"" + 0.3 + "\">" + "Server Response is:<br>" + response + "</font></p> ";
                textBox += "    <p><font size =\"" + 0.3 + "\">" + "This is a : " + type + " request" + "</font></p>";
                textBox += "</html>";

                /* dump html into GUI component */
                browserMessageDisp.Navigate("about:blank");
                browserMessageDisp.Document.OpenNew(false);
                browserMessageDisp.Document.Write(textBox);
                browserMessageDisp.Refresh();
            }
        }

        void PaintSelfTestAndConfigParamsTabs(DeviceDescrip deviceToGenFrom)
        {
            /* clear everything */
            ClearSelfTestAndConfigParamsTabs();
            /* paint the self-test tab ... */
            groupedControls.TabPages.Add(GetSelfTestTabPage());
            /* ... then paint the device specific tabs*/
            GenerateTabs(deviceToGenFrom, groupedControls);

        }
        void ClearSelfTestAndConfigParamsTabs()
        {
            groupedControls.TabPages.Clear();
            ClearSelfTestPage();
        }
        TabPage GetSelfTestTabPage()
        {
            /* if we have not made the self-test tab yet, make it once */
            if (_tabPageSelfTest == null)
            {
                /* create the tab page */
                _tabPageSelfTest = new TabPage("Self-Test");
                _tabPageSelfTest.Controls.Add(txtSelfTestResults);
                /* render the actual text box */
                txtSelfTestResults.Visible = true;
                txtSelfTestResults.Dock = DockStyle.Fill;
            }
            return _tabPageSelfTest;
        }
        void ClearSelfTestPage()
        {
            txtSelfTestResults.Text = "This is the Self Test Box, select the device you wish to perform the self test on and Press the \"Self Test\" button";
        }
        //-------------------------- Polling info on the concurrent diagnostic action --------------------------------------//
        /// <summary>
        /// Clear the cached action so that our timer-driven process knows nothing is happening.
        /// </summary>
        void ClearActionResponse()
        {
            _actionResponse.action = null;
        }
        /// <summary>
        /// Callback from BackEnd thread with status info on a finished action.
        /// </summary>
        /// Save the info so our form's timer task can react.
        /// <param name="action"></param>
        /// <param name="err"></param>
        public void ActionCallBack(BackEndAction action, Status err)
        {
            /* save all the info, next timer loop will process */
            _actionResponse.action = action;
            _actionResponse.error = err;
        }
        /// <summary>
        /// Form timer task will call this to poll for status on action.
        /// </summary>
        /// <returns> true iff action has finished.</returns>
        bool HasReceivedActionResponse(out BackEndAction action)
        {
            /* init outputs */
            action = null;

            if (_actionResponse.action != null) {
                /* we've gotton our response */
                PrintActionError(_actionResponse.error);
                /* pass up ref */
                action = _actionResponse.action;
                /* clear response */
                _actionResponse.action = null;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Form timer task will call this to poll for status on action.
        /// </summary>
        /// <returns> true iff action has finished.</returns>
        bool HasReceivedActionResponse()
        {
            BackEndAction notused;
            return HasReceivedActionResponse(out notused);
        }
        //-------------------------- Sticky Settings  --------------------------------------//
        private void SaveStickySettings()
        {
            if (false == _bAutoSaveGuiChanges) {
                /* do not react to changes, we are loading the settings */
            } else {
                /* make the folder path sticky */
                Properties.Settings.Default.tabIndex = tabControl.SelectedIndex;
                Properties.Settings.Default.hostname = cboHostSelectorAddr.Text;
                Properties.Settings.Default.hostport = cboHostSelectorPrt.Text;
                Properties.Settings.Default.crfpath = txtDeviceCRFPath.Text;
                Properties.Settings.Default.AutoRefreshDevices = BackEnd.Instance.EnableAutoRefresh;
                Properties.Settings.Default.Save();
            }
        }
        private void LoadStickySettings()
        {
            /* load our sticky settings */
            tabControl.SelectedIndex = Properties.Settings.Default.tabIndex;
            cboHostSelectorAddr.Text = Properties.Settings.Default.hostname;
            cboHostSelectorPrt.Text = Properties.Settings.Default.hostport;
            txtDeviceCRFPath.Text = Properties.Settings.Default.crfpath;
            BackEnd.Instance.EnableAutoRefresh = Properties.Settings.Default.AutoRefreshDevices;
            /* allow saving of sticky settings */
            _bAutoSaveGuiChanges = true;
        }
        //-------------------------- GUI State getter/setters ----------------------------//
        private void SetGuiState(GuiState newState, int timeoutMs)
        {
            Debug.Print("GUI", newState.ToString());

            lock (_lock) {
                _guiState = newState;
                _guiTimeoutMs = timeoutMs;
            }

        }
        private GuiState GetGuiState()
        {
            lock (_lock) { return _guiState; }
        }
        //-------- Convenient display routines for updating bottom status bar --------------//
        private void DisplayVersionNumber(string vers)
        {
            _BottomStatusBar.PrintRight(Color.Black, "Server Version: " + vers);
        }
        private void PrintBackEndStatus(Color col, String text, String hoverMsg)
        {
            _BottomStatusBar.SetHoverString(hoverMsg);
            _BottomStatusBar.PrintMiddleLeft(col, text);
        }
        private void PrintActionError(Status error)
        {
            if (error == Status.Ok)
                _BottomStatusBar.PrintMiddleRight(Color.DarkGreen, "Ok");
            else
                _BottomStatusBar.PrintMiddleRight(Color.Red, error.ToString());
        }
        private void ClearActionError()
        {
            _BottomStatusBar.PrintMiddleRight(Color.DarkGreen, "");
        }
        /// <returns> True if last action was a successful one or if action has been cleared </returns>
        private bool IsLastActionErrorNotOk()
        {
            string s = _BottomStatusBar.GetMiddleRight();
            if (s.Equals("Ok"))
                return false;
            if (s.Equals(""))
                return false;
            return true;
        }
        private void PrintConnectionStatus(Color col, String text)
        {
            _BottomStatusBar.PrintRight(col, text);
        }
        
        private void UpdateHostNameAndPort()
        {
            BackEnd.Instance.SetHostName(cboHostSelectorAddr.Text, cboHostSelectorPrt.Text);
            SaveStickySettings();
        }
        //------------------- Every action has a common pre and post  ------------------------//
        /**
         * Call this before performing a GUI-blocking action.  If return is OK, invoke the action
         * by calling the BackEnd routine, then process the result using PostOperation().
         * This will ensure the GUI will cleanly disable while the action is being performed,
         * and will restore when finished.
         * 
         * This routine will also check that a device is actually selected.
         */
        Status PreOperation(out DeviceDescrip dd, bool bRequireSelectedDevice = true)
        {
            /* init the outputs */
            dd = null;

            /* ignore clicks when form is busy */
            if (GetGuiState() != GuiState.Enabled)
                return Status.UIError;

            /* get selected device */
            dd = _deviceListContainer.SelectedDeviceDescriptor;

            /* fail if device is not selected */
            if (bRequireSelectedDevice == false)
            {
                /* caller does not need a device to be selected */
            }
            else if (dd == null)
            {
                /* caller needs a device to be selected, return appropriate error */
                return Status.DeviceNotSelected; /* no device found */
            }
            else
            {
                /* caller needs a device to be selected, and one is selected */
            }

            /* disable form so user can't press anything until action is finished */
            EnableDisableEntireGui(false);

            /* clear the action response, which is periodically checked in a timer event */
            ClearActionResponse();

            /* wipe the status from prev action, we are about to start a new one */
            ClearActionError();

            return Status.Ok;
        }
        Status PreOperation()
        {
            DeviceDescrip dd;
            return PreOperation(out dd, false); // bRequireSelectedDevice is false
        }

        void PostOperation(Status er, GuiState nextSt = GuiState.Disabled_WaitForAction)
        {
            if (er == Status.Ok) {
                /* since the status is OK, that means the action is being performed, so 
                 wait for the callback before re-enabling GUI. */
                SetGuiState(nextSt, 0);
            } else {
                /* action was not started, report why */
                PrintActionError(er);
                /* restore GUI so user can press buttons again */
                EnableDisableEntireGui(true);
            }
        }
        //------------------- actions user can choose ------------------------//
        void BlinkSelectedDevice()
        {
            /* common pre-action checks, including getting the selected device */
            DeviceDescrip dd;
            Status er = PreOperation(out dd);
            /* request the action */
            if (er == Status.Ok)
                er = BackEnd.Instance.RequestBlink(dd, new BackEndAction.CallBack(ActionCallBack));
            /* common post-action checks, transition to wait for response handler */
            PostOperation(er, GuiState.Disabled_WaitForAction);
        }
        void ChangeIDOfSelectedDevice()
        {
            /* get the new device ID from the GUI element */
            byte newID = (byte)numNewDevId.Value;
            /* common pre-action checks, including getting the selected device */
            DeviceDescrip dd;
            Status er = PreOperation(out dd);
            /* Check with backend if newID is already in use */
            if(er != Status.Ok)
            {
                // Do nothing
            }
            else if(BackEnd.Instance.NewIdConflicts(dd, newID) == false)
            {
				/* there are no ID conflicts, nothing to do */
			}
			else if(!PromptUserConflictID())
            {
				/* user aborted */
				er = Status.Aborted;
            }
            else
            {
                er = _deviceListContainer.RemoveSelectedItem();
                if(er == Status.Ok)
                    er = BackEnd.Instance.RemoveDD(dd);
            }
            /* request the action */
            if (er == Status.Ok)
                er = BackEnd.Instance.RequestChangeDeviceID(dd, newID, new BackEndAction.CallBack(ActionCallBack));
            /* common post-action checks, transition to wait for response handler */
            PostOperation(er, GuiState.Disabled_WaitForAction);
        }
        void ChangeNameOfSelectedDevice()
        {
            /* get the user's requested new name */
            string newName = txtDeviceNewName.Text;
            /* common pre-action checks, including getting the selected device */
            DeviceDescrip dd;
            Status er = PreOperation(out dd);
            /* request the action */
            if (er == Status.Ok)
                er = BackEnd.Instance.RequestChangeName(dd, newName, new BackEndAction.CallBack(ActionCallBack));
            /* common post-action checks, transition to wait for response handler */
            PostOperation(er, GuiState.Disabled_WaitForAction);
        }
        void FetchSelfTestOfSelectedDevice()
        {
            /* common pre-action checks, including getting the selected device */
            DeviceDescrip dd;
            Status er = PreOperation(out dd);
            /* request the action */
            if (er == Status.Ok)
                er = BackEnd.Instance.RequestSelfTest(dd, new BackEndAction.CallBack(ActionCallBack));
            /* common post-action checks, transition to wait for response handler */
            PostOperation(er, GuiState.Disabled_WaitForSelfTest);
        }
        void FieldUpgradeSelectedDevice()
        {
            /* common pre-action checks, including getting the selected device */
            DeviceDescrip dd;
            Status er = PreOperation(out dd);
            /* request the action */
            if (er == Status.Ok)
                er = BackEnd.Instance.RequestFieldUpgrade(dd, txtDeviceCRFPath.Text, new BackEndAction.CallBack(ActionCallBack));
            /* common post-action checks, transition to wait for FIELD UPGRADE progress */
            PostOperation(er, GuiState.Disabled_waitForReflash);
        }
        void SaveConfigsOfSelectedDevice()
        {
            /* First prep by getting all the objects required together and filling them with data */
            DeviceConfigs configs = GetConfigsFromTabs();
            /* Then serialize the data to be sent to the backend */
            string serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(configs);
            /* common pre-action checks, including getting the selected device */
            DeviceDescrip dd;
            Status er = PreOperation(out dd);
            /* request the action */
            if (er == Status.Ok)
                er = BackEnd.Instance.SaveConfigs(dd, serializedData, new BackEndAction.CallBack(ActionCallBack));
            /* common post-action checks, transition to wait for FIELD UPGRADE progress */
            PostOperation(er, GuiState.Disabled_WaitForAction);
        }
        void InstallDiagServerToRobotController()
        {
            rtbRioUpdateBox.Clear(); //Clear text

            /* common pre-action checks, do not request selected device */
            Status er = PreOperation();
            /* request the action */
            if (er == Status.Ok) {
                er = BackEnd.Instance.UpdateRIO(new BackEndAction.CallBack(ActionCallBack));
			}
            /* common post-action checks, transition to wait for FIELD UPGRADE progress */
            PostOperation(er, GuiState.Disabled_WaitForInstallIntoRobotController);
        }
        void UninstallDiagServerToRobotController()
        {
            rtbRioUpdateBox.Clear(); //Clear text

            /* common pre-action checks, do not request selected device */
            Status er = PreOperation();
            /* request the action */
            if (er == Status.Ok) {
				er = BackEnd.Instance.RevertRIO(new BackEndAction.CallBack(ActionCallBack));
			}
            /* common post-action checks, transition to wait for FIELD UPGRADE progress */
            PostOperation(er, GuiState.Disabled_WaitForInstallIntoRobotController);
        }
        void StartServer()
        {
            rtbRioUpdateBox.Clear(); //Clear text

            Status er = BackEnd.Instance.StartServer(new BackEndAction.CallBack(ActionCallBack));
            PostOperation(er, GuiState.Disabled_WaitForInstallIntoRobotController);
        }
        void StopServer()
        {
            rtbRioUpdateBox.Clear(); //Clear text

            Status er = BackEnd.Instance.StopServer(new BackEndAction.CallBack(ActionCallBack));
            PostOperation(er, GuiState.Disabled_WaitForInstallIntoRobotController);
        }

        //------------------- FORM Timer event ------------------------//
        private void timer1_Tick(object sender, EventArgs e)
        {
            /* interval decisions */
            timer1.Interval = 40; /* 10ms => 40ms */

            /* get backup status, color, and hover message */
            String msg, messageColor, hoverMsg;
            BackEnd.State state = BackEnd.Instance.GetStatus(out msg, out messageColor, out hoverMsg);
            String connectionStatus = BackEnd.Instance.GetConnectionStatus();

            /* its up to the GUI how much of this stuff to show */
            PrintConnectionStatus(Color.Blue, connectionStatus);
            Color textColor = DecodeMessageColor(messageColor);
            PrintBackEndStatus(textColor, msg, hoverMsg);

            string version = BackEnd.Instance.GetVersionNumbers();
            DisplayVersionNumber(version);

            /* check the testing tasks */
            TestingPeriodic();

            /* empty web responses into GUI elements*/
            HttpTelemetryPeriodic();

            /* get devices */
            IEnumerable<DeviceDescrip> devs = BackEnd.Instance.GetDeviceDescriptors();
            /* update tree list */
            _deviceListContainer.RefreshDeviceTree(devs);

            /* count down timeout */
            if (_guiTimeoutMs > 0) {
                _guiTimeoutMs -= timer1.Interval;
            } else {
                /* GUI state machine */
                switch (_guiState) {
                    case GuiState.Disabled_WaitForAction:
                        /* if response is captured, transition */
                        if (HasReceivedActionResponse()) {
                            /* back to normal */
                            EnableDisableEntireGui(true);
                            /* back to enabled */
                            SetGuiState(GuiState.Enabled, 0);
                        }
                        break;

                    case GuiState.Disabled_WaitForSelfTest:
                        /* if response is captured, transition */
                        BackEndAction selfTestAction;
                        if (HasReceivedActionResponse(out selfTestAction))
                        {
                            /* update tab */
                            txtSelfTestResults.Text = selfTestAction.selfTestResults;
                            /* back to normal */
                            EnableDisableEntireGui(true);
                            /* back to enabled */
                            SetGuiState(GuiState.Enabled, 0);
                            /* select the self-test tab */
                            groupedControls.SelectedTab = GetSelfTestTabPage();
                        }
                        break;
                    case GuiState.Disabled_waitForReflash:
                        /* periodically update progress bar */
                        int prog = (int)BackEnd.Instance.FieldUpgradeProgressPerc;
                        prgCanDev.Value = prog;
                        if ((prog == 0) || (prog == 100))
                            prgCanDev.Visible = false;
                        else
                            prgCanDev.Visible = true;

                        lblDevFlashResults.Text = BackEnd.Instance.FieldUpgradeStatus;

                        /* if response is captured, transition */
                        if (HasReceivedActionResponse()) {
                            /* hide the GUI elems we no longer care about */
                            prgCanDev.Visible = false;

                            /* back to normal */
                            EnableDisableEntireGui(true);
                            /* back to enabled */
                            SetGuiState(GuiState.Enabled, 0);
                        }
                        break;
                    case GuiState.Disabled_WaitForInstallIntoRobotController:
                        /* Periodically update RioUpdateBox */
                        bool done = false;
                        string status = BackEnd.Instance.GetRioUpdateStatus(ref done);
                        /* add message to presentation */
                        rtbRioUpdateBox.AppendText(status);
                        if (done)
                        {
                            /* back to normal */
                            EnableDisableEntireGui(true);
                            /* back to enabled */
                            SetGuiState(GuiState.Enabled, 0);
                        }
                        break;
                    case GuiState.Disabled_WaitForUnitTest:
                        rtbUnitTestBox.HideSelection = false;
                        rtbUnitTestBox.AppendText(UnitTesting.Instance.GetLog());
                        if (UnitTesting.Instance.Done())
                            SetGuiState(GuiState.Enabled, 0);
                        break;

                    case GuiState.Enabled:
                        /* nothing to do but wait for user */
                        break;
                }
            }
        }
        //------------------- Update debugging tab  ------------------------//
        void HttpTelemetryPeriodic()
        {
            /* leave routine IMMEDIETELY if user paused the rendering */
            if (_toggleWebPaused.IsPressed == true) { return; }

            /* leave routine IMMEDIETELY if there are no new exchanges */
            var exchanges = BackEnd.Instance.GetExchanges();
            if (exchanges == null) { return; }

            /* stop rendering grid */
            ControlHelper.SuspendDrawing(gridDiagnosticLog);

            /* go thru each one */
            foreach (var exchange in exchanges)
            {
                /* create row with strings */
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(gridDiagnosticLog, exchange.ToStringArray());

                /* pick color */
                Color color = Color.White;
                switch (exchange.actionType)
                {
                    case ActionType.FieldUpgradeDevice:
                        color = Color.LightCoral;
                        break;
                    case ActionType.SetConfig:
                    case ActionType.SetDeviceName:
                    case ActionType.SetID:
                        color = Color.Gold;
                        break;
                    case ActionType.SelfTest:
                        color = Color.Cyan;
                        break;
                    default:
                        if (exchange.type.Equals("POST")) { color = Color.LightGreen; }
                        break;
                }
                row.DefaultCellStyle.BackColor = color;

                /* and finally add to grid */
                gridDiagnosticLog.Rows.Add(row);
            }

            /* jump to bottom if enabled */
            if (_toggleWebJumpToBtm.IsPressed == false)
            {
                /* do nothing, leave user selected alone */
            }
            else if (gridDiagnosticLog.RowCount == 0)
            {
                /* there are no rows, so nothing to do */
            }
            else
            {
                /* jump to last one */
                gridDiagnosticLog.FirstDisplayedScrollingRowIndex = gridDiagnosticLog.RowCount - 1;
            }

            /* resume rendering grid */
            ControlHelper.ResumeDrawing(gridDiagnosticLog);
        }

        private void CopyDiagnosticLogToClipboard()
        {
            if (false == FormRoutines.ClipboardSet(gridDiagnosticLog.GetClipboardContent()))
            {
                PromptClipboardNotAvailable();
            }
        }

        private void ClearDiagnosticLog()
        {
            gridDiagnosticLog.Rows.Clear();
        }
        //------------------- Testing ------------------------//
        void TestingPeriodic()
        {
            /* do nothing */
        }

        //--------------------------------------------------------------------------------------------------//
        //------------------------------------------- Unit Tests -------------------------------------------//
        //--------------------------------------------------------------------------------------------------//
        private void StartTests()
        {
            uint tests = 0;
            foreach(var item in unitTestingCheckboxes.CheckedItems)
            {
                tests |= ((uint)1 << unitTestingCheckboxes.Items.IndexOf(item));
            }
            Status err = UnitTesting.Instance.StartTesting(tests);
            PostOperation(err, GuiState.Disabled_WaitForUnitTest);
        }

        private void StopTests()
        {
            Status err = UnitTesting.Instance.Dispose();
            PostOperation(err, GuiState.Disabled_WaitForUnitTest);
        }

        //--------------------------------------------------------------------------------------------------//
        //----------------------------------------- Easter Egg / Workarounds -------------------------------//
        //--------------------------------------------------------------------------------------------------//
        private void EasterEgg_Init()
        {
            tabControl.TabPages.Remove(tbUnitTesting);

            /* Set process and version info test to true for default tests */
            unitTestingCheckboxes.SetItemChecked(0, true);
            unitTestingCheckboxes.SetItemChecked(1, true);
        }
        private void EasterEgg_Show()
        {
            if(tabControl.TabPages.IndexOf(tbUnitTesting) == -1) {
                tabControl.TabPages.Add(tbUnitTesting);
			}
        }
        private void EasterEggHandler()
        {
            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.LeftShift)) {
                EasterEgg_Show();
			}
        }
        //--------------------------------------------------------------------------------------------------//
        //----------------------------------------- User Prompts -------------------------------------------//
        //--------------------------------------------------------------------------------------------------//
        private bool PromptUserConflictID()
        {
            var result = MessageBox.Show("There exists another device with same device ID.  Are you sure you want to do this?",
                                            "Device ID Conflict",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Question,
                                            MessageBoxDefaultButton.Button2); // Default to the "second" button, which is NO
            return result == DialogResult.Yes;
        }        
        private void PromptClipboardNotAvailable()
        {
               var result = MessageBox.Show("Application could not get access to clipboard.",
                                            "Clipboard not available",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information,
                                            MessageBoxDefaultButton.Button1); // Default to OK
        }

        void OpenFileBrowserCRF()
        {
            /* parse the path setting */
            string folderPath;
            string fileName;
            CTRE.Phoenix.dotNET.File.ParseResult res = CTRE.Phoenix.dotNET.File.ParseArbitraryPath(txtDeviceCRFPath.Text, out folderPath, out fileName);

            /* make a new dialog */
            OpenFileDialog firmwareDialog = new OpenFileDialog();

            /* place some shortcuts in the left pane */
            //C:\Users\Public\Documents\Cross The Road Electronics\LifeBoat\HERO Firmware Files
            //C:\Users\Public\Documents\FRC
            firmwareDialog.CustomPlaces.Add(@"C:\Users\Public\Documents\FRC");
            firmwareDialog.CustomPlaces.Add(@"C:\Users\Public\Documents\Cross The Road Electronics\LifeBoat\HERO Firmware Files");

            /* setup the dialog */
            firmwareDialog.Filter = "CRF Firmware File|*.crf";
            firmwareDialog.Title = "Choose a CRF File";

            /* the goal is to jump to the file/folder in the text entry */
            if (res == CTRE.Phoenix.dotNET.File.ParseResult.File)
            {
                /* there is a file path, jump right to the file */
                firmwareDialog.FileName = fileName;
                firmwareDialog.InitialDirectory = folderPath;
            }
            else if (res == CTRE.Phoenix.dotNET.File.ParseResult.Folder)
            {
                /* there is a dir path, jump right to the folder*/
                firmwareDialog.FileName = string.Empty;
                firmwareDialog.InitialDirectory = folderPath;
            }
            else
            {
                /* not sure what the path is, so let Windows decide the home folder,
                 * normally it falls back to the last used folder. */
            }

            /* prompt the user */
            if (firmwareDialog.ShowDialog() == DialogResult.OK)
            {
                /* user pressed ok, save the full path and sticky setting*/
                txtDeviceCRFPath.Text = firmwareDialog.FileName;
                SaveStickySettings();
            }
        }
        //--------------------------------------------------------------------------------------------------//
        //----------------------------------------- GUI Element generation ---------------------------------//
        //--------------------------------------------------------------------------------------------------//
        void GenerateTabs(DeviceDescrip dd, System.Windows.Forms.TabControl tabControl)
        {
            String strNamespace = "CTRE_Phoenix_GUI_Dashboard";
            /* lots of missing error checking here */

            if (dd.configCache != null)
            {
                foreach (ConfigGroup group in dd.configCache.Device.Configs)
                {
                    Type t = Type.GetType(strNamespace + "." + group.Type);

                    IControlGroup newGroup = (IControlGroup)Activator.CreateInstance(t);

                    GroupTabPage newTab = new GroupTabPage(newGroup);
                    newTab.Text = group.Name;

                    newGroup.SetFromValues(group.Values, group.Ordinal.Value);

                    newTab.Controls.Add(newGroup.CreateLayout());

                    tabControl.TabPages.Add(newTab);
                }
            }
        }

        DeviceConfigs GetConfigsFromTabs()
        {
            /* Create a DeviceConfigs variable that has all the config data from the tabs */
            DeviceConfigs allConfigs = new DeviceConfigs();
            List<ConfigGroup> listOfConfigs = new List<ConfigGroup>();
            foreach (TabPage tab in groupedControls.TabPages)
            {
                if (tab is GroupTabPage)
                {
                    /* Cast tab as a GroupTabPage */
                    GroupTabPage groupTab = (GroupTabPage)tab;
                    /* Fill its group with data */
                    groupTab.group.GetFromValues(groupTab);

                    /* Take ConfigGroup and fill it with relevant data */
                    ConfigGroup newGroup = new ConfigGroup();
                    newGroup.Name = groupTab.Text;
                    newGroup.Type = groupTab.group.GetType().Name;
                    newGroup.Ordinal = 0;
                    if (groupTab.group is SlotGroup)
                        newGroup.Ordinal = (int)((SlotGroup)groupTab.group).SlotNumber;
                    newGroup.Values = groupTab.group;
                    
                    /* Add ConfigGroup to list */
                    listOfConfigs.Add(newGroup);
                }
            }
            allConfigs.Configs = listOfConfigs.ToArray();
            return allConfigs;
        }
        //--------------------------------------------------------------------------------------------------//
        //----------------------------------------- GUI EVENTS----------------------------------------------//
        //--------------------------------------------------------------------------------------------------//
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            /* let user know we are shutting down */
            PrintBackEndStatus(Color.DarkGoldenrod, "Shutting Down Backend...", null);
            Refresh();
            Debug.Print("GUI", "Shutting Down Backend...");
            /* shutdown back end */
            BackEnd.Instance.Dispose();
            UnitTesting.Instance.Dispose();
        }

        private void lstDevices_SelectedIndexChanged(object sender, EventArgs e) { _deviceListContainer.SelectedIndexChanged(sender, e); }

        private void ListChanged(object sender, EventArgs e, DeviceDescrip dd) { RefreshAllDataEntry(); }

        private void enableToolStripMenuItem_Click(object sender, EventArgs e) { BackEnd.Instance.EnableAutoRefresh = (true); }

        private void disableToolStripMenuItem_Click(object sender, EventArgs e) { BackEnd.Instance.EnableAutoRefresh = (false); }

        private void numNewDevId_ValueChanged(object sender, EventArgs e) { PaintBlueUnappliedSettings(); }

        private void cboCanBitrate_SelectedIndexChanged(object sender, EventArgs e) { PaintBlueUnappliedSettings(); }

        private void txtDeviceNewName_TextChanged(object sender, EventArgs e) { PaintBlueUnappliedSettings(); }

        private void enableDisableDeviceAutoRefresh_Click(object sender, EventArgs e) {
            bool bEnableAutoRefresh = (sender == enableAutoRefreshMenuItem1);
            BackEnd.Instance.EnableAutoRefresh = bEnableAutoRefresh;
            PaintEnablePollingCheckbox();
            SaveStickySettings();
        }
  
        private void txtDevicePath_TextChanged(object sender, EventArgs e) { SaveStickySettings(); }

        private void tabOptions_SelectedIndexChanged(object sender, EventArgs e) { SaveStickySettings(); }
        
        private void chkShowAdvanced_CheckedChanged(object sender, EventArgs e) { SaveStickySettings(); }

        private void blinkButton_Click(object sender, EventArgs e) { BlinkSelectedDevice(); }

        private void nameChangeButton_Click(object sender, EventArgs e) { ChangeNameOfSelectedDevice(); }

        private void idChangeButton_Click(object sender, EventArgs e) { ChangeIDOfSelectedDevice(); }

        private void btnUpdateDevice_Click(object sender, EventArgs e) { FieldUpgradeSelectedDevice(); }

        private void btnSelfTest_Click(object sender, EventArgs e) { FetchSelfTestOfSelectedDevice(); }

        private void cboHostSelector_TextChanged(object sender, EventArgs e) { UpdateHostNameAndPort(); }

        private void cboHostSelectorPrt_TextChanged(object sender, EventArgs e) { UpdateHostNameAndPort(); }

        private void btnSaveConfigs_Click(object sender, EventArgs e) { SaveConfigsOfSelectedDevice(); }

        private void btnFirmwareDialog_Click(object sender, EventArgs e) { OpenFileBrowserCRF(); }

        private void btnCopyHttpLog_Click(object sender, EventArgs e) { CopyDiagnosticLogToClipboard(); }

        private void btnUpdateBinaries_Click(object sender, EventArgs e) { InstallDiagServerToRobotController(); }

        private void btnRevertBinaries_Click(object sender, EventArgs e) { UninstallDiagServerToRobotController(); }

        private void btnEasterEgg_Click(object sender, EventArgs e) { EasterEggHandler(); }

        private void overnightTestButton_Click(object sender, EventArgs e) { StartTests(); }

        private void btnStopUnitTest_Click(object sender, EventArgs e) { StopTests(); }

        private void btnStartServer_Click(object sender, EventArgs e) { StartServer(); }

        private void btnStopServer_Click(object sender, EventArgs e) { StopServer(); }

        private void diagnosticLog_CellClick(object sender, DataGridViewCellEventArgs e) { RenderHTTPBrowser(e.RowIndex); }

        private void gridDiagnosticLog_CellEnter(object sender, DataGridViewCellEventArgs e) { RenderHTTPBrowser(e.RowIndex); }

        private void btnClrDiagLog_Click(object sender, EventArgs e) { ClearDiagnosticLog(); }

        private void btnPauseTelem_Click(object sender, EventArgs e) { _toggleWebPaused.Toggle(); }

        private void btnJumpToBottom_Click(object sender, EventArgs e) { _toggleWebJumpToBtm.Toggle(); }

        private void panelSelfTestAndConfigControls_Resize(object sender, EventArgs e)
        {
            /* shape self-test and save-configs buttons to be equidistant.
               panelWidth = space + btnWidth + space + btnWidth + space */
            int space = 2;
            int panelWidth = panelSelfTestAndConfigControls.Width;
            int btnWidth = (panelWidth - 3 * space) / 2;
            if (btnWidth < 1) { btnWidth = 1; }
            /* update btn dimensions */
            btnSelfTest.Left = space;
            btnSelfTest.Width = btnWidth;
            btnSaveConfigs.Left = space + btnWidth + space;
            btnSaveConfigs.Width = btnWidth;
        }

        private void selectAllCtrlAToolStripMenuItem_Click(object sender, EventArgs e) { gridDiagnosticLog.SelectAll(); }

        private void btnRefreshDevices_Click(object sender, EventArgs e) { BackEnd.Instance.ManualRefresh(); }

        private void txtDeviceCRFPath_TextChanged(object sender, EventArgs e) { SaveStickySettings(); }

        private void captureAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var exportPackager = new ExportPackager(gridDiagnosticLog,
                                     _deviceListContainer,
                                     lstDevices,
                                     cboHostSelectorAddr,
                                     cboHostSelectorPrt);
            exportPackager.Export();
        }
    }
}
