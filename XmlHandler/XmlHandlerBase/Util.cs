using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace XmlHandler.UnitTests
{
    public enum Software
    {
        Notepad,
        SublimeText3
    }

    public static class Util
    {
        private const string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        public static bool IsAppInstalled(string app)
        {
            using (var rk = Registry.LocalMachine.OpenSubKey(uninstallKey))
            {
                var apps = rk.GetSubKeyNames().Select(i => rk.OpenSubKey(i).GetValue("DisplayName")).FirstOrDefault(i => i!=null && i.ToString().Contains(app));
                return apps != null;
            }
        }

        public static void StartEditApplication(string path, int line, Software software)
        {
            if (software == Software.Notepad)
            {
                if (!IsAppInstalled("Notepad++")) return;
                var nppDir = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Notepad++", null, null);
                var nppExePath = Path.Combine(nppDir, "Notepad++.exe");
                var nppReadmePath = Path.Combine(nppDir, path);
                var sb = new StringBuilder();
                sb.AppendFormat("\"{0}\" -n{1}", nppReadmePath, line);
                Process.Start(nppExePath, sb.ToString());
            }
            else if (software == Software.SublimeText3)
            {
                var sublimePath = "C:\\Program Files\\Sublime Text 3\\subl.exe";
                var sb = new StringBuilder();
                sb.AppendFormat("\"{0}\":{1}", path, line);
                Process.Start(sublimePath, sb.ToString());
            }
        }
    }
}
