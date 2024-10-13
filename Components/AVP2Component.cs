using Livesplit.AVP2;
using Livesplit.AVP2.Memory;
using Livesplit.UI.Components;
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

        public List<string> CompletedLevels = new List<string>();
        private string _currSplit = "";

        public AVP2Component(LiveSplitState state)
        {
            Settings = new AVP2Settings(state);

            if (state != null)
            {
                Model = new TimerModel() { CurrentState = state };
                Model.InitializeGameTime();
                CurrentState = state;

                Model.CurrentState.OnSplit += OnSplit;
                Model.CurrentState.OnUndoSplit += OnUndoSplit;
                Model.CurrentState.OnSkipSplit += OnSkipSplit;
                Model.CurrentState.OnReset += OnReset;
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

            Logging();

            HandleLoading();
            if (state.CurrentPhase == TimerPhase.Running && ShouldSplit())
            {
                Model.Split();
                Utility.Log("---Splitting Timer--- LN: " + AVP2Memory.LevelName + ", " + AVP2Memory.OldLevelName);
            }
            else if(state.CurrentPhase == TimerPhase.NotRunning && ShouldStart())
            {
                Utility.Log("---Starting Timer--- LN: " + AVP2Memory.LevelName + ", " + AVP2Memory.OldLevelName);
                Model.Start();
            }
        }

        public void HandleLoading()
        {
            Model.CurrentState.IsGameTimePaused = AVP2Memory.GameState == AVP2Memory.GameStates.Loading;
        }
        
        public bool ShouldStart()
        {
            if (AVP2Memory.GameState == AVP2Memory.GameStates.InGame
             && AVP2Memory.HasControl && !AVP2Memory.HadControl)
            {
                if (AVP2Memory.info.CampaignStarts.Contains(AVP2Memory.LevelName)) return true;

            }

            if (Settings.ILTimer && AVP2Memory.OldLevelName != AVP2Memory.LevelName
             && AVP2Memory.info.ILStarts.Contains(AVP2Memory.LevelName)) return true;
            
            return false;
        }

        public bool ShouldSplit()
        {
            if (AVP2Memory.OldLevelName == "" || AVP2Memory.LevelName == "")
            {
                return false;
            }

            // split on switching maps
            if (AVP2Memory.OldLevelName != AVP2Memory.LevelName
             && !CompletedLevels.Contains(AVP2Memory.info.Name + "_" + AVP2Memory.OldLevelName)
             && !CompletedLevels.Contains(AVP2Memory.info.Name + "_" + AVP2Memory.LevelName)
             && !AVP2Memory.info.Cutscenes.Contains(AVP2Memory.OldLevelName)
             && !AVP2Memory.info.CampaignStarts.Contains(AVP2Memory.LevelName))
            {
                _currSplit = AVP2Memory.info.Name + "_" + AVP2Memory.OldLevelName;
                return true;
            }

            // last level split on cutscene
            if (AVP2Memory.HadControl && !AVP2Memory.HasControl
             && !CompletedLevels.Contains(AVP2Memory.info.Name + "_" + AVP2Memory.LevelName)
             && AVP2Memory.info.CampaignEnds.Contains(AVP2Memory.LevelName)
             && AVP2Memory.Health > 0)
            {
                _currSplit = AVP2Memory.info.Name + "_" + AVP2Memory.LevelName;
                return true;
            }

            return false;
        }

        public void OnSplit(object sender, EventArgs e)
        {
            CompletedLevels.Add(_currSplit);
            _currSplit = "";
        }

        public void OnUndoSplit(object sender, EventArgs e)
        {
            CompletedLevels.RemoveAt(CompletedLevels.Count - 1);
        }

        public void OnSkipSplit(object sender, EventArgs e)
        {
            CompletedLevels.Add(_currSplit);
        }

        public void OnReset(object sender, TimerPhase e)
        {
            CompletedLevels.Clear();
        }

        public void Dispose()
        {
            Model.CurrentState.OnSplit -= OnSplit;
            Model.CurrentState.OnUndoSplit -= OnUndoSplit;
            Model.CurrentState.OnSkipSplit -= OnSkipSplit;
            Model.CurrentState.OnReset -= OnReset;
        }

        public void Logging()
        {
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

            if (AVP2Memory.Health != AVP2Memory.Health)
            {
                Utility.Log("HT: " + AVP2Memory.Health + " from " + AVP2Memory.Health);
            }
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
