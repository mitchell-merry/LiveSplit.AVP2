﻿using Livesplit.AVP2;
using Livesplit.AVP2.Memory;
using Livesplit.AVP2.UI.Components;
using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace LiveSplit.UI.Components
{
    public class AVP2Component : IComponent
    {
        public TimerModel Model { get; set; }

        public AVP2Settings Settings { get; set; }
        protected LiveSplitState CurrentState { get; set; }

        public string ComponentName => "AVP2 Autosplitter";

        public IDictionary<string, Action> ContextMenuControls => null;

        public AVP2Component(LiveSplitState state)
        {
            Settings = new AVP2Settings();

            if (state != null)
            {
                Model = new TimerModel() { CurrentState = state };
                Model.InitializeGameTime();
                CurrentState = state;
            }
        }

        public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion) { }
        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion) { }

        public Control GetSettingsControl(LayoutMode mode)
        {
            Settings.Mode = mode;
            return Settings;
        }

        public System.Xml.XmlNode GetSettings(System.Xml.XmlDocument document)
        {
            return Settings.GetSettings(document);
        }

        public void SetSettings(System.Xml.XmlNode settings)
        {
            Settings.SetSettings(settings);
        }

        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            AVP2Memory.UpdateValues();

            if (!AVP2Memory.attached) return;

            if (AVP2Memory.LevelName != AVP2Memory.OldLevelName)
            {
                Utility.Log("LN: " + AVP2Memory.LevelName + " from " + AVP2Memory.OldLevelName);
            }

            if (AVP2Memory.GameState != AVP2Memory.OldGameState)
            {
                Utility.Log("GS: " + AVP2Memory.GameState + " from " + AVP2Memory.OldGameState);
            }

            if (AVP2Memory.HasControl != AVP2Memory.HadControl)
            {
                Utility.Log("HC: " + AVP2Memory.HasControl + " from " + AVP2Memory.HadControl);
            }

            HandleLoading();
            if (state.CurrentPhase == TimerPhase.Running)
            {
                if (ShouldSplit())
                {
                    Model.Split();
                    Utility.Log("---Splitting Timer--- LN: " + AVP2Memory.LevelName + ", " + AVP2Memory.OldLevelName);
                }
            }
            else if(state.CurrentPhase == TimerPhase.NotRunning)
            {
                HandleStart();
            }
        }

        public void HandleLoading()
        {
            Model.CurrentState.IsGameTimePaused = AVP2Memory.GameState == AVP2Memory.GameStates.Loading;
        }
        
        public void HandleStart()
        {
            if (AVP2Memory.GameState == AVP2Memory.GameStates.InGame
             && AVP2Memory.HasControl && !AVP2Memory.HadControl
             && AVP2Memory.info.CampaignStarts.Contains(AVP2Memory.LevelName))
            {
                Utility.Log("---Starting Timer--- LN: " + AVP2Memory.LevelName + ", " + AVP2Memory.OldLevelName);
                Model.Start();
            }
            else
            {
                //Utility.Log("GS:  " + AVP2Memory.GameState);
                //Utility.Log("HSC: " + AVP2Memory.HasControl);
                //Utility.Log("HDC: " + AVP2Memory.HadControl);
                //Utility.Log("LN:  " + AVP2Memory.LevelName);
            }
        }

        public bool ShouldSplit()
        {
            if (AVP2Memory.OldLevelName == "" || AVP2Memory.LevelName == "")
            {
                return false;
            }

            if (AVP2Memory.OldLevelName != AVP2Memory.LevelName
             && !AVP2Memory.info.Cutscenes.Contains(AVP2Memory.OldLevelName))
            {
                return true;
            }

            if (AVP2Memory.HadControl && !AVP2Memory.HasControl
             && AVP2Memory.info.CampaignEnds.Contains(AVP2Memory.LevelName))
            {
                return true;
            }

            return false;
        }

        public void Dispose()
        {

        }

        public int GetSettingsHashCode() => Settings.GetSettingsHashCode();
        public float HorizontalWidth => 0;
        public float MinimumHeight => 0;
        public float MinimumWidth => 0;
        public float PaddingBottom => 0;
        public float PaddingLeft => 0;
        public float PaddingRight => 0;
        public float PaddingTop => 0;
        public float VerticalHeight => 0;
    }
}
