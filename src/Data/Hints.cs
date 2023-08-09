using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using static TunicArchipelago.GhostHints;

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

        public static void PopulateHints() {
            HintMessages.Clear();
            System.Random random = new System.Random(SaveFile.GetInt("seed"));
            string Hint = "";
            string Scene = "";
            string Prefix = "";
            List<char> Vowels = new List<char>() { 'A', 'E', 'I', 'O', 'U' };

            int Player = Archipelago.instance.GetPlayerSlot();

            ArchipelagoHint Lantern = Locations.MajorItemLocations["Lantern"][0];
            Scene = Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[Lantern.Location]].Location.SceneName].ToUpper();
            Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
            if (Lantern.Player == Player) {
                Hint = $"lehjehnd sehz {Prefix} \"{Scene.ToUpper()}\"\nwil hehlp yoo \"<#00FFFF>LIGHT THE WAY<#ffffff>...\"";
            } else {
                Hint = $"lehjehnd sehz \"{Archipelago.instance.GetPlayerName((int)Lantern.Player).ToUpper()}'S WORLD\"\nwil hehlp yoo \"<#00FFFF>LIGHT THE WAY<#ffffff>...\"";
            }
            HintMessages.Add("Mailbox", Hint);

            ArchipelagoHint Hyperdash = Locations.MajorItemLocations["Hero's Laurels"][0];
            Hint = $"lehjehnd sehz <#FF00FF>suhm%i^ ehkstruhordinArE<#FFFFFF>\nuhwAts yoo in ";
            if (Hyperdash.Player == Player) {
                Scene = Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[Hyperdash.Location]].Location.SceneName].ToUpper();
                Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                Hint += $"{Prefix} \"{Scene}...\"";
            } else {
                Hint += $"\"{Archipelago.instance.GetPlayerName((int)Hyperdash.Player).ToUpper()}'S WORLD...\"";  
            }
            HintMessages.Add("Temple Statue", Hint);

            List<string> HintItems = new List<string>() { "Magic Wand", "Magic Orb", "Magic Dagger" };
            if (SaveFile.GetInt("randomizer shuffled abilities") == 1 && SaveFile.GetInt("randomizer hexagon quest enabled") == 0) {
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
                    Scene = Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[ItemHint.Location]].Location.SceneName].ToUpper();
                    if (HintItem == "Pages 24-25 (Prayer)" && Scene == "Fortress Relic") {
                        continue;
                    }
                    Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                    Hint = $"lehjehnd sehz {Prefix} \"{Scene}\"";
                } else {
                    Hint = $"lehjehnd sehz \"{Archipelago.instance.GetPlayerName((int)ItemHint.Player).ToUpper()}'S WORLD...\"";
                }
                Hint += $"\niz lOkAtid awn #uh \"<#ffd700>PATH OF THE HERO<#ffffff>...\"";
                Hints.HintMessages.Add(HintGrave, Hint);

                HintItems.Remove(HintItem);
                HintGraves.Remove(HintGrave);
            }

            List<string> Hexagons;
            Dictionary<string, string> HexagonColors = new Dictionary<string, string>() { { "Red Hexagon", "<#FF3333>" }, { "Green Hexagon", "<#33FF33>" }, { "Blue Hexagon", "<#3333FF>" }, { "Gold Hexagon", "<#ffd700>" } };
            if (SaveFile.GetInt("randomizer hexagon quest enabled") == 1) {
                Hexagons = new List<string>() { "Gold Hexagon", "Gold Hexagon", "Gold Hexagon" };
            } else {
                Hexagons = new List<string>() { "Red Hexagon", "Green Hexagon", "Blue Hexagon" };
            }

            List<string> HexagonHintGraves = new List<string>() { "Swamp Relic", "Library Relic", "Monastery Relic" };
            for (int i = 0; i < 3; i++) {
                string Hexagon = Hexagons[random.Next(Hexagons.Count)];
                string HexagonHintArea = HexagonHintGraves[random.Next(HexagonHintGraves.Count)];
                ArchipelagoHint HexHint = Locations.MajorItemLocations[Hexagon][0];

                if (HexHint.Player == Player) {
                    Scene = Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[HexHint.Location]].Location.SceneName].ToUpper();
                    Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                    Hint = $"#A sA {Prefix} \"{Scene.ToUpper()}\" iz \nwAr #uh {HexagonColors[Hexagon]}kwehstuhgawn [hexagram]<#FFFFFF> iz fownd\"...\"";
                } else {
                    Hint = $"#A sA \"{Archipelago.instance.GetPlayerName((int)HexHint.Player).ToUpper()}'S WORLD\" iz \nwAr #uh {HexagonColors[Hexagon]}kwehstuhgawn [hexagram]<#FFFFFF> iz fownd\"...\"";
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
