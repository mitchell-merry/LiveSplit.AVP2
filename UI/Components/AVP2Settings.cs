using System;
using System.Windows.Forms;
using System.Xml;
using LiveSplit.Model;
using LiveSplit.UI;
using System.Reflection;

namespace Livesplit.AVP2.UI.Components
{
    public partial class AVP2Settings : UserControl
    {
        public LayoutMode Mode { get; set; }
        public AVP2Settings()
        {
            InitializeComponent();
        }

        private void AVP2Settings_Load(object sender, EventArgs e)
        {

        }

        private int CreateSettingsNode(XmlDocument document, XmlElement parent)
        {
            return SettingsHelper.CreateSetting(document, parent, "Version", Assembly.GetExecutingAssembly().GetName().Version);
        }

        public XmlNode GetSettings(XmlDocument document)
        {
            var parent = document.CreateElement("Settings");
            CreateSettingsNode(document, parent);
            return parent;
        }

        public int GetSettingsHashCode()
        {
            return CreateSettingsNode(null, null);
        }

        public void SetSettings(XmlNode node)
        {
            var element = (XmlElement)node;
        }
    }
}
