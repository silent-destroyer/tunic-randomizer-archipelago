using Archipelago.MultiClient.Net.Models;
using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.Globalization;
using Archipelago.MultiClient.Net.Enums;
using static TunicArchipelago.SaveFlags;

namespace TunicArchipelago {
    public class PlayerCharacterPatches {

        private static ManualLogSource Logger = TunicArchipelago.Logger;

        public static string SaveName = null;
        public static int HeirAssistModeDamageValue = 0;
        public static bool StungByBee = false;
        public static bool IsTeleporting = false;
        public static bool DiedToDeathLink = false;
        public static string DeathLinkMessage = "";
        public static int index = 0;

        public static bool LoadSecondSword = false;
        public static bool LoadThirdSword = false;
        public static float LoadSwordTimer = 0.0f;
        public static bool LoadCustomTexture = false;
        public static bool WearHat = false;
        public static float TimeWhenLastChangedDayNight = 0.0f;
        public static float FinishLineSwordTimer = 0.0f;
        public static float CompletionTimer = 0.0f;
        public static float ResetDayNightTimer = -1.0f;

        public static void PlayerCharacter_Update_PostfixPatch(PlayerCharacter __instance) {
            Cheats.FastForward = Input.GetKey(KeyCode.Backslash);

            if (DiedToDeathLink) {
                if (DeathLinkMessage != "") {
                    Archipelago.instance.integration.ShowNotification(DeathLinkMessage, DeathLinkMessages.SecondaryMessages[new System.Random().Next(DeathLinkMessages.SecondaryMessages.Count)]);
                    DeathLinkMessage = "";
                }
                __instance.hp = -1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                if (SpeedrunFinishlineDisplayPatches.CompletionCanvas != null) {
                    SpeedrunFinishlineDisplayPatches.CompletionCanvas.SetActive(!SpeedrunFinishlineDisplayPatches.CompletionCanvas.active);
                }
            }

            if (Input.GetKeyDown(KeyCode.R)) {
                Archipelago.instance.Release();
            }

            if (Input.GetKeyDown(KeyCode.C)) {
                Archipelago.instance.Collect();
            }

            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                if (OptionsGUIPatches.BonusOptionsUnlocked) {
                    PlayerCharacter.instance.GetComponent<Animator>().SetBool("wave", true);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha5)) {
                PaletteEditor.RandomizeFoxColors();
            }
            if (Input.GetKeyDown(KeyCode.Alpha6)) {
                PaletteEditor.LoadCustomTexture();
            }

            if (StungByBee) {
                __instance.gameObject.transform.Find("Fox/root/pelvis/chest/head").localScale = new Vector3(3f, 3f, 3f);
            }
            if ((LoadSecondSword || LoadThirdSword || WearHat) && (GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy/") != null)) {
                try {
                    if (LoadSecondSword) { SwordProgression.EnableSecondSword(); }
                    if (LoadThirdSword) { SwordProgression.EnableThirdSword(); }
                    if (WearHat) {
                        GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/head/floppy hat").SetActive(true);
                        WearHat = false;
                    }
                } catch (Exception ex) {
                    Logger.LogError("Error applying upgraded sword!");
                }
            }
            if (LoadCustomTexture && GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/head/GameObject") != null) {
                PaletteEditor.LoadCustomTexture();
                LoadCustomTexture = false;
            }
            if (SpeedrunData.timerRunning && ResetDayNightTimer != -1.0f && SaveFile.GetInt(DiedToHeir) != 1) {
                ResetDayNightTimer += Time.fixedUnscaledDeltaTime;
                CycleController.IsNight = false;
                if (ResetDayNightTimer >= 5.0f) {
                    CycleController.AnimateSunrise();
                    SaveFile.SetInt(DiedToHeir, 1);
                    ResetDayNightTimer = -1.0f;
                }
            }
            if (SpeedrunFinishlineDisplayPatches.ShowSwordAfterDelay) {
                FinishLineSwordTimer += Time.fixedUnscaledDeltaTime;
                if (FinishLineSwordTimer > 3.5f) {
                    FinishLineSwordTimer = 0.0f;
                    SpeedrunFinishlineDisplayPatches.ShowSwordAfterDelay = false;
                    int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);
                    GameObject.Find("_FinishlineDisplay(Clone)/Finishline Camera/Vertical Group/Item Parade Group/").transform.GetChild(1).GetChild(1).GetComponent<Image>().enabled = false;
                    GameObject.Instantiate(SwordLevel == 3 ? ModelSwaps.SecondSwordImage : ModelSwaps.ThirdSwordImage, GameObject.Find("_FinishlineDisplay(Clone)/Finishline Camera/Vertical Group/Item Parade Group/").transform.GetChild(1)).GetComponent<RawImage>().color = new UnityEngine.Color(1, 1, 1, 0.65f);
                }
            }
            if (SpeedrunFinishlineDisplayPatches.ShowCompletionStatsAfterDelay) {
                CompletionTimer += Time.fixedUnscaledDeltaTime;
                if (CompletionTimer > 6.0f) {
                    CompletionTimer = 0.0f;
                    SpeedrunFinishlineDisplayPatches.CompletionCanvas.SetActive(true);
                    SpeedrunFinishlineDisplayPatches.ShowCompletionStatsAfterDelay = false;
                }
            }
            if (SpeedrunData.timerRunning && SceneLoaderPatches.SceneName != null && Locations.AllScenes.Count > 0) {
                float AreaPlaytime = SaveFile.GetFloat($"randomizer play time {SceneLoaderPatches.SceneName}");
                SaveFile.SetFloat($"randomizer play time {SceneLoaderPatches.SceneName}", AreaPlaytime + Time.unscaledDeltaTime);
            }
            if (IsTeleporting) {
                PlayerCharacter.instance.cheapIceParticleSystemEmission.enabled = true;
                PlayerCharacter.instance.damageBoostParticleSystemEmission.enabled = true;
                PlayerCharacter.instance.staminaBoostParticleSystemEmission.enabled = true;
                PlayerCharacter.instance._CompletelyInvulnerableEvenToIFrameIgnoringAttacks_k__BackingField = true;
                PlayerCharacter.instance.AddPoison(1f);
                if (PlayerCharacter.instance.gameObject.GetComponent<Rotate>() != null) {
                    PlayerCharacter.instance.gameObject.GetComponent<Rotate>().eulerAnglesPerSecond += new Vector3(0, 3.5f, 0);
                }
            }
            if(SaveFile.GetInt(AbilityShuffle) == 1) { 
                if(SaveFile.GetInt(PrayerUnlocked) == 0) {
                    __instance.prayerBeginTimer = 0;
                }
                if(SaveFile.GetInt(IceRodUnlocked) == 0) {
                    TechbowItemBehaviour.kIceShotWindow = 0;
                }
            }

            foreach (string Key in EnemyRandomizer.Enemies.Keys.ToList()) {
                EnemyRandomizer.Enemies[Key].SetActive(false);
                EnemyRandomizer.Enemies[Key].transform.position = new Vector3(-30000f, -30000f, -30000f);
            }

        }


        public static void PlayerCharacter_Start_PostfixPatch(PlayerCharacter __instance) {
            if (!Archipelago.instance.integration.connected) {
                Archipelago.instance.Connect();
            } else {
                if (TunicArchipelago.Settings.DeathLinkEnabled) {
                    Archipelago.instance.integration.EnableDeathLink();
                } else {
                    Archipelago.instance.integration.DisableDeathLink();
                }
            }
            if (Locations.AllScenes.Count == 0) {
                for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
                    string SceneName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                    Locations.AllScenes.Add(SceneName);
                }
            }

            if (PaletteEditor.ToonFox.GetComponent<MeshRenderer>() == null) {
                PaletteEditor.ToonFox.AddComponent<MeshRenderer>().material = __instance.transform.GetChild(25).GetComponent<SkinnedMeshRenderer>().material;
            }

            StateVariable.GetStateVariableByName("SV_ShopTrigger_Fortress").BoolValue = true;
            StateVariable.GetStateVariableByName("SV_ShopTrigger_Sewer").BoolValue = true;
            StateVariable.GetStateVariableByName("SV_ShopTrigger_Swamp(Night)").BoolValue = true;
            StateVariable.GetStateVariableByName("SV_ShopTrigger_WestGarden").BoolValue = true;

            CustomItemBehaviors.CanTakeGoldenHit = false;
            CustomItemBehaviors.CanSwingGoldenSword = false;

            TunicArchipelago.Tracker.ImportantItems["Coins Tossed"] = StateVariable.GetStateVariableByName("Trinket Coins Tossed").IntValue;

            Inventory.GetItemByName("Homeward Bone Statue").icon = Inventory.GetItemByName("Dash Stone").icon;
            Inventory.GetItemByName("Spear").icon = Inventory.GetItemByName("MoneyBig").icon;
            if (Inventory.GetItemByName("Spear").TryCast<ButtonAssignableItem>() != null) {
                Inventory.GetItemByName("Spear").TryCast<ButtonAssignableItem>().useMPUsesForQuantity = true;
                Dat.floatDatabase["mpCost_Spear_mp2"] = 40f;
            }
            Inventory.GetItemByName("MoneyLevelItem").Quantity = 1;
            Inventory.GetItemByName("Key (House)").icon = Inventory.GetItemByName("Key Special").icon;

            if (SaveFile.GetInt(SwordProgressionEnabled) != 0) {
                int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);

                if (SwordLevel == 3) {
                    LoadSecondSword = true;
                }
                if (SwordLevel >= 4) {
                    LoadThirdSword = true;
                }
            }

            if (Archipelago.instance.integration.connected) {
                Archipelago.instance.integration.sentCompletion = false;
                Archipelago.instance.integration.sentRelease = false;
                Archipelago.instance.integration.sentCollect = false;

                Dictionary<string, object> slotData = Archipelago.instance.GetPlayerSlotData();
                SaveFile.SetInt("archipelago", 1);

                if (Locations.VanillaLocations.Count == 0) {
                    Locations.CreateLocationLookups();
                }
                if (slotData.TryGetValue("hexagon_quest", out var hexagonQuest)) {
                    if (SaveFile.GetInt(HexagonQuestEnabled) == 0 && hexagonQuest.ToString() == "1") {
                        SaveFile.SetInt(HexagonQuestEnabled, 1);
                        for (int i = 0; i < 28; i++) {
                            SaveFile.SetInt($"randomizer obtained page {i}", 1);
                        }
                        StateVariable.GetStateVariableByName("Placed Hexagon 1 Red").BoolValue = true;
                        StateVariable.GetStateVariableByName("Placed Hexagon 2 Green").BoolValue = true;
                        StateVariable.GetStateVariableByName("Placed Hexagon 3 Blue").BoolValue = true;
                        StateVariable.GetStateVariableByName("Placed Hexagons ALL").BoolValue = true;
                        StateVariable.GetStateVariableByName("Has Been Betrayed").BoolValue = true;
                        StateVariable.GetStateVariableByName("Has Died To God").BoolValue = true;
                        
                        if(slotData.TryGetValue("Hexagon Quest Goal", out var hexagonGoal)) {
                            SaveFile.SetInt(HexagonQuestGoal, int.Parse(hexagonGoal.ToString()));
                        }
                    }
                }
                if (slotData.TryGetValue("start_with_sword", out var startWithSword)) {
                    if (SaveFile.GetInt("randomizer started with sword") == 0 && startWithSword.ToString() == "1") {
                        Logger.LogInfo("start with sword enabled");
                        SaveFile.SetInt("randomizer started with sword", 1);
                        Inventory.GetItemByName("Sword").Quantity += 1;
                    }
                }
                if (slotData.TryGetValue("ability_shuffling", out var abilityShuffling)) {
                    if (SaveFile.GetInt(AbilityShuffle) == 0 && abilityShuffling.ToString() == "1") {
                        Logger.LogInfo("ability shuffling enabled");
                        SaveFile.SetInt(AbilityShuffle, 1);
                        if (SaveFile.GetInt(HexagonQuestEnabled) == 1)
                        {
                            SaveFile.SetInt(HexagonQuestPrayer, int.Parse(slotData["Hexagon Quest Prayer"].ToString(), CultureInfo.InvariantCulture));
                            SaveFile.SetInt(HexagonQuestHolyCross, int.Parse(slotData["Hexagon Quest Holy Cross"].ToString(), CultureInfo.InvariantCulture));
                            SaveFile.SetInt(HexagonQuestIceRod, int.Parse(slotData["Hexagon Quest Ice Rod"].ToString(), CultureInfo.InvariantCulture));
                        }
                    }
                    if(abilityShuffling.ToString() == "0") {
                        SaveFile.SetInt(PrayerUnlocked, 1);
                        SaveFile.SetInt(HolyCrossUnlocked, 1);
                        SaveFile.SetInt(IceRodUnlocked, 1);
                    }
                }
                if (slotData.TryGetValue("sword_progression", out var swordProgression)) {
                    if (SaveFile.GetInt(SwordProgressionEnabled) == 0 && swordProgression.ToString() == "1") {
                        Logger.LogInfo("sword progression enabled");
                        SaveFile.SetInt(SwordProgressionEnabled, 1);
                    }
                }
                if (slotData.TryGetValue("keys_behind_bosses", out var keysBehindBosses)) {
                    if (SaveFile.GetInt(KeysBehindBosses) == 0 && keysBehindBosses.ToString() == "1") {
                        Logger.LogInfo("keys behind bosses enabled");
                        SaveFile.SetInt(KeysBehindBosses, 1);
                    }
                }
                if(slotData.TryGetValue("seed", out var Seed)) {
                    if (SaveFile.GetInt("seed") == 0) {
                        SaveFile.SetInt("seed", int.Parse(Seed.ToString(), CultureInfo.InvariantCulture));
                        EnemyRandomizer.CreateAreaSeeds();
                        Logger.LogInfo("Imported seed from archipelago: " + Seed);
                    } else {
                        Logger.LogInfo("Loading seed: " + SaveFile.GetInt("seed"));
                    }
                }

                Locations.PopulateMajorItemLocations(slotData);

                Locations.CheckedLocations.Clear();
                ItemLookup.ItemList.Clear();
                List<long> LocationIDs = new List<long>();
                foreach (string Key in Locations.VanillaLocations.Keys) {
                    Locations.CheckedLocations.Add(Key, SaveFile.GetInt($"randomizer picked up {Key}") == 1);
                    LocationIDs.Add(Archipelago.instance.integration.session.Locations.GetLocationIdFromName("Tunic", Locations.LocationIdToDescription[Key]));
                }

                SceneLoaderPatches.TimeOfLastSceneTransition = SaveFile.GetFloat("playtime");
                Archipelago.instance.integration.session.Locations.ScoutLocationsAsync(LocationIDs.ToArray()).ContinueWith(locationInfoPacket => {
                    foreach (NetworkItem Location in locationInfoPacket.Result.Locations) {
                        ItemLookup.ItemList.Add(Locations.LocationDescriptionToId[Archipelago.instance.integration.session.Locations.GetLocationNameFromId(Location.Location)], new ArchipelagoItem(Archipelago.instance.integration.session.Items.GetItemName(Location.Item), Location.Player, Location.Flags));
                    }
                }).Wait();
                ItemTracker.PopulateSpoilerLog();
                GhostHints.GenerateHints();
                Hints.PopulateHints();

                if (SaveFile.GetInt(AbilityShuffle) == 1 && SaveFile.GetInt(HolyCrossUnlocked) == 0) {
                    foreach (ToggleObjectBySpell SpellToggle in Resources.FindObjectsOfTypeAll<ToggleObjectBySpell>()) {
                        foreach (ToggleObjectBySpell Spell in SpellToggle.gameObject.GetComponents<ToggleObjectBySpell>()) {
                            Spell.enabled = false;
                        }
                    }
                }

                if(SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                    TunicArchipelago.Tracker.ImportantItems["Pages"] = 28;
                    SaveFile.SetInt("last page viewed", 0);
                    ModelSwaps.SetupHexagonQuest();
                } else {
                    ModelSwaps.RestoreOriginalHexagons();
                }

                FairyTargets.CreateFairyTargets();

                if (!SceneLoaderPatches.SpawnedGhosts) {
                    GhostHints.SpawnHintGhosts(SceneLoaderPatches.SceneName);
                }

                if (!ModelSwaps.SwappedThisSceneAlready) {
                    ModelSwaps.SwapItemsInScene();
                }

                OptionsGUIPatches.SaveSettings();
            }

            ModelSwaps.SetupDathStoneItemPresentation();

            PaletteEditor.SetupPartyHat(__instance);

            if (TunicArchipelago.Settings.RandomFoxColorsEnabled) {
                PaletteEditor.RandomizeFoxColors();
            }

            if (TunicArchipelago.Settings.UseCustomTexture) {
                LoadCustomTexture = true;
            }

            if (TunicArchipelago.Settings.RealestAlwaysOn) {
                GameObject.FindObjectOfType<RealestSpell>().SpellEffect();
            }

            if (PaletteEditor.CelShadingEnabled) {
                PaletteEditor.ApplyCelShading();
            }

            if (PaletteEditor.PartyHatEnabled) {
                WearHat = true;
            }
        }

        public static void PlayerCharacter_creature_Awake_PostfixPatch(PlayerCharacter __instance) {
            __instance.gameObject.AddComponent<WaveSpell>();
        }

        public static void PlayerCharacter_Die_MoveNext_PostfixPatch(PlayerCharacter._Die_d__481 __instance, ref bool __result) {

            if (!__result) {
                int Deaths = SaveFile.GetInt(PlayerDeathCount);
                SaveFile.SetInt(PlayerDeathCount, Deaths + 1);
                if (TunicArchipelago.Settings.DeathLinkEnabled && Archipelago.instance.integration.session.ConnectionInfo.Tags.Contains("DeathLink") && !DiedToDeathLink) {
                    Archipelago.instance.integration.SendDeathLink();
                }
                DiedToDeathLink = false;
            }
        }

        public static bool Monster_IDamageable_ReceiveDamage_PrefixPatch(Monster __instance) {

            if (__instance.name == "Foxgod" && SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                return false;
            }
            if (__instance.name == "_Fox(Clone)") {
                if (CustomItemBehaviors.CanTakeGoldenHit) {
                    GameObject.Find("_Fox(Clone)/fox").GetComponent<CreatureMaterialManager>().originalMaterials = CustomItemBehaviors.FoxBody.GetComponent<MeshRenderer>().materials;
                    GameObject.Find("_Fox(Clone)/fox hair").GetComponent<CreatureMaterialManager>().originalMaterials = CustomItemBehaviors.FoxHair.GetComponent<MeshRenderer>().materials;
                    GameObject.Find("_Fox(Clone)/fox").GetComponent<CreatureMaterialManager>()._ghostMaterialArray = CustomItemBehaviors.GhostFoxBody.GetComponent<MeshRenderer>().materials;
                    GameObject.Find("_Fox(Clone)/fox hair").GetComponent<CreatureMaterialManager>()._ghostMaterialArray = CustomItemBehaviors.GhostFoxHair.GetComponent<MeshRenderer>().materials;
                    SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
                    CustomItemBehaviors.CanTakeGoldenHit = false;
                    return false;
                }
            } else {
                if (__instance.name == "Foxgod" && TunicArchipelago.Settings.HeirAssistModeEnabled) {
                    __instance.hp -= HeirAssistModeDamageValue;
                }
                if (CustomItemBehaviors.CanSwingGoldenSword) {
                    __instance.hp -= 30;
                    if (SaveFile.GetInt(SwordProgressionLevel) == 3) {
                        GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy").transform.GetChild(4).GetComponent<MeshRenderer>().materials = ModelSwaps.SecondSword.GetComponent<MeshRenderer>().materials;
                    } else if (SaveFile.GetInt(SwordProgressionLevel) >= 4) {
                        GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy").transform.GetChild(4).GetComponent<MeshRenderer>().materials = ModelSwaps.ThirdSword.GetComponent<MeshRenderer>().materials;
                    } else {
                        GameObject.Find("_Fox(Clone)/Fox/root/pelvis/chest/arm_upper.R/arm_lower.R/hand.R/sword_proxy").GetComponent<MeshRenderer>().materials = CustomItemBehaviors.Sword.GetComponent<MeshRenderer>().materials;
                    }
                    SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
                    CustomItemBehaviors.CanSwingGoldenSword = false;
                }
            }
            return true;
        }

    }
}
