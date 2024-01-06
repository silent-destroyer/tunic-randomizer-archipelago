using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using static TunicArchipelago.GhostHints;
using Archipelago.MultiClient.Net.Enums;
using static TunicArchipelago.SaveFlags;
using BepInEx.Logging;
using UnityEngine;
using Archipelago.MultiClient.Net.Models;
using Il2CppSystem;

namespace TunicArchipelago {
    public class Hints {
        private static ManualLogSource Logger = TunicArchipelago.Logger;

        public static Dictionary<string, string> HintLocations = new Dictionary<string, string>() {
            {"Overworld Redux (-7.0, 12.0, -136.4)", "Mailbox"},
            {"Archipelagos Redux (-170.0, 11.0, 152.5)", "West Garden Relic"},
            {"Swamp Redux 2 (92.0, 7.0, 90.8)", "Swamp Relic"},
            {"Sword Access (28.5, 5.0, -190.0)", "East Forest Relic"},
            {"Library Hall (131.0, 19.0, -8.5)", "Library Relic"},
            {"Monastery (-6.0, 26.0, 180.5)", "Monastery Relic"},
            {"Fortress Reliquary (198.5, 5.0, -40.0)", "Fortress Relic"},
            {"Temple (14.0, -0.5, 49.0)", "Temple Statue"},
            {"Overworld Redux (89.0, 44.0, -107.0)", "East Forest Sign"},
            {"Overworld Redux (-5.0, 36.0, -70.0)", "Town Sign"},
            {"Overworld Redux (8.0, 20.0, -115.0)", "Ruined Hall Sign"},
            {"Overworld Redux (-156.0, 12.0, -44.3)", "West Garden Sign"},
            {"Overworld Redux (73.0, 44.0, -38.0)", "Fortress Sign"},
            {"Overworld Redux (-141.0, 40.0, 34.8)", "Quarry Sign"},
            {"East Forest Redux (128.0, 0.0, 33.5)", "West East Forest Sign"},
            {"East Forest Redux (144.0, 0.0, -23.0)", "East East Forest Sign"},
        };

        public struct HeroGraveHint {
            public string PathHintId;
            public string PathHint;
            public string RelicHintId;
            public string RelicHint;
            public string SceneName;
            public string GraveObjectPath;
            public bool PointLight;

            public HeroGraveHint(string pathHintId, string pathHint, string relicHintId, string relicHint, string sceneName, string graveObjectPath, bool pointLight) { 
                PathHintId = pathHintId;
                PathHint = pathHint;
                RelicHintId = relicHintId;
                RelicHint = relicHint; 
                SceneName = sceneName;
                GraveObjectPath = graveObjectPath;
                PointLight = pointLight;
            }
        }
        
        public static Dictionary<string, HeroGraveHint> HeroGraveHints = new Dictionary<string, HeroGraveHint>();

        public static Dictionary<string, string> HintMessages = new Dictionary<string, string>();

        // Used for getting what sphere 1 is if you have ER on
        // Gives you items in Overworld or items in adjacent scenes
        // will need updating if/when we do a different starting spot
        public static List<string> GetERSphereOne()
        {
            List<Portal> PortalInventory = new List<Portal>();
            List<string> CombinedInventory = new List<string>{"Overworld"};
            
            // add starting sword and abilities if applicable
            if (SaveFile.GetInt("randomizer started with sword") == 1)
            { CombinedInventory.Add("Sword"); }
            if (SaveFile.GetInt(AbilityShuffle) == 0)
            {
                CombinedInventory.Add("12");
                CombinedInventory.Add("21");
            }
            
            // find which portals you can reach from spawn without additional progression
            foreach (PortalCombo portalCombo in TunicPortals.RandomizedPortals.Values)
            {
                if (portalCombo.Portal1.Region == "Overworld")
                { PortalInventory.Add(portalCombo.Portal2); }
                if (portalCombo.Portal1.Region == "Overworld Ability" && SaveFile.GetInt(AbilityShuffle) == 0)
                { PortalInventory.Add(portalCombo.Portal2); }

                if (portalCombo.Portal2.Region == "Overworld")
                { PortalInventory.Add(portalCombo.Portal1); }
                if (portalCombo.Portal2.Region == "Overworld Ability" && SaveFile.GetInt(AbilityShuffle) == 0)
                { PortalInventory.Add(portalCombo.Portal1); }
            }

            // add the new portals and any applicable new scenes to the inventory
            foreach (Portal portal in PortalInventory)
            {
                CombinedInventory.Add(portal.SceneDestinationTag);
                CombinedInventory.AddRange(portal.Rewards(CombinedInventory));
            }

            return CombinedInventory;
        }

        public static void PopulateHints() {
            HintMessages.Clear();
            HeroGraveHints.Clear();
            System.Random random = new System.Random(SaveFile.GetInt("seed"));
            string Hint = "";
            string Scene = "";
            string Prefix = "";
            List<char> Vowels = new List<char>() { 'A', 'E', 'I', 'O', 'U' };

            int Player = Archipelago.instance.GetPlayerSlot();
            List<string> MailboxItems = new List<string>() { "Stick", "Sword", "Sword Upgrade", "Magic Dagger", "Magic Wand", "Magic Orb", "Lantern", "Gun", "Scavenger Mask", "Pages 24-25 (Prayer)", "Pages 42-43 (Holy Cross)" };
            Dictionary<string, ArchipelagoItem> SphereOnePlayer = new Dictionary<string, ArchipelagoItem>();
            Dictionary<string, ArchipelagoItem> SphereOneOthers = new Dictionary<string, ArchipelagoItem>();
            List<string> ERSphereOneItemsAndAreas = GetERSphereOne();
            foreach (string itemkey in ItemLookup.ItemList.Keys) {
                ArchipelagoItem item = ItemLookup.ItemList[itemkey];
                // In ER, we need to check more info, since every item has a required item count
                if (SaveFile.GetInt(EntranceRando) == 1) {
                    if (Archipelago.instance.GetPlayerGame(item.Player) == "Tunic" && MailboxItems.Contains(item.ItemName)) {
                        var requirements = Locations.VanillaLocations[itemkey].Location.RequiredItemsDoors[0].Keys;
                        foreach (string req in requirements) {
                            int checkCount = 0;
                            if (ERSphereOneItemsAndAreas.Contains(req)) {
                                checkCount++;
                            } else { 
                                continue;
                            }
                            if (checkCount == requirements.Count) {
                                SphereOnePlayer.Add(itemkey, item);
                            }
                        }
                    }
                    else if (item.Player != Archipelago.instance.GetPlayerSlot() && item.Classification == ItemFlags.Advancement) {
                        var requirements = Locations.VanillaLocations[itemkey].Location.RequiredItemsDoors[0].Keys;
                        foreach (string req in requirements) {
                            int checkCount = 0;
                            if (ERSphereOneItemsAndAreas.Contains(req)) {
                                checkCount++;
                            } else {
                                continue;
                            }
                            if (checkCount == requirements.Count)
                            { SphereOneOthers.Add(itemkey, item); }
                        }
                    }
                } else {
                    if (Archipelago.instance.GetPlayerGame(item.Player) == "Tunic" && MailboxItems.Contains(item.ItemName) && Locations.VanillaLocations[itemkey].Location.RequiredItems.Count == 0) {
                        SphereOnePlayer.Add(itemkey, item);
                    }
                    if (item.Player != Archipelago.instance.GetPlayerSlot() && item.Classification == ItemFlags.Advancement && Locations.VanillaLocations[itemkey].Location.RequiredItems.Count == 0) {
                        SphereOneOthers.Add(itemkey, item);
                    }
                }
            }
            ArchipelagoItem mailboxitem = null;
            string key = "";
            if (SphereOnePlayer.Count > 0) {
                key = SphereOnePlayer.Keys.ToList()[random.Next(SphereOnePlayer.Count)];
                mailboxitem = SphereOnePlayer[key];
            } else if (SphereOneOthers.Count > 0) {
                key = SphereOneOthers.Keys.ToList()[random.Next(SphereOneOthers.Count)];
                mailboxitem = SphereOneOthers[key];
            }
            if (mailboxitem != null) {
                Scene = Locations.SimplifiedSceneNames[Locations.VanillaLocations[key].Location.SceneName].ToUpper();
                Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                SaveFile.SetString("randomizer mailbox hint location", key);
                Hint = $"lehjehnd sehz {Prefix} \"{Scene.ToUpper()}\"\nkuhntAnz wuhn uhv mehnE \"<#00FFFF>FIRST STEPS<#ffffff>\" ahn yor jurnE.";
            } else {
                SaveFile.SetString("randomizer mailbox hint location", "no first steps");
                Hint = $"yor frehndz muhst furst hehlp yoo fInd yor wA...\ngoud luhk, rooin sEkur.";
            }
            HintMessages.Add("Mailbox", Hint);
            ArchipelagoHint Hyperdash = Locations.MajorItemLocations["Hero's Laurels"][0];
            Hint = $"lehjehnd sehz <#FF00FF>suhm%i^ ehkstruhordinArE<#FFFFFF>  [laurels] ";
            if (Hyperdash.Player == Player) {
                Scene = Hyperdash.Location == "Your Pocket" ? Hyperdash.Location.ToUpper() : Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[Hyperdash.Location]].Location.SceneName].ToUpper();
                Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                Hint += $"\nuhwAts yoo in {Prefix} \"{Scene}...\"";
            } else if (Archipelago.instance.GetPlayerGame((int)Hyperdash.Player) == "Tunic") {
                Scene = Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[Hyperdash.Location]].Location.SceneName].ToUpper();
                Hint += $"\nuhwAts yoo in \"{Archipelago.instance.GetPlayerName((int)Hyperdash.Player).ToUpper()}'S\"\n\"{Scene}...\"";
            } else {
                Hint += $" uhwAts yoo aht\n\"{Hyperdash.Location.Replace("_", " ").ToUpper()}\"\nin\"{Archipelago.instance.GetPlayerName((int)Hyperdash.Player).ToUpper()}'S WORLD...\"";  
            }
            HintMessages.Add("Temple Statue", Hint);

            List<(string, string)> relicHints = CreateHeroRelicHints();
            List<string> HintItems = new List<string>() { "Magic Wand", "Magic Orb", "Magic Dagger" };
            if (SaveFile.GetInt(AbilityShuffle) == 1 && SaveFile.GetInt(HexagonQuestEnabled) == 0) {
                HintItems.Add("Pages 24-25 (Prayer)");
                HintItems.Add("Pages 42-43 (Holy Cross)");
                HintItems.Remove("Magic Dagger");
            }
            List<string> HintGraves = new List<string>() { "East Forest Relic", "Fortress Relic", "West Garden Relic" };
            while (HintGraves.Count > 0) {
                string HintItem = HintItems[random.Next(HintItems.Count)];
                ArchipelagoHint ItemHint = Locations.MajorItemLocations[HintItem][0];
                string HintGrave = HintGraves[random.Next(HintGraves.Count)];
                (string, string) RelicHint = relicHints[random.Next(relicHints.Count)];

                if (ItemHint.Player == Player) {
                    Scene = ItemHint.Location == "Your Pocket" ? ItemHint.Location.ToUpper() : Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[ItemHint.Location]].Location.SceneName].ToUpper();
                    if (HintItem == "Pages 24-25 (Prayer)" && Scene == "Fortress Relic") {
                        continue;
                    }
                    Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                    Hint = $"lehjehnd sehz {Prefix} \"{Scene}\"";
                } else if (Archipelago.instance.GetPlayerGame((int)ItemHint.Player) == "Tunic") {
                    Scene = Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[ItemHint.Location]].Location.SceneName].ToUpper();
                    Hint = $"lehjehnd sehz \"{Archipelago.instance.GetPlayerName((int)ItemHint.Player).ToUpper()}'S\"\n\"{Scene}\"";
                } else {
                    Hint = $"lehjehnd sehz \"{Archipelago.instance.GetPlayerName((int)ItemHint.Player).ToUpper()}'S WORLD\" aht\n\"{ItemHint.Location.Replace("_", " ").ToUpper()}\"";
                }
                Hint += $"\niz lOkAtid awn #uh \"<#ffd700>PATH OF THE HERO<#ffffff>...\"";

                string slotLocation = ItemHint.Location == "Your Pocket" ? $"0, Server" : $"{ItemHint.Player}, {ItemHint.Location}";
                if (HintGrave == "East Forest Relic") {
                    HeroGraveHints.Add(HintGrave, new HeroGraveHint(slotLocation, Hint, RelicHint.Item1, RelicHint.Item2, "Sword Access", "_Setpieces/RelicPlinth (1)/", true));
                } else if (HintGrave == "Fortress Relic") {
                    HeroGraveHints.Add(HintGrave, new HeroGraveHint(slotLocation, Hint, RelicHint.Item1, RelicHint.Item2, "Fortress Reliquary", "RelicPlinth", true));
                } else if (HintGrave == "West Garden Relic") {
                    HeroGraveHints.Add(HintGrave, new HeroGraveHint(slotLocation, Hint, RelicHint.Item1, RelicHint.Item2, "Archipelagos Redux", "_Environment Prefabs/RelicPlinth/", false));
                }
                relicHints.Remove(RelicHint);
                HintItems.Remove(HintItem);
                HintGraves.Remove(HintGrave);
            }

            List<string> Hexagons;
            Dictionary<string, string> HexagonColors = new Dictionary<string, string>() { { "Red Questagon", "<#FF3333>" }, { "Green Questagon", "<#33FF33>" }, { "Blue Questagon", "<#3333FF>" }, { "Gold Questagon", "<#ffd700>" } };
            if (SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                Hexagons = new List<string>() { "Gold Questagon", "Gold Questagon", "Gold Questagon" };
            } else {
                Hexagons = new List<string>() { "Red Questagon", "Green Questagon", "Blue Questagon" };
            }
            List<string> strings = new List<string>();
            List<string> HexagonHintGraves = new List<string>() { "Swamp Relic", "Library Relic", "Monastery Relic" };
            for (int i = 0; i < 3; i++) {
                string Hexagon = Hexagons[random.Next(Hexagons.Count)];
                string HexagonHintArea = HexagonHintGraves[random.Next(HexagonHintGraves.Count)];
                (string, string) RelicHint = relicHints[random.Next(relicHints.Count)];
                ArchipelagoHint HexHint = Hexagon == "Gold Questagon" ? Locations.MajorItemLocations[Hexagon][i] : Locations.MajorItemLocations[Hexagon][0];

                if (HexHint.Player == Player) {
                    Scene = HexHint.Location == "Your Pocket" ? HexHint.Location.ToUpper() : Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[HexHint.Location]].Location.SceneName].ToUpper();
                    Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                    Hint = $"#A sA {Prefix} \"{Scene.ToUpper()}\" iz \nwAr #uh {HexagonColors[Hexagon]}kwehstuhgawn [hexagram]<#FFFFFF> iz fownd\"...\"";
                } else if (Archipelago.instance.GetPlayerGame((int)HexHint.Player) == "Tunic") {
                    Scene = Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[HexHint.Location]].Location.SceneName].ToUpper();
                    Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                    Hint = $"#A sA \"{Archipelago.instance.GetPlayerName((int)HexHint.Player).ToUpper()}'S\"\n\"{Scene}\"\niz wAr #uh {HexagonColors[Hexagon]}kwehstuhgawn [hexagram]<#FFFFFF> iz fownd\"...\"";
                } else {
                    Hint = $"#A sA #uh {HexagonColors[Hexagon]}kwehstuhgawn [hexagram]<#FFFFFF> iz fownd aht\n\"{HexHint.Location.Replace("_", " ").ToUpper()}\"\nin \"{Archipelago.instance.GetPlayerName((int)HexHint.Player).ToUpper()}'S WORLD...\"";
                }

                string slotLocation = HexHint.Location == "Your Pocket" ? $"0, Server" : $"{HexHint.Player}, {HexHint.Location}";

                if (HexagonHintArea == "Swamp Relic") {
                    HeroGraveHints.Add(HexagonHintArea, new HeroGraveHint(slotLocation, Hint, RelicHint.Item1, RelicHint.Item2, "Swamp Redux 2", "_Setpieces Etc/RelicPlinth/", false));
                } else if (HexagonHintArea == "Monastery Relic") {
                    HeroGraveHints.Add(HexagonHintArea, new HeroGraveHint(slotLocation, Hint, RelicHint.Item1, RelicHint.Item2, "Monastery", "Root/RelicPlinth (1)/", false));
                } else if (HexagonHintArea == "Library Relic") {
                    HeroGraveHints.Add(HexagonHintArea, new HeroGraveHint(slotLocation, Hint, RelicHint.Item1, RelicHint.Item2, "Library Hall", "_Special/RelicPlinth/", false));
                }

                relicHints.Remove(RelicHint);
                Hexagons.Remove(Hexagon);
                HexagonHintGraves.Remove(HexagonHintArea);
            }

            // Hack to make start inventory from pool'd items toggle the hero graves
            SaveFile.SetInt($"randomizer hint found 0, Server", 1);

            // make the in-game signs tell you what area they're pointing to
            if (SaveFile.GetInt(EntranceRando) == 1)
            {
                foreach (PortalCombo Portal in TunicPortals.RandomizedPortals.Values)
                {
                    if (Portal.Portal1.SceneDestinationTag == "Overworld Redux, Forest Belltower_")
                    { HintMessages.Add("East Forest Sign", $"\"{Locations.SimplifiedSceneNames[Portal.Portal2.Scene]}\" [arrow_right]"); }
                    if (Portal.Portal2.SceneDestinationTag == "Overworld Redux, Forest Belltower_")
                    { HintMessages.Add("East Forest Sign", $"\"{Locations.SimplifiedSceneNames[Portal.Portal1.Scene]}\" [arrow_right]"); }

                    if (Portal.Portal1.SceneDestinationTag == "Overworld Redux, Archipelagos Redux_lower")
                    { HintMessages.Add("West Garden Sign", $"[arrow_left] \"{Locations.SimplifiedSceneNames[Portal.Portal2.Scene]}\""); }
                    if (Portal.Portal2.SceneDestinationTag == "Overworld Redux, Archipelagos Redux_lower")
                    { HintMessages.Add("West Garden Sign", $"[arrow_left] \"{Locations.SimplifiedSceneNames[Portal.Portal1.Scene]}\""); }

                    if (Portal.Portal1.SceneDestinationTag == "Overworld Redux, Fortress Courtyard_")
                    { HintMessages.Add("Fortress Sign", $"\"{Locations.SimplifiedSceneNames[Portal.Portal2.Scene]}\" [arrow_right]"); }
                    if (Portal.Portal2.SceneDestinationTag == "Overworld Redux, Fortress Courtyard_")
                    { HintMessages.Add("Fortress Sign", $"\"{Locations.SimplifiedSceneNames[Portal.Portal1.Scene]}\" [arrow_right]"); }

                    if (Portal.Portal1.SceneDestinationTag == "Overworld Redux, Darkwoods Tunnel_")
                    { HintMessages.Add("Quarry Sign", $"\"{Locations.SimplifiedSceneNames[Portal.Portal2.Scene]}\" [arrow_up]"); }
                    if (Portal.Portal2.SceneDestinationTag == "Overworld Redux, Darkwoods Tunnel_")
                    { HintMessages.Add("Quarry Sign", $"\"{Locations.SimplifiedSceneNames[Portal.Portal1.Scene]}\" [arrow_up]"); }

                    if (Portal.Portal1.SceneDestinationTag == "Overworld Redux, Ruins Passage_west")
                    { HintMessages.Add("Ruined Hall Sign", $"\"{Locations.SimplifiedSceneNames[Portal.Portal2.Scene]}\" [arrow_right]"); }
                    if (Portal.Portal2.SceneDestinationTag == "Overworld Redux, Ruins Passage_west")
                    { HintMessages.Add("Ruined Hall Sign", $"\"{Locations.SimplifiedSceneNames[Portal.Portal1.Scene]}\" [arrow_right]"); }

                    if (Portal.Portal1.SceneDestinationTag == "Overworld Redux, Overworld Interiors_house")
                    { HintMessages.Add("Town Sign", $"[arrow_left] \"{Locations.SimplifiedSceneNames[Portal.Portal2.Scene]}\""); }
                    if (Portal.Portal2.SceneDestinationTag == "Overworld Redux, Overworld Interiors_house")
                    { HintMessages.Add("Town Sign", $"[arrow_left] \"{Locations.SimplifiedSceneNames[Portal.Portal1.Scene]}\""); }

                    if (Portal.Portal1.SceneDestinationTag == "East Forest Redux, Sword Access_lower") {
                        HintMessages.Add("East East Forest Sign", $"\"{Locations.SimplifiedSceneNames[Portal.Portal2.Scene]}\" [arrow_right]");
                    }
                    if (Portal.Portal2.SceneDestinationTag == "East Forest Redux, Sword Access_lower") {
                        HintMessages.Add("East East Forest Sign", $"\"{Locations.SimplifiedSceneNames[Portal.Portal1.Scene]}\" [arrow_right]");
                    }

                    if (Portal.Portal1.SceneDestinationTag == "East Forest Redux, East Forest Redux Laddercave_lower") {
                        HintMessages.Add("West East Forest Sign", $"[arrow_left] \"{Locations.SimplifiedSceneNames[Portal.Portal2.Scene]}\"");
                    }
                    if (Portal.Portal2.SceneDestinationTag == "East Forest Redux, East Forest Redux Laddercave_lower") {
                        HintMessages.Add("West East Forest Sign", $"[arrow_left] \"{Locations.SimplifiedSceneNames[Portal.Portal1.Scene]}\"");
                    }
                }
            }
        }

        private static List<(string, string)> CreateHeroRelicHints() {
            List<ItemData> Relics = ItemLookup.Items.Values.Where(item => item.Type == ItemTypes.RELIC).ToList();
            List<(string, string)> RelicHints = new List<(string, string)>();
            string Scene = "";
            string Prefix = "";
            string RelicHint = "";
            int Player = Archipelago.instance.GetPlayerSlot();
            foreach (ItemData Relic in Relics) {
                ArchipelagoHint RelicItemHint = Locations.MajorItemLocations[Relic.Name][0];
                string itemDisplayText = $"{TextBuilderPatches.ItemNameToAbbreviation[Relic.Name]}  {ItemLookup.BonusUpgrades[Relic.ItemNameForInventory].CustomPickupMessage.ToUpper()}";
                if (RelicItemHint.Player == Player) {
                    Scene = RelicItemHint.Location == "Your Pocket" ? RelicItemHint.Location.ToUpper() : Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[RelicItemHint.Location]].Location.SceneName].ToUpper();

                    Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                    RelicHint = $"lehjehnd sehz #uh  {itemDisplayText}\nkahn bE fownd aht {Prefix} \"{Scene}.\"";
                } else if (Archipelago.instance.GetPlayerGame((int)RelicItemHint.Player) == "Tunic") {
                    Scene = Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[RelicItemHint.Location]].Location.SceneName].ToUpper();
                    Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                    RelicHint = $"lehjehnd sehz #uh  {itemDisplayText}\nkahn bE fownd aht {Prefix} \"{Scene}\"\nin \"{Archipelago.instance.GetPlayerName((int)RelicItemHint.Player).ToUpper()}'S WORLD.\"";
                } else {
                    RelicHint = $"lehjehnd sehz #uh  {itemDisplayText}\nkahn bE fownd in \"{Archipelago.instance.GetPlayerName((int)RelicItemHint.Player).ToUpper()}'S WORLD\"\naht \"{RelicItemHint.Location.Replace("_", " ").ToUpper()}.\"";
                }
                string slotLocation = RelicItemHint.Player == Player && RelicItemHint.Location == "Your Pocket" ? "0, Server" : $"{RelicItemHint.Player}, {RelicItemHint.Location}";
                RelicHints.Add((slotLocation, RelicHint));
            }

            return RelicHints;
        }

        public static string WordWrapString(string Hint) {
            string formattedHint = "";

            int length = 40;
            foreach (string split in Hint.Split(' ')) {
                string split2 = split;
                if (split.StartsWith($"\"") && !split.EndsWith($"\"")) {
                    split2 += $"\"";
                } else if (split.EndsWith($"\"") && !split.StartsWith($"\"")) {
                    split2 = $"\"{split2}";
                }
                if ((formattedHint + split2).Length < length) {
                    formattedHint += split2 + " ";
                } else {
                    formattedHint += split2 + "\n";
                    length += 40;
                }
            }

            return formattedHint;
        }

        public static void SetupHeroGraveToggle() {
            if (Hints.HeroGraveHints.Values.Where(hint => hint.SceneName == SceneManager.GetActiveScene().name).Any()) {
                HeroGraveHint hint = Hints.HeroGraveHints.Values.Where(hintgrave => hintgrave.SceneName == SceneManager.GetActiveScene().name).First();
                GameObject relicPlinth = GameObject.Find(hint.GraveObjectPath);
                if (relicPlinth.GetComponent<HeroGraveToggle>() == null) {
                    relicPlinth.AddComponent<HeroGraveToggle>().heroGravehint = hint;
                } else {
                    relicPlinth.GetComponent<HeroGraveToggle>().heroGravehint = hint;
                }
            }
        }
    }
}
