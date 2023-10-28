using BepInEx.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TunicArchipelago {
    public class QuickSettings : MonoBehaviour {

        private static ManualLogSource Logger = TunicArchipelago.Logger;

        public static int CustomSeed = 0;
        public static Font OdinRounded;
        public static List<string> FoolChoices = new List<string>() { "Off", "Normal", "Double", "<size=19>Onslaught</size>" };
        public static List<string> FoolColors = new List<string>() { "white", "#4FF5D4", "#E3D457", "#FF3333" };
        private static float height = 450f;

        private void OnGUI() {
            if (SceneLoaderPatches.SceneName == "TitleScreen" && GameObject.FindObjectOfType<TitleScreen>() != null) {
                GUI.skin.font = PaletteEditor.OdinRounded == null ? GUI.skin.font : PaletteEditor.OdinRounded;
                Cursor.visible = true;
                GUI.Window(101, new Rect(20f, 150f, 430f, height), new Action<int>(QuickSettingsWindow), "Quick Settings");
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
            if (Archipelago.instance.integration != null && Archipelago.instance.integration.connected) {
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

            bool OpenSettings = GUI.Button(new Rect(10f, 140f, 200f, 30f), "Open Settings File");
            if (OpenSettings) {
                try {
                    System.Diagnostics.Process.Start(TunicArchipelago.SettingsPath);
                } catch (Exception e) {
                    Logger.LogError(e);
                }
            }

            GUI.Label(new Rect(10f, 180f, 200f, 30f), $"World Settings");
            float y = 340f;
            if (Archipelago.instance.integration != null && Archipelago.instance.integration.connected) {
                Dictionary<string, object> slotData = Archipelago.instance.GetPlayerSlotData();
                GUI.Toggle(new Rect(10f, 220f, 180f, 30f), slotData["keys_behind_bosses"].ToString() == "1", "Keys Behind Bosses");
                GUI.Toggle(new Rect(220f, 220f, 210f, 30f), slotData["sword_progression"].ToString() == "1", "Sword Progression");
                GUI.Toggle(new Rect(10f, 260f, 175f, 30f), slotData["start_with_sword"].ToString() == "1", "Start With Sword");
                GUI.Toggle(new Rect(220f, 260f, 175f, 30f), slotData["ability_shuffling"].ToString() == "1", "Shuffle Abilities");
                GUI.Toggle(new Rect(10f, 300f, 185f, 30f), slotData["hexagon_quest"].ToString() == "1", slotData["hexagon_quest"].ToString() == "1" ? 
                    $"Hexagon Quest (<color=#E3D457>{slotData["Hexagon Quest Goal"].ToString()}</color>)" : $"Hexagon Quest");
                int FoolIndex = int.Parse(slotData["fool_traps"].ToString());
                GUI.Toggle(new Rect(220f, 300f, 195f, 60f), FoolIndex != 0, $"Fool Traps: {(FoolIndex == 0 ? "Off" : $"<color={FoolColors[FoolIndex]}>{FoolChoices[FoolIndex]}</color>")}");

                if (slotData.ContainsKey("entrance_rando")) {
                    height = 490f;
                    y += 40f;
                    GUI.Toggle(new Rect(10f, 340f, 195f, 30f), slotData["entrance_rando"].ToString() == "1", $"Entrance Randomizer");
                }
            } else {
                height = 450f;
                GUI.Toggle(new Rect(10f, 220f, 180f, 30f), false, "Keys Behind Bosses");
                GUI.Toggle(new Rect(220f, 220f, 210f, 30f), false, "Sword Progression");
                GUI.Toggle(new Rect(10f, 260f, 175f, 30f), false, "Start With Sword");
                GUI.Toggle(new Rect(220f, 260f, 175f, 30f), false, "Shuffle Abilities");
                GUI.Toggle(new Rect(10f, 300f, 175f, 30f), false, "Hexagon Quest");
                GUI.Toggle(new Rect(220f, 300f, 175f, 30f), false, "Fool Traps: Off");

            }

            GUI.Label(new Rect(10f, y, 200f, 30f), $"Other Settings");
            y += 40f;
            bool DeathLink = GUI.Toggle(new Rect(10f, y, 115f, 30f), TunicArchipelago.Settings.DeathLinkEnabled, "Death Link");
            TunicArchipelago.Settings.DeathLinkEnabled = DeathLink;
            bool EnemyRandomizer = GUI.Toggle(new Rect(150f, y, 180f, 30f), TunicArchipelago.Settings.EnemyRandomizerEnabled, "Enemy Randomizer");
            TunicArchipelago.Settings.EnemyRandomizerEnabled = EnemyRandomizer;
            GUI.skin.label.fontSize = 20;
            y += 30f;
            GUI.Label(new Rect(10f, y, 500f, 30f), $"More settings in options menu!");
        }

    }
}
