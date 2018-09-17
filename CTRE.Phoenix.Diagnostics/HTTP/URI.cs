using CTRE.Phoenix.Diagnostics.BackEnd;
using System.Collections.Generic;
namespace CTRE.Phoenix.Diagnostics.HTTP
{
    public static class URI
    {
        public static readonly Dictionary<Model, string> DeviceMap = new Dictionary<Model, string>()
        {
            { Model.None, "" },
            { Model.Unknown, "" },
            { Model.TalonSRX, "srx" },
            { Model.VictorSPX, "spx" },
            { Model.PigeonIMU, "pigeon" },
            { Model.PigeonIMURibbon, "ribbonPigeon" },
            { Model.CANifier, "canif" },
            { Model.PCM, "pcm" },
            { Model.PDP, "pdp" },
        };
   
        public static readonly Dictionary<string, Model> DeviceStringMap = new Dictionary<string, Model>()
        {
            { "", Model.None},
            { "Talon SRX", Model.TalonSRX },
            { "Victor SPX", Model.VictorSPX },
            { "Pigeon", Model.PigeonIMU },
            { "Pigeon Over Ribbon", Model.PigeonIMURibbon },
            { "CANifier", Model.CANifier },
            { "PCM", Model.PCM },
            { "PDP", Model.PDP },
        };

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
