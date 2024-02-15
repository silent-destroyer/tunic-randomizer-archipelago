using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TunicArchipelago {
    public class QuickSettings : MonoBehaviour {

        private static ManualLogSource Logger = TunicArchipelago.Logger;

        public static string CustomSeed = "";
        public static Font OdinRounded;
        public static List<string> FoolChoices = new List<string>() { "Off", "Normal", "Double", "<size=19>Onslaught</size>" };
        public static List<string> FoolColors = new List<string>() { "white", "#4FF5D4", "#E3D457", "#FF3333" };
        private static bool ShowAdvancedSinglePlayerOptions = false;
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
                switch (TunicArchipelago.Settings.Mode) {
                    case RandomizerSettings.RandomizerType.SINGLEPLAYER:
                        GUI.Window(101, new Rect(20f, 150f, 430f, TunicArchipelago.Settings.MysterySeed ? 430f : 510f), new Action<int>(SinglePlayerQuickSettingsWindow), "Single Player Settings");
                        ShowAPSettingsWindow = false;
                        editingPlayer = false;
                        editingHostname = false;
                        editingPort = false;
                        editingPassword = false;
                        break;
                    case RandomizerSettings.RandomizerType.ARCHIPELAGO:
                        GUI.Window(101, new Rect(20f, 150f, 430f, 540f), new Action<int>(ArchipelagoQuickSettingsWindow), "Archipelago Settings");
                        break;
                }

                if (ShowAPSettingsWindow && TunicArchipelago.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO) {
                    GUI.Window(103, new Rect(460f, 150f, 350f, 490f), new Action<int>(ArchipelagoConfigEditorWindow), "Archipelago Config");
                }
                if (ShowAdvancedSinglePlayerOptions && TunicArchipelago.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER && !TunicArchipelago.Settings.MysterySeed) {
                    GUI.Window(105, new Rect(460f, 150f, 405f, 485f), new Action<int>(AdvancedLogicOptionsWindow), "Advanced Logic Options");
                }
                GameObject.Find("elderfox_sword graphic").GetComponent<Renderer>().enabled = !ShowAdvancedSinglePlayerOptions && !ShowAPSettingsWindow;
            }
        }

        private void Update() {
            if (TunicArchipelago.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO && ShowAPSettingsWindow && SceneManager.GetActiveScene().name == "TitleScreen") {
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
                    TunicArchipelago.Settings.ConnectionSettings.Port = stringToEdit;
                }
                if (editingPassword) {
                    TunicArchipelago.Settings.ConnectionSettings.Password = stringToEdit;
                }
            }
        }

        private static void ArchipelagoQuickSettingsWindow(int windowID) {
            GUI.skin.label.fontSize = 25;
            GUI.skin.button.fontSize = 20;
            GUI.skin.toggle.fontSize = 20;
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUI.skin.label.clipping = TextClipping.Overflow;
            GUI.color = Color.white;
            GUI.DragWindow(new Rect(500f, 50f, 500f, 30f));

            float y = 20f;

            GUI.skin.toggle.fontSize = 15; 
            GUI.skin.button.fontSize = 15;
            if (TunicArchipelago.Settings.RaceMode) {
                TunicArchipelago.Settings.RaceMode = GUI.Toggle(new Rect(330f, y, 90f, 30f), TunicArchipelago.Settings.RaceMode, "Race Mode");
            } else {
                bool ToggleSpoilerLog = GUI.Toggle(new Rect(TunicArchipelago.Settings.CreateSpoilerLog ? 280f : 330f, y, 90f, 30f), TunicArchipelago.Settings.CreateSpoilerLog, "Spoiler Log");
                TunicArchipelago.Settings.CreateSpoilerLog = ToggleSpoilerLog;
                if (ToggleSpoilerLog) {
                    GUI.skin.button.fontSize = 15;
                    bool OpenSpoilerLog = GUI.Button(new Rect(370f, y, 50f, 25f), "Open");
                    if (OpenSpoilerLog) {
                        if (File.Exists(TunicArchipelago.SpoilerLogPath)) {
                            System.Diagnostics.Process.Start(TunicArchipelago.SpoilerLogPath);
                        }
                    }
                }
            }

            GUI.skin.toggle.fontSize = 20;
            GUI.skin.button.fontSize = 20;

            GUI.Label(new Rect(10f, 20f, 300f, 30f), "Randomizer Mode");
            y += 40f;
            bool ToggleSinglePlayer = GUI.Toggle(new Rect(10f, y, 130f, 30f), TunicArchipelago.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER, "Single Player");
            if (ToggleSinglePlayer && TunicArchipelago.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO) {
                TunicArchipelago.Settings.Mode = RandomizerSettings.RandomizerType.SINGLEPLAYER;
                OptionsGUIPatches.SaveSettings();
            }
            bool ToggleArchipelago = GUI.Toggle(new Rect(150f, y, 150f, 30f), TunicArchipelago.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO, "Archipelago");
            if (ToggleArchipelago && TunicArchipelago.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER) {
                TunicArchipelago.Settings.Mode = RandomizerSettings.RandomizerType.ARCHIPELAGO;
                OptionsGUIPatches.SaveSettings();
            }
            y += 40f;
            GUI.Label(new Rect(10f, y, 500f, 30f), $"Player: {(TunicArchipelago.Settings.ConnectionSettings.Player)}");
            y += 40f;
            GUI.Label(new Rect(10f, y, 80f, 30f), $"Status:");
            if (Archipelago.instance.integration != null && Archipelago.instance.integration.connected) {
                GUI.color = Color.green;
                GUI.Label(new Rect(95f, y, 150f, 30f), $"Connected!");
                GUI.color = Color.white;
                GUI.Label(new Rect(250f, y, 300f, 30f), $"(world {Archipelago.instance.integration.session.ConnectionInfo.Slot} of {Archipelago.instance.integration.session.Players.Players[0].Count-1})");
            } else {
                GUI.color = Color.red;
                GUI.Label(new Rect(95f, y, 300f, 30f), $"Disconnected");
            }
            GUI.color = Color.white;
            y += 40f;
            bool Connect = GUI.Button(new Rect(10f, y, 200f, 30f), "Connect");
            if (Connect) {
                Archipelago.instance.Connect();
            }

            bool Disconnect = GUI.Button(new Rect(220f, y, 200f, 30f), "Disconnect");
            if (Disconnect) {
                Archipelago.instance.Disconnect();
            }
            y += 40f;
            bool OpenSettings = GUI.Button(new Rect(10f, y, 200f, 30f), "Open Settings File");
            if (OpenSettings) {
                try {
                    System.Diagnostics.Process.Start(TunicArchipelago.SettingsPath);
                } catch (Exception e) {
                    Logger.LogError(e);
                }
            }
            bool OpenAPSettings = GUI.Button(new Rect(220f, y, 200f, 30f), ShowAPSettingsWindow ? "Close AP Config" : "Edit AP Config");
            if (OpenAPSettings) {
                if (ShowAPSettingsWindow) {
                    CloseAPSettingsWindow();
                    Archipelago.instance.Connect();
                } else {
                    ShowAPSettingsWindow = true;
                }
            }
            y += 40f;
            GUI.Label(new Rect(10f, y, 200f, 30f), $"World Settings");
            if (Archipelago.instance.integration != null && Archipelago.instance.integration.connected) {
                Dictionary<string, object> slotData = Archipelago.instance.GetPlayerSlotData();
                y += 40f;
                GUI.Toggle(new Rect(10f, y, 180f, 30f), slotData["keys_behind_bosses"].ToString() == "1", "Keys Behind Bosses");
                GUI.Toggle(new Rect(220f, y, 210f, 30f), slotData["sword_progression"].ToString() == "1", "Sword Progression");
                y += 40f;
                GUI.Toggle(new Rect(10f, y, 175f, 30f), slotData["start_with_sword"].ToString() == "1", "Start With Sword");
                GUI.Toggle(new Rect(220f, y, 175f, 30f), slotData["ability_shuffling"].ToString() == "1", "Shuffled Abilities");
                y += 40f;
                GUI.Toggle(new Rect(10f, y, 185f, 30f), slotData["hexagon_quest"].ToString() == "1", slotData["hexagon_quest"].ToString() == "1" ? 
                    $"Hexagon Quest (<color=#E3D457>{slotData["Hexagon Quest Goal"].ToString()}</color>)" : $"Hexagon Quest");
                int FoolIndex = int.Parse(slotData["fool_traps"].ToString());
                GUI.Toggle(new Rect(220f, y, 195f, 60f), FoolIndex != 0, $"Fool Traps: {(FoolIndex == 0 ? "Off" : $"<color={FoolColors[FoolIndex]}>{FoolChoices[FoolIndex]}</color>")}");

                if (slotData.ContainsKey("entrance_rando")) {
                    y += 40f;
                    GUI.Toggle(new Rect(10f, y, 195f, 30f), slotData["entrance_rando"].ToString() == "1", $"Entrance Randomizer");
                } else {
                    y += 40f;
                    GUI.Toggle(new Rect(10f, y, 195f, 30f), false, $"Entrance Randomizer");
                }
            } else {
                y += 40f;
                GUI.Toggle(new Rect(10f, y, 180f, 30f), false, "Keys Behind Bosses");
                GUI.Toggle(new Rect(220f, y, 210f, 30f), false, "Sword Progression");
                y += 40f;
                GUI.Toggle(new Rect(10f, y, 175f, 30f), false, "Start With Sword");
                GUI.Toggle(new Rect(220f, y, 175f, 30f), false, "Shuffled Abilities");
                y += 40f;
                GUI.Toggle(new Rect(10f, y, 175f, 30f), false, "Hexagon Quest");
                GUI.Toggle(new Rect(220f, y, 175f, 30f), false, "Fool Traps: Off");
                y += 40f;
                GUI.Toggle(new Rect(10f, y, 195f, 30f), false, $"Entrance Randomizer");
            }
            y += 40f;
            GUI.Label(new Rect(10f, y, 400f, 30f), "Other Settings <size=18>(more in options menu!)</size>");
            y += 40f;
            bool DeathLink = GUI.Toggle(new Rect(10f, y, 115f, 30f), TunicArchipelago.Settings.DeathLinkEnabled, "Death Link");
            TunicArchipelago.Settings.DeathLinkEnabled = DeathLink;
            bool EnemyRandomizer = GUI.Toggle(new Rect(150f, y, 180f, 30f), TunicArchipelago.Settings.EnemyRandomizerEnabled, "Enemy Randomizer");
            TunicArchipelago.Settings.EnemyRandomizerEnabled = EnemyRandomizer;
            GUI.skin.label.fontSize = 20;
        }

        private static void SinglePlayerQuickSettingsWindow(int windowID) {
            GUI.skin.label.fontSize = 25;
            //GUI.skin.toggle.fontSize = 25;
            //GUI.skin.toggle.alignment = TextAnchor.UpperLeft;
            GUI.skin.toggle.fontSize = 20;
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUI.skin.label.clipping = TextClipping.Overflow;
            GUI.color = Color.white;
            GUI.DragWindow(new Rect(500f, 50f, 500f, 30f));
            float y = 20f;

            GUI.skin.toggle.fontSize = 15;
            if (TunicArchipelago.Settings.RaceMode) {
                TunicArchipelago.Settings.RaceMode = GUI.Toggle(new Rect(330f, y, 90f, 30f), TunicArchipelago.Settings.RaceMode, "Race Mode");
            } else {
                bool ToggleSpoilerLog = GUI.Toggle(new Rect(TunicArchipelago.Settings.CreateSpoilerLog ? 280f : 330f, y, 90f, 30f), TunicArchipelago.Settings.CreateSpoilerLog, "Spoiler Log");
                TunicArchipelago.Settings.CreateSpoilerLog = ToggleSpoilerLog;
                if (ToggleSpoilerLog) {
                    GUI.skin.button.fontSize = 15;
                    bool OpenSpoilerLog = GUI.Button(new Rect(370f, y, 50f, 25f), "Open");
                    if (OpenSpoilerLog) {
                        if (File.Exists(TunicArchipelago.SpoilerLogPath)) {
                            System.Diagnostics.Process.Start(TunicArchipelago.SpoilerLogPath);
                        }
                    }
                }
            }

            GUI.skin.toggle.fontSize = 20;

            GUI.Label(new Rect(10f, 20f, 300f, 30f), "Randomizer Mode");
            y += 40f;
            bool ToggleSinglePlayer = GUI.Toggle(new Rect(10f, y, 130f, 30f), TunicArchipelago.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER, "Single Player");
            if (ToggleSinglePlayer && TunicArchipelago.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO) {
                TunicArchipelago.Settings.Mode = RandomizerSettings.RandomizerType.SINGLEPLAYER;
                OptionsGUIPatches.SaveSettings();
            }
            bool ToggleArchipelago = GUI.Toggle(new Rect(150f, y, 150f, 30f), TunicArchipelago.Settings.Mode == RandomizerSettings.RandomizerType.ARCHIPELAGO, "Archipelago");
            if (ToggleArchipelago && TunicArchipelago.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER) {
                TunicArchipelago.Settings.Mode = RandomizerSettings.RandomizerType.ARCHIPELAGO;
                OptionsGUIPatches.SaveSettings();
            }

            GUI.skin.toggle.fontSize = 20;
            y += 40f;
            GUI.Label(new Rect(10f, y, 200f, 30f), "Logic Settings");
            TunicArchipelago.Settings.MysterySeed = GUI.Toggle(new Rect(240f, y+5, 200f, 30f), TunicArchipelago.Settings.MysterySeed, "Mystery Seed");
            y += 45f; 
            if (TunicArchipelago.Settings.MysterySeed) {
                GUI.Label(new Rect(10f, y, 400f, 30f), "Mystery Seed Enabled!");
                GUI.skin.label.fontSize = 20;
                y += 40f;
                GUI.Label(new Rect(10f, y, 400f, 30f), "Settings will be chosen randomly on New Game.");
                y += 40f;
            } else {
                bool ToggleHexagonQuest = GUI.Toggle(new Rect(10f, y, 175f, 30f), TunicArchipelago.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST, "Hexagon Quest");
                if (ToggleHexagonQuest) {
                    TunicArchipelago.Settings.GameMode = RandomizerSettings.GameModes.HEXAGONQUEST;
                } else if (!ToggleHexagonQuest && TunicArchipelago.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST) {
                    TunicArchipelago.Settings.GameMode = RandomizerSettings.GameModes.RANDOMIZER;
                }
                TunicArchipelago.Settings.SwordProgressionEnabled = GUI.Toggle(new Rect(240f, y, 180f, 30f), TunicArchipelago.Settings.SwordProgressionEnabled, "Sword Progression");
                y += 40f; 
                TunicArchipelago.Settings.KeysBehindBosses = GUI.Toggle(new Rect(10f, y, 200f, 30f), TunicArchipelago.Settings.KeysBehindBosses, "Keys Behind Bosses");
                TunicArchipelago.Settings.ShuffleAbilities  = GUI.Toggle(new Rect(240f, y, 175f, 30f), TunicArchipelago.Settings.ShuffleAbilities, "Shuffle Abilities");
                y += 40f;
                TunicArchipelago.Settings.EntranceRandoEnabled = GUI.Toggle(new Rect(10f, y, 200f, 30f), TunicArchipelago.Settings.EntranceRandoEnabled, "Entrance Randomizer");
                TunicArchipelago.Settings.StartWithSwordEnabled = GUI.Toggle(new Rect(240f, y, 175f, 30f), TunicArchipelago.Settings.StartWithSwordEnabled, "Start With Sword");

                y += 40f;
                GUI.skin.button.fontSize = 20;
                bool ShowAdvancedOptions = GUI.Button(new Rect(10f, y, 410f, 30f), $"{(ShowAdvancedSinglePlayerOptions ? "Hide" : "Show")} Advanced Options");
                if (ShowAdvancedOptions) {
                    ShowAdvancedSinglePlayerOptions = !ShowAdvancedSinglePlayerOptions;
                }
                y += 40f;

            }
            GUI.Label(new Rect(10f, y, 400f, 30f), "Other Settings <size=18>(more in options menu!)</size>");
            y += 40f;
            TunicArchipelago.Settings.EnemyRandomizerEnabled = GUI.Toggle(new Rect(10f, y, 200f, 30f), TunicArchipelago.Settings.EnemyRandomizerEnabled, "Enemy Randomizer");
            GUI.skin.button.fontSize = 20;
            y += 40f;
            GUI.Label(new Rect(10f, y, 300f, 30f), $"Custom Seed: {(CustomSeed == "" ? "Not Set" : CustomSeed.ToString())}");
            if (CustomSeed != "") {
                bool ClearSeed = GUI.Button(new Rect(300f, y, 110f, 30f), "Clear");
                if (ClearSeed) {
                    CustomSeed = "";
                }
            }
            y += 40f;
            bool GenerateSeed = GUI.Button(new Rect(10f, y, 200f, 30f), "Generate Seed");
            if (GenerateSeed) {
                CustomSeed = new System.Random().Next().ToString();
            }

            bool PasteSeed = GUI.Button(new Rect(220f, y, 200f, 30f), "Paste Seed");
            if (PasteSeed) {
                try {
                    CustomSeed = int.Parse(GUIUtility.systemCopyBuffer, CultureInfo.InvariantCulture).ToString();
                } catch (System.Exception e) {
                    Logger.LogError("Invalid custom seed pasted!");
                }
            }
            y += 40f;
            bool CopySettings = GUI.Button(new Rect(10f, y, 200f, 30f), "Copy Seed + Settings");
            if (CopySettings) {
                CopyQuickSettings();
            }
            bool PasteSettings = GUI.Button(new Rect(220f, y, 200f, 30f), "Paste Seed + Settings");
            if (PasteSettings) {
                PasteQuickSettings();
            }
        }

        private static void AdvancedLogicOptionsWindow(int windowID) {
            GUI.skin.label.fontSize = 25;
            float y = 20f;
            GUI.Label(new Rect(10f, y, 300f, 30f), $"Hexagon Quest");
            y += 30;
            GUI.Label(new Rect(10f, y, 220f, 20f), $"<size=18>Hexagons Required:</size>");
            GUI.Label(new Rect(190f, y, 30f, 30f), $"<size=18>{(TunicArchipelago.Settings.HexagonQuestGoal)}</size>");
            TunicArchipelago.Settings.HexagonQuestGoal = (int)GUI.HorizontalSlider(new Rect(220f, y + 15, 175f, 20f), TunicArchipelago.Settings.HexagonQuestGoal, 15, 50);
            y += 30f;
            GUI.Label(new Rect(10f, y, 220f, 30f), $"<size=18>Hexagons in Item Pool:</size>");
            GUI.Label(new Rect(190f, y, 30f, 30f), $"<size=18>{((int)Math.Round((100f + TunicArchipelago.Settings.HexagonQuestExtraPercentage) / 100f * TunicArchipelago.Settings.HexagonQuestGoal))}</size>");
            TunicArchipelago.Settings.HexagonQuestExtraPercentage = (int)GUI.HorizontalSlider(new Rect(220f, y + 15, 175f, 30f), TunicArchipelago.Settings.HexagonQuestExtraPercentage, 0, 100);
            y += 40f;
            GUI.Label(new Rect(10f, y, 300f, 30f), $"Entrance Randomizer");
            y += 40f;
            TunicArchipelago.Settings.ERFixedShop = GUI.Toggle(new Rect(10f, y, 200f, 30f), TunicArchipelago.Settings.ERFixedShop, "Fewer Shop Entrances");
            y += 40f;
            GUI.Label(new Rect(10f, y, 300f, 30f), $"Fool Traps");
            y += 40f;
            bool NoFools = GUI.Toggle(new Rect(10f, y, 90f, 30f), TunicArchipelago.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.NONE, "None");
            if (NoFools) {
                TunicArchipelago.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.NONE;
            }
            bool NormalFools = GUI.Toggle(new Rect(110f, y, 90f, 30f), TunicArchipelago.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.NORMAL, "<color=#4FF5D4>Normal</color>");
            if (NormalFools) {
                TunicArchipelago.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.NORMAL;
            }
            bool DoubleFools = GUI.Toggle(new Rect(200f, y, 90f, 30f), TunicArchipelago.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.DOUBLE, "<color=#E3D457>Double</color>");
            if (DoubleFools) {
                TunicArchipelago.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.DOUBLE;
            }
            bool OnslaughtFools = GUI.Toggle(new Rect(290f, y, 100f, 30f), TunicArchipelago.Settings.FoolTrapIntensity == RandomizerSettings.FoolTrapOption.ONSLAUGHT, "<color=#FF3333>Onslaught</color>");
            if (OnslaughtFools) {
                TunicArchipelago.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.ONSLAUGHT;
            }
            y += 40f;
            GUI.Label(new Rect(10f, y, 300f, 30f), $"Hero's Laurels Location");
            y += 40f;
            bool RandomLaurels = GUI.Toggle(new Rect(10f, y, 90f, 30f), TunicArchipelago.Settings.FixedLaurelsOption == RandomizerSettings.FixedLaurelsType.RANDOM, "Random");
            if (RandomLaurels) {
                TunicArchipelago.Settings.FixedLaurelsOption = RandomizerSettings.FixedLaurelsType.RANDOM;
            }
            bool SixCoinsLaurels = GUI.Toggle(new Rect(110f, y, 90f, 30f), TunicArchipelago.Settings.FixedLaurelsOption == RandomizerSettings.FixedLaurelsType.SIXCOINS, "6 Coins");
            if (SixCoinsLaurels) {
                TunicArchipelago.Settings.FixedLaurelsOption = RandomizerSettings.FixedLaurelsType.SIXCOINS;
            }
            bool TenCoinsLaurels = GUI.Toggle(new Rect(200f, y, 90f, 30f), TunicArchipelago.Settings.FixedLaurelsOption == RandomizerSettings.FixedLaurelsType.TENCOINS, "10 Coins");
            if (TenCoinsLaurels) {
                TunicArchipelago.Settings.FixedLaurelsOption = RandomizerSettings.FixedLaurelsType.TENCOINS;
            }
            bool TenFairiesLaurels = GUI.Toggle(new Rect(290f, y, 100f, 30f), TunicArchipelago.Settings.FixedLaurelsOption == RandomizerSettings.FixedLaurelsType.TENFAIRIES, "10 Fairies");
            if (TenFairiesLaurels) {
                TunicArchipelago.Settings.FixedLaurelsOption = RandomizerSettings.FixedLaurelsType.TENFAIRIES;
            }
            y += 40f;
            GUI.Label(new Rect(10f, y, 300f, 30f), $"Difficulty Options");
            y += 40f;
            TunicArchipelago.Settings.Lanternless = GUI.Toggle(new Rect(10f, y, 175f, 30f), TunicArchipelago.Settings.Lanternless, "Lanternless Logic");
            TunicArchipelago.Settings.Maskless = GUI.Toggle(new Rect(195f, y, 175f, 30f), TunicArchipelago.Settings.Maskless, "Maskless Logic");
            y += 40f;
            bool Close = GUI.Button(new Rect(10f, y, 200f, 30f), "Close");
            if (Close) {
                ShowAdvancedSinglePlayerOptions = false;
                OptionsGUIPatches.SaveSettings();
            }
        }

        private static void ArchipelagoConfigEditorWindow(int windowID) {
            GUI.skin.label.fontSize = 25;
            GUI.skin.button.fontSize = 17;
            GUI.Label(new Rect(10f, 20f, 300f, 30f), $"Player: {(TunicArchipelago.Settings.ConnectionSettings.Player)}");
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
            bool ClearPlayer = GUI.Button(new Rect(190f, 70f, 75f, 30f), "Clear");
            if (ClearPlayer) {
                TunicArchipelago.Settings.ConnectionSettings.Player = "";
                OptionsGUIPatches.SaveSettings();
            }

            GUI.Label(new Rect(10f, 120f, 300f, 30f), $"Host: {(TunicArchipelago.Settings.ConnectionSettings.Hostname)}");
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
            bool ClearHost = GUI.Button(new Rect(190f, 200f, 75f, 30f), "Clear");
            if (ClearHost) {
                TunicArchipelago.Settings.ConnectionSettings.Hostname = "";
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
                    if (int.TryParse(GUIUtility.systemCopyBuffer, out int num)) {
                        TunicArchipelago.Settings.ConnectionSettings.Port = GUIUtility.systemCopyBuffer;
                    }
                    editingPort = false;
                    OptionsGUIPatches.SaveSettings();
                } catch (Exception e) {
                    Logger.LogError("invalid input pasted for port number!");
                }
            }
            bool ClearPort = GUI.Button(new Rect(190f, 300f, 75f, 30f), "Clear");
            if (ClearPort) {
                TunicArchipelago.Settings.ConnectionSettings.Port = "";
                OptionsGUIPatches.SaveSettings();
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

        public static void CopyQuickSettings() {
            if (TunicArchipelago.Settings.MysterySeed) {
                GUIUtility.systemCopyBuffer = $"{CustomSeed},mystery_seed";
                return;
            }
            List<string> Settings = new List<string>() { CustomSeed.ToString(), Enum.GetName(typeof(RandomizerSettings.GameModes), TunicArchipelago.Settings.GameMode).ToLower() };
            if (TunicArchipelago.Settings.GameMode == RandomizerSettings.GameModes.HEXAGONQUEST) {
                Settings.Add($"hexagon_quest_goal={(TunicArchipelago.Settings.HexagonQuestGoal)}=");
                Settings.Add($"hexagon_quest_extras~{(TunicArchipelago.Settings.HexagonQuestExtraPercentage)}~");
            }
            if (TunicArchipelago.Settings.KeysBehindBosses) {
                Settings.Add("keys_behind_bosses");
            }
            if (TunicArchipelago.Settings.SwordProgressionEnabled) {
                Settings.Add("sword_progression");
            }
            if (TunicArchipelago.Settings.StartWithSwordEnabled) {
                Settings.Add("start_with_sword");
            }
            if (TunicArchipelago.Settings.ShuffleAbilities) {
                Settings.Add("shuffle_abilities");
            }
            if (TunicArchipelago.Settings.EntranceRandoEnabled) {
                Settings.Add("entrance_randomizer");
            }
            if (TunicArchipelago.Settings.ERFixedShop) {
                Settings.Add("er_fixed_shop");
            }
            Settings.Add($"fool_traps${(int)TunicArchipelago.Settings.FoolTrapIntensity}$");
            Settings.Add($"laurels_location#{(int)TunicArchipelago.Settings.FixedLaurelsOption}#");
            if (TunicArchipelago.Settings.Lanternless) {
                Settings.Add("lanternless");
            }
            if (TunicArchipelago.Settings.Maskless) {
                Settings.Add("maskless");
            }

            // Enemy Rando 
            if (TunicArchipelago.Settings.EnemyRandomizerEnabled) {
                Settings.Add("enemy_randomizer");
                if (TunicArchipelago.Settings.ExtraEnemiesEnabled) {
                    Settings.Add("extra_enemies");
                }
                Settings.Add($"enemy_generation!{(int)TunicArchipelago.Settings.EnemyDifficulty}!{(int)TunicArchipelago.Settings.EnemyGeneration}!");
            }

            // Race Settings
            if (TunicArchipelago.Settings.RaceMode) {
                Settings.Add("race_mode");
                if (TunicArchipelago.Settings.DisableIceboltInHeirFight) {
                    Settings.Add("no_heir_icebolt");
                }
                if (TunicArchipelago.Settings.DisableDistantBellShots) {
                    Settings.Add("no_distant_bell_shot");
                }
                if (TunicArchipelago.Settings.DisableIceGrappling) {
                    Settings.Add("no_ice_grapple");
                }
                if (TunicArchipelago.Settings.DisableLadderStorage) {
                    Settings.Add("no_ladder_storage");
                }
                if (TunicArchipelago.Settings.DisableUpgradeStealing) {
                    Settings.Add("no_upgrade_stealing");
                }
            }

            GUIUtility.systemCopyBuffer = string.Join(",", Settings.ToArray());
        }

        public static void CopyQuickSettingsInGame() {
            if (SaveFile.GetInt("randomizer mystery seed") == 1) {
                GUIUtility.systemCopyBuffer = $"{SaveFile.GetInt("seed")},mystery_seed";
                return;
            }
            List<string> Settings = new List<string>() { SaveFile.GetInt("seed").ToString(), SaveFile.GetString("randomizer game mode").ToLower() };
            if (SaveFile.GetInt("randomizer hexagon quest enabled") == 1) {
                Settings.Add($"hexagon_quest_goal={SaveFile.GetInt("randomizer hexagon quest goal")}=");
                Settings.Add($"hexagon_quest_extras~{SaveFile.GetInt("randomizer hexagon quest extras")}~");
            }
            if (SaveFile.GetInt("randomizer keys behind bosses") == 1) {
                Settings.Add("keys_behind_bosses");
            }
            if (SaveFile.GetInt("randomizer sword progression enabled") == 1) {
                Settings.Add("sword_progression");
            }
            if (SaveFile.GetInt("randomizer started with sword") == 1) {
                Settings.Add("start_with_sword");
            }
            if (SaveFile.GetInt("randomizer shuffled abilities") == 1) {
                Settings.Add("shuffle_abilities");
            }
            if (SaveFile.GetInt("randomizer entrance rando enabled") == 1) {
                Settings.Add("entrance_randomizer");
            }
            if (SaveFile.GetInt("randomizer ER fixed shop") == 1) {
                Settings.Add("er_fixed_shop");
            }
            Settings.Add($"fool_traps${(int)TunicArchipelago.Settings.FoolTrapIntensity}$");
            Settings.Add($"laurels_location#{SaveFile.GetInt("randomizer laurels location")}#");
            if (SaveFile.GetInt(SaveFlags.LanternlessLogic) == 1) {
                Settings.Add("lanternless");
            }
            if (SaveFile.GetInt(SaveFlags.MasklessLogic) == 1) {
                Settings.Add("maskless");
            }

            // Enemy Rando 
            if (TunicArchipelago.Settings.EnemyRandomizerEnabled) {
                Settings.Add("enemy_randomizer");
                if (TunicArchipelago.Settings.ExtraEnemiesEnabled) {
                    Settings.Add("extra_enemies");
                }
                Settings.Add($"enemy_generation!{(int)TunicArchipelago.Settings.EnemyDifficulty}!{(int)TunicArchipelago.Settings.EnemyGeneration}!");
            }

            // Race Settings
            if (TunicArchipelago.Settings.RaceMode) {
                Settings.Add("race_mode");
                if (TunicArchipelago.Settings.DisableIceboltInHeirFight) {
                    Settings.Add("no_heir_icebolt");
                }
                if (TunicArchipelago.Settings.DisableDistantBellShots) {
                    Settings.Add("no_distant_bell_shot");
                }
                if (TunicArchipelago.Settings.DisableIceGrappling) {
                    Settings.Add("no_ice_grapple");
                }
                if (TunicArchipelago.Settings.DisableLadderStorage) {
                    Settings.Add("no_ladder_storage");
                }
                if (TunicArchipelago.Settings.DisableUpgradeStealing) {
                    Settings.Add("no_upgrade_stealing");
                }
            }

            GUIUtility.systemCopyBuffer = string.Join(",", Settings.ToArray());
        }


        public static void PasteQuickSettings() {
            try {
                string SettingsString = GUIUtility.systemCopyBuffer;
                string[] SplitSettings = SettingsString.Split(',');
                CustomSeed = int.Parse(SplitSettings[0], CultureInfo.InvariantCulture).ToString();
                RandomizerSettings.GameModes NewGameMode;
                if (SettingsString.Contains("mystery_seed")) {
                    TunicArchipelago.Settings.MysterySeed = true;
                    return;
                }
                if (Enum.TryParse<RandomizerSettings.GameModes>(SplitSettings[1].ToUpper(), true, out NewGameMode)) {
                    TunicArchipelago.Settings.GameMode = NewGameMode;
                } else {
                    TunicArchipelago.Settings.GameMode = RandomizerSettings.GameModes.RANDOMIZER;
                }
                if (SettingsString.Split('=').Count() > 1) {
                    try {
                        TunicArchipelago.Settings.HexagonQuestGoal = int.Parse(SettingsString.Split('=')[1]);
                    } catch (Exception e) {
                        TunicArchipelago.Settings.HexagonQuestGoal = 20;
                    }
                }
                if (SettingsString.Split('~').Count() > 1) {
                    try {
                        TunicArchipelago.Settings.HexagonQuestExtraPercentage = int.Parse(SettingsString.Split('~')[1]);
                    } catch (Exception e) {
                        TunicArchipelago.Settings.HexagonQuestExtraPercentage = 50;
                    }
                }
                if (SettingsString.Split('$').Count() > 1) {
                    try {
                        TunicArchipelago.Settings.FoolTrapIntensity = (RandomizerSettings.FoolTrapOption)int.Parse(SettingsString.Split('$')[1]);
                    } catch (Exception e) {
                        TunicArchipelago.Settings.FoolTrapIntensity = RandomizerSettings.FoolTrapOption.NORMAL;
                    }
                }
                if (SettingsString.Split('#').Count() > 1) {
                    try {
                        TunicArchipelago.Settings.FixedLaurelsOption = (RandomizerSettings.FixedLaurelsType)int.Parse(SettingsString.Split('#')[1]);
                    } catch(Exception e) {
                        TunicArchipelago.Settings.FixedLaurelsOption = RandomizerSettings.FixedLaurelsType.RANDOM;
                    }
                }
                TunicArchipelago.Settings.KeysBehindBosses = SettingsString.Contains("keys_behind_bosses");
                TunicArchipelago.Settings.SwordProgressionEnabled = SettingsString.Contains("sword_progression");
                TunicArchipelago.Settings.StartWithSwordEnabled = SettingsString.Contains("start_with_sword");
                TunicArchipelago.Settings.ShuffleAbilities = SettingsString.Contains("shuffle_abilities");
                TunicArchipelago.Settings.EntranceRandoEnabled = SettingsString.Contains("entrance_randomizer");
                TunicArchipelago.Settings.ERFixedShop = SettingsString.Contains("er_fixed_shop");
                TunicArchipelago.Settings.Lanternless = SettingsString.Contains("lanternless");
                TunicArchipelago.Settings.Maskless = SettingsString.Contains("maskless");

                TunicArchipelago.Settings.EnemyRandomizerEnabled = SettingsString.Contains("enemy_randomizer");
                if (TunicArchipelago.Settings.EnemyRandomizerEnabled) {
                    TunicArchipelago.Settings.EnemyRandomizerEnabled = true;
                    TunicArchipelago.Settings.ExtraEnemiesEnabled = SettingsString.Contains("extra_enemies");
                    if (SettingsString.Split('!').Count() > 1) {
                        try {
                            TunicArchipelago.Settings.EnemyDifficulty = (RandomizerSettings.EnemyRandomizationType)int.Parse(SettingsString.Split('!')[2]);
                            TunicArchipelago.Settings.EnemyGeneration = (RandomizerSettings.EnemyGenerationType)int.Parse(SettingsString.Split('!')[1]); 

                        } catch (Exception e) {
                            TunicArchipelago.Settings.EnemyDifficulty = RandomizerSettings.EnemyRandomizationType.BALANCED;
                            TunicArchipelago.Settings.EnemyGeneration = RandomizerSettings.EnemyGenerationType.SEEDED;
                        }
                    }
                }
                TunicArchipelago.Settings.RaceMode = SettingsString.Contains("race_mode");
                if (TunicArchipelago.Settings.RaceMode) {
                    TunicArchipelago.Settings.DisableIceboltInHeirFight = SettingsString.Contains("no_heir_icebolt");
                    TunicArchipelago.Settings.DisableDistantBellShots = SettingsString.Contains("no_distant_bell_shot");
                    TunicArchipelago.Settings.DisableIceGrappling = SettingsString.Contains("no_ice_grapple");
                    TunicArchipelago.Settings.DisableLadderStorage = SettingsString.Contains("no_ladder_storage");
                    TunicArchipelago.Settings.DisableUpgradeStealing = SettingsString.Contains("no_upgrade_stealing");
                }

                OptionsGUIPatches.SaveSettings();
            } catch (Exception e) {
                Logger.LogError("Error parsing quick settings string!");
            }

        }


        public static bool TitleScreen___NewGame_PrefixPatch(TitleScreen __instance) {
            CloseAPSettingsWindow();
            return true;
        }

        public static bool FileManagement_LoadFileAndStart_PrefixPatch(FileManagementGUI __instance, string filename) {
            CloseAPSettingsWindow();
            SaveFile.LoadFromFile(filename);
            if (SaveFile.GetInt("archipelago") == 0 && SaveFile.GetInt("randomizer") == 0) {
                Logger.LogInfo("Non-Randomizer file selected!");
                GenericMessage.ShowMessage("<#FF0000>[death] \"<#FF0000>warning!\" <#FF0000>[death]\n\"Non-Randomizer file selected.\"\n\"Returning to menu.\"");
                return false;
            }
            if (SaveFile.GetString("archipelago player name") != "") {
                if (SaveFile.GetString("archipelago player name") != TunicArchipelago.Settings.ConnectionSettings.Player || (Archipelago.instance.integration.connected && int.Parse(Archipelago.instance.integration.slotData["seed"].ToString()) != SaveFile.GetInt("seed"))) {
                    Logger.LogInfo("Save does not match connected slot! Connected to " + TunicArchipelago.Settings.ConnectionSettings.Player + " [seed " + Archipelago.instance.integration.slotData["seed"].ToString() + "] but slot name in save file is " + SaveFile.GetString("archipelago player name") + " [seed " + SaveFile.GetInt("seed") + "]");
                    GenericMessage.ShowMessage("<#FF0000>[death] \"<#FF0000>warning!\" <#FF0000>[death]\n\"Save does not match connected slot.\"\n\"Returning to menu.\"");
                    return false;
                }
            }
            return true;
        }

    }

}
