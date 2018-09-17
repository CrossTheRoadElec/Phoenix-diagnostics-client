namespace CTRE.Phoenix.Diagnostics.BackEnd
{
    /// <summary>
    /// Types of actions that the BackEnd can execute.  Many of these correspond to URIs sent to the diagnostic server.
    /// </summary>
    public enum ActionType
    {
        None,
        GetVersion,
        GetDeviceList,
        Blink,
        SetID,
        SetDeviceName,
        SelfTest,
        FieldUpgradeDevice,
        CheckUpdateProgress,
        GetConfig,
        SetConfig,
        //Robot Controller 
        InstallDiagServerToRobotController,
        UninstallDiagServerToRobotController,
        StartServer,
        StopServer,
        //Unit Testing Actions
        CheckProcess,
        GetNumOfDevices,
        RebootRio,
    };
}
