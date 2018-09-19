
namespace CTRE.Phoenix.Diagnostics {

    public class DeviceDescrip
    {
        public JSON.Serializers.JsonDevice jsonStrings;
        public JSON.Serializers.GetConfigsReturn configCache = null;
        public byte deviceID;
        public string model;
        public System.DateTime updateTimestamp;

        public DeviceDescrip(string model, byte deviceID)
        {
            this.model = model;
            this.deviceID = deviceID;
        }
        public DeviceDescrip()
        {
            // MT
        }

        public override string ToString()
        {
            return jsonStrings.Name;
        }
        public uint TimeSinceUpdateMs()
        {
            System.TimeSpan diff = System.DateTime.UtcNow - updateTimestamp;
            return (uint)diff.TotalMilliseconds;
        }
        public string ToKeyValue()
        {
            return model + deviceID;
        }

        public class KeyValue
        {
            /// <summary>
            /// Produce a sortable value for a give model/deviceID pair.
            /// </summary>
            /// <param name="model"></param>
            /// <param name="id"></param>
            /// <returns>sortable value</returns>
            public static string ToKeyValue(string model, byte id)
            {
                return model + id;
            }
        }
    }
}
