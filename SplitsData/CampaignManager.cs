using System.Xml.Linq;
using System.Collections.Generic;

namespace Livesplit.AVP2.SplitsData
{
    public static class CampaignManager
    {
        private const string CAMPAIGNS_XML = @"Components/AVP2.Campaigns.xml";
        public static Dictionary<string, string> GameToLabel = new Dictionary<string, string>()
        {
            { "avp2", "Aliens versus Predator 2" },
            { "avp2ph", "Aliens versus Predator 2: Primal Hunt" }
        };
        
        public static List<Campaign> GetCampaigns()
        {
            var ret = new List<Campaign>();

            var xml = XDocument.Load(CAMPAIGNS_XML);
            foreach (var campaignData in xml.Element("Campaigns").Elements("Campaign"))
            {
                var campaign = new Campaign() 
                {
                    Name     = campaignData.Attribute("Name").Value,
                    Game     = campaignData.Attribute("Game").Value,
                    Missions = new List<Mission>(),
                    LevelCount = 0,
                };

                foreach (var missionData in campaignData.Elements("Mission"))
                {
                    int Levels = int.Parse(missionData.Attribute("Levels").Value);
                    campaign.Missions.Add(new Mission()
                    {
                        Name   = missionData.Attribute("Name").Value,
                        Levels = Levels,
                    });

                    campaign.LevelCount += Levels;
                }

                ret.Add(campaign);
            }

            return ret;
        }

        public static Campaign GetCampaign(List<Campaign> campaigns, string Game, string Name)
        {
            foreach (var campaign in campaigns)
            {
                if (campaign.Game.ToLower() == Game.ToLower()
                 && campaign.Name.ToLower() == Name.ToLower())
                {
                    return campaign;
                }
            }

            return null;
        }
    }
}
