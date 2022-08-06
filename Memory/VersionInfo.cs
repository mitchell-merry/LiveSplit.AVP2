using System;
using System.Collections.Generic;

namespace Livesplit.AVP2.Memory
{
    public class PointerPath
    {
        public int Base { get; set; }
        public int[] Offsets { get; set; }
}

    public class AVP2Version
    {
        public PointerPath GameState { get; set; }
        public PointerPath LevelName { get; set; }
        public PointerPath HasControl { get; set; }
        public Dictionary<byte, AVP2Memory.GameStates> GameStates { get; set; }
    }
    static class VersionInfo
    {
        /// Information about each version, indexed by it's MMS (module memory size)
        public static Dictionary<int, AVP2Version> info = new Dictionary<int, AVP2Version>
        {
            // AVP2
            { 0xEF000, new AVP2Version {
                    GameState = new PointerPath { Base = 0x5627C, Offsets = new int[] { } },
                    LevelName = new PointerPath { Base = 0x2FD9B4, Offsets = new int[] { } },
                    HasControl = new PointerPath { Base = 0x1C9A64, Offsets = new int[] { 0xD2C, 0x0 } },
                    GameStates = new Dictionary<byte, AVP2Memory.GameStates> {
                        { 0x0,  AVP2Memory.GameStates.GameNotLoaded },
                        { 0x88, AVP2Memory.GameStates.InGame },
                        { 0xA0, AVP2Memory.GameStates.PauseMenu },
                        { 0xC8, AVP2Memory.GameStates.MainMenu },
                        { 0x98, AVP2Memory.GameStates.Loading },
                    }
                }
            },
            // AVP2: PH
            { 0x100000, new AVP2Version {
                    GameState = new PointerPath { Base = 0x5C14C, Offsets = new int[] { } },
                    LevelName = new PointerPath { Base = 0x316155, Offsets = new int[] { } },
                    // TODO
                    HasControl = new PointerPath { Base = 0x1C9A64, Offsets = new int[] { 0xD2C, 0x0 } },
                    GameStates = new Dictionary<byte, AVP2Memory.GameStates> {
                        { 0x0,  AVP2Memory.GameStates.GameNotLoaded },
                        { 0x10, AVP2Memory.GameStates.InGame },
                        { 0x64, AVP2Memory.GameStates.PauseMenu },
                        { 0xB8, AVP2Memory.GameStates.MainMenu },
                        { 0x94, AVP2Memory.GameStates.Loading },
                    }
                }
            }
        };
    }
}
