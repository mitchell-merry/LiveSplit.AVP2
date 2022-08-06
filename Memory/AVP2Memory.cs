using System;
using System.Linq;
using System.Diagnostics;

namespace Livesplit.AVP2.Memory
{
    public static class AVP2Memory
    {
        public const string PROCESS_NAME = "lithtech";

        public enum GameStates { GameNotLoaded, InGame, PauseMenu, MainMenu, Loading }

        // Values we need, refreshed on every UpdateValues() call.
        public static GameStates GameState { get; set; }
        public static string LevelName { get; set; }
        public static bool HasControl { get; set; }

        public static bool attached = false;

        private static Process _process;
        private static ProcessMemory _pm;
        private static AVP2Version attachedVersion = null;

        public static void UpdateValues()
        {
            if(!attached)
            {
                var tempProc = Process.GetProcesses().FirstOrDefault(x => PROCESS_NAME.Contains(x.ProcessName.ToLower()));

                if (tempProc != null && !tempProc.HasExited)
                {
                    _process = tempProc;
                    _pm = new ProcessMemory(_process, (IntPtr)_process.Id);

                    attached = true;
                    attachedVersion = VersionInfo.info[_process.MainModule.ModuleMemorySize];

                    if (attachedVersion == null)
                    {
                        Trace.WriteLine("Unknown version: " + _process.MainModule.ModuleMemorySize.ToString("X"));
                        return;
                    }
                }
                return;
            }

            if (_pm.getBaseAddress == 0)
            {
                attached = false;
                attachedVersion = null;
                Trace.WriteLine("GBA is 0");
                return;
            }

            try
            {
                
                var d3d = GameMemory.GetProcessModules(_process).FirstOrDefault(x => x.ModuleName.ToLower() == "d3d.ren");
                var objectlto = GameMemory.GetProcessModules(_process).FirstOrDefault(x => x.ModuleName.ToLower() == "object.lto");
                var cshell = GameMemory.GetProcessModules(_process).FirstOrDefault(x => x.ModuleName.ToLower() == "cshell.dll");

                if (d3d != null)
                {
                    UpdateGameState(d3d);
                }
                
                if (objectlto != null)
                {
                    UpdateLevelName(objectlto);
                }

                if (cshell != null)
                {
                    UpdateHasControl(cshell);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }

        private static void UpdateGameState(ProcessModuleEx d3d)
        {
            var val = _pm.TraverseByte(d3d.BaseAddress + attachedVersion.GameState.Base, attachedVersion.GameState.Offsets) ?? 0;
            GameState = attachedVersion.GameStates[val];
        }

        private static void UpdateLevelName(ProcessModuleEx objectlto)
        {
            LevelName = _pm.TraverseStringASCII(objectlto.BaseAddress + attachedVersion.LevelName.Base, attachedVersion.LevelName.Offsets, 32);
        }

        private static void UpdateHasControl(ProcessModuleEx cshell)
        {
            HasControl = _pm.TraverseBoolean(cshell.BaseAddress + attachedVersion.HasControl.Base, attachedVersion.HasControl.Offsets) ?? false;
        }
    }
}
