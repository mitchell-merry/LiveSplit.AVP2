using System;
using System.Windows.Forms;
using System.Xml;
using LiveSplit.Model;
using LiveSplit.UI;
using System.Reflection;
using System.Collections.Generic;
using Livesplit.AVP2.SplitsData;
using System.Linq;

namespace Livesplit.AVP2.Components
{
    public partial class AVP2Settings : UserControl
    {
        public LayoutMode Mode { get; set; }
        public LiveSplitState State { get; set; }
        private List<Campaign> Campaigns { get; set; }

        public AVP2Settings(LiveSplitState state)
        {
            InitializeComponent();

            Campaigns = CampaignManager.GetCampaigns();
            State = state;
        }

        private void AVP2Settings_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
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

        private void AddSplits_Click(object sender, EventArgs e)
        {
            var a = ((Button)sender).Name.Split('_');
            var Game = a[0];
            var Name = a[1];

            Utility.Log("campaign: " + Game + " - " + Name);

            var campaign = CampaignManager.GetCampaign(Campaigns, Game, Name);
            
            // show that there's been an error and exit
            if (campaign == null)
            {
                Utility.MakeError("Invalid game / campaign: " + Game + " - " + Name + ".");
                return;
            }

            using (var asd = new AddSplitsDialog(campaign.Game, campaign.Name, campaign.LevelCount))
            {
                DialogResult res = asd.ShowDialog();

                if (res != DialogResult.OK || asd.Action == AddSplitsOption.Cancel || asd.Action == AddSplitsOption.Pending) return;

                //var splits = new List<Segment>();
                int countIndex = 0;

                foreach (var mission in campaign.Missions)
                {
                    for (int i = 1; i <= mission.Levels; i++)
                    {
                        countIndex++;
                        var prefix = "";

                        if (asd.Action == AddSplitsOption.AddSubsplits)
                        {
                            prefix = countIndex == campaign.LevelCount ? "{" + campaign.Name + "}" : "-";
                        }

                        var suffix = mission.Levels == 1 ? "" : " " + i;

                        State.Run.AddSegment(prefix + mission.Name + suffix);
                    }
                }

                if (State.Run[0].Name == "") State.Run.Remove(State.Run[0]);

                State.CallRunManuallyModified();
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void avp2_Marine_Click(object sender, EventArgs e) { AddSplits_Click(sender, e); }
        private void avp2_Predator_Click(object sender, EventArgs e) { AddSplits_Click(sender, e); }
        private void avp2_Alien_Click(object sender, EventArgs e) { AddSplits_Click(sender, e); }
        private void avp2ph_Corporate_Click(object sender, EventArgs e) { AddSplits_Click(sender, e); }
        private void avp2ph_Predator_Click(object sender, EventArgs e) { AddSplits_Click(sender, e); }
        private void avp2ph_Predalien_Click(object sender, EventArgs e) { AddSplits_Click(sender, e); }
    }
}