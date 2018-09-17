namespace CTRE.Phoenix.Diagnostics
{
    /// <summary>
    /// Long term we will not need to parse model as client should simply bounce back string-value that server provided.
    /// </summary>
    public enum Model
    {
        None,
        PCM,
        PDP,
        TalonSRX,
        VictorSPX,
        PigeonIMU,
        PigeonIMURibbon,
        CANifier,
        Unknown,
    }
    public class ModelUtility
    {
        public static Model Parse(string modelString, long fullID)
        {
            if ((fullID & 0xFFFFFF00) == 0x204f400)
            {
                return Model.PigeonIMURibbon;
            }

            if (modelString.Length == 0)
            {
                return Model.None;
            }

            switch (modelString[0])
            {
                case 'T': return Model.TalonSRX;
                case 'V': return Model.VictorSPX;
                case 'C': return Model.CANifier;
                case 'P':
                    if (modelString.StartsWith("PCM"))
                        return Model.PCM;
                    if (modelString.StartsWith("PDP"))
                        return Model.PDP;
                    if (modelString.StartsWith("Pigeon"))
                        return Model.PigeonIMU;
                    break;
            }

            return Model.Unknown; // new/future device 
        }
    }
}