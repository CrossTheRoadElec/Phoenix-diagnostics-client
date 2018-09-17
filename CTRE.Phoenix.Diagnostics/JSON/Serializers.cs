#pragma warning disable 0649
namespace CTRE.Phoenix.Diagnostics.JSON.Serializers
{
    /* Primitives must be nullable in case a value is missing in serialized string */
    public class VersionReturn
    {
        public JsonStatus GeneralReturn;
        public string Version;
    }
    public class JsonStatus
    {
        public string Action;
        public string Device;
        public int? Error;
        public string ErrorMessage;
        public string ID;
    }
    public class JsonDevice
    {
        public string BootloaderRev;
        public string CurrentVers;
        public int? DynID;
        public string HardwareRev;
        public long? ID;
        public string ManDate;
        public string Model;
        public string Name;
        public string SoftStatus;
        public int? UniqID;
    }
    class EmptyReturn
    {
        public JsonStatus GeneralReturn;
    }
    class BlinkReturn
    {
        public JsonStatus GeneralReturn;
    }
    class IDReturn
    {
        public JsonStatus GeneralReturn;
        public int? NewID;
    }
    class NameReturn
    {
        public JsonStatus GeneralReturn;
        public string NewName;
    }
    class ProgressReturn
    {
        public JsonStatus GeneralReturn;
        public int? progress;
    }
    class SelfTestReturn
    {
        public JsonStatus GeneralReturn;
        public string SelfTest;
    }
    class GetDevicesReturn
    {
        public JsonStatus GeneralReturn;
        public JsonDevice[] DeviceArray;
    }
    public class GetConfigsReturn
    {
        public JsonStatus GeneralReturn;
        public DeviceConfigs Device;
    }
    class SetConfigReturn
    {
        public JsonStatus GeneralReturn;
        public DeviceConfigs Device;
    }
    class FirmwareUpdateReturn
    {
        public JsonStatus GeneralReturn;
        public string UpdateMessage;
        public string Path;
        public string Size;
    }
    //Config JSON's////
    public class ConfigGroup
    {
        public string Name;
        public string Type;
        public string Description;
        public int? Ordinal;

        public object Values;
    }
    public class DeviceConfigs
    {
        public ConfigGroup[] Configs;
    }
}
#pragma warning restore 0649
