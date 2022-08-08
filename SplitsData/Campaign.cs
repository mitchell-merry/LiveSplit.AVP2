using System.Collections.Generic;

namespace Livesplit.AVP2.SplitsData
{
    public class Campaign
    {
        public string Name { get; set; }
        public string Game { get; set; }
        public List<Mission> Missions { get; set; }
        public int LevelCount { get; set; }
    }

    public class Mission
    {
        public string Name { get; set; }
        public int Levels { get; set; }
    }
}
