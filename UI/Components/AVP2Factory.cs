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

        // Fill in this empty string with the URL of the repository where your component is hosted.
        // This should be the raw content version of the repository. If you're not uploading this
        // to GitHub or somewhere, you can ignore this.
        public string UpdateURL => "";

        // Fill in this empty string with the path of the XML file containing update information.
        // Check other LiveSplit components for examples of this. If you're not uploading this to
        // GitHub or somewhere, you can ignore this.
        public string XMLURL => UpdateURL + "UI/Components/Updates.xml";

        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
    }
}
