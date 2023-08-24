using Archipelago.MultiClient.Net.Enums;
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

        public ItemFlags Classification { 
            get; 
            set; 
        }

        public ArchipelagoItem(string itemName, int player, ItemFlags classification) { 
            ItemName = itemName;
            Player = player;
            Classification = classification;
        }

    }
}
