using System;

namespace CTRE.Phoenix.dotNET
{
    public static class Debug
    {
        private static long _bootTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        public static void Assert(bool cond)
        {
            /* comment/uncomment as the need arises.  Assert is harmless when built in RELEASE */
            // System.Diagnostics.Debug.Assert(cond);
            if (!cond) {
                Print("Assert","Assert failed");
            }
        }
        public static void Print(string tag, string message)
        {
            long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            milliseconds -= _bootTime;

            System.Diagnostics.Debug.Print("(" + milliseconds + ")\t" + "[" + tag + "]" + message);
        }
        public static void Nop()
        {
            /* do nothing, caller just wants a spot to land a brkpt without creating a warning */
        }
    }
}
