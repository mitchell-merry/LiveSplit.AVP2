using System;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO;

namespace Livesplit.AVP2.Memory
{
    public static class ModuleInfo
    {
        public static Dictionary<string, IntPtr> GetModuleBases(Process p)
        {
            if (p.HasExited) return null;

            if(!Environment.Is64BitProcess) return GetModuleBases32(p);

            var ret = new Dictionary<string, IntPtr>();

            // Get all module handles (LIST_MODULES_ALL, 32 and 64 bit)
            // https://docs.microsoft.com/en-us/windows/win32/api/psapi/nf-psapi-enumprocessmodulesex
            IntPtr[] lphModule = new IntPtr[1024];
            uint cb = (uint)(IntPtr.Size * lphModule.Length);
            const int LIST_MODULES_ALL = 0x03;
            EnumProcessModulesEx(p.Handle, lphModule, cb, out uint lpcbNeeded, LIST_MODULES_ALL);

            // "To determine if the lphModule array is too small to hold all module handles for the process,
            // compare the value returned in lpcbNeeded with the value specified in cb. If lpcbNeeded is
            // greater than cb, increase the size of the array and call EnumProcessModulesEx again."
            while (lpcbNeeded > cb)
            {
                lphModule = new IntPtr[lpcbNeeded / IntPtr.Size];
                EnumProcessModulesEx(p.Handle, lphModule, lpcbNeeded, out lpcbNeeded, LIST_MODULES_ALL);
            }

            // buffers for info
            // https://docs.microsoft.com/en-us/windows/win32/fileio/maximum-file-path-limitation?tabs=registry
            var filepath = new StringBuilder(260);
            var info = new MODULEINFO();
            for (int i = 0; i < lpcbNeeded / IntPtr.Size; i++)
            {
                var moduleHandle = lphModule[i];

                // https://docs.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-getmodulefilenamea
                if (GetModuleFileNameEx(p.Handle, moduleHandle, filepath, filepath.Capacity) <= 0) continue;
                // https://docs.microsoft.com/en-us/windows/win32/api/psapi/nf-psapi-getmoduleinformation
                if (!GetModuleInformation(p.Handle, moduleHandle, out info, (uint) Marshal.SizeOf(info))) continue;
                
                var name = Path.GetFileName(filepath.ToString());
                var baseAddr = info.lpBaseOfDll;

                if (name == "" || baseAddr == IntPtr.Zero) continue;
                
                ret[name] = baseAddr;
            }

            return ret;
        }

        public static Dictionary<string, IntPtr> GetModuleBases32(Process p)
        {
            var ret = new Dictionary<string, IntPtr>();
            foreach (ProcessModule module in p.Modules)
            {
                ret[module.ModuleName.ToLower()] = module.BaseAddress;
            }
            return ret;
        }

        // Win32 API calls to get information about the process modules
        [DllImport("psapi.dll", SetLastError = true)]
        public static extern bool EnumProcessModulesEx(
            IntPtr hProcess,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.FunctionPtr)][In][Out] IntPtr[] lphModule,
            uint cb,
            [MarshalAs(UnmanagedType.U4)] out uint lpcbNeeded,
            uint dwFiltrFlag);

        [StructLayout(LayoutKind.Sequential)]
        public struct MODULEINFO
        {
            public IntPtr lpBaseOfDll;
            public uint SizeOfImage;
            public IntPtr EntryPoint;
        }

        [DllImport("psapi.dll", SetLastError = true)]
        public static extern bool GetModuleInformation(IntPtr hProcess, IntPtr hModule, out MODULEINFO lpmodinfo, uint cb);

        [DllImport("psapi.dll", SetLastError = true)]
        public static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [In][Out] StringBuilder lpBaseName, int nSize);
    }
}
