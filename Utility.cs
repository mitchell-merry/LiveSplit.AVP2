using System.Diagnostics;

namespace Livesplit.AVP2
{
    public static class Utility
    {
        public static void Log(object str)
        {
            Trace.WriteLine("[AVP2] " + str.ToString());
        }
    }
}
