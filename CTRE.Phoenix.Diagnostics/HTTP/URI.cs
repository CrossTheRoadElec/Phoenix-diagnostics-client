using CTRE.Phoenix.Diagnostics.BackEnd;
using System.Collections.Generic;
namespace CTRE.Phoenix.Diagnostics.HTTP
{
    public static class URI
    {
        public static readonly Dictionary<ActionType, string> ActionMap = new Dictionary<ActionType, string>()
        {
            { ActionType.None, "" },
            { ActionType.GetVersion, "action=getversion" },
            { ActionType.GetDeviceList, "action=getdevices"},
            { ActionType.Blink, "action=blink" },
            { ActionType.SetID, "action=setid" },
            { ActionType.SetDeviceName, "action=setname" },
            { ActionType.SelfTest, "action=selftest" },
            { ActionType.FieldUpgradeDevice, "action=fieldupgrade" },
            { ActionType.CheckUpdateProgress, "action=progress" },
            { ActionType.GetConfig, "action=getconfig" },
            { ActionType.SetConfig, "action=setconfig" },
        };
    }
}
