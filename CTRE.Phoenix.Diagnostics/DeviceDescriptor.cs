
namespace CTRE.Phoenix.Diagnostics {

    public class DeviceDescrip
    {
        public JSON.Serializers.JsonDevice jsonStrings;
        public JSON.Serializers.GetConfigsReturn configCache = null;
        public byte deviceID;
        public Model model;
        public System.DateTime updateTimestamp;

        public DeviceDescrip(Model model, byte deviceID)
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
        public uint ToKeyValue()
        {
            uint retval;
            retval = (uint)model;
            retval <<= 8;
            retval |= (uint)deviceID;
            return retval;
        }

        public class KeyValue
        {
            /// <summary>
            /// Produce a sortable value for a give model/deviceID pair.
            /// </summary>
            /// <param name="model"></param>
            /// <param name="id"></param>
            /// <returns>sortable value</returns>
            public static uint ToKeyValue(Model model, byte id)
            {
                uint retval;
                retval = (uint)model;
                retval <<= 8;
                retval |= (uint)id; 
                return retval;
            }
        }
    }
}
