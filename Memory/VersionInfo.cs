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
        public string Name { get; set; }
        public PointerPath GameState { get; set; }
        public PointerPath LevelName { get; set; }
        public PointerPath HasControl { get; set; }
        public PointerPath Health { get; set; }
        public Dictionary<byte, AVP2Memory.GameStates> GameStates { get; set; }
        public string[] CampaignStarts { get; set; }
        public string[] CampaignEnds { get; set; }
        public string[] ILStarts { get; set; }
        /// <summary>
        /// Scenes to split on enter, but not on exit.
        /// </summary>
        public string[] Cutscenes { get; set; }

    }
    static class VersionInfo
    {
        /// Information about each version, indexed by it's MMS (module memory size)
        public static Dictionary<int, AVP2Version> info = new Dictionary<int, AVP2Version>
        {
            // AVP2
            { 0xEF000, new AVP2Version {
                    Name = "avp2",
                    GameState  = new PointerPath { Base = 0x5627C,  Offsets = new int[] { } },
                    LevelName  = new PointerPath { Base = 0x2FD9B4, Offsets = new int[] { } },
                    HasControl = new PointerPath { Base = 0x1C9A64, Offsets = new int[] { 0xD2C, 0x0 } },
                    Health     = new PointerPath { Base = 0x1C5868, Offsets = new int[] { 0x788 } },
                    GameStates = new Dictionary<byte, AVP2Memory.GameStates> {
                        { 0x0,  AVP2Memory.GameStates.GameNotLoaded },
                        { 0x88, AVP2Memory.GameStates.InGame },
                        { 0xA0, AVP2Memory.GameStates.PauseMenu },
                        { 0xC8, AVP2Memory.GameStates.MainMenu },
                        { 0x98, AVP2Memory.GameStates.Loading },
                    },
                    CampaignStarts = new[] { "M1S1", "P1S1", "A1S1" },
                    CampaignEnds   = new[] { "M7S2", "P7S2", "A7S3" },
                    ILStarts       = new[] { "M1S1", "M2S1", "M3S1", "M4S1", "M5S1", "M6S1", "M7S1", 
                                             "P1S1", "P2S1", "P3S1", "P4S1", "P5S1", "P6S1", "P7S1",
                                             "A1S1", "A2S1", "A3S1", "A4S1", "A5S1", "A6S1", "A7S1" },
                    Cutscenes      = new[] { "M_CLOSE", "M_OPEN", "M3_OPEN", "M4_OPEN",
                                             "P_OPEN", "A_OPEN", "A4_OPEN", "OUTRO" },
                }
            },
            // AVP2: PH
            { 0x100000, new AVP2Version {
                    Name = "avp2ph",
                    GameState = new PointerPath  { Base = 0x5C14C,  Offsets = new int[] { } },
                    LevelName = new PointerPath  { Base = 0x316155, Offsets = new int[] { } },
                    HasControl = new PointerPath { Base = 0x1F4BDC, Offsets = new int[] { 0x4, 0x1FC } },
                    Health     = new PointerPath { Base = 0x1F4A40, Offsets = new int[] { 0x788 } },
                    GameStates = new Dictionary<byte, AVP2Memory.GameStates> {
                        { 0x0,  AVP2Memory.GameStates.GameNotLoaded },
                        { 0x10, AVP2Memory.GameStates.InGame },
                        { 0x64, AVP2Memory.GameStates.PauseMenu },
                        { 0xB8, AVP2Memory.GameStates.MainMenu },
                        { 0x94, AVP2Memory.GameStates.Loading },
                    },
                    CampaignStarts = new[] { "H1S2", "P1S1", "A1S1" },
                    CampaignEnds   = new[] { "H3S2", "P3S3", "A3S3" },
                    ILStarts       = new[] { "H1S1", "H2S1", "H3S1",
                                             "P1S1", "P2S1", "P3S1",
                                             "A1S1", "A2S1", "A3S1" },
                    Cutscenes      = new[] { "OUTRO" },
                }
            }
        };
    }
}
