﻿using System;
using System.Linq;
using System.Diagnostics;
using LiveSplit.AVP2.Memory;
using LiveSplit.ComponentUtil;
using System.Text;

namespace Livesplit.AVP2.Memory
{
    public static class AVP2Memory
    {
        public const string PROCESS_NAME = "lithtech";

        public enum GameStates { GameNotLoaded, InGame, PauseMenu, MainMenu, Loading }

        // Values we need, refreshed on every UpdateValues() call.
        public static GameStates GameState { get; set; }
        public static GameStates OldGameState { get; set; }
        public static string LevelName { get; set; }
        public static string OldLevelName { get; set; }
        public static bool HasControl { get; set; }
        public static bool HadControl { get; set; }
        public static int Health { get; set; }

        public static bool attached = false;
        public static AVP2Version info = null;

        private static Process _process;
        private static ProcessMemory _pm;

        private static IntPtr object_lto;

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
                UpdateGameState();
                UpdateLevelName();
                UpdateHasControl();
                UpdateHealth();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }

        private static void UpdateGameState()
        {
            OldGameState = GameState;
            
            var val = new DeepPointer("d3d.ren", info.GameState.Base, info.GameState.Offsets).Deref<byte>(_process);
            //var val = _pm.TraverseByte(d3d.BaseAddress + info.GameState.Base, info.GameState.Offsets) ?? 0;

            GameState = info.GameStates[val];
        }

        private static void UpdateLevelName()
        {
            OldLevelName = LevelName;

            // Refind the objectlto base address - gets unloaded on returning to main menu
            if (OldGameState == GameStates.Loading && GameState == GameStates.InGame)
            {
                foreach (var m in GameMemory.GetProcessModules(_process))
                {
                    if (m.ModuleName.ToLower() == "object.lto")
                    {
                        Utility.Log("old: " + object_lto + ", new: " + m.BaseAddress);
                        object_lto = m.BaseAddress;
                        break;
                    }
                }
            }

            if (GameState == GameStates.MainMenu || object_lto == IntPtr.Zero) LevelName = "NONE";

            var sb = new StringBuilder();
            new DeepPointer(object_lto + info.LevelName.Base, info.LevelName.Offsets)
                 .DerefString(_process, ReadStringType.ASCII, sb);
            LevelName = sb.ToString() ?? "NONE";
            //LevelName = _pm.TraverseStringASCII(objectlto.BaseAddress + info.LevelName.Base, info.LevelName.Offsets, 32);
        }

        private static void UpdateHasControl()
        {
            HadControl = HasControl;
            HasControl = new DeepPointer("cshell.dll", info.HasControl.Base, info.HasControl.Offsets).Deref<bool>(_process, false);
            //HasControl = _pm.TraverseBoolean(cshell.BaseAddress + info.HasControl.Base, info.HasControl.Offsets) ?? false;
        }

        private static void UpdateHealth()
        {
            Health = new DeepPointer("cshell.dll", info.Health.Base, info.Health.Offsets).Deref<int>(_process);
        }
    }
}
