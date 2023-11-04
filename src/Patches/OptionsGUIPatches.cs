using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using BepInEx.Logging;
using Newtonsoft.Json;
using static TunicArchipelago.SaveFlags;

namespace TunicArchipelago {
    public class OptionsGUIPatches {

        private static ManualLogSource Logger = TunicArchipelago.Logger;

        public static bool BonusOptionsUnlocked = false;

        public static bool OptionsGUI_page_root_PrefixPatch(OptionsGUI __instance) {
            addPageButton("Randomizer Settings", RandomizerSettingsPage);
            return true;
        }

        public static void RandomizerSettingsPage() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.setHeading("Randomizer");
            addPageButton("Archipelago Settings", ArchipelagoSettingsPage);
            if (SceneLoaderPatches.SceneName != "TitleScreen") {
                addPageButton("Logic Settings", LogicSettingsPage);
            }
            addPageButton("Hint Settings", HintsSettingsPage);
            addPageButton("General Settings", GeneralSettingsPage);
            addPageButton("Enemy Randomizer Settings", EnemyRandomizerSettings);
            addPageButton("Fox Customization", CustomFoxSettingsPage);
        }

        public static void ArchipelagoSettingsPage() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.setHeading("Archipelago");
            OptionsGUI.addToggle("Death Link", "Off", "On", TunicArchipelago.Settings.DeathLinkEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleDeathLink);
        }

        public static void LogicSettingsPage() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.addButton("Game Mode", SaveFile.GetString("randomizer game mode"), null);
            OptionsGUI.addButton("Keys Behind Bosses", SaveFile.GetInt(KeysBehindBosses) == 1 ? "<#00ff00>On" : "<#ff0000>Off", null);
            OptionsGUI.addButton("Sword Progression", SaveFile.GetInt(SwordProgressionEnabled) == 1 ? "<#00ff00>On" : "<#ff0000>Off", null);
            OptionsGUI.addButton("Started With Sword", SaveFile.GetInt("randomizer started with sword") == 1 ? "<#00ff00>Yes" : "<#ff0000>No", null);
            OptionsGUI.addButton("Shuffled Abilities", SaveFile.GetInt(AbilityShuffle) == 1 ? "<#00ff00>On" : "<#ff0000>Off", null);
            OptionsGUI.setHeading("Logic");
        }

        public static void HintsSettingsPage() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.addToggle("Path of the Hero Hints", "Off", "On", TunicArchipelago.Settings.HeroPathHintsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)TogglePathOfHeroHints);
            OptionsGUI.addToggle("Ghost Fox Hints", "Off", "On", TunicArchipelago.Settings.GhostFoxHintsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleGhostFoxHints);
            OptionsGUI.addToggle("Freestanding Items Match Contents", "Off", "On", TunicArchipelago.Settings.ShowItemsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleShowItems);
            OptionsGUI.addToggle("Chests Match Contents", "Off", "On", TunicArchipelago.Settings.ChestsMatchContentsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleChestsMatchContents);
            OptionsGUI.setHeading("Hints");
        }

        public static void GeneralSettingsPage() {
            Il2CppStringArray FoolTrapOptions = (Il2CppStringArray)new string[] { "<#FFFFFF>None", "<#4FF5D4>Normal", "<#E3D457>Double", "<#FF3333>Onslaught" };

            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.setHeading("General");
            OptionsGUI.addToggle("Easier Heir Fight", "Off", "On", TunicArchipelago.Settings.HeirAssistModeEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleHeirAssistMode);
            OptionsGUI.addToggle("Cheaper Shop Items", "Off", "On", TunicArchipelago.Settings.CheaperShopItemsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleCheaperShopItems);
            OptionsGUI.addToggle("Bonus Upgrades", "Off", "On", TunicArchipelago.Settings.BonusStatUpgradesEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleBonusStatUpgrades);
            OptionsGUI.addToggle("Disable Chest Interruption", "Off", "On", TunicArchipelago.Settings.DisableChestInterruption ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleChestInterruption);
            OptionsGUI.addToggle("???", "Off", "On", CameraController.Flip ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleWeirdMode);
        }

        public static void EnemyRandomizerSettings() {
            Il2CppStringArray EnemyDifficulties = (Il2CppStringArray)new string[] { "Random", "Balanced" };
            Il2CppStringArray EnemyGenerationTypes = (Il2CppStringArray)new string[] { "Random", "Seeded" };
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.setHeading("Enemy Randomization");
            OptionsGUI.addToggle("Enemy Randomizer", "Off", "On", TunicArchipelago.Settings.EnemyRandomizerEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleEnemyRandomizer);
            OptionsGUI.addToggle("Extra Enemies", "Off", "On", TunicArchipelago.Settings.ExtraEnemiesEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleExtraEnemies);
            OptionsGUI.addMultiSelect("Enemy Difficulty", EnemyDifficulties, GetEnemyDifficulty(), (OptionsGUIMultiSelect.MultiSelectAction)ChangeEnemyRandomizerDifficulty).wrap = true;
            OptionsGUI.addMultiSelect("Enemy Generation", EnemyGenerationTypes, GetEnemyGenerationType(), (OptionsGUIMultiSelect.MultiSelectAction)ChangeEnemyRandomizerGenerationType).wrap = true;

        }

        public static void CustomFoxSettingsPage() {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.setHeading("Fox Customization");
            OptionsGUI.addToggle("Random Fox Colors", "Off", "On", TunicArchipelago.Settings.RandomFoxColorsEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleRandomFoxPalette);
            OptionsGUI.addToggle("Keepin' It Real", "Off", "On", TunicArchipelago.Settings.RealestAlwaysOn ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleSunglasses);
            OptionsGUI.addToggle("Show Fox Color Editor", "Off", "On", PaletteEditor.EditorOpen ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)TogglePaletteEditor);
            OptionsGUI.addToggle("Use Custom Texture", "Off", "On", TunicArchipelago.Settings.UseCustomTexture ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleCustomTexture);
            if (BonusOptionsUnlocked && SceneLoaderPatches.SceneName != "TitleScreen") {
                OptionsGUI.addToggle("<#FFA500>BONUS: Cel Shaded Fox", "Off", "On", PaletteEditor.CelShadingEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)ToggleCelShading);
                OptionsGUI.addToggle("<#00FFFF>BONUS: Party Hat", "Off", "On", PaletteEditor.PartyHatEnabled ? 1 : 0, (OptionsGUIMultiSelect.MultiSelectAction)TogglePartyHat);
            }
            OptionsGUI.addButton("Reset to Defaults", (Action)ResetToDefaults);
        }

        public static void addPageButton(string pageName, Action pageMethod) {
            Action<Action> pushPageAction = new Action<Action>(pushPage);
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.addButton(pageName, (Action)delegate () {
                pushPageAction.Invoke(pageMethod);
            });
        }

        public static void pushPage(Action pageMethod) {
            OptionsGUI OptionsGUI = GameObject.FindObjectOfType<OptionsGUI>();
            OptionsGUI.pushPage(DelegateSupport.ConvertDelegate<OptionsGUI.PageMethod>(pageMethod));
            OptionsGUI.addButton("Return", new Action(OptionsGUI.popPage));
        }

        public static void ToggleDeathLink(int index) {
            TunicArchipelago.Settings.DeathLinkEnabled = !TunicArchipelago.Settings.DeathLinkEnabled;

            if (Archipelago.instance.integration != null) {
                if (TunicArchipelago.Settings.DeathLinkEnabled) {
                    Archipelago.instance.integration.EnableDeathLink();
                } else {
                    Archipelago.instance.integration.DisableDeathLink();
                }
            }

            SaveSettings();
        }

        public static void ToggleSunglasses(int index) {
            TunicArchipelago.Settings.RealestAlwaysOn = !TunicArchipelago.Settings.RealestAlwaysOn;
            if (TunicArchipelago.Settings.RealestAlwaysOn) {
                if (GameObject.FindObjectOfType<RealestSpell>() != null) {
                    GameObject.FindObjectOfType<RealestSpell>().SpellEffect();
                }
            }
            if (!TunicArchipelago.Settings.RealestAlwaysOn) {
                GameObject Realest = GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/head/therealest");
                if (Realest != null) {
                    Realest.SetActive(false);
                }
            }
            SaveSettings();
        }

        public static void SaveSettings() {
            if (!File.Exists(TunicArchipelago.SettingsPath)) {
                File.WriteAllText(TunicArchipelago.SettingsPath, JsonConvert.SerializeObject(TunicArchipelago.Settings, Formatting.Indented));
            } else {
                File.Delete(TunicArchipelago.SettingsPath);
                File.WriteAllText(TunicArchipelago.SettingsPath, JsonConvert.SerializeObject(TunicArchipelago.Settings, Formatting.Indented));
            }
        }

        // Hints
        public static void TogglePathOfHeroHints(int index) {
            TunicArchipelago.Settings.HeroPathHintsEnabled = !TunicArchipelago.Settings.HeroPathHintsEnabled;
            SaveSettings();
        }

        public static void ToggleGhostFoxHints(int index) {
            TunicArchipelago.Settings.GhostFoxHintsEnabled = !TunicArchipelago.Settings.GhostFoxHintsEnabled;
            SaveSettings();
        }


        public static void ToggleShowItems(int index) {
            TunicArchipelago.Settings.ShowItemsEnabled = !TunicArchipelago.Settings.ShowItemsEnabled;
            SaveSettings();
        }

        public static void ToggleChestsMatchContents(int index) {
            TunicArchipelago.Settings.ChestsMatchContentsEnabled = !TunicArchipelago.Settings.ChestsMatchContentsEnabled;
            SaveSettings();
        }

        // Gameplay

        public static void ToggleHeirAssistMode(int index) {
            TunicArchipelago.Settings.HeirAssistModeEnabled = !TunicArchipelago.Settings.HeirAssistModeEnabled;
            SaveSettings();
        }

        public static void ToggleCheaperShopItems(int index) {
            TunicArchipelago.Settings.CheaperShopItemsEnabled = !TunicArchipelago.Settings.CheaperShopItemsEnabled;
            SaveSettings();
        }

        public static void ToggleBonusStatUpgrades(int index) {
            TunicArchipelago.Settings.BonusStatUpgradesEnabled = !TunicArchipelago.Settings.BonusStatUpgradesEnabled;
            SaveSettings();
        }

        public static void ToggleChestInterruption(int index) {
            TunicArchipelago.Settings.DisableChestInterruption = !TunicArchipelago.Settings.DisableChestInterruption;
            SaveSettings();
        }

        // Enemy Randomizer
        public static void ToggleEnemyRandomizer(int index) {
            TunicArchipelago.Settings.EnemyRandomizerEnabled = !TunicArchipelago.Settings.EnemyRandomizerEnabled;
            SaveSettings();
        }

        public static void ToggleExtraEnemies(int index) {
            TunicArchipelago.Settings.ExtraEnemiesEnabled = !TunicArchipelago.Settings.ExtraEnemiesEnabled;
            SaveSettings();
        }

        public static int GetEnemyDifficulty() {
            return (int)TunicArchipelago.Settings.EnemyDifficulty;
        }

        public static int GetEnemyGenerationType() {
            return (int)TunicArchipelago.Settings.EnemyGeneration;
        }

        public static void ChangeEnemyRandomizerDifficulty(int index) {
            TunicArchipelago.Settings.EnemyDifficulty = (RandomizerSettings.EnemyRandomizationType)index;
            SaveSettings();
        }

        public static void ChangeEnemyRandomizerGenerationType(int index) {
            TunicArchipelago.Settings.EnemyGeneration = (RandomizerSettings.EnemyGenerationType)index;
            SaveSettings();
        }

        public static void ToggleWeirdMode(int index) {
            CameraController.Flip = !CameraController.Flip;
        }

        public static void ToggleRandomFoxPalette(int index) {
            TunicArchipelago.Settings.RandomFoxColorsEnabled = !TunicArchipelago.Settings.RandomFoxColorsEnabled;
            if (TunicArchipelago.Settings.RandomFoxColorsEnabled) {
                PaletteEditor.RandomizeFoxColors();
            } else {
                if (TunicArchipelago.Settings.UseCustomTexture) {
                    PaletteEditor.LoadCustomTexture();
                } else {
                    PaletteEditor.RevertFoxColors();
                }
            }
            SaveSettings();
        }

        public static void TogglePaletteEditor(int index) {
            PaletteEditor.EditorOpen = !PaletteEditor.EditorOpen;
            CameraController.DerekZoom = PaletteEditor.EditorOpen ? 0.35f : 1f;
        }

        public static void ToggleCustomTexture(int index) {
            TunicArchipelago.Settings.UseCustomTexture = !TunicArchipelago.Settings.UseCustomTexture;
            if (TunicArchipelago.Settings.UseCustomTexture) {
                PaletteEditor.LoadCustomTexture();
            } else {
                if (TunicArchipelago.Settings.RandomFoxColorsEnabled) {
                    PaletteEditor.RandomizeFoxColors();
                } else {
                    PaletteEditor.RevertFoxColors();
                }
            }
        }

        public static void ToggleCelShading(int index) {
            if (PaletteEditor.CelShadingEnabled) {
                PaletteEditor.DisableCelShading();
            } else {
                PaletteEditor.ApplyCelShading();
            }
        }

        public static void TogglePartyHat(int index) {
            try {
                GameObject PartyHat = GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/head/floppy hat");
                PartyHat.SetActive(!PartyHat.active);
                PaletteEditor.PartyHatEnabled = PartyHat.active;
            } catch (Exception ex) {

            }
        }

        public static void ResetToDefaults() {
            PaletteEditor.RevertFoxColors();
        }

        public static void SaveFile_GetNewSaveFileName_PostfixPatch(SaveFile __instance, ref string __result) {

            __result = $"{__result.Split('.')[0]}-archipelago.tunic";
        }

        public static void FileManagementGUI_rePopulateList_PostfixPatch(FileManagementGUI __instance) {
            foreach (FileManagementGUIButton button in GameObject.FindObjectsOfType<FileManagementGUIButton>()) {
                SaveFile.LoadFromPath(SaveFile.GetRootSaveFileNameList()[button.index]);
                if (SaveFile.GetInt("archipelago") != 0 && !button.isSpecial) {
                    // Display special icon and "randomized" text to indicate randomizer file
                    button.specialBadge.gameObject.active = true;
                    button.specialBadge.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    button.specialBadge.transform.localPosition = new Vector3(-75f, -27f, 0f);
                    button.playtimeString.enableAutoSizing = false;
                    button.playtimeString.text += $" <size=65%>archipelago";
                    button.filenameTMP.text += $" <size=65%>({SaveFile.GetString("archipelago player name")})";
                    // Display randomized page count instead of "vanilla" pages picked up
                    int Pages = 0;
                    for (int i = 0; i < 28; i++) {
                        if (SaveFile.GetInt($"randomizer obtained page {i}") == 1) {
                            Pages++;
                        }
                    }
                    button.manpageTMP.text = Pages.ToString();
                }
            }
        }

    }
}
