using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using static TunicArchipelago.GhostHints;
using Archipelago.MultiClient.Net.Enums;
using static TunicArchipelago.SaveFlags;

namespace TunicArchipelago {
    public class Hints {

        public static Dictionary<string, string> HintLocations = new Dictionary<string, string>() {
            {"Overworld Redux (-7.0, 12.0, -136.4)", "Mailbox"},
            {"Archipelagos Redux (-170.0, 11.0, 152.5)", "West Garden Relic"},
            {"Swamp Redux 2 (92.0, 7.0, 90.8)", "Swamp Relic"},
            {"Sword Access (28.5, 5.0, -190.0)", "East Forest Relic"},
            {"Library Hall (131.0, 19.0, -8.5)", "Library Relic"},
            {"Monastery (-6.0, 26.0, 180.5)", "Monastery Relic"},
            {"Fortress Reliquary (198.5, 5.0, -40.0)", "Fortress Relic"},
            {"Temple (14.0, -0.5, 49.0)", "Temple Statue"}
        };

        public static Dictionary<string, string> HintMessages = new Dictionary<string, string>();

        public static String MailboxHintId = "";

        public static void PopulateHints() {
            HintMessages.Clear();
            System.Random random = new System.Random(SaveFile.GetInt("seed"));
            string Hint = "";
            string Scene = "";
            string Prefix = "";
            List<char> Vowels = new List<char>() { 'A', 'E', 'I', 'O', 'U' };

            int Player = Archipelago.instance.GetPlayerSlot();
            List<string> MailboxItems = new List<string>() { "Stick", "Sword", "Sword Upgrade", "Magic Dagger", "Magic Wand", "Magic Orb", "Lantern", "Gun", "Scavenger Mask", "Pages 24-25 (Prayer)", "Pages 42-43 (Holy Cross)" };
            Dictionary<string, ArchipelagoItem> SphereOnePlayer = new Dictionary<string, ArchipelagoItem>();
            Dictionary<string, ArchipelagoItem> SphereOneOthers = new Dictionary<string, ArchipelagoItem>();
            foreach(string itemkey in ItemLookup.ItemList.Keys) {
                ArchipelagoItem item = ItemLookup.ItemList[itemkey];
                if (Archipelago.instance.GetPlayerGame(item.Player) == "Tunic" && MailboxItems.Contains(item.ItemName) && Locations.VanillaLocations[itemkey].Location.RequiredItems.Count == 0) {
                    SphereOnePlayer.Add(itemkey, item);
                }
                if (item.Player != Archipelago.instance.GetPlayerSlot() && item.Classification == ItemFlags.Advancement && Locations.VanillaLocations[itemkey].Location.RequiredItems.Count == 0) {
                    SphereOneOthers.Add(itemkey, item);
                }
            }
            ArchipelagoItem mailboxitem = null;
            string key = "";
            if (SphereOnePlayer.Count > 0) {
                key = SphereOnePlayer.Keys.ToList()[random.Next(SphereOnePlayer.Count)];
                mailboxitem = SphereOnePlayer[key];
                MailboxHintId = key;
            } else if (SphereOneOthers.Count > 0) {
                key = SphereOneOthers.Keys.ToList()[random.Next(SphereOneOthers.Count)];
                mailboxitem = SphereOneOthers[key];
                MailboxHintId = key;
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
            Hint = $"lehjehnd sehz <#FF00FF>suhm%i^ ehkstruhordinArE<#FFFFFF>";
            if (Hyperdash.Player == Player) {
                Scene = Hyperdash.Location == "Your Pocket" ? Hyperdash.Location.ToUpper() : Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[Hyperdash.Location]].Location.SceneName].ToUpper();
                Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                Hint += $"\nuhwAts yoo in {Prefix} \"{Scene}...\"";
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
                Hints.HintMessages.Add(HintGrave, Hint);

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
                } else if (Archipelago.instance.GetPlayerGame((int)HexHint.Player) == "Tunic") {
                    Scene = Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[HexHint.Location]].Location.SceneName].ToUpper();
                    Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                    Hint = $"#A sA \"{Archipelago.instance.GetPlayerName((int)HexHint.Player).ToUpper()}'S\"\n\"{Scene}\"\niz wAr #uh {HexagonColors[Hexagon]}kwehstuhgawn [hexagram]<#FFFFFF> iz fownd\"...\"";
                } else {
                    Hint = $"#A sA #uh {HexagonColors[Hexagon]}kwehstuhgawn [hexagram]<#FFFFFF> iz fownd aht\n\"{HexHint.Location.ToUpper()}\"\nin \"{Archipelago.instance.GetPlayerName((int)HexHint.Player).ToUpper()}'S WORLD...\"";
                }
                
                Hints.HintMessages.Add(HexagonHintArea, Hint);

                Hexagons.Remove(Hexagon);
                HexagonHintGraves.Remove(HexagonHintArea);
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

    }
}
