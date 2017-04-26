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

        public static void StartNotepadPlusPlusApplication(string path, int line)
        {
            if (!IsAppInstalled("Notepad++")) return;
            var nppDir = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Notepad++", null, null);
            var nppExePath = Path.Combine(nppDir, "Notepad++.exe");
            var nppReadmePath = Path.Combine(nppDir, path);
            var sb = new StringBuilder();
            sb.AppendFormat("\"{0}\" -n{1}", nppReadmePath, line);
            Process.Start(nppExePath, sb.ToString());
        }
    }
}
