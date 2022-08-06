using System;
using System.Linq;
using System.Diagnostics;
using LiveSplit.AVP2.Memory;

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
        public static bool HadControl { get; set; }

        public static bool attached = false;
        public static AVP2Version info = null;

        private static Process _process;
        private static ProcessMemory _pm;

        public static void UpdateValues()
        {
            if (!attached)
            {
                var tempProc = Process.GetProcesses().FirstOrDefault(x => PROCESS_NAME.Contains(x.ProcessName.ToLower()));

                if (tempProc != null && !tempProc.HasExited)
                {
                    _process = tempProc;
                    _pm = new ProcessMemory(_process, (IntPtr)_process.Id);

                    attached = true;
                    info = VersionInfo.info[_process.MainModule.ModuleMemorySize];

                    if (info == null)
                    {
                        Utility.Log("Unknown version: " + _process.MainModule.ModuleMemorySize.ToString("X"));
                        return;
                    }
                }
                return;
            }

            if (_pm.getBaseAddress == 0)
            {
                attached = false;
                info = null;
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
            var val = _pm.TraverseByte(d3d.BaseAddress + info.GameState.Base, info.GameState.Offsets) ?? 0;
            GameState = info.GameStates[val];
        }

        private static void UpdateLevelName(ProcessModuleEx objectlto)
        {
            LevelName = _pm.TraverseStringASCII(objectlto.BaseAddress + info.LevelName.Base, info.LevelName.Offsets, 32);
        }

        private static void UpdateHasControl(ProcessModuleEx cshell)
        {
            HadControl = HasControl;
            HasControl = _pm.TraverseBoolean(cshell.BaseAddress + info.HasControl.Base, info.HasControl.Offsets) ?? false;
        }
    }
}
