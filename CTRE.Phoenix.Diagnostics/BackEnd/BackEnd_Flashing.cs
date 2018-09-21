using CTRE.Phoenix.Diagnostics.HTTP;
using CTRE.Phoenix.Diagnostics.JSON.Serializers;
using CTRE.Phoenix.dotNET;
using Newtonsoft.Json;
using System;

namespace CTRE.Phoenix.Diagnostics.BackEnd
{
    public partial class BackEnd
    {
        private Status ConfirmIfFieldUpgradeIsOccuring(DeviceDescrip ddRef, bool bExpectToBeFlashing)
        {
            String response = String.Empty;
            Status retval = Status.Ok;

            /* get status update */
            if (retval == Status.Ok)
            {
                retval = _WebServerScripts.HttpGet(_hostName, ddRef.model, ddRef.deviceID, ActionType.CheckUpdateProgress, out response, "", 200);
            }
            if (retval != Status.Ok) { return retval; }


            /* get the error code in JSON */
            ProgressReturn jsonResp = JsonConvert.DeserializeObject<ProgressReturn>(response);
            retval = (Status)jsonResp.GeneralReturn.Error;

            if (bExpectToBeFlashing == false)
            {
                /* caller expects the server to be not be flashing anything -  we should get CTRE_DI_NotUpdating */

                if (retval == Status.CTRE_DI_NotUpdating)
                {
                    retval = Status.Ok;
                }
                else if (retval == Status.Ok)
                {
                    retval = Status.ServerIsFlashingAlready;
                }
            }
            else
            {
                /* server should be flashing - leave retval alone */
            }

            return retval;
        }

        private Status CheckAsyncWebServerResp(AsyncWebExchange asyncWebExchange, int timeoutMs, out bool isDone)
        {
            String response = String.Empty;
            Status retval = Status.Ok;

            isDone = false;

            /* async web response handler  */
            retval = asyncWebExchange.PollResponse(timeoutMs, out response);
            if (retval == Status.AsyncWebReqNotDone)
            {
                /* this is okay, its just not done yet */
                retval = Status.Ok;
            }
            else if (retval != Status.Ok)
            {
                /* let caller something went wrong  */
                _firmwareStatus.progressPerc = 100;
                _firmwareStatus.message = "Received Error" + retval;
                isDone = true;
            }
            else
            {
                /*  we are done - parse firmware-update resp */
                FirmwareUpdateReturn jsonDeser = JsonConvert.DeserializeObject<FirmwareUpdateReturn>(response);
                retval = (Status)jsonDeser.GeneralReturn.Error;

                /* save status update so calling application can poll it */
                if (retval == Status.Ok)
                {
                    _firmwareStatus.progressPerc = 100;
                    _firmwareStatus.message = "Successful Firmware Update";
                }
                else
                {
                    _firmwareStatus.progressPerc = 100;
                    _firmwareStatus.message = jsonDeser.UpdateMessage;
                }

                /* let caller know the field upgrade did stop/finish */
                isDone = true; 
            }

            return retval;
        }
        private Status ExecuteFieldUpgrade(DeviceDescrip ddRef, AsyncWebExchange asyncWebExchange, string fileName, bool usingSftp)
        {
            Status retval = Status.Ok;
            String response = string.Empty;

            /* let user know action is being processed */
            SetFieldUpgradeStatus("Starting field-upgrade...", 0);


            byte[] fileContents = null;
            /* If we're using POST, we need to make sure the file has contents */
            if (!usingSftp)
            {
                /* copy out CRF */
                fileContents = File.Read(_action.filePath);

                /* check file read */
                if (retval == Status.Ok)
                {
                    if (fileContents == null) { retval = Status.CouldNotOpenFile; }
                }
            }

            /* check firmUpdate progress first */
            if (retval == Status.Ok)
            {
                SetFieldUpgradeStatus("Confirm server is ready", 0); /* we are confirming if field upgrade is already occuring */
                /* Make double sure that we aren't field upgrading. Server already does this but there's no harm in checking again */
                retval = ConfirmIfFieldUpgradeIsOccuring(ddRef, false);
                SetFieldUpgradeStatus("Confirm server is ready : " + retval, 0); /* display the result of this check */
            }

            /* request firmware-update of device */
            if (retval == Status.Ok)
            {
                SetFieldUpgradeStatus("Starting field-upgrade", 0); /* now we will request to start a new FU session */

                /* If we're using sftp, use Get */
                if (usingSftp)
                {
                    /* start firmware-update */
                    retval = asyncWebExchange.StartHttpGet(_hostName, ddRef.model, ddRef.deviceID, ActionType.FieldUpgradeDevice, 60000, "&file=" + fileName);
                }
                /* Otherwise use Post */
                else
                {
                    /* start firmware-update */
                    retval = asyncWebExchange.StartHttpPost(_hostName, ddRef.model, ddRef.deviceID, ActionType.FieldUpgradeDevice, fileContents, 60000);
                }
            }
            /* confirm FU started okay */
            if (retval == Status.Ok)
            { 
                /* wait until we get the rising edge of the flash event - a percent update with healthy error code, or a failed response from async web request. */
                int i = 0;
                const int kMaxLoops = 50;
                for (; i < kMaxLoops; ++i)
                {
                    const int kTimePerLoopMs = 100;

                    /* check if flashing has started */
                    retval = ConfirmIfFieldUpgradeIsOccuring(ddRef, true);

                    /* leave for-loop if flashing started */
                    if (retval == Status.Ok) { break; }

                    /* wait for kTimePerLoopMs to see if firmUpdate finishes early */
                    bool bIsDone = false;
                    var respErr = CheckAsyncWebServerResp(asyncWebExchange, kTimePerLoopMs, out bIsDone);

                    /* leave for-loop if FU request completed */
                    if (bIsDone) {
                        /* Since we're done, return with response error */
                        return respErr;
                    }
                }
                SetFieldUpgradeStatus("Starting field-upgrade : " + retval, 0);
            }
            /* poll status - loop while status is ok and firmUpdate is not complete*/
            int notUpdatingCnt = 0;
            while (retval == Status.Ok)
            {
                /* request an update */
                if (retval == Status.Ok)
                {
                    retval = _WebServerScripts.HttpGet(_hostName, ddRef.model, ddRef.deviceID, ActionType.CheckUpdateProgress, out response, "", 200);
                }
                /* proces the update response */
                if (retval == Status.Ok)
                {
                    /* parse the response that has the field-upgrade status */
                    ProgressReturn jsonResp = JsonConvert.DeserializeObject<ProgressReturn>(response);
                    retval = (Status)jsonResp.GeneralReturn.Error;

                    if (retval == Status.CTRE_DI_NotUpdating)
                    {
                        ++notUpdatingCnt; /* most likely flash has finished */
                        retval = Status.Ok;
                    }
                    else
                    {
                        /* save it so calling application can poll it */
                        SetFieldUpgradeStatus(jsonResp);
                    }
                }
                /* check on the first async web exchange */
                if (retval == Status.Ok)
                {
                    bool bIsDone = false;
                    retval = CheckAsyncWebServerResp(asyncWebExchange, 100, out bIsDone);

                    /* leave while-loop if FU request completed */
                    if (bIsDone) { break; }
                }
                /* check on our state in case GUI is shutting down */
                if (GetState_NoLock() != State.ExecAction)
                {
                    retval = Status.Aborted;
                }
            }
            return retval;
        }
    }
}