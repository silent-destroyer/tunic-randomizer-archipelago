﻿using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TunicArchipelago {
    public class FairyTargets {
        private static ManualLogSource Logger = TunicArchipelago.Logger;
        public static Il2CppSystem.Collections.Generic.List<FairyTarget> EntranceTargets = new Il2CppSystem.Collections.Generic.List<FairyTarget> { };
        public static Il2CppSystem.Collections.Generic.List<FairyTarget> AltFairyTargets = new Il2CppSystem.Collections.Generic.List<FairyTarget> { };

        public static void CreateFairyTargets() {
            foreach (FairyTarget FairyTarget in Resources.FindObjectsOfTypeAll<FairyTarget>()) {
                GameObject.Destroy(FairyTarget);
            }
            if (ItemLookup.ItemList.Count > 0) {
                List<string> ItemIdsInScene = Locations.VanillaLocations.Keys.Where(ItemId => Locations.VanillaLocations[ItemId].Location.SceneName == SceneManager.GetActiveScene().name
                && SaveFile.GetInt($"randomizer picked up {ItemId}") == 0 &&
                (SaveFlags.IsArchipelago() && TunicArchipelago.Settings.CollectReflectsInWorld ? SaveFile.GetInt($"randomizer {ItemId} was collected") == 0 : true)).ToList();

                if (ItemIdsInScene.Count > 0) {
                    foreach (string ItemId in ItemIdsInScene) {
                        Location Location = Locations.VanillaLocations[ItemId].Location;

                        if (GameObject.Find($"fairy target {ItemId}") == null) {
                            CreateFairyTarget($"fairy target {ItemId}", StringToVector3(Location.Position));
                        }
                    }
                    if (GameObject.FindObjectOfType<TrinketWell>() != null) {
                        int CoinCount = Inventory.GetItemByName("Trinket Coin").Quantity + TunicArchipelago.Tracker.ImportantItems["Coins Tossed"];
                        Dictionary<int, int> CoinLevels = new Dictionary<int, int>() { { 0, 3 }, { 1, 6 }, { 2, 10 }, { 3, 15 }, { 4, 20 } };
                        int CoinsNeededForNextReward = CoinLevels[Locations.VanillaLocations.Keys.Where(ItemId => Locations.VanillaLocations[ItemId].Location.SceneName == "Trinket Well" &&
                        (SaveFile.GetInt($"randomizer picked up {ItemId}") == 1 || (SaveFlags.IsArchipelago() && TunicArchipelago.Settings.CollectReflectsInWorld && SaveFile.GetInt($"randomizer {ItemId} was collected") == 1))).ToList().Count];

                        if ((Inventory.GetItemByName("Trinket Coin").Quantity + TunicArchipelago.Tracker.ImportantItems["Coins Tossed"]) > CoinsNeededForNextReward) {
                            CreateFairyTarget($"fairy target Well Reward ({CoinsNeededForNextReward} Coins)", GameObject.FindObjectOfType<TrinketWell>().transform.position);
                        }
                    }
                } else {
                    CreateLoadZoneTargets();
                }
            }
        }

        public static void CreateLoadZoneTargets() {
            HashSet<string> ScenesWithItems = new HashSet<string>();

            foreach (FairyTarget FairyTarget in Resources.FindObjectsOfTypeAll<FairyTarget>()) {
                FairyTarget.enabled = false;
            }

            foreach (string ItemId in Locations.VanillaLocations.Keys.Where(itemId => Locations.VanillaLocations[itemId].Location.SceneName != SceneLoaderPatches.SceneName && (SaveFile.GetInt($"randomizer picked up {itemId}") == 0 &&
            (SaveFlags.IsArchipelago() && TunicArchipelago.Settings.CollectReflectsInWorld ? SaveFile.GetInt($"randomizer {itemId} was collected") == 0 : true)))) {
                ScenesWithItems.Add(Locations.VanillaLocations[ItemId].Location.SceneName);
            }

            foreach (ScenePortal ScenePortal in Resources.FindObjectsOfTypeAll<ScenePortal>()) {
                if (ScenesWithItems.Contains(ScenePortal.destinationSceneName)) {
                    CreateFairyTarget($"fairy target {ScenePortal.destinationSceneName}", ScenePortal.transform.position);
                }
            }
        }

        public static void CreateEntranceTargets() {
            foreach (ScenePortal ScenePortal in Resources.FindObjectsOfTypeAll<ScenePortal>()) {
                if (ScenePortal.isActiveAndEnabled) {
                    CreateFairyTarget($"entrance target {ScenePortal.destinationSceneName}", ScenePortal.transform.position);
                }
            }
        }

        private static void CreateFairyTarget(string Name, Vector3 Position) {
            GameObject FairyTarget = new GameObject(Name);
            FairyTarget.SetActive(true);
            FairyTarget.AddComponent<FairyTarget>();
            FairyTarget.GetComponent<FairyTarget>().stateVariable = StateVariable.GetStateVariableByName("false");
            FairyTarget.transform.position = Position;
        }

        public static void FindFairyTargets() {
            AltFairyTargets.Clear();
            EntranceTargets.Clear();
            foreach (FairyTarget fairyTarget in Resources.FindObjectsOfTypeAll<FairyTarget>()) {
                Logger.LogInfo("Fairy target found is " +  fairyTarget.name);
                if (fairyTarget.isActiveAndEnabled) {
                    Logger.LogInfo("enabled fairy target is " + fairyTarget.name);
                    if (fairyTarget.name.StartsWith("entrance")) {
                        Logger.LogInfo("Entrance target is " + fairyTarget.name);
                        EntranceTargets.Add(fairyTarget);
                    } else {
                        Logger.LogInfo("Fairy target is " + fairyTarget.name);
                        AltFairyTargets.Add(fairyTarget);
                    }
                }
            }
            FairyTarget.registered = AltFairyTargets;
        }

        private static Vector3 StringToVector3(string Position) {
            Position = Position.Replace("(", "").Replace(")", "");
            string[] coords = Position.Split(',');
            Vector3 vector = new Vector3(float.Parse(coords[0], CultureInfo.InvariantCulture), float.Parse(coords[1], CultureInfo.InvariantCulture), float.Parse(coords[2], CultureInfo.InvariantCulture));
            return vector;
        }

    }

    public class EntranceSeekerSpell : FairySpell {
        private static ManualLogSource Logger = TunicArchipelago.Logger;
        public static List<DPAD> CustomInputs = new List<DPAD>() { };
        
        public EntranceSeekerSpell(IntPtr ptr) : base(ptr) { }

        private void Awake() {
            base.inputsToCast = new UnhollowerBaseLib.Il2CppStructArray<DPAD>(1L);

            CustomInputs = new List<DPAD>() { DPAD.RIGHT, DPAD.DOWN, DPAD.RIGHT, DPAD.UP, DPAD.LEFT, DPAD.UP };
        }

        public override bool CheckInput(Il2CppStructArray<DPAD> inputs, int length) {
            if (length == CustomInputs.Count) {
                for (int i = 0; i < length; i++) {
                    if (inputs[i] != CustomInputs[i]) {
                        return false;
                    }
                }
                DoSpell();
            }
            return false;
        }

        public void DoSpell() {
            FairyTarget.registered = FairyTargets.EntranceTargets;
            PlayerCharacter.instance.GetComponent<FairySpell>().SpellEffect();
            FairyTarget.registered = FairyTargets.AltFairyTargets;
        }

    }
}
