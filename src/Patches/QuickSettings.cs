using BepInEx.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TunicArchipelago {
    public class QuickSettings : MonoBehaviour {

        private static ManualLogSource Logger = TunicArchipelago.Logger;

        public static int CustomSeed = 0;
        public static Font OdinRounded;

        private void OnGUI() {
            if (SceneLoaderPatches.SceneName == "TitleScreen" && GameObject.FindObjectOfType<TitleScreen>() != null) {
                GUI.skin.font = PaletteEditor.OdinRounded == null ? GUI.skin.font : PaletteEditor.OdinRounded;
                Cursor.visible = true;
                GUI.Window(101, new Rect(20f, 150f, 430f, 410f), new Action<int>(QuickSettingsWindow), "Quick Settings");
            }
        }

        private static void QuickSettingsWindow(int windowID) {
            GUI.skin.label.fontSize = 25;
            GUI.skin.button.fontSize = 20;
            GUI.skin.toggle.fontSize = 20;
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUI.skin.label.clipping = TextClipping.Overflow;
            GUI.color = Color.white;
            GUI.DragWindow(new Rect(500f, 50f, 500f, 30f));
            GUI.Label(new Rect(10f, 20f, 500f, 30f), $"Player: {TunicArchipelago.Settings.ConnectionSettings.Player}");
            GUI.Label(new Rect(10f, 60f, 80f, 30f), $"Status:");
            if (Archipelago.instance.integration.connected) {
                GUI.color = Color.green;
                GUI.Label(new Rect(95f, 60f, 150f, 30f), $"Connected!");
                GUI.color = Color.white;
                GUI.Label(new Rect(250f, 60f, 300f, 30f), $"(world {Archipelago.instance.integration.session.ConnectionInfo.Slot} of {Archipelago.instance.integration.session.Players.Players[0].Count-1})");
            } else {
                GUI.color = Color.red;
                GUI.Label(new Rect(95f, 60f, 300f, 30f), $"Disconnected");
            }
            GUI.color = Color.white;

            bool Connect = GUI.Button(new Rect(10f, 100f, 200f, 30f), "Connect");
            if (Connect) {
                Archipelago.instance.Connect();
            }
            
            bool Disconnect = GUI.Button(new Rect(220f, 100f, 200f, 30f), "Disconnect");
            if (Disconnect) {
                Archipelago.instance.Disconnect();
            }

            bool ReloadSettings = GUI.Button(new Rect(10f, 140f, 200f, 30f), "Reload Settings");
            if(ReloadSettings) {
                RefreshSettings();
            }

            if (Archipelago.instance.integration.connected) {
                Dictionary<string, object> slotData = Archipelago.instance.GetPlayerSlotData();
                GUI.Label(new Rect(10f, 180f, 500f, 30f), $"World Settings");

                GUI.Toggle(new Rect(10f, 220f, 180f, 30f), slotData["keys_behind_bosses"].ToString() == "1", "Keys Behind Bosses");
                GUI.Toggle(new Rect(240f, 220f, 210f, 30f), slotData["sword_progression"].ToString() == "1", "Sword Progression");
                GUI.Toggle(new Rect(10f, 260f, 175f, 30f), slotData["start_with_sword"].ToString() == "1", "Start With Sword");
                GUI.Toggle(new Rect(240f, 260f, 175f, 30f), slotData["ability_shuffling"].ToString() == "1", "Shuffle Abilities");
            }

            GUI.Label(new Rect(10f, 300f, 200f, 30f), $"Other Settings");
            bool DeathLink = GUI.Toggle(new Rect(10f, 340f, 125f, 30f), TunicArchipelago.Settings.DeathLinkEnabled, "Death Link");
            TunicArchipelago.Settings.DeathLinkEnabled = DeathLink;
            GUI.skin.label.fontSize = 20;
            GUI.Label(new Rect(10f, 370f, 500f, 30f), $"More settings in options menu!");

            //bool Disconnect = GUI.Toggle(new Rect(10f, 60f, 125f, 30f), TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.RANDOMIZER, "Play");
            /*bool ToggleRandomizer = GUI.Toggle(new Rect(10f, 60f, 125f, 30f), TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.RANDOMIZER, "Play");
            if (ToggleRandomizer) {
                TunicRandomizer.Settings.GameMode = RandomizerSettings.GameModes.RANDOMIZER;
            }
            bool ToggleHexagonQuest = GUI.Toggle(new Rect(140f, 60f, 175f, 30f), TunicRandomizer.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST, "Hexagon Quest");

            if (ToggleHexagonQuest) {
                TunicRandomizer.Settings.GameMode = RandomizerSettings.GameModes.HEXAGONQUEST;
            }
            GUI.Label(new Rect(10f, 95f, 200f, 30f), "Logic Settings");
            bool TopggleBossKeys = GUI.Toggle(new Rect(10f, 140f, 180f, 30f), TunicRandomizer.Settings.KeysBehindBosses, "Keys Behind Bosses");
            TunicRandomizer.Settings.KeysBehindBosses = TopggleBossKeys;
            bool ToggleSwordProgression = GUI.Toggle(new Rect(240f, 140f, 180f, 30f), TunicRandomizer.Settings.SwordProgressionEnabled, "Sword Progression");
            TunicRandomizer.Settings.SwordProgressionEnabled = ToggleSwordProgression;
            bool ToggleSwordStart = GUI.Toggle(new Rect(10f, 180f, 175f, 30f), TunicRandomizer.Settings.StartWithSwordEnabled, "Start With Sword");
            TunicRandomizer.Settings.StartWithSwordEnabled = ToggleSwordStart;
            bool ToggleAbilityShuffle = GUI.Toggle(new Rect(240f, 180f, 175f, 30f), TunicRandomizer.Settings.ShuffleAbilities, "Shuffle Abilities");
            TunicRandomizer.Settings.ShuffleAbilities = ToggleAbilityShuffle;
            GUI.skin.button.fontSize = 20;
            GUI.Label(new Rect(10f, 220f, 300f, 30f), $"Custom Seed: {(CustomSeed == 0 ? "Not Set" : CustomSeed.ToString())}");

            bool GenerateSeed = GUI.Button(new Rect(10f, 260f, 200f, 30f), "Generate Seed");
            if (GenerateSeed) {
                CustomSeed = new System.Random().Next();
            }
            bool PasteSeed = GUI.Button(new Rect(220f, 260f, 200f, 30f), "Paste Seed");
            if (PasteSeed) {
                try {
                    CustomSeed = int.Parse(GUIUtility.systemCopyBuffer, CultureInfo.InvariantCulture);
                } catch (System.Exception e) {

                }
            }
            bool CopySettings = GUI.Button(new Rect(10f, 300f, 200f, 30f), "Copy Seed + Settings");
            if (CopySettings) {
                CopyQuickSettings();
            }
            bool PasteSettings = GUI.Button(new Rect(220f, 300f, 200f, 30f), "Paste Seed + Settings");
            if (PasteSettings) {
                PasteQuickSettings();
            }

            if (CustomSeed != 0) {
                bool ClearSeed = GUI.Button(new Rect(300f, 220f, 110f, 30f), "Clear");
                if (ClearSeed) {
                    CustomSeed = 0;
                }
            }*/
        }

        private static void RefreshSettings() {
            RandomizerSettings settings = JsonConvert.DeserializeObject<RandomizerSettings>(File.ReadAllText(TunicArchipelago.SettingsPath));
            if (settings.ConnectionSettings.Player != TunicArchipelago.Settings.ConnectionSettings.Player || settings.ConnectionSettings.Hostname != TunicArchipelago.Settings.ConnectionSettings.Hostname
                || settings.ConnectionSettings.Port != TunicArchipelago.Settings.ConnectionSettings.Port) {
                Archipelago.instance.Disconnect();
            }
            TunicArchipelago.Settings.ConnectionSettings = settings.ConnectionSettings;
        }
    }
}
