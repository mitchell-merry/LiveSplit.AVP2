using Livesplit.AVP2.Memory;
using Livesplit.AVP2.UI.Components;
using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace LiveSplit.UI.Components
{
    public class AVP2Component : IComponent
    {
        public AVP2Settings Settings { get; set; }
        protected LiveSplitState CurrentState { get; set; }

        public string ComponentName => "AVP2 Autosplitter";

        public IDictionary<string, Action> ContextMenuControls => null;

        public AVP2Component(LiveSplitState state)
        {
            Settings = new AVP2Settings();
            CurrentState = state;
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
