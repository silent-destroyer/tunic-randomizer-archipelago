using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TunicArchipelago.SaveFlags;

namespace TunicArchipelago {
    public class InteractionPatches {

        public static bool InteractionTrigger_Interact_PrefixPatch(Item item, InteractionTrigger __instance) {
            string InteractionLocation = SceneLoaderPatches.SceneName + " " + __instance.transform.position;

            if (__instance.gameObject.GetComponent<NPC>() != null) {
                if (SceneManager.GetActiveScene().name == "g_elements") {
                    if (Inventory.GetItemByName("Homeward Bone Statue").Quantity == 0) {
                        __instance.gameObject.GetComponent<NPC>().script.text = $"I lawst mI mahjik stOn ahnd kahnt gO hOm...---if yoo fInd it, kahn yoo bri^ it too mE?\nit louks lIk #is: [dath]";
                    } else {
                        __instance.gameObject.GetComponent<NPC>().script.text = $"I lawst mI mahjik stOn [dath] ahnd kahnt gO hOm...---... wAt, yoo fownd it! plEz, yooz it now!";
                    }
                }

                if (TunicArchipelago.Settings.SendHintsToServer) {
                    GhostHints.CheckForServerHint(__instance.name);
                }
            }
            if (Hints.HintLocations.ContainsKey(InteractionLocation) && Hints.HintMessages.ContainsKey(Hints.HintLocations[InteractionLocation]) && TunicArchipelago.Settings.HeroPathHintsEnabled) {
                LanguageLine Hint = ScriptableObject.CreateInstance<LanguageLine>();
                Hint.text = Hints.HintMessages[Hints.HintLocations[InteractionLocation]];

                if (TunicArchipelago.Settings.SendHintsToServer && Hints.LocalHintsForServer.ContainsKey(Hints.HintLocations[InteractionLocation]) && SaveFile.GetInt($"archipelago sent optional hint to server {Hints.LocalHintsForServer[Hints.HintLocations[InteractionLocation]]}") == 0) {
                    string LocationName = Hints.LocalHintsForServer[Hints.HintLocations[InteractionLocation]];
                    Archipelago.instance.integration.session.Locations.ScoutLocationsAsync(true, Archipelago.instance.GetLocationId(LocationName)).ContinueWith(locationInfoPacket => { }).Wait();
                    SaveFile.SetInt($"archipelago sent optional hint to server {Hints.LocalHintsForServer[Hints.HintLocations[InteractionLocation]]}", 1);
                }

                GenericMessage.ShowMessage(Hint);

                return false;
            }
            if (SceneLoaderPatches.SceneName == "Waterfall" && __instance.transform.position.ToString() == "(-47.4, 46.9, 3.0)" && TunicArchipelago.Tracker.ImportantItems["Fairies"] < 10) {
                GenericMessage.ShowMessage($"\"Locked. (10\" fArEz \"required)\"");
                return false;
            }
            if (SceneLoaderPatches.SceneName == "Waterfall" && __instance.transform.position.ToString() == "(-47.5, 45.0, -0.5)" && TunicArchipelago.Tracker.ImportantItems["Fairies"] < 20) {
                GenericMessage.ShowMessage($"\"Locked. (20\" fArEz \"required)\"");
                return false;
            }
            if (SceneLoaderPatches.SceneName == "Overworld Interiors" && __instance.transform.position.ToString() == "(-26.4, 28.9, -46.2)") {
                if ((StateVariable.GetStateVariableByName("Has Been Betrayed").BoolValue || StateVariable.GetStateVariableByName("Has Died To God").BoolValue) && (PlayerCharacterPatches.TimeWhenLastChangedDayNight + 3.0f < Time.fixedTime)) {
                    GenericPrompt.ShowPrompt(CycleController.IsNight ? $"wAk fruhm #is drEm?" : $"rEtirn too yor drEm?", (Il2CppSystem.Action)ChangeDayNightHourglass, null);
                }
                return false;
            }
            if (SceneLoaderPatches.SceneName == "Overworld Redux" && __instance.transform.position.ToString() == "(-38.0, 29.0, -55.0)") {
                PlayerCharacter.instance.transform.GetChild(0).GetChild(0).GetChild(10).GetChild(0).gameObject.GetComponent<MeshRenderer>().materials = ModelSwaps.Items["Key (House)"].GetComponent<MeshRenderer>().materials;
            }
            if ((SceneLoaderPatches.SceneName == "Overworld Redux" && __instance.transform.position.ToString() == "(21.0, 20.0, -122.0)") || 
                (SceneLoaderPatches.SceneName == "Atoll Redux") && __instance.transform.position.ToString() == "(64.0, 4.0, 0.0)") {
                PlayerCharacter.instance.transform.GetChild(0).GetChild(0).GetChild(10).GetChild(0).gameObject.GetComponent<MeshRenderer>().materials = ModelSwaps.Items["Key"].GetComponent<MeshRenderer>().materials;
            }
            if (SaveFile.GetInt(HexagonQuestEnabled) == 1) {
                if (__instance.transform.position.ToString() == "(0.0, 0.0, 0.0)" && SceneLoaderPatches.SceneName == "Spirit Arena" && SaveFile.GetInt(GoldHexagonQuantity) < SaveFile.GetInt(HexagonQuestGoal)) {
                    GenericMessage.ShowMessage($"\"<#EAA615>Sealed Forever.\"");
                    return false;
                }
                if (__instance.transform.position.ToString() == "(2.0, 46.0, 0.0)" && SceneLoaderPatches.SceneName == "Overworld Redux" && !(StateVariable.GetStateVariableByName("Rung Bell 1 (East)").BoolValue && StateVariable.GetStateVariableByName("Rung Bell 2 (West)").BoolValue)) {
                    GenericMessage.ShowMessage($"\"Sealed Forever.\"");
                    return false;
                }
            }

            return true;
        }

        private static void ChangeDayNightHourglass() {
            PlayerCharacterPatches.TimeWhenLastChangedDayNight = Time.fixedTime;
            bool isNight = CycleController.IsNight;
            if (isNight) {
                CycleController.AnimateSunrise();
            } else {
                CycleController.AnimateSunset();
            }
            CycleController.IsNight = !isNight;
            CycleController.nightStateVar.BoolValue = !isNight;
            GameObject.Find("day night hourglass/rotation/hourglass").GetComponent<MeshRenderer>().materials[0].color = CycleController.IsNight ? new Color(1f, 0f, 1f, 1f) : new Color(1f, 1f, 0f, 1f);
        }

        public static void SetupDayNightHourglass() {
            GameObject DayNightSwitch = GameObject.Instantiate(GameObject.Find("Trophy Stuff/TROPHY POINT (1)/"));
            DayNightSwitch.name = "day night hourglass";
            DayNightSwitch.GetComponent<GoldenTrophyRoom>().item = null;
            DayNightSwitch.transform.GetChild(0).gameObject.SetActive(true);
            GameObject Hourglass = DayNightSwitch.transform.GetChild(0).GetChild(0).gameObject;
            Hourglass.GetComponent<MeshFilter>().mesh = ModelSwaps.Items["SlowmoItem"].GetComponent<MeshFilter>().mesh;
            Hourglass.GetComponent<MeshRenderer>().materials = ModelSwaps.Items["SlowmoItem"].GetComponent<MeshRenderer>().materials;
            Hourglass.GetComponent<MeshRenderer>().materials[0].color = CycleController.IsNight ? new Color(1f, 0f, 1f, 1f) : new Color(1f, 1f, 0f, 1f);
            Hourglass.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            Hourglass.transform.localPosition = Vector3.zero;
            Hourglass.name = "hourglass";
            Hourglass.SetActive(true);
            GameObject GlowEffect = GameObject.Instantiate(ModelSwaps.GlowEffect);
            GlowEffect.transform.parent = DayNightSwitch.transform;
            GlowEffect.transform.GetChild(0).gameObject.SetActive(false);
            GlowEffect.transform.GetChild(1).gameObject.SetActive(false);
            GlowEffect.transform.GetChild(2).gameObject.SetActive(false);
            GlowEffect.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            GlowEffect.transform.localPosition = new Vector3(-0.5f, -1f, -0.1f);
            GlowEffect.SetActive(true);
            DayNightSwitch.transform.position = new Vector3(-26.3723f, 28.9452f, -46.1847f);
            DayNightSwitch.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
        }

        public static bool BloodstainChest_IInteractionReceiver_Interact_PrefixPatch(Item i, BloodstainChest __instance) {
            if (SceneLoaderPatches.SceneName == "Changing Room") {
                CoinSpawner.SpawnCoins(20, __instance.transform.position);
                __instance.doPushbackBlast();
                return false;
            }

            return true;
        }
    }
}
