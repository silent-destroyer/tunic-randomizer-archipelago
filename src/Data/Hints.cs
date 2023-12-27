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

        public static Dictionary<string, string> HintMessages = new Dictionary<string, string>();
        public static Dictionary<string, string> LocalHintsForServer = new Dictionary<string, string>();
        public static Dictionary<string, string> HintStructureScenes = new Dictionary<string, string>();

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
            LocalHintsForServer.Clear();
            HintStructureScenes.Clear();
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
                HintStructureScenes.Add(Locations.LocationIdToDescription[key], "Overworld Redux");
            } else if (SphereOneOthers.Count > 0) {
                key = SphereOneOthers.Keys.ToList()[random.Next(SphereOneOthers.Count)];
                mailboxitem = SphereOneOthers[key];
                HintStructureScenes.Add(Locations.LocationIdToDescription[key], "Overworld Redux");
            }
            if (mailboxitem != null) {
                Scene = Locations.SimplifiedSceneNames[Locations.VanillaLocations[key].Location.SceneName].ToUpper();
                Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                Hint = $"lehjehnd sehz {Prefix} \"{Scene.ToUpper()}\"\nkuhntAnz wuhn uhv mehnE \"<#00FFFF>FIRST STEPS<#ffffff>\" ahn yor jurnE.";
            } else {
                Hint = $"yor frehndz muhst furst hehlp yoo fInd yor wA...\ngoud luhk, rooin sEkur.";
            }
            HintMessages.Add("Mailbox", Hint);
            ArchipelagoHint Hyperdash = Locations.MajorItemLocations["Hero's Laurels"][0];
            Hint = $"lehjehnd sehz <#FF00FF>suhm%i^ ehkstruhordinArE<#FFFFFF>  [laurels] ";
            if (Hyperdash.Player == Player) {
                Scene = Hyperdash.Location == "Your Pocket" ? Hyperdash.Location.ToUpper() : Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[Hyperdash.Location]].Location.SceneName].ToUpper();
                Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                Hint += $"\nuhwAts yoo in {Prefix} \"{Scene}...\"";
                if (Locations.LocationDescriptionToId.ContainsKey(Hyperdash.Location)) {
                    LocalHintsForServer.Add("Temple Statue", Hyperdash.Location);
                }
            } else if (Archipelago.instance.GetPlayerGame((int)Hyperdash.Player) == "Tunic") {
                Scene = Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[Hyperdash.Location]].Location.SceneName].ToUpper();
                Hint += $"\nuhwAts yoo in \"{Archipelago.instance.GetPlayerName((int)Hyperdash.Player).ToUpper()}'S\"\n\"{Scene}...\"";
            } else {
                Hint += $" uhwAts yoo aht\n\"{Hyperdash.Location.ToUpper()}\"\nin\"{Archipelago.instance.GetPlayerName((int)Hyperdash.Player).ToUpper()}'S WORLD...\"";  
            }
            HintMessages.Add("Temple Statue", Hint);
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
                    Hint = $"lehjehnd sehz \"{Archipelago.instance.GetPlayerName((int)ItemHint.Player).ToUpper()}'S WORLD\" aht\n\"{ItemHint.Location.ToUpper()}\"";
                }
                Hint += $"\niz lOkAtid awn #uh \"<#ffd700>PATH OF THE HERO<#ffffff>...\"";
                HintMessages.Add(HintGrave, Hint);

                if (HintGrave == "East Forest Relic") {
                    HintStructureScenes.Add($"{ItemHint.Player}, {ItemHint.Location}", "East Forest Redux");
                } else if (HintGrave == "Fortress Relic") {
                    HintStructureScenes.Add($"{ItemHint.Player}, {ItemHint.Location}", "Fortress Reliquary");
                } else if (HintGrave == "West Garden Relic") {
                    HintStructureScenes.Add($"{ItemHint.Player}, {ItemHint.Location}", "Archipelagos Redux");
                }

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
                ArchipelagoHint HexHint = Hexagon == "Gold Questagon" ? Locations.MajorItemLocations[Hexagon][i] : Locations.MajorItemLocations[Hexagon][0];

                if (HexHint.Player == Player) {
                    Scene = HexHint.Location == "Your Pocket" ? HexHint.Location.ToUpper() : Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[HexHint.Location]].Location.SceneName].ToUpper();
                    Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                    Hint = $"#A sA {Prefix} \"{Scene.ToUpper()}\" iz \nwAr #uh {HexagonColors[Hexagon]}kwehstuhgawn [hexagram]<#FFFFFF> iz fownd\"...\"";

                    if (Locations.LocationDescriptionToId.ContainsKey(HexHint.Location)) {
                        LocalHintsForServer.Add(HexagonHintArea, HexHint.Location);
                    }
                } else if (Archipelago.instance.GetPlayerGame((int)HexHint.Player) == "Tunic") {
                    Scene = Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[HexHint.Location]].Location.SceneName].ToUpper();
                    Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                    Hint = $"#A sA \"{Archipelago.instance.GetPlayerName((int)HexHint.Player).ToUpper()}'S\"\n\"{Scene}\"\niz wAr #uh {HexagonColors[Hexagon]}kwehstuhgawn [hexagram]<#FFFFFF> iz fownd\"...\"";
                } else {
                    Hint = $"#A sA #uh {HexagonColors[Hexagon]}kwehstuhgawn [hexagram]<#FFFFFF> iz fownd aht\n\"{HexHint.Location.ToUpper()}\"\nin \"{Archipelago.instance.GetPlayerName((int)HexHint.Player).ToUpper()}'S WORLD...\"";
                }
                HintMessages.Add(HexagonHintArea, Hint);

                if (HexagonHintArea == "Swamp Relic") {
                    HintStructureScenes.Add($"{HexHint.Player}, {HexHint.Location}", "Swamp Redux 2");
                } else if (HexagonHintArea == "Library Relic") {
                    HintStructureScenes.Add($"{HexHint.Player}, {HexHint.Location}", "Library Hall");
                } else if (HexagonHintArea == "Monastery Relic") {
                    HintStructureScenes.Add($"{HexHint.Player}, {HexHint.Location}", "Monastery");
                }

                Hexagons.Remove(Hexagon);
                HexagonHintGraves.Remove(HexagonHintArea);
            }
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

        public static void ToggleHintIndicator(string sceneName, bool onOrOff) {
            if (sceneName == "Sword Access") {
                GameObject.Find("_Setpieces/RelicPlinth (1)/cathedral_candleflame").SetActive(onOrOff);
            } else if (sceneName == "Fortress Reliquary") {
                GameObject.Find("RelicPlinth/cathedral_candleflame").SetActive(onOrOff);
            } else if (sceneName == "Archipelagos Redux") {
                GameObject.Find("_Environment Prefabs/RelicPlinth/cathedral_candleflame").SetActive(onOrOff);
                GameObject.Find("_Environment Prefabs/RelicPlinth/Point Light").SetActive(onOrOff);
            } else if (sceneName == "Swamp Redux 2") {
                GameObject.Find("_Setpieces Etc/RelicPlinth/cathedral_candleflame").SetActive(onOrOff);
                GameObject.Find("_Setpieces Etc/RelicPlinth/Point Light").SetActive(onOrOff);
            } else if (sceneName == "Library Hall") {
                GameObject.Find("_Special/RelicPlinth/cathedral_candleflame").SetActive(onOrOff);
                GameObject.Find("_Special/RelicPlinth/Point Light").SetActive(onOrOff);
            } else if (sceneName == "Monastery") {
                GameObject.Find("Root/RelicPlinth (1)/cathedral_candleflame").SetActive(onOrOff);
                GameObject.Find("Root/RelicPlinth (1)/Point Light").SetActive(onOrOff);
            } else if (sceneName == "Overworld Redux") {
                if (onOrOff) {
                    GameObject.Find("_Environment/_Decorations/Mailbox (1)/mailbox flag").transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                } else {
                    GameObject.Find("_Environment/_Decorations/Mailbox (1)/mailbox flag").transform.rotation = new Quaternion(0.5f, -0.5f, 0.5f, 0.5f);
                }
            }
        }

    }
}
