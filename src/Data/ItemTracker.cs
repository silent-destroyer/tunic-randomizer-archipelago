﻿using BepInEx.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static TunicArchipelago.GhostHints;
using static TunicArchipelago.SaveFlags;

namespace TunicArchipelago {

    public class ItemTracker {
        private static ManualLogSource Logger = TunicArchipelago.Logger;

        public struct SceneInfo {
            public int SceneId;
            public string SceneName;
        }

        public int Seed;

        public SceneInfo CurrentScene;

        public Dictionary<string, int> ImportantItems = new Dictionary<string, int>() {
            {"Stick", 0},
            {"Sword", 0},
            {"Sword Progression", 0},
            {"Stundagger", 0},
            {"Techbow", 0},
            {"Wand", 0},
            {"Hyperdash", 0},
            {"Lantern", 0},
            {"Shield", 0},
            {"Shotgun", 0},
            {"SlowmoItem", 0},
            {"Key (House)", 0},
            {"Vault Key (Red)", 0},
            {"Trinket Coin", 0},
            {"Coins Tossed", 0},
            {"Trinket Slot", 1},
            {"Trinket Cards", 0},
            {"Flask Shard", 0},
            {"Flask Container", 0},
            {"Pages", 0},
            {"Prayer Page", 0},
            {"Holy Cross Page", 0},
            {"Ice Rod Page", 0},
            {"Fairies", 0},
            {"Golden Trophies", 0},
            {"Dath Stone", 0},
            {"Mask", 0},
            {"Upgrade Offering - Attack - Tooth", 0},
            {"Upgrade Offering - DamageResist - Effigy", 0},
            {"Upgrade Offering - PotionEfficiency Swig - Ash", 0},
            {"Upgrade Offering - Health HP - Flower", 0},
            {"Upgrade Offering - Magic MP - Mushroom", 0},
            {"Upgrade Offering - Stamina SP - Feather", 0},
            {"Relic - Hero Sword", 0},
            {"Relic - Hero Crown", 0},
            {"Relic - Hero Water", 0},
            {"Relic - Hero Pendant HP", 0},
            {"Relic - Hero Pendant MP", 0},
            {"Relic - Hero Pendant SP", 0},
            {"Level Up - Attack", 0},
            {"Level Up - DamageResist", 0},
            {"Level Up - PotionEfficiency", 0},
            {"Level Up - Health", 0},
            {"Level Up - Stamina", 0},
            {"Level Up - Magic", 0},
            {"Hexagon Red", 0},
            {"Hexagon Green", 0},
            {"Hexagon Blue", 0},
            {"Hexagon Gold", 0},
        };

        public List<ItemData> ItemsCollected = new List<ItemData>();

        public ItemTracker() {
            CurrentScene = new SceneInfo();

            Seed = 0;
           
            foreach (string Key in ImportantItems.Keys.ToList()) {
                ImportantItems[Key] = 0;
            }
            ImportantItems["Trinket Slot"] = 1;
            ImportantItems["Coins Tossed"] = StateVariable.GetStateVariableByName("Trinket Coins Tossed").IntValue;

            ItemsCollected = new List<ItemData>();
        }

        public ItemTracker(int seed) {
            CurrentScene = new SceneInfo();
            Seed = seed;
        }

        public void ResetTracker() {
            Seed = 0;
            foreach (string Key in ImportantItems.Keys.ToList()) {
                ImportantItems[Key] = 0;
            }
            ImportantItems["Trinket Slot"] = 1;
        }

        public void SetCollectedItem(string ItemName, bool WriteToDisk) {

            ItemData Item = ItemLookup.Items[ItemName];

            if (ImportantItems.ContainsKey(Item.ItemNameForInventory) && Item.Type != ItemTypes.SWORDUPGRADE) {
                ImportantItems[Item.ItemNameForInventory]++;

                if (Item.ItemNameForInventory == "Flask Shard" && ImportantItems["Flask Shard"] % 3 == 0) {
                    ImportantItems["Flask Container"]++;
                }
            }

            if(Item.Type == ItemTypes.FAIRY) {
                ImportantItems["Fairies"]++;
            }

            if (Item.Type == ItemTypes.TRINKET) {
                ImportantItems["Trinket Cards"]++;
            }
            
            if (Item.Type == ItemTypes.PAGE) {
                ImportantItems["Pages"]++;

                if (Item.Name == "Pages 24-25 (Prayer)") { ImportantItems["Prayer Page"]++; }
                if (Item.Name == "Pages 42-43 (Holy Cross)") { ImportantItems["Holy Cross Page"]++; }
                if (Item.Name == "Pages 52-53 (Ice Rod)") { ImportantItems["Ice Rod Page"]++; }
            }
            
            if (Item.Type == ItemTypes.GOLDENTROPHY) {
                ImportantItems["Golden Trophies"]++;

                if (ImportantItems["Golden Trophies"] >= 12) {
                    Inventory.GetItemByName("Spear").Quantity = 1;
                }
            }
            
            if (Item.Type == ItemTypes.SWORDUPGRADE) {
                if (SaveFile.GetInt(SwordProgressionEnabled) == 1) {
                    int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);
                    ImportantItems["Stick"] = 1;
                    if (SwordLevel > 1) {
                        ImportantItems["Sword"] = SwordLevel-1;
                    }
                    ImportantItems["Sword Progression"]++;
                } else {
                    ImportantItems[Item.ItemNameForInventory]++;
                }
            }

            foreach (string LevelUp in ItemLookup.LevelUpItems) {
                ImportantItems[LevelUp] = Inventory.GetItemByName(LevelUp).Quantity;
            }

            ItemsCollected.Add(Item);
            if (WriteToDisk) {
                SaveTrackerFile();
            }
        }

        public static void SaveTrackerFile() {
            if (File.Exists(TunicArchipelago.ItemTrackerPath)) {
                File.Delete(TunicArchipelago.ItemTrackerPath);
            }
            File.WriteAllText(TunicArchipelago.ItemTrackerPath, JsonConvert.SerializeObject(TunicArchipelago.Tracker, Formatting.Indented));
        }

        public static void PopulateSpoilerLog() {
            int seed = SaveFile.GetInt("seed");
            Dictionary<string, List<string>> SpoilerLog = new Dictionary<string, List<string>>();
            foreach (string Key in Locations.SceneNamesForSpoilerLog.Keys)
            {
                SpoilerLog[Key] = new List<string>();
            }

            foreach (string Key in ItemLookup.ItemList.Keys) {
                ArchipelagoItem Item = ItemLookup.ItemList[Key];

                string Spoiler = $"\t{(Locations.CheckedLocations[Key] ? "x" : "-")} {Locations.LocationIdToDescription[Key]}: {Item.ItemName} ({Archipelago.instance.GetPlayerName(Item.Player)})";

                SpoilerLog[Locations.VanillaLocations[Key].Location.SceneName].Add(Spoiler);
            }

            List<string> SpoilerLogLines = new List<string>() {
                "Archipelago Seed: " + SaveFile.GetString("archipelago seed"),
                "Lines that start with 'x' instead of '-' represent items that have been collected\n",
                "Major Items"
            };

            foreach (string MajorItem in ItemLookup.MajorItems) {
                if(MajorItem == "Gold Questagon") { continue; }
                if(Locations.MajorItemLocations.ContainsKey(MajorItem) && Locations.MajorItemLocations[MajorItem].Count > 0) {
                    foreach (ArchipelagoHint apHint in Locations.MajorItemLocations[MajorItem]) {

                        bool HasItem = false;
                        if (Archipelago.instance.integration.session.Locations.AllLocationsChecked.Contains(Archipelago.instance.integration.session.Locations.GetLocationIdFromName(Archipelago.instance.GetPlayerGame((int)apHint.Player), apHint.Location))) { 
                            HasItem = true;
                        }
                        string Spoiler = $"\t{(HasItem ? "x" : "-")} {MajorItem}: {apHint.Location} ({Archipelago.instance.GetPlayerName((int)apHint.Player)}'s World)";
                        SpoilerLogLines.Add(Spoiler);
                    }
                }
            }
            foreach (string Key in SpoilerLog.Keys) {
                SpoilerLogLines.Add(Locations.SceneNamesForSpoilerLog[Key]);
                SpoilerLog[Key].Sort();
                foreach (string line in SpoilerLog[Key]) {
                    SpoilerLogLines.Add(line);
                }
            }
            if (SaveFile.GetInt(HexagonQuestEnabled) == 1 && SaveFile.GetInt(AbilityShuffle) == 1) {
                SpoilerLogLines.Add($"\t{(SaveFile.GetInt(PrayerUnlocked) == 1 ? "x" : "-")} Prayer: {SaveFile.GetInt(HexagonQuestPrayer)} Gold Questagons");
                SpoilerLogLines.Add($"\t{(SaveFile.GetInt(HolyCrossUnlocked) == 1 ? "x" : "-")} Holy Cross: {SaveFile.GetInt(HexagonQuestHolyCross)} Gold Questagons");
                SpoilerLogLines.Add($"\t{(SaveFile.GetInt(IceRodUnlocked) == 1 ? "x" : "-")} Ice Rod: {SaveFile.GetInt(HexagonQuestIceRod)} Gold Questagons");
            }

            if (SaveFile.GetInt(EntranceRando) == 1)
            {
                List<string> PortalSpoiler = new List<string>();
                SpoilerLogLines.Add("\nEntrance Connections");
                foreach (PortalCombo portalCombo in TunicPortals.RandomizedPortals.Values)
                {
                    PortalSpoiler.Add("\t- " + portalCombo.Portal1.Name + " -- " + portalCombo.Portal2.Name);
                }
                foreach (string combo in PortalSpoiler)
                {
                    SpoilerLogLines.Add(combo);
                }
            }
            if (!File.Exists(TunicArchipelago.SpoilerLogPath)) {
                File.WriteAllLines(TunicArchipelago.SpoilerLogPath, SpoilerLogLines);
            } else {
                File.Delete(TunicArchipelago.SpoilerLogPath);
                File.WriteAllLines(TunicArchipelago.SpoilerLogPath, SpoilerLogLines);
            }
            Logger.LogInfo("Wrote spoiler log to " + TunicArchipelago.SpoilerLogPath);
        }
    }
}
