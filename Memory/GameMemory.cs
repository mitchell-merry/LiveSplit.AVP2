using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Livesplit.AVP2.Memory
{
    public class GameMemory
    {
        /// <summary>
        /// Get the modules that have been loaded by the associated process.
        /// This will get an x86 process' modules when running from x64 code,
        /// unlike Process.Modules.
        /// </summary>
        public static ProcessModuleEx[] GetProcessModules(Process p)
        {
            if (p.HasExited)
            {

            }
                //throw new ArgumentException("Process should be alive.");
            if (!Environment.Is64BitProcess)
                return ModuleToModuleEx(p);

            var ret = new List<ProcessModuleEx>();

            IntPtr[] hMods = new IntPtr[1024];

            uint uiSize = (uint)(IntPtr.Size * hMods.Length);
            uint cbNeeded;
            try
            {
                const int LIST_MODULES_ALL = 3;
                if (!SafeNativeMethods.EnumProcessModulesEx(p.Handle, hMods, uiSize, out cbNeeded, LIST_MODULES_ALL))
                {

                }
                    //throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }
            catch (EntryPointNotFoundException) // this function is only on vista and higher. this is likely only to happen on XP x64
            {
                return ModuleToModuleEx(p); // fall back
            }

            uint count = (uint)(cbNeeded / IntPtr.Size);
            for (int i = 0; i < count; i++)
            {
                var info = new SafeNativeMethods.MODULEINFO();
                var path = new StringBuilder(260);
                var module = new ProcessModuleEx();

                if (SafeNativeMethods.GetModuleFileNameEx(p.Handle, hMods[i], path, path.Capacity) > 0)
                {
                    module.FileName = path.ToString();
                    module.ModuleName = Path.GetFileName(module.FileName);
                }

                if (SafeNativeMethods.GetModuleInformation(p.Handle, hMods[i], out info, (uint)Marshal.SizeOf(info)))
                {
                    module.BaseAddress = info.lpBaseOfDll;
                    module.EntryPointAddress = info.EntryPoint;
                    module.ModuleMemorySize = (int)info.SizeOfImage;
                    ret.Add(module);
                }
            }

            return ret.ToArray();
        }

        public static ProcessModuleEx[] ModuleToModuleEx(Process p)
        {
            var ret = new List<ProcessModuleEx>();
            foreach (ProcessModule module in p.Modules)
            {
                var ex = new ProcessModuleEx
                {
                    BaseAddress = module.BaseAddress,
                    EntryPointAddress = module.EntryPointAddress,
                    FileName = module.FileName,
                    ModuleMemorySize = module.ModuleMemorySize,
                    ModuleName = module.ModuleName
                };
                ret.Add(ex);
            }
            return ret.ToArray();
        }
    }
    public class ProcessModuleEx
    {
        public IntPtr BaseAddress { get; set; }
        public IntPtr EntryPointAddress { get; set; }
        public string FileName { get; set; }
        public int ModuleMemorySize { get; set; }
        public string ModuleName { get; set; }

        public ProcessModuleEx()
        {
            this.FileName = String.Empty;
            this.ModuleName = String.Empty;
        }
    }
}
