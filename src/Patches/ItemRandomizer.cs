using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using UnityEngine.InputSystem.Utilities;
using Newtonsoft.Json;

namespace TunicArchipelago {
    public class ItemRandomizer {
        private static ManualLogSource Logger = TunicArchipelago.Logger;

        public static Dictionary<string, int> SphereZero = new Dictionary<string, int>();

        public static void PopulateSphereZero() {
            SphereZero.Clear();
            if (SaveFile.GetInt("randomizer shuffled abilities") == 0) {
                SphereZero.Add("12", 1);
                SphereZero.Add("21", 1);
                SphereZero.Add("26", 1);
            }
            if (SaveFile.GetInt("randomizer started with sword") == 1) {
                SphereZero.Add("Sword", 1);
            }
        }

        public static void RandomizeAndPlaceItems() {
            Logger.LogInfo("randomize and place items starting");
            Logger.LogInfo("laurels option is " + SaveFile.GetInt("randomizer laurels location"));
            Locations.RandomizedLocations.Clear();
            Locations.CheckedLocations.Clear();

            List<string> ProgressionNames = new List<string>{
                "Hyperdash", "Wand", "Techbow", "Stundagger", "Trinket Coin", "Lantern", "Stick", "Sword", "Sword Progression", "Key", "Key (House)", "Mask", "Vault Key (Red)" };
            if (SaveFile.GetInt("randomizer shuffled abilities") == 1) {
                if (SaveFile.GetInt(SaveFlags.HexagonQuestEnabled) == 1) {
                    ProgressionNames.Add("Hexagon Gold");
                } else {
                    ProgressionNames.Add("12"); // Prayer
                    ProgressionNames.Add("21"); // Holy Cross
                    ProgressionNames.Add("26"); // Ice Rod
                }
            }

            List<Check> InitialItems = JsonConvert.DeserializeObject<List<Check>>(ItemListJson.ItemList);
            List<Reward> InitialRewards = new List<Reward>();
            List<Location> InitialLocations = new List<Location>();
            List<Check> Hexagons = new List<Check>();
            Check Laurels = new Check();
            List<Reward> ProgressionRewards = new List<Reward>();
            Dictionary<string, int> UnplacedInventory = new Dictionary<string, int>(SphereZero);
            Dictionary<string, int> SphereZeroInventory = new Dictionary<string, int>(SphereZero);
            Dictionary<string, Check> ProgressionLocations = new Dictionary<string, Check> { };
            int GoldHexagonsAdded = 0;
            int HexagonsToAdd = (int)Math.Round((100f + TunicArchipelago.Settings.HexagonQuestExtraPercentage) / 100f * TunicArchipelago.Settings.HexagonQuestGoal);
            if (SaveFile.GetInt(SaveFlags.HexagonQuestEnabled) == 1 && SaveFile.GetInt("randomizer shuffled abilities") == 1) {
                int HexGoal = SaveFile.GetInt("randomizer hexagon quest goal");
                List<string> abilities = new List<string>() { "prayer", "holy cross", "ice rod" }.OrderBy(r => TunicArchipelago.Randomizer.Next()).ToList();
                List<int> ability_unlocks = new List<int>() { (int)(HexGoal / 4f), (int)((HexGoal / 4f) * 2), (int)((HexGoal / 4f) * 3) }.OrderBy(r => TunicArchipelago.Randomizer.Next()).ToList();
                for (int i = 0; i < 3; i++) {
                    int index = TunicArchipelago.Randomizer.Next(abilities.Count);
                    int index2 = TunicArchipelago.Randomizer.Next(ability_unlocks.Count);
                    SaveFile.SetInt($"randomizer hexagon quest {abilities[index]} requirement", ability_unlocks[index2]);
                    abilities.RemoveAt(index);
                    ability_unlocks.RemoveAt(index2);
                }
            }
            Shuffle(InitialItems);
            foreach (Check Item in InitialItems) {

                if (SaveFile.GetInt("randomizer keys behind bosses") != 0 && (Item.Reward.Name.Contains("Hexagon") || Item.Reward.Name == "Vault Key (Red)")) {
                    if (Item.Reward.Name == "Hexagon Green" || Item.Reward.Name == "Hexagon Blue") {
                        Hexagons.Add(Item);
                    } else if (Item.Reward.Name == "Vault Key (Red)") {
                        Item.Reward.Name = "Hexagon Red";
                        Hexagons.Add(Item);
                    } else if (Item.Reward.Name == "Hexagon Red") {
                        Item.Reward.Name = "Vault Key (Red)";
                        InitialRewards.Add(Item.Reward);
                        InitialLocations.Add(Item.Location);
                    }
                } else if ((SaveFile.GetInt("randomizer laurels location") == 1 && Item.Location.LocationId == "Well Reward (6 Coins)")
                    || (SaveFile.GetInt("randomizer laurels location") == 2 && Item.Location.LocationId == "Well Reward (10 Coins)")
                    || (SaveFile.GetInt("randomizer laurels location") == 3 && Item.Location.LocationId == "waterfall")) {
                    InitialRewards.Add(Item.Reward);
                    Laurels.Location = Item.Location;
                } else if (SaveFile.GetInt("randomizer laurels location") != 0 && Item.Reward.Name == "Hyperdash") {
                    InitialLocations.Add(Item.Location);
                    Laurels.Reward = Item.Reward;
                } else {
                    if (SaveFile.GetInt("randomizer sword progression enabled") != 0 && (Item.Reward.Name == "Stick" || Item.Reward.Name == "Sword" || Item.Location.LocationId == "5")) {
                        Item.Reward.Name = "Sword Progression";
                        Item.Reward.Type = "SPECIAL";
                    }
                    if (SaveFile.GetInt(SaveFlags.HexagonQuestEnabled) == 1) {
                        if (Item.Reward.Type == "PAGE" || Item.Reward.Name.Contains("Hexagon")) {
                            string FillerItem = ItemLookup.FillerItems.Keys.ToList()[TunicArchipelago.Randomizer.Next(ItemLookup.FillerItems.Count)];
                            Item.Reward.Name = FillerItem;
                            Item.Reward.Type = FillerItem == "money" ? "MONEY" : "INVENTORY";
                            Item.Reward.Amount = ItemLookup.FillerItems[FillerItem][TunicArchipelago.Randomizer.Next(ItemLookup.FillerItems[FillerItem].Count)];
                        }
                        if (ItemLookup.FillerItems.ContainsKey(Item.Reward.Name) && ItemLookup.FillerItems[Item.Reward.Name].Contains(Item.Reward.Amount) && GoldHexagonsAdded < HexagonsToAdd) {
                            Item.Reward.Name = "Hexagon Gold";
                            Item.Reward.Type = "SPECIAL";
                            Item.Reward.Amount = 1;
                            GoldHexagonsAdded++;
                        }
                        if (SaveFile.GetInt("randomizer shuffled abilities") == 1) {
                            if (Item.Location.RequiredItems.Count > 0) {
                                for (int i = 0; i < Item.Location.RequiredItems.Count; i++) {
                                    if (Item.Location.RequiredItems[i].ContainsKey("12") && Item.Location.RequiredItems[i].ContainsKey("21")) {
                                        int amt = Math.Max(SaveFile.GetInt($"randomizer hexagon quest prayer requirement"), SaveFile.GetInt($"randomizer hexagon quest holy cross requirement"));
                                        Item.Location.RequiredItems[i].Remove("12");
                                        Item.Location.RequiredItems[i].Remove("21");
                                        Item.Location.RequiredItems[i].Add("Hexagon Gold", amt);
                                    }
                                    if (Item.Location.RequiredItems[i].ContainsKey("12")) {
                                        Item.Location.RequiredItems[i].Remove("12");
                                        Item.Location.RequiredItems[i].Add("Hexagon Gold", SaveFile.GetInt($"randomizer hexagon quest prayer requirement"));
                                    }
                                    if (Item.Location.RequiredItems[i].ContainsKey("21")) {
                                        Item.Location.RequiredItems[i].Remove("21");
                                        Item.Location.RequiredItems[i].Add("Hexagon Gold", SaveFile.GetInt($"randomizer hexagon quest holy cross requirement"));
                                    }
                                    if (Item.Location.RequiredItems[i].ContainsKey("26")) {
                                        Item.Location.RequiredItems[i].Remove("26");
                                        Item.Location.RequiredItems[i].Add("Hexagon Gold", SaveFile.GetInt($"randomizer hexagon quest ice rod requirement"));
                                    }
                                }
                            }
                            if (Item.Location.RequiredItemsDoors.Count > 0) {
                                for (int i = 0; i < Item.Location.RequiredItemsDoors.Count; i++) {
                                    if (Item.Location.RequiredItemsDoors[i].ContainsKey("12") && Item.Location.RequiredItemsDoors[i].ContainsKey("21")) {
                                        int amt = Math.Max(SaveFile.GetInt($"randomizer hexagon quest prayer requirement"), SaveFile.GetInt($"randomizer hexagon quest holy cross requirement"));
                                        Item.Location.RequiredItemsDoors[i].Remove("12");
                                        Item.Location.RequiredItemsDoors[i].Remove("21");
                                        Item.Location.RequiredItemsDoors[i].Add("Hexagon Gold", amt);
                                    }
                                    if (Item.Location.RequiredItemsDoors[i].ContainsKey("12")) {
                                        Item.Location.RequiredItemsDoors[i].Remove("12");
                                        Item.Location.RequiredItemsDoors[i].Add("Hexagon Gold", SaveFile.GetInt($"randomizer hexagon quest prayer requirement"));
                                    }
                                    if (Item.Location.RequiredItemsDoors[i].ContainsKey("21")) {
                                        Item.Location.RequiredItemsDoors[i].Remove("21");
                                        Item.Location.RequiredItemsDoors[i].Add("Hexagon Gold", SaveFile.GetInt($"randomizer hexagon quest holy cross requirement"));
                                    }
                                    if (Item.Location.RequiredItemsDoors[i].ContainsKey("26")) {
                                        Item.Location.RequiredItemsDoors[i].Remove("26");
                                        Item.Location.RequiredItemsDoors[i].Add("Hexagon Gold", SaveFile.GetInt($"randomizer hexagon quest ice rod requirement"));
                                    }
                                }
                            }
                        }
                    }
                    if (ProgressionNames.Contains(Item.Reward.Name) || ItemLookup.FairyLookup.Keys.Contains(Item.Reward.Name)) {
                        ProgressionRewards.Add(Item.Reward);
                    } else {
                        InitialRewards.Add(Item.Reward);
                    }
                    InitialLocations.Add(Item.Location);
                }
            }

            // adding the progression rewards to the start inventory, so we can reverse fill
            foreach (Reward item in ProgressionRewards) {
                string itemName = ItemLookup.FairyLookup.Keys.Contains(item.Name) ? "Fairy" : item.Name;
                if (UnplacedInventory.ContainsKey(itemName)) {
                    UnplacedInventory[itemName] += 1;
                } else {
                    UnplacedInventory.Add(itemName, 1);
                }
            }
            // if laurels location is on, manually add laurels to the unplaced inventory
            if (!UnplacedInventory.ContainsKey("Hyperdash")) {
                UnplacedInventory.Add("Hyperdash", 1);
            }

            // make a scene inventory, so we can keep the item inventory separated. Add overworld to start (change later if we do start rando)
            Dictionary<string, int> SceneInventory = new Dictionary<string, int>();
            Dictionary<string, int> CombinedInventory = new Dictionary<string, int>();
            TunicPortals.RandomizePortals(SaveFile.GetInt("seed"));
            int fairyCount = 0;
            bool laurelsPlaced = false;

            // put progression items in locations
            foreach (Reward item in ProgressionRewards.OrderBy(r => TunicArchipelago.Randomizer.Next())) {

                // pick an item
                string itemName = ItemLookup.FairyLookup.Keys.Contains(item.Name) ? "Fairy" : item.Name;
                //Logger.LogInfo("placing the item " + itemName);
                // remove item from inventory for reachability checks
                if (UnplacedInventory.Keys.Contains(itemName)) {
                    UnplacedInventory[itemName] -= 1;
                }
                if (UnplacedInventory[itemName] == 0) {
                    UnplacedInventory.Remove(itemName);
                }

                if (itemName == "Fairy") {
                    fairyCount++;
                }
                if (SaveFile.GetInt("randomizer laurels location") != 0 && !laurelsPlaced && (
                    (SaveFile.GetInt("randomizer laurels location") == 1 && UnplacedInventory["Trinket Coin"] == 10)
                    || (SaveFile.GetInt("randomizer laurels location") == 2 && UnplacedInventory["Trinket Coin"] == 6)
                    || (SaveFile.GetInt("randomizer laurels location") == 3 && fairyCount == 11))) {
                    // laurels will no longer be accessible, remove it from the pool
                    laurelsPlaced = true;
                    UnplacedInventory.Remove("Hyperdash");
                }

                // door rando time
                if (SaveFile.GetInt("randomizer entrance rando enabled") == 1) {
                    // this should keep looping until every portal either doesn't give a reward, or has already given its reward
                    int checkP = 0;
                    SceneInventory.Clear();
                    SceneInventory.Add("Overworld Redux", 1);
                    // fill up our SceneInventory with scenes until we stop getting new scenes -- these are of the portals and regions we can currently reach
                    //Logger.LogInfo("number of portals is " + TunicPortals.RandomizedPortals.Count.ToString());
                    while (checkP < TunicPortals.RandomizedPortals.Count) {
                        checkP = 0;
                        CombinedInventory.Clear();
                        foreach (KeyValuePair<string, int> sceneItem in SceneInventory) {
                            CombinedInventory.Add(sceneItem.Key, sceneItem.Value);
                        }
                        foreach (KeyValuePair<string, int> placedItem in UnplacedInventory) {
                            CombinedInventory.Add(placedItem.Key, placedItem.Value);
                        }

                        foreach (PortalCombo portalCombo in TunicPortals.RandomizedPortals.Values) {
                            if (portalCombo.ComboRewards(CombinedInventory).Count > 0) {
                                int testValue = 0;
                                int testValue2 = 0;
                                foreach (string itemDoors in portalCombo.ComboRewards(CombinedInventory)) {
                                    testValue2++;
                                    if (!SceneInventory.ContainsKey(itemDoors)) {
                                        SceneInventory.Add(itemDoors, 1);
                                    } else {
                                        testValue++;
                                    }
                                }
                                if (testValue == testValue2) {
                                    checkP++;
                                }
                            } else {
                                checkP++;
                            }
                        }
                        //Logger.LogInfo("checkP equals " + checkP.ToString());
                    }
                }

                //Logger.LogInfo("scene inventory contains");
                //foreach (string sceneItem in SceneInventory.Keys)
                //{ Logger.LogInfo(sceneItem); }

                // pick a location
                int l;
                l = TunicArchipelago.Randomizer.Next(InitialLocations.Count);

                // empty combined inventory, refill it with whatever is currently in scene inventory and placed inventory
                CombinedInventory.Clear();
                foreach (KeyValuePair<string, int> sceneItem in SceneInventory) {
                    CombinedInventory.Add(sceneItem.Key, sceneItem.Value);
                }
                foreach (KeyValuePair<string, int> placedItem in UnplacedInventory) {
                    CombinedInventory.Add(placedItem.Key, placedItem.Value);
                }

                //foreach (Location loc in InitialLocations)
                //{ if (!loc.reachable(CombinedInventory)) { Logger.LogInfo("location " + loc.SceneName + " " + loc.LocationId + " is not reachable"); } }
                //Logger.LogInfo(InitialLocations[l].SceneName + " " + InitialLocations[l].LocationId);

                // if location isn't reachable with current inventory excluding the item to be placed, pick a new location
                while (!InitialLocations[l].reachable(CombinedInventory)) {
                    //Logger.LogInfo(InitialLocations[l].SceneName + " " + InitialLocations[l].LocationId);
                    //Logger.LogInfo("the above location failed");
                    l = TunicArchipelago.Randomizer.Next(InitialLocations.Count);
                }

                // prepare matched list of progression items and locations
                string DictionaryId = $"{InitialLocations[l].LocationId} [{InitialLocations[l].SceneName}]";
                Check Check = new Check(item, InitialLocations[l]);
                ProgressionLocations.Add(DictionaryId, Check);

                InitialLocations.Remove(InitialLocations[l]);
            }

            SphereZero = CombinedInventory;

            if (SaveFile.GetInt("randomizer entrance rando enabled") == 1) {
                SphereZero = GetERSphereOne();
            }

            // shuffle remaining rewards and locations
            Shuffle(InitialRewards, InitialLocations);

            for (int i = 0; i < InitialRewards.Count; i++) {
                string DictionaryId = $"{InitialLocations[i].LocationId} [{InitialLocations[i].SceneName}]";
                Check Check = new Check(InitialRewards[i], InitialLocations[i]);
                Locations.RandomizedLocations.Add(DictionaryId, Check);
            }

            // add progression items and locations back
            foreach (string key in ProgressionLocations.Keys) {
                Locations.RandomizedLocations.Add(key, ProgressionLocations[key]);
            }

            if (SaveFile.GetInt("randomizer keys behind bosses") != 0) {
                foreach (Check Hexagon in Hexagons) {
                    if (SaveFile.GetInt(SaveFlags.HexagonQuestEnabled) == 1) {
                        Hexagon.Reward.Name = "Hexagon Gold";
                        Hexagon.Reward.Type = "SPECIAL";
                    }
                    string DictionaryId = $"{Hexagon.Location.LocationId} [{Hexagon.Location.SceneName}]";
                    Locations.RandomizedLocations.Add(DictionaryId, Hexagon);
                }
            }

            if (TunicArchipelago.Settings.FixedLaurelsOption != 0) {
                string DictionaryId = $"{Laurels.Location.LocationId} [{Laurels.Location.SceneName}]";
                Locations.RandomizedLocations.Add(DictionaryId, Laurels);
            }

            if (SaveFile.GetString("randomizer game mode") == "VANILLA") {
                Locations.RandomizedLocations.Clear();
                foreach (Check item in JsonConvert.DeserializeObject<List<Check>>(ItemListJson.ItemList)) {
                    if (SaveFile.GetInt("randomizer sword progression enabled") != 0) {
                        if (item.Reward.Name == "Stick" || item.Reward.Name == "Sword" || item.Location.LocationId == "5") {
                            item.Reward.Name = "Sword Progression";
                            item.Reward.Type = "SPECIAL";
                        }
                    }
                    string DictionaryId = $"{item.Location.LocationId} [{item.Location.SceneName}]";
                    Locations.RandomizedLocations.Add(DictionaryId, item);
                }
            }

            foreach (string key in Locations.RandomizedLocations.Keys.ToList()) {
                Check check = Locations.RandomizedLocations[key];
                if (check.Reward.Type == "MONEY") {
                    if ((TunicArchipelago.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.NORMAL && check.Reward.Amount < 20)
                    || (TunicArchipelago.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.DOUBLE && check.Reward.Amount <= 20)
                    || (TunicArchipelago.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.ONSLAUGHT && check.Reward.Amount <= 30)) {
                        check.Reward.Name = "Fool Trap";
                        check.Reward.Type = "FOOL";
                    }
                }
            }

            foreach (string Key in Locations.RandomizedLocations.Keys) {
                int ItemPickedUp = SaveFile.GetInt($"randomizer picked up {Key}");
                Locations.CheckedLocations.Add(Key, ItemPickedUp == 1 ? true : false);
            }
            if (TunicArchipelago.Tracker.ItemsCollected.Count == 0) {
                foreach (KeyValuePair<string, bool> PickedUpItem in Locations.CheckedLocations.Where(item => item.Value)) {
                    Check check = Locations.RandomizedLocations[PickedUpItem.Key];
                    ItemData itemData = ItemLookup.GetItemDataFromCheck(check);
                    TunicArchipelago.Tracker.SetCollectedItem(itemData.Name, false);
                }
                ItemTracker.SaveTrackerFile();
                TunicArchipelago.Tracker.ImportantItems["Flask Container"] += TunicArchipelago.Tracker.ItemsCollected.Where(Item => Item.Name == "Flask Shard").Count() / 3;
                if (SaveFile.GetInt("randomizer started with sword") == 1) {
                    TunicArchipelago.Tracker.ImportantItems["Sword"] += 1;
                }
            }
        }

        private static void Shuffle(List<Reward> Rewards, List<Location> Locations) {
            int n = Rewards.Count;
            int r;
            int l;
            while (n > 1) {
                n--;
                r = TunicArchipelago.Randomizer.Next(n + 1);
                l = TunicArchipelago.Randomizer.Next(n + 1);

                Reward Reward = Rewards[r];
                Rewards[r] = Rewards[n];
                Rewards[n] = Reward;

                Location Location = Locations[l];
                Locations[l] = Locations[n];
                Locations[n] = Location;
            }
        }

        private static void Shuffle(List<Check> list) {
            int n = list.Count;
            int r;
            while (n > 1) {
                n--;
                r = TunicArchipelago.Randomizer.Next(n + 1);

                Check holder = list[r];
                list[r] = list[n];
                list[n] = holder;
            }
        }

        public static Check FindRandomizedItemByName(string Name) {
            foreach (Check Check in Locations.RandomizedLocations.Values) {
                if (Check.Reward.Name == Name) {
                    return Check;
                }
            }
            return null;
        }

        public static List<Check> FindAllRandomizedItemsByName(string Name) {
            List<Check> results = new List<Check>();

            foreach (Check Check in Locations.RandomizedLocations.Values) {
                if (Check.Reward.Name == Name) {
                    results.Add(Check);
                }
            }

            return results;
        }

        public static List<Check> FindAllRandomizedItemsByType(string type) {
            List<Check> results = new List<Check>();

            foreach (Check Check in Locations.RandomizedLocations.Values) {
                if (Check.Reward.Type == type) {
                    results.Add(Check);
                }
            }

            return results;
        }

        // Used for getting what sphere 1 is if you have ER on
        // Gives you items in Overworld or items in adjacent scenes
        // will need updating if/when we do a different starting spot
        public static Dictionary<string, int> GetERSphereOne() {
            List<Portal> PortalInventory = new List<Portal>();
            Dictionary<string, int> CombinedInventory = new Dictionary<string, int> { { "Overworld Redux", 1 } };

            // add starting sword and abilities if applicable
            if (SaveFile.GetInt("randomizer started with sword") == 1) { CombinedInventory.Add("Sword", 1); }
            if (SaveFile.GetInt("randomizer shuffled abilities") == 0) {
                CombinedInventory.Add("12", 1);
                CombinedInventory.Add("21", 1);
            }

            // find which portals you can reach from spawn without additional progression
            foreach (PortalCombo portalCombo in TunicPortals.RandomizedPortals.Values) {
                if (portalCombo.Portal1.Scene == "Overworld Redux" && portalCombo.Portal1.Region != "Overworld Well to Furnace Rail" && portalCombo.Portal1.Region != "Overworld West Garden Furnace Connector") {
                    var portal = portalCombo.Portal1;
                    if (!portal.IgnoreScene && portal.RequiredItemsOr != null) {
                        if (SaveFile.GetInt("randomizer shuffled abilities") == 1 && portal.RequiredItems == null && portal.EntryItems == null && !portal.PrayerPortal) { PortalInventory.Add(portalCombo.Portal2); } else if (SaveFile.GetInt("randomizer shuffled abilities") == 0 && (portal.SceneDestinationTag == "Overworld Redux, Town_FiligreeRoom_" || portal.SceneDestinationTag == "Overworld Redux, EastFiligreeCache_" || portal.PrayerPortal || portal.RequiredItems == null)) { PortalInventory.Add(portalCombo.Portal2); }
                    }
                }
                if (portalCombo.Portal2.Scene == "Overworld Redux" && portalCombo.Portal2.Region != "Overworld Well to Furnace Rail" && portalCombo.Portal2.Region != "Overworld West Garden Furnace Connector") {
                    var portal = portalCombo.Portal2;
                    if (!portal.IgnoreScene && portal.RequiredItemsOr == null) {
                        if (SaveFile.GetInt("randomizer shuffled abilities") == 1 && portal.RequiredItems == null && portal.EntryItems == null && !portal.PrayerPortal) { PortalInventory.Add(portalCombo.Portal1); } else if (SaveFile.GetInt("randomizer shuffled abilities") == 0 && (portal.SceneDestinationTag == "Overworld Redux, Town_FiligreeRoom_" || portal.SceneDestinationTag == "Overworld Redux, EastFiligreeCache_" || portal.PrayerPortal || portal.RequiredItems == null)) { PortalInventory.Add(portalCombo.Portal1); }
                    }
                }
            }

            // add the new portals and any applicable new scenes to the inventory
            foreach (Portal portal in PortalInventory) {
                if (!CombinedInventory.ContainsKey(portal.SceneDestinationTag)) { CombinedInventory.Add(portal.SceneDestinationTag, 1); }

                foreach (string reward in portal.Rewards(CombinedInventory)) {
                    if (!CombinedInventory.ContainsKey(reward)) {
                        CombinedInventory.Add(reward, 1);
                    }
                }
            }
            return CombinedInventory;
        }
    }
}
