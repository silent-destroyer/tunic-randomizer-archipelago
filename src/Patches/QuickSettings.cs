using BepInEx.Logging;
using System;
using System.Collections.Generic;
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
        private static bool ShowAPSettingsWindow = false;
        private static string stringToEdit = "";
        private static bool editingPlayer = false;
        private static bool editingHostname = false;
        private static bool editingPort = false;
        private static bool editingPassword = false;
        private static bool showPort = false;
        private static bool showPassword = false;

        private void OnGUI() {
            if (SceneManager.GetActiveScene().name == "TitleScreen" && GameObject.FindObjectOfType<TitleScreen>() != null) {
                GUI.skin.font = PaletteEditor.OdinRounded == null ? GUI.skin.font : PaletteEditor.OdinRounded;
                Cursor.visible = true;
                GUI.Window(101, new Rect(20f, 150f, 430f, 490f), new Action<int>(QuickSettingsWindow), "Quick Settings");
                
                if (ShowAPSettingsWindow) {
                    GUI.Window(103, new Rect(460f, 150f, 350f, 490f), new Action<int>(QuickAPSettings), "Archipelago Config");
                }
            }
        }

        private void Update() {
            
            if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.Tab) && !Input.GetKeyDown(KeyCode.Backspace)) {

                if (editingPort && Input.inputString != "" && int.TryParse(Input.inputString, out int num)) {
                    stringToEdit += Input.inputString;
                } else if (!editingPort && Input.inputString != "") {
                    stringToEdit += Input.inputString;
                }
            }
            if (Input.GetKeyDown(KeyCode.Backspace)) {
                if (stringToEdit.Length >= 2) {
                    stringToEdit = stringToEdit.Substring(0, stringToEdit.Length - 1);
                } else {
                    stringToEdit = "";
                }
            }
            if (editingPlayer) {
                TunicArchipelago.Settings.ConnectionSettings.Player = stringToEdit;
            }
            if (editingHostname) { 
                TunicArchipelago.Settings.ConnectionSettings.Hostname = stringToEdit;
            }

            if (editingPort) {
                if (int.TryParse(stringToEdit, out int num)) {
                    TunicArchipelago.Settings.ConnectionSettings.Port = num;
                } else if (stringToEdit == "") {
                    TunicArchipelago.Settings.ConnectionSettings.Port = 0;
                }
            }
            if (editingPassword) {
                TunicArchipelago.Settings.ConnectionSettings.Password = stringToEdit;
            }
        }

        private static void QuickAPSettings(int windowID) {
            GUI.skin.label.fontSize = 25;
            GUI.skin.button.fontSize = 17;
            GUI.Label(new Rect(10f, 20f, 300f, 30f), $"Player: {TunicArchipelago.Settings.ConnectionSettings.Player}");
            bool EditPlayer = GUI.Button(new Rect(10f, 70f, 75f, 30f), editingPlayer ? "Save" : "Edit");
            if (EditPlayer) {
                if (editingPlayer) {
                    stringToEdit = "";
                    editingPlayer = false;
                    OptionsGUIPatches.SaveSettings();
                } else {
                    stringToEdit = TunicArchipelago.Settings.ConnectionSettings.Player;
                    editingPlayer = true;
                    editingHostname = false;
                    editingPort = false;
                    editingPassword = false;
                }
            }
            bool PastePlayer = GUI.Button(new Rect(100f, 70f, 75f, 30f), "Paste");
            if (PastePlayer) {
                TunicArchipelago.Settings.ConnectionSettings.Player = GUIUtility.systemCopyBuffer;
                editingPlayer = false;
                OptionsGUIPatches.SaveSettings();
            }

            GUI.Label(new Rect(10f, 120f, 300f, 30f), $"Host: {TunicArchipelago.Settings.ConnectionSettings.Hostname}");
            bool setLocalhost = GUI.Toggle(new Rect(160f, 160f, 90f, 30f), TunicArchipelago.Settings.ConnectionSettings.Hostname == "localhost", "localhost");
            if (setLocalhost && TunicArchipelago.Settings.ConnectionSettings.Hostname != "localhost") {
                TunicArchipelago.Settings.ConnectionSettings.Hostname = "localhost";
                OptionsGUIPatches.SaveSettings();
            }
            bool setArchipelagoHost = GUI.Toggle(new Rect(10f, 160f, 140f, 30f), TunicArchipelago.Settings.ConnectionSettings.Hostname == "archipelago.gg", "archipelago.gg");
            if (setArchipelagoHost && TunicArchipelago.Settings.ConnectionSettings.Hostname != "archipelago.gg") {
                TunicArchipelago.Settings.ConnectionSettings.Hostname = "archipelago.gg";
                OptionsGUIPatches.SaveSettings();
            }
            bool EditHostname = GUI.Button(new Rect(10f, 200f, 75f, 30f), editingHostname ? "Save" : "Edit");
            if (EditHostname) {
                if (editingHostname) {
                    stringToEdit = "";
                    editingHostname = false;
                    OptionsGUIPatches.SaveSettings();
                } else {
                    stringToEdit = TunicArchipelago.Settings.ConnectionSettings.Hostname;
                    editingPlayer = false;
                    editingHostname = true;
                    editingPort = false;
                    editingPassword = false;
                }
            }
            bool PasteHostname = GUI.Button(new Rect(100f, 200f, 75f, 30f), "Paste");
            if (PasteHostname) {
                TunicArchipelago.Settings.ConnectionSettings.Hostname = GUIUtility.systemCopyBuffer;
                editingHostname = false;
                OptionsGUIPatches.SaveSettings();
            }
            
            GUI.Label(new Rect(10f, 250f, 300f, 30f), $"Port: {(editingPort ? (showPort ? stringToEdit : new string('*', stringToEdit.Length)) : (showPort ? TunicArchipelago.Settings.ConnectionSettings.Port.ToString() : new string('*', TunicArchipelago.Settings.ConnectionSettings.Port.ToString().Length)))}");
            showPort = GUI.Toggle(new Rect(270f, 260f, 75f, 30f), showPort, "Show");
            bool EditPort = GUI.Button(new Rect(10f, 300f, 75f, 30f), editingPort ? "Save" : "Edit");
            if (EditPort) {
                if (editingPort) {
                    stringToEdit = "";
                    editingPort = false;
                    OptionsGUIPatches.SaveSettings();
                } else {
                    stringToEdit = TunicArchipelago.Settings.ConnectionSettings.Port.ToString();
                    editingPlayer = false;
                    editingHostname = false;
                    editingPort = true;
                    editingPassword = false;
                }
            }
            bool PastePort = GUI.Button(new Rect(100f, 300f, 75f, 30f), "Paste");
            if (PastePort) {
                try {
                    TunicArchipelago.Settings.ConnectionSettings.Port = int.Parse(GUIUtility.systemCopyBuffer);
                    editingPort = false;
                    OptionsGUIPatches.SaveSettings();
                } catch (Exception e) {
                    Logger.LogError("invalid input pasted for port number!");
                }
            }

            GUI.Label(new Rect(10f, 350f, 300f, 30f), $"Password: {(showPassword ? TunicArchipelago.Settings.ConnectionSettings.Password : new string('*', TunicArchipelago.Settings.ConnectionSettings.Password.Length))}");
            showPassword = GUI.Toggle(new Rect(270f, 360f, 75f, 30f), showPassword, "Show");
            bool EditPassword = GUI.Button(new Rect(10f, 400f, 75f, 30f), editingPassword ? "Save" : "Edit");
            if (EditPassword) {
                if (editingPassword) {
                    stringToEdit = "";
                    editingPassword = false;
                    OptionsGUIPatches.SaveSettings();
                } else {
                    stringToEdit = TunicArchipelago.Settings.ConnectionSettings.Password;
                    editingPlayer = false;
                    editingHostname = false;
                    editingPort = false;
                    editingPassword = true;
                }
            }

            bool PastePassword = GUI.Button(new Rect(100f, 400f, 75f, 30f), "Paste");
            if (PastePassword) {
                TunicArchipelago.Settings.ConnectionSettings.Password = GUIUtility.systemCopyBuffer;
                editingPassword = false;
                OptionsGUIPatches.SaveSettings();
            }
            bool ClearPassword = GUI.Button(new Rect(190f, 400f, 75f, 30f), "Clear");
            if (ClearPassword) {
                TunicArchipelago.Settings.ConnectionSettings.Password = "";
                OptionsGUIPatches.SaveSettings();
            }
            bool Close = GUI.Button(new Rect(10f, 450f, 165f, 30f), "Close");
            if (Close) {
                CloseAPSettingsWindow();
                Archipelago.instance.Connect();
            }

        }

        private static void CloseAPSettingsWindow() {
            ShowAPSettingsWindow = false;
            stringToEdit = "";
            editingPlayer = false;
            editingHostname = false;
            editingPort = false;
            editingPassword = false;
            OptionsGUIPatches.SaveSettings();
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
            bool OpenAPSettings = GUI.Button(new Rect(220f, 140f, 200f, 30f), ShowAPSettingsWindow ? "Close AP Config" : "Edit AP Config");
            if (OpenAPSettings) {
                if (ShowAPSettingsWindow) {
                    CloseAPSettingsWindow();
                    Archipelago.instance.Connect();
                } else {
                    Archipelago.instance.Disconnect();
                    ShowAPSettingsWindow = true;
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
                } else {
                    height = 490f;
                    y += 40f;
                    GUI.Toggle(new Rect(10f, 340f, 195f, 30f), false, $"Entrance Randomizer");
                }
            } else {
                height = 490f;
                y += 40f;
                GUI.Toggle(new Rect(10f, 220f, 180f, 30f), false, "Keys Behind Bosses");
                GUI.Toggle(new Rect(220f, 220f, 210f, 30f), false, "Sword Progression");
                GUI.Toggle(new Rect(10f, 260f, 175f, 30f), false, "Start With Sword");
                GUI.Toggle(new Rect(220f, 260f, 175f, 30f), false, "Shuffle Abilities");
                GUI.Toggle(new Rect(10f, 300f, 175f, 30f), false, "Hexagon Quest");
                GUI.Toggle(new Rect(220f, 300f, 175f, 30f), false, "Fool Traps: Off");
                GUI.Toggle(new Rect(10f, 340f, 195f, 30f), false, $"Entrance Randomizer");
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

        public static bool TitleScreen___NewGame_PrefixPatch(TitleScreen __instance) {
            CloseAPSettingsWindow();
            return true;
        }

        public static bool FileManagement_LoadFileAndStart_PrefixPatch(FileManagementGUI __instance, string filename) {
            CloseAPSettingsWindow();
            SaveFile.LoadFromFile(filename);
            if (SaveFile.GetInt("archipelago") == 0) {
                GenericMessage.ShowMessage("<#FF0000>[death] \"<#FF0000>warning!\" <#FF0000>[death]\n\"Non-Archipelago file selected.\"\n\"Returning to menu.\"");
                return false;
            }
            return true;
        }

    }

}
