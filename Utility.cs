using System.Diagnostics;
using System.Windows.Forms;

namespace Livesplit.AVP2
{
    public static class Utility
    {
        public static void Log(object str)
        {
            Trace.WriteLine("[AVP2] " + str.ToString());
        }
        
        public static void MakeError(string message="An unknown error has occured.")
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK);
        }
    }
}
