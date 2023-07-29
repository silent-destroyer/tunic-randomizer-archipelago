using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TunicArchipelago {
    public class ArchipelagoItem {

        public string ItemName {
            get;
            set;
        }

        public int Player {
            get;
            set;
        }

        public ArchipelagoItem(string itemName, int player) { 
            ItemName = itemName;
            Player = player;
        }

    }
}
