using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunicArchipelago {

    public struct VanillaLocation {
        public string LocationId;
        public string Position;
        public List<Dictionary<string, int>> RequiredItems;
        public List<Dictionary<string, int>> RequiredItemsDoors;
        public int SceneId;
        public string SceneName;
    }
    public struct VanillaReward {
        public int Amount;
        public string Name;
        public string Type;
    }
    public class VanillaCheck {
        public VanillaLocation Location;
        public VanillaReward Reward;

        public VanillaCheck() { }

        public VanillaCheck(VanillaLocation location, VanillaReward reward) {
            Location = location;
            Reward = reward;
        }
    }
}
