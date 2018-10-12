using System;
using System.Text;

namespace CTRE.Phoenix.dotNET.Form
{
    public static class FormRoutines
    {
        public static string GetAppVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            return version;
        }
        public static string MakeVersionString(string kAppName)
        {
            string appver = GetAppVersion();
            StringBuilder sb = new StringBuilder();
            /* assembly (app) version */
            if (appver.Length > 0)
            {
                sb.Append("Version (");
                sb.Append(appver);
                sb.Append(") ");
            }
            return kAppName + " " + sb.ToString();
        }
        public static bool ClipboardSet(string toCopy)
        {
            try
            {
                System.Windows.Clipboard.SetText(toCopy, System.Windows.TextDataFormat.Text);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }
        public static bool ClipboardSet(object objToCopy)
        {
            try
            {
                System.Windows.Clipboard.SetDataObject(objToCopy, true);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }
    }
}
