using CTRE.Phoenix.Diagnostics.JSON.Serializers;

namespace CTRE.Phoenix.Diagnostics.JSON
{
    public static class Adapter
    {
        /// <summary>
        /// Convert jd into the strongly-typed device descriptor type.
        /// </summary>
        /// <param name="jd"></param>
        /// <returns></returns>
        public static DeviceDescrip Convert(JsonDevice jd)
        {
            DeviceDescrip dd = new DeviceDescrip();
            dd.jsonStrings = jd;
            dd.deviceID = (byte)(jd.ID & 0x3F);
            dd.model = ModelUtility.Parse(jd.Model, jd.ID.Value);
            return dd; ;
        }
    }
}
