using LiveSplit.Model;
using System;
using System.Reflection;

namespace LiveSplit.UI.Components
{
    public class AVP2Factory : IComponentFactory
    {
        public string ComponentName => "AVP2 Autosplitter v" + Version.ToString();
        public string Description => "Controls the timer for Aliens versus Predator 2 and Aliens versus Predator 2: Primal Hunt speedruns.";
        public ComponentCategory Category => ComponentCategory.Control;
        public IComponent Create(LiveSplitState state) => new AVP2Component(state);
        public string UpdateName => ComponentName;
        public string UpdateURL => "https://raw.githubusercontent.com/mitchell-merry/LiveSplit.AVP2/main/";
        public string XMLURL => UpdateURL + "UI/Components/Updates.xml";
        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
    }
}
