﻿using Archipelago.MultiClient.Net.Enums;
using BepInEx.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TunicArchipelago.SaveFlags;

namespace TunicArchipelago {
    public class SceneLoaderPatches {
        private static ManualLogSource Logger = TunicArchipelago.Logger;

        public static string SceneName;
        public static int SceneId;
        public static float TimeOfLastSceneTransition = 0.0f;
        public static bool SpawnedGhosts = false;

        public static bool SceneLoader_OnSceneLoaded_PrefixPatch(Scene loadingScene, LoadSceneMode mode, SceneLoader __instance) {
            // ladder storage fix
            if (PlayerCharacter.instance != null)
            {
                PlayerCharacter.instance.currentLadder = null;
                PlayerCharacter.instance.GetComponent<Animator>().SetBool("climbing", false);
            }
            TimeOfLastSceneTransition = SaveFile.GetFloat("playtime");
            if (SceneName == "Forest Belltower") {
                SaveFile.SetInt("chest open 19", SaveFile.GetInt("randomizer picked up 19 [Forest Belltower]"));
            }
            if (SceneName == "Sword Cave") {
                SaveFile.SetInt("chest open 19", SaveFile.GetInt("randomizer picked up 19 [Sword Cave]"));
            }

            if (Archipelago.instance != null && Archipelago.instance.integration != null && Archipelago.instance.integration.connected) {
                foreach (long location in Archipelago.instance.integration.session.Locations.AllLocationsChecked) {
                    string LocationId = Archipelago.instance.integration.session.Locations.GetLocationNameFromId(location);
                    string GameObjectId = Locations.LocationDescriptionToId[LocationId];
                    if (SaveFile.GetInt(ItemCollectedKey + GameObjectId) == 0) {
                        SaveFile.SetInt($"randomizer {GameObjectId} was collected", 1);
                    }
                }
            }

            return true;
        }

        public static void SceneLoader_OnSceneLoaded_PostfixPatch(Scene loadingScene, LoadSceneMode mode, SceneLoader __instance) {

            ModelSwaps.SwappedThisSceneAlready = false;
            SpawnedGhosts = false;

            if (loadingScene.name == "Posterity" && !EnemyRandomizer.Enemies.ContainsKey("Phage")) {
                EnemyRandomizer.InitializeEnemies("Posterity");
                ModelSwaps.CreateOtherWorldItemBlocks();
                SceneLoader.LoadScene("TitleScreen");
                return;
            }
            if (loadingScene.name == "Library Hall" && !EnemyRandomizer.Enemies.ContainsKey("administrator_servant")) {
                EnemyRandomizer.InitializeEnemies("Library Hall");
                SceneLoader.LoadScene("Posterity");
                return;
            }
            if (loadingScene.name == "Cathedral Redux" && !EnemyRandomizer.Enemies.ContainsKey("Voidtouched")) {
                EnemyRandomizer.InitializeEnemies("Cathedral Redux");
                SceneLoader.LoadScene("Library Hall");
                return;
            }
            if (loadingScene.name == "Fortress Main" && !EnemyRandomizer.Enemies.ContainsKey("woodcutter")) {
                EnemyRandomizer.InitializeEnemies("Fortress Main");
                SceneLoader.LoadScene("Cathedral Redux");
                return;
            }
            if (loadingScene.name == "Fortress Reliquary" && !EnemyRandomizer.Enemies.ContainsKey("voidling redux")) {
                EnemyRandomizer.InitializeEnemies("Fortress Reliquary");
                SceneLoader.LoadScene("Fortress Main");
                return;
            }
            if (loadingScene.name == "ziggurat2020_3" && !EnemyRandomizer.Enemies.ContainsKey("Centipede")) {
                EnemyRandomizer.InitializeEnemies("ziggurat2020_3");
                SceneLoader.LoadScene("Fortress Reliquary");
                return;
            }
            if (loadingScene.name == "ziggurat2020_1" && !EnemyRandomizer.Enemies.ContainsKey("administrator")) {
                EnemyRandomizer.InitializeEnemies("ziggurat2020_1");
                SceneLoader.LoadScene("ziggurat2020_3");
                return;
            }
            if (loadingScene.name == "Swamp Redux 2" && !EnemyRandomizer.Enemies.ContainsKey("bomezome_easy")) {
                EnemyRandomizer.InitializeEnemies("Swamp Redux 2");
                SceneLoader.LoadScene("ziggurat2020_1");
                return;
            }
            if (loadingScene.name == "Quarry" && !EnemyRandomizer.Enemies.ContainsKey("Scavenger_stunner")) {
                EnemyRandomizer.InitializeEnemies("Quarry");
                SceneLoader.LoadScene("Swamp Redux 2");
                return;
            }
            if (loadingScene.name == "Quarry Redux" && !EnemyRandomizer.Enemies.ContainsKey("Scavenger")) {
                EnemyRandomizer.InitializeEnemies("Quarry Redux");
                SceneLoader.LoadScene("Quarry");
                return;
            }
            if (loadingScene.name == "Crypt" && !EnemyRandomizer.Enemies.ContainsKey("Shadowreaper")) {
                EnemyRandomizer.InitializeEnemies("Crypt");
                SceneLoader.LoadScene("Quarry Redux");
                return;
            }
            if (loadingScene.name == "Fortress Basement" && !EnemyRandomizer.Enemies.ContainsKey("Spider Small")) {
                EnemyRandomizer.InitializeEnemies("Fortress Basement");
                SceneLoader.LoadScene("Crypt");
                return;
            }
            if (loadingScene.name == "frog cave main" && !EnemyRandomizer.Enemies.ContainsKey("Frog Small")) {
                EnemyRandomizer.InitializeEnemies("frog cave main");
                SceneLoader.LoadScene("Fortress Basement");
                return;
            }
            if (loadingScene.name == "Atoll Redux" && !EnemyRandomizer.Enemies.ContainsKey("plover")) {
                EnemyRandomizer.InitializeEnemies("Atoll Redux");
                SceneLoader.LoadScene("frog cave main");
                return;
            }
            if (loadingScene.name == "Archipelagos Redux" && ModelSwaps.GlowEffect == null) {
                ModelSwaps.SetupGlowEffect();
                EnemyRandomizer.InitializeEnemies("Archipelagos Redux");
                SceneLoader.LoadScene("Atoll Redux");
                return;
            }
            if (loadingScene.name == "Transit" && !ModelSwaps.Items.ContainsKey("Relic - Hero Sword")) {
                ModelSwaps.InitializeHeroRelics();
                SceneLoader.LoadScene("Archipelagos Redux");
                return;
            }
            if (loadingScene.name == "Spirit Arena" && ModelSwaps.ThirdSword == null) {
                ModelSwaps.InitializeThirdSword();
                SceneLoader.LoadScene("Transit");
                return;
            }
            if (loadingScene.name == "Library Arena" && ModelSwaps.SecondSword == null) {
                ModelSwaps.InitializeSecondSword();
                SceneLoader.LoadScene("Spirit Arena");
                return;
            }
            if (loadingScene.name == "Cathedral Arena" && !ModelSwaps.Chests.ContainsKey("Hyperdash")) {
                ModelSwaps.InitializeChestType("Hyperdash");
                SceneLoader.LoadScene("Library Arena");
                EnemyRandomizer.InitializeEnemies("Cathedral Arena");
                return;
            }
            if (loadingScene.name == "Overworld Redux" && ModelSwaps.Chests.Count == 0) {
                if (GhostHints.GhostFox == null) {
                    GhostHints.InitializeGhostFox();
                }
                ModelSwaps.InitializeItems();
                EnemyRandomizer.InitializeEnemies("Overworld Redux");
                SceneLoader.LoadScene("Cathedral Arena");
                return;
            }
            if (ModelSwaps.Chests.Count == 0 && loadingScene.name == "TitleScreen") {

                SwordProgression.CreateSwordItems();
                GameObject ArchipelagoObject = new GameObject("archipelago");
                Archipelago.instance = ArchipelagoObject.AddComponent<Archipelago>();   
                GameObject.DontDestroyOnLoad(ArchipelagoObject);
                if (Locations.VanillaLocations.Count == 0) {
                    Locations.CreateLocationLookups();
                }
                PaletteEditor.OdinRounded = Resources.FindObjectsOfTypeAll<Font>().Where(Font => Font.name == "Odin Rounded").ToList()[0];
                SceneLoader.LoadScene("Overworld Redux");
                return;
            }

            if (Camera.main != null && Camera.main.gameObject.GetComponentInParent<CycleController>() == null) {
                Camera.main.transform.parent.gameObject.AddComponent<CycleController>();
            }

            Logger.LogInfo("Entering scene " + loadingScene.name + " (" + loadingScene.buildIndex + ")");
            SceneName = loadingScene.name;
            SceneId = loadingScene.buildIndex;

            if (SceneName == "Overworld Redux" && (StateVariable.GetStateVariableByName("Has Been Betrayed").BoolValue &&
                StateVariable.GetStateVariableByName("Has Died To God").BoolValue) && SaveFile.GetInt(DiedToHeir) != 1 && SaveFile.GetInt(HexagonQuestEnabled) == 0) {
                PlayerCharacterPatches.ResetDayNightTimer = 0.0f;
                Logger.LogInfo("Resetting time of day to daytime!");
            }

            PlayerCharacterPatches.StungByBee = false;
            // Fur, Puff, Details, Tunic, Scarf
            if (TunicArchipelago.Settings.RandomFoxColorsEnabled) {
                PaletteEditor.RandomizeFoxColors();
            }

            if (PlayerCharacterPatches.IsTeleporting) {
                PlayerCharacter.instance.cheapIceParticleSystemEmission.enabled = false;
                PlayerCharacter.instance.damageBoostParticleSystemEmission.enabled = false;
                PlayerCharacter.instance.staminaBoostParticleSystemEmission.enabled = false;
                PlayerCharacter.instance._CompletelyInvulnerableEvenToIFrameIgnoringAttacks_k__BackingField = false;
                PlayerCharacter.instance.ClearPoison();
                PlayerCharacterPatches.IsTeleporting = false;
                GameObject.Destroy(PlayerCharacter.instance.gameObject.GetComponent<Rotate>());
            }

            // Failsafe for potion flasks not combining due to receiving 3rd shard during a load zone or at some other weird moment
            if (Inventory.GetItemByName("Flask Shard").Quantity >= 3) {
                Inventory.GetItemByName("Flask Shard").Quantity -= 3;
                Inventory.GetItemByName("Flask Container").Quantity += 1;
            }

            foreach (string Key in ItemLookup.FairyLookup.Keys) {
                StateVariable.GetStateVariableByName(ItemLookup.FairyLookup[Key].Flag).BoolValue = SaveFile.GetInt("randomizer opened fairy chest " + Key) == 1;
            }
            for (int i = 0; i < 28; i++) {
                SaveFile.SetInt("unlocked page " + i, SaveFile.GetInt("randomizer picked up page " + i) == 1 ? 1 : 0);
            }
            /*            foreach (string Key in ItemLookup.HeroRelicLookup.Keys) {
                            StateVariable.GetStateVariableByName(ItemLookup.HeroRelicLookup[Key].Flag).BoolValue = SaveFile.GetInt("randomizer picked up " + ItemLookup.HeroRelicLookup[Key].OriginalPickupLocation) == 1;
                        }*/


            if (SceneName == "Waterfall") {
                List<string> RandomObtainedFairies = new List<string>();
                foreach (string Key in ItemLookup.FairyLookup.Keys) {
                    StateVariable.GetStateVariableByName(ItemLookup.FairyLookup[Key].Flag).BoolValue = SaveFile.GetInt("randomizer obtained fairy " + Key) == 1;
                    if (SaveFile.GetInt("randomizer obtained fairy " + Key) == 1) {
                        RandomObtainedFairies.Add(Key);
                    }
                }

                StateVariable.GetStateVariableByName("SV_Fairy_5_Waterfall_Opened").BoolValue = SaveFile.GetInt("randomizer opened fairy chest Waterfall-(-47.0, 45.0, 10.0)") == 1;

                StateVariable.GetStateVariableByName("SV_Fairy_00_Enough Fairies Found").BoolValue = true;

                StateVariable.GetStateVariableByName("SV_Fairy_00_All Fairies Found").BoolValue = true;

            } else if (SceneName == "Spirit Arena") {
                for (int i = 0; i < 28; i++) {
                    SaveFile.SetInt("unlocked page " + i, SaveFile.GetInt("randomizer obtained page " + i) == 1 ? 1 : 0);
                }
                PlayerCharacterPatches.HeirAssistModeDamageValue = Locations.CheckedLocations.Values.ToList().Where(item => item).ToList().Count / 15;
                if (SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                    Resources.FindObjectsOfTypeAll<Foxgod>().ToList()[0].gameObject.transform.GetChild(0).GetComponent<CreatureMaterialManager>().originalMaterials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                    Resources.FindObjectsOfTypeAll<Foxgod>().ToList()[0].gameObject.transform.GetChild(1).GetComponent<CreatureMaterialManager>().originalMaterials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                }
            } else if (SceneName == "Overworld Interiors") {
                GameObject.Find("Trophy Stuff").transform.GetChild(4).gameObject.SetActive(true);
                /*                foreach (string Key in ItemLookup.HeroRelicLookup.Keys) {
                                    StateVariable.GetStateVariableByName(ItemLookup.HeroRelicLookup[Key].Flag).BoolValue = Inventory.GetItemByName(Key).Quantity == 1;
                                }*/
                ToggleOldHouseRelics();
                GameObject.Destroy(GameObject.Find("_Special/Bed Toggle Trigger/"));
                if ((StateVariable.GetStateVariableByName("Has Been Betrayed").BoolValue || StateVariable.GetStateVariableByName("Has Died To God").BoolValue) && SaveFile.GetInt(HexagonQuestEnabled) == 0) {
                    InteractionPatches.SetupDayNightHourglass();
                }
                if (GameObject.Find("_Offerings/ash group/")) {
                    GameObject.Find("_Offerings/ash group/").transform.position = new Vector3(-24.2824f, 29.8f, -45.4f);
                }
            } else if (SceneName == "Forest Belltower") {
                SaveFile.SetInt("chest open 19", 0);
            } else if (SceneName == "TitleScreen") {
                TitleVersion.Initialize();
                if (!Archipelago.instance.integration.connected) {
                    Archipelago.instance.Connect();
                }
            } else if (SceneName == "Temple") {
                if (SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                    foreach (GameObject Questagon in Resources.FindObjectsOfTypeAll<GameObject>().Where(Obj => Obj.name == "questagon")) {
                        Questagon.GetComponent<MeshRenderer>().materials = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                        Questagon.GetComponent<MeshRenderer>().receiveShadows = false;
                    }
                }
                if (TunicArchipelago.Settings.HeroPathHintsEnabled && Inventory.GetItemByName("Hyperdash").Quantity == 0) {
                    GameObject HintStatueGlow = GameObject.Instantiate(ModelSwaps.GlowEffect);
                    HintStatueGlow.SetActive(true);
                    HintStatueGlow.transform.position = new Vector3(13f, 0f, 49f);
                }
            } else if (SceneName == "Overworld Redux") {
                GameObject.Find("_Signposts/Signpost (3)/").GetComponent<Signpost>().message.text = $"#is wA too \"West Garden\"\n<#33FF33>[death] bEwAr uhv tArE [death]";
                GameObject.Find("_Environment Special/Door (1)/door/key twist").GetComponent<MeshRenderer>().materials = ModelSwaps.Items["Key (House)"].GetComponent<MeshRenderer>().materials;
                if (TunicArchipelago.Settings.HeroPathHintsEnabled && SaveFile.GetInt($"randomizer picked up {Hints.MailboxHintId}") == 0) {
                    GameObject.Find("_Environment/_Decorations/Mailbox (1)/mailbox flag").transform.rotation = new Quaternion(0.5f, -0.5f, 0.5f, 0.5f);
                }

                if (SaveFile.GetInt("randomizer entrance rando enabled") == 1 || (Archipelago.instance.integration.slotData.ContainsKey("entrance_rando") && Archipelago.instance.integration.slotData["entrance_rando"].ToString() == "1" && SaveFile.GetInt("seed") == 0)) {
                    GhostHints.SpawnTorchHintGhost();
                }
            } else if (SceneName == "Swamp Redux 2") {
                GhostHints.SpawnCathedralDoorGhost();

                // Removes the barricades from the swamp shop during the day 
                if (GameObject.Find("_Setpieces Etc/plank_4u") != null && GameObject.Find("_Setpieces Etc/plank_4u (1)") != null) {
                    GameObject.Find("_Setpieces Etc/plank_4u").SetActive(false);
                    GameObject.Find("_Setpieces Etc/plank_4u (1)").SetActive(false);
                }
                // Activate night bridge to allow access to shortcut ladder
                GameObject.Find("_Setpieces Etc/NightBridge/").GetComponent<DayNightBridge>().dayOrNight = StateVariable.GetStateVariableByName("Is Night").BoolValue ? DayNightBridge.DayNight.NIGHT : DayNightBridge.DayNight.DAY;
            } else if (SceneName == "g_elements") {
                GhostHints.SpawnLostGhostFox();
            } else if (SceneName == "Posterity") {
                GhostHints.SpawnRescuedGhostFox();
            } else if (SceneName == "Shop") {
                if (new System.Random().Next(100) < 3) {
                    GameObject.Find("merchant").SetActive(false);
                    GameObject.Find("Environment").transform.GetChild(3).gameObject.SetActive(true);
                }
                ModelSwaps.AddNewShopItems();
            } else if (SceneName == "ShopSpecial") {
                if (new System.Random().Next(100) < 3) {
                    GameObject.Find("merchant (1)").SetActive(false);
                    GameObject.Find("Environment").transform.GetChild(3).gameObject.SetActive(true);
                }
            } else if (SceneName == "Cathedral Arena") {
                if (SaveFile.GetInt("randomizer entrance rando enabled") == 1) {
                    StateVariable.GetStateVariableByName("SV_cathedral elevator").BoolValue = false;
                }
            } else if (SceneName == "Cathedral Redux") {
                if (SaveFile.GetInt("randomizer entrance rando enabled") == 1) {
                    StateVariable.GetStateVariableByName("SV_cathedral elevator").BoolValue = true;
                }
            } else if (SceneName == "Maze Room") {
                foreach (Chest chest in Resources.FindObjectsOfTypeAll<Chest>().Where(chest => chest.name == "Chest: Fairy")) { 
                    chest.transform.GetChild(4).gameObject.SetActive(false);
                    chest.transform.GetChild(7).gameObject.SetActive(false);
                    chest.transform.parent.GetChild(2).gameObject.SetActive(false);
                }
            }

            if (Archipelago.instance != null && Archipelago.instance.integration != null && Archipelago.instance.integration.connected) {
                if (TunicArchipelago.Settings.EnemyRandomizerEnabled && EnemyRandomizer.Enemies.Count > 0 && !EnemyRandomizer.ExcludedScenes.Contains(SceneName)) {
                    EnemyRandomizer.SpawnNewEnemies();
                }

                try {
                    if (TunicArchipelago.Settings.UseCustomTexture) {
                        PaletteEditor.LoadCustomTexture();
                    }
                } catch (Exception ex) {
                    Logger.LogError("An error occurred applying custom texture:");
                    Logger.LogError(ex.Message + " " + ex.StackTrace);
                }
                try {
                    if (!ModelSwaps.SwappedThisSceneAlready && (ItemLookup.ItemList.Count > 0 && SaveFile.GetInt("seed") != 0)) {
                        ModelSwaps.SwapItemsInScene();
                    }
                } catch (Exception ex) {
                    Logger.LogError("An error occurred swapping item models in this scene:");
                    Logger.LogError(ex.Message + " " + ex.StackTrace);
                }

                if (SaveFile.GetInt("randomizer entrance rando enabled") == 1)
                {
                    TunicPortals.CreatePortalPairs(((JObject)Archipelago.instance.GetPlayerSlotData()["Entrance Rando"]).ToObject<Dictionary<string, string>>());
                    TunicPortals.ModifyPortals(loadingScene);

                }

                if (SaveFile.GetInt(AbilityShuffle) == 1 && SaveFile.GetInt(HolyCrossUnlocked) == 0) {
                    foreach (ToggleObjectBySpell SpellToggle in Resources.FindObjectsOfTypeAll<ToggleObjectBySpell>()) {
                        foreach (ToggleObjectBySpell Spell in SpellToggle.gameObject.GetComponents<ToggleObjectBySpell>()) {
                            Spell.enabled = false;
                        }
                    }
                }

                try {
                    if (TunicArchipelago.Settings.GhostFoxHintsEnabled && GhostHints.HintGhosts.Count > 0 && SaveFile.GetInt("seed") != 0) {
                        GhostHints.SpawnHintGhosts(SceneName);
                        SpawnedGhosts = true;
                    }
                } catch (Exception ex) {
                    Logger.LogError("An error occurred spawning hint ghost foxes:");
                    Logger.LogError(ex.Message + " " + ex.StackTrace);
                }

                try {
                    FairyTargets.CreateFairyTargets();
                } catch (Exception ex) {
                    Logger.LogError("An error occurred creating new fairy seeker spell targets:");
                    Logger.LogError(ex.Message + " " + ex.StackTrace);
                }

                if (TunicArchipelago.Settings.RealestAlwaysOn) {
                    try {
                        GameObject.FindObjectOfType<RealestSpell>().SpellEffect();
                    } catch (Exception e) {

                    }
                }

                Archipelago.instance.integration.UpdateDataStorageOnLoad();
            }

            ItemTracker.SaveTrackerFile();
        }

        public static void ToggleOldHouseRelics() {
            if (SceneManager.GetActiveScene().name == "Overworld Interiors" && GameObject.Find("_Offerings") != null && GameObject.Find("_Offerings").transform.childCount >= 5) {
                GameObject.Find("_Offerings").transform.GetChild(0).gameObject.SetActive(Inventory.GetItemByName("Relic - Hero Water").Quantity > 0);
                GameObject.Find("_Offerings").transform.GetChild(1).gameObject.SetActive(Inventory.GetItemByName("Relic - Hero Crown").Quantity > 0);
                GameObject.Find("_Offerings").transform.GetChild(2).gameObject.SetActive(Inventory.GetItemByName("Relic - Hero Pendant SP").Quantity > 0);
                GameObject.Find("_Offerings").transform.GetChild(3).gameObject.SetActive(Inventory.GetItemByName("Relic - Hero Pendant HP").Quantity > 0);
                GameObject.Find("_Offerings").transform.GetChild(4).gameObject.SetActive(Inventory.GetItemByName("Relic - Hero Pendant MP").Quantity > 0);
                GameObject.Find("_Offerings").transform.GetChild(5).gameObject.SetActive(Inventory.GetItemByName("Relic - Hero Sword").Quantity > 0);
            }
        }

        public static void PauseMenu___button_ReturnToTitle_PostfixPatch(PauseMenu __instance) {

            if (ItemStatsHUD.HexagonQuest != null) {
                ItemStatsHUD.HexagonQuest.SetActive(false);
            }
            SceneName = "TitleScreen";
        }

    }

}