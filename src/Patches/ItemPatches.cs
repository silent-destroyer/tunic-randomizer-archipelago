using Archipelago.MultiClient.Net.Models;
using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TunicArchipelago.GhostHints;
using static TunicArchipelago.SaveFlags;

namespace TunicArchipelago {

    public class ItemPatches {
        private static ManualLogSource Logger = TunicArchipelago.Logger;

        public enum ItemResult {
            Success,
            TemporaryFailure, // Can't accept right now, but can accept in the future
            PermanentFailure // Can never accept it
        }

        public static string SaveFileCollectedKey = "randomizer picked up ";

        public static string GetChestRewardID(Chest Chest) {
            return Chest.chestID == 0 ? $"{SceneLoaderPatches.SceneName}-{Chest.transform.position.ToString()} [{SceneLoaderPatches.SceneName}]" : $"{Chest.chestID} [{SceneLoaderPatches.SceneName}]";
        }

        public static bool Chest_IInteractionReceiver_Interact_PrefixPatch(Item i, Chest __instance) {

            __instance.isFairy = false;
            return true;
        }

        public static void Chest_openSequence_MoveNext_PostfixPatch(Chest._openSequence_d__35 __instance, ref bool __result) {
            __instance._delay_5__2 = 1.35f;
            if (!__result && !__instance.__4__this.interrupted) {
                string LocationId = GetChestRewardID(__instance.__4__this);
                if (__instance.__4__this.chestID == 0) {
                    string FairyId = $"{__instance.__4__this.gameObject.scene.name}-{__instance.__4__this.gameObject.transform.position.ToString()}";
                    SaveFile.SetInt(ItemLookup.FairyLookup[FairyId].Flag, 1);
                    SaveFile.SetInt($"randomizer opened fairy chest {FairyId}", 1);
                }
                Logger.LogInfo("Checking Location: " + LocationId + " - " + Locations.LocationIdToDescription[LocationId]);
                Archipelago.instance.ActivateCheck(Locations.LocationIdToDescription[LocationId]);
            }

        }

        public static bool Chest_InterruptOpening_PrefixPatch(Chest __instance) {
            if (TunicArchipelago.Settings.DisableChestInterruption) {
                return false;
            }
/*            if (__instance.chestID == 0 || __instance.chestID == 5) {
                return false;
            }*/
            return true;
        }

        public static bool Chest_moneySprayQuantityFromDatabase_GetterPatch(Chest __instance, ref int __result) {

            __result = 0;

            return false;
        }

        public static bool Chest_itemContentsfromDatabase_GetterPatch(Chest __instance, ref Item __result) {
            
            __result = null;
            
            return false;
        }

        public static bool Chest_itemQuantityFromDatabase_GetterPatch(Chest __instance, ref int __result) {

            __result = 0;

            return false;
        }

        public static bool Chest_shouldShowAsOpen_GetterPatch(Chest __instance, ref bool __result) {
            string ActiveScene = SceneManager.GetActiveScene().name;
            if (ActiveScene == "Quarry") {
                __result = false;
                return false;
            }

            string ChestObjectId = __instance.chestID == 0 ? $"{__instance.gameObject.scene.name}-{__instance.transform.position.ToString()} [{ActiveScene}]" : $"{__instance.chestID} [{ActiveScene}]";
            if (Locations.LocationIdToDescription.ContainsKey(ChestObjectId)) {
                __result = SaveFile.GetInt($"randomizer picked up {ChestObjectId}") == 1 || (TunicArchipelago.Settings.CollectReflectsInWorld && SaveFile.GetInt($"randomizer {ChestObjectId} was collected") == 1);
                if (__result && __instance.GetComponentInParent<ToggleObjectBySpell>() != null && TunicArchipelago.Settings.CollectReflectsInWorld) {
                    __instance.GetComponentInParent<ToggleObjectBySpell>().stateVar = StateVariable.GetStateVariableByName("true");
                }
                return false;
            }
            return true;
        }

        public static bool ItemPickup_onGetIt_PrefixPatch(ItemPickup __instance) {
            if (__instance.itemToGive.Type == Item.ItemType.MONEY) {
                return true;
            }

            string LocationId = $"{__instance.itemToGive.name} [{SceneLoaderPatches.SceneName}]";
            Archipelago.instance.ActivateCheck(Locations.LocationIdToDescription[LocationId]);

            __instance.pickupStateVar.BoolValue = true;
            return false;
        }

        public static bool HeroRelicPickup_onGetIt_PrefixPatch(HeroRelicPickup __instance) {
            string LocationId = $"{__instance.name} [{SceneLoaderPatches.SceneName}]";
            Archipelago.instance.ActivateCheck(Locations.LocationIdToDescription[LocationId]);

            __instance.pickupStateVar.BoolValue = true;
            __instance.destroyOrDisable();
            return false;
        }

        public static bool PagePickup_onGetIt_PrefixPatch(PagePickup __instance) {
            string LocationId = $"{__instance.pageName} [{SceneLoaderPatches.SceneName}]";
            Archipelago.instance.ActivateCheck(Locations.LocationIdToDescription[LocationId]);

            SaveFile.SetInt($"unlocked page {PagePickup.LeafNameToLeafNumber(__instance.pageName)}", 1);
            SaveFile.SetInt($"randomizer picked up page {PagePickup.LeafNameToLeafNumber(__instance.pageName)}", 1);
            return false;
        }

        public static bool ShopItem_IInteractionReceiver_Interact_PrefixPatch(Item i, ShopItem __instance) {
            string LocationId = $"{__instance.name} [Shop]";
            if (Locations.LocationIdToDescription.ContainsKey(LocationId)) {
                int Price = TunicArchipelago.Settings.CheaperShopItemsEnabled ? 300 : __instance.price;
                ArchipelagoItem ShopItem = ItemLookup.ItemList[LocationId];
                string itemToDisplay = Archipelago.instance.GetPlayerGame(ShopItem.Player) == "Tunic" && TextBuilderPatches.ItemNameToAbbreviation.ContainsKey(ShopItem.ItemName) ? TextBuilderPatches.ItemNameToAbbreviation[ShopItem.ItemName] : "[archipelago]";
                __instance.confirmPurchaseFormattedLanguageLine.text = $"bI for {Price} [money]?\n\t" + GhostHints.WordWrapString($"\"({Archipelago.instance.GetPlayerName(ShopItem.Player).ToUpper().Replace(" ", "\" \"")}'S\" {itemToDisplay} \"{ShopItem.ItemName.ToUpper().Replace($" ", $"\" \"")})\"");

                string CheckName = Locations.LocationIdToDescription[LocationId];
                if (TunicArchipelago.Settings.SendHintsToServer && SaveFile.GetInt($"archipelago sent optional hint to server {CheckName}") == 0) {
                    Archipelago.instance.integration.session.Locations.ScoutLocationsAsync(true, Archipelago.instance.GetLocationId(CheckName));
                    SaveFile.SetInt($"archipelago sent optional hint to server {CheckName}", 1);
                }
            } else {
                __instance.confirmPurchaseFormattedLanguageLine.text = $"bI for {__instance.price} [money]?";
            }

            return true;
        }

        public static bool ShopItem_buy_PrefixPatch(ShopItem __instance) {
            string LocationId = $"{__instance.name} [Shop]";
            if (!Locations.LocationIdToDescription.ContainsKey(LocationId)) {
                if (TunicArchipelago.Settings.SkipItemAnimations && __instance.itemToGive != null) {
                    TunicArchipelago.Settings.SkipItemAnimations = false;
                    ItemPresentation.PresentItem(__instance.itemToGive, __instance.quantityToGive);
                    TunicArchipelago.Settings.SkipItemAnimations = true;
                }
                return true;
            }
            int Price = TunicArchipelago.Settings.CheaperShopItemsEnabled ? 300 : __instance.price;
            if (Inventory.GetItemByName("MoneySmall").Quantity < Price) {
                GenericMessage.ShowMessage($"nawt Enuhf [money]...");
            } else {
                Inventory.GetItemByName("MoneySmall").Quantity -= Price;

                Archipelago.instance.ActivateCheck(Locations.LocationIdToDescription[LocationId]);
                __instance.boughtStatevar.BoolValue = true;
            }
            
            InShopPlayerMoneyDisplay.Show = false;
            return false;
        }

        public static void TrinketWell_TossedInCoin_PostfixPatch(TrinketWell __instance) {
            TunicArchipelago.Tracker.ImportantItems["Coins Tossed"]++;
            ItemTracker.SaveTrackerFile();
        }

        public static bool TrinketWell_giveTrinketUpgrade_PrefixPatch(TrinketWell._giveTrinketUpgrade_d__14 __instance) {
            string LocationId = $"Well Reward ({StateVariable.GetStateVariableByName("Trinket Coins Tossed").IntValue} Coins) [Trinket Well]";
            if (Locations.LocationIdToDescription.ContainsKey(LocationId)) {
                Archipelago.instance.ActivateCheck(Locations.LocationIdToDescription[LocationId]);
            }
            return false;
        }

        public static ItemResult GiveItem(string ItemName, NetworkItem networkItem) {
            if(ItemPresentation.instance.isActiveAndEnabled || GenericMessage.instance.isActiveAndEnabled || 
                NPCDialogue.instance.isActiveAndEnabled || PageDisplay.instance.isActiveAndEnabled || GenericPrompt.instance.isActiveAndEnabled ||
                GameObject.Find("_GameGUI(Clone)/PauseMenu/") != null || GameObject.Find("_OptionsGUI(Clone)") != null || PlayerCharacter.InstanceIsDead) {
                return ItemResult.TemporaryFailure;
            }
            
            if (!ItemLookup.Items.ContainsKey(ItemName)) {
                return ItemResult.PermanentFailure;
            }

            string NotificationTop = "";
            string NotificationBottom = "";
            bool DisplayMessageAnyway = false;

            ItemData Item = ItemLookup.Items[ItemName];
            string LocationId = Archipelago.instance.integration.session.Locations.GetLocationNameFromId(networkItem.Location);
            
            if (Item.Type == ItemTypes.MONEY) {
                int AmountToGive = Item.QuantityToGive;

                Dictionary<string, int> OriginalShopPrices = new Dictionary<string, int>() {
                    { "Shop - Potion 1", 300 },
                    { "Shop - Potion 2", 1000 },
                    { "Shop - Coin 1", 999 },
                    { "Shop - Coin 2", 999 }
                };
                // If buying your own money item from the shop, increase amount rewarded
                if (OriginalShopPrices.ContainsKey(LocationId) && (networkItem.Player == Archipelago.instance.GetPlayerSlot())) {
                    AmountToGive += TunicArchipelago.Settings.CheaperShopItemsEnabled ? 300 : OriginalShopPrices[LocationId];
                }

                CoinSpawner.SpawnCoins(AmountToGive, PlayerCharacter.instance.transform.position);
            }

            if (Item.Type == ItemTypes.INVENTORY || Item.Type == ItemTypes.TRINKET) {
                Item InventoryItem = Inventory.GetItemByName(Item.ItemNameForInventory);
                InventoryItem.Quantity += Item.QuantityToGive;
                if (Item.Name == "Stick" || Item.Name == "Sword") {
                    InventoryItem.collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                    InventoryItem.collectionMessage.text = $"fownd ahn Itehm!";
                }
                if (Item.Name == "Dath Stone") {
                    Inventory.GetItemByName("Torch").Quantity = 1;
                }
                ItemPresentation.PresentItem(InventoryItem, Item.QuantityToGive);
                if (TunicArchipelago.Settings.SkipItemAnimations && Item.Name == "Flask Shard" && Inventory.GetItemByName("Flask Shard").Quantity >= 3) {
                    Inventory.GetItemByName("Flask Shard").Quantity -= 3;
                    Inventory.GetItemByName("Flask Container").Quantity += 1;
                }
            }

            if (Item.Type == ItemTypes.SWORDUPGRADE) {

                if (SaveFile.GetInt(SwordProgressionEnabled) == 1 && Item.Name == "Sword Upgrade") {
                    int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);
                    SwordProgression.UpgradeSword(SwordLevel+1);
                }
                if (TunicArchipelago.Settings.ShowItemsEnabled) {
                    ModelSwaps.SwapItemsInScene();
                }
            }

            if (Item.Type == ItemTypes.FAIRY) {
                foreach(string Fairy in ItemLookup.FairyLookup.Keys) { 
                    if(SaveFile.GetInt($"randomizer obtained fairy {Fairy}") == 0) {
                        SaveFile.SetInt($"randomizer obtained fairy {Fairy}", 1);
                        break;
                    }
                }
                GameObject.Instantiate(ModelSwaps.FairyAnimation, PlayerCharacter.instance.transform.position, Quaternion.identity).SetActive(true);
                NotificationBottom = $"\"{TunicArchipelago.Tracker.ImportantItems["Fairies"] + 1}/20\" fArEz fownd.";
            }

            if (Item.Type == ItemTypes.PAGE) {
                SaveFile.SetInt($"randomizer obtained page {Item.ItemNameForInventory}", 1);
                bool HasAllPages = true;
                for (int i = 0; i < 28; i++) {
                    if (SaveFile.GetInt($"randomizer obtained page {i}") == 0) {
                        HasAllPages = false;
                        break;
                    }                    
                }
                if (!StateVariable.GetStateVariableByName("Has Been Betrayed").BoolValue && HasAllPages) {
                    StateVariable.GetStateVariableByName("Has Been Betrayed").BoolValue = HasAllPages;
                }
                if (SaveFile.GetInt(AbilityShuffle) == 1) {
                    Dictionary<string, (string, string, string)> pagesForAbilities = new Dictionary<string, (string, string, string)>() {
                        { "12", (PrayerUnlocked, PrayerUnlockedTime, ItemLookup.PrayerUnlockedLine) },
                        { "21", (HolyCrossUnlocked, HolyCrossUnlockedTime, ItemLookup.HolyCrossUnlockedLine) },
                        { "26", (IceRodUnlocked, IceRodUnlockedTime, ItemLookup.IceRodUnlockedLine) },
                    };
                    if (pagesForAbilities.ContainsKey(Item.ItemNameForInventory)) {
                        SaveFile.SetInt(pagesForAbilities[Item.ItemNameForInventory].Item1, 1);
                        SaveFile.SetFloat(pagesForAbilities[Item.ItemNameForInventory].Item2, SpeedrunData.inGameTime);
                        NotificationBottom = pagesForAbilities[Item.ItemNameForInventory].Item3;
                        DisplayMessageAnyway = true;
                        if (Item.ItemNameForInventory == "21") {
                            ToggleHolyCrossObjects(true);
                        }
                    }
                }
                if (!TunicArchipelago.Settings.SkipItemAnimations) {
                    PageDisplay.ShowPage(int.Parse(Item.ItemNameForInventory, CultureInfo.InvariantCulture));
                } else {
                    SaveFile.SetInt("last page viewed", int.Parse(Item.ItemNameForInventory, CultureInfo.InvariantCulture));
                }
            }

            if (Item.Type == ItemTypes.GOLDENTROPHY) {

                Item GoldenTrophy = Inventory.GetItemByName(Item.ItemNameForInventory);
                GoldenTrophy.Quantity += Item.QuantityToGive;
                // Apply bonus upgrade text
                if (TunicArchipelago.Settings.BonusStatUpgradesEnabled) {
                    GoldenTrophy.collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                    GoldenTrophy.collectionMessage.text = ItemLookup.BonusUpgrades[Item.ItemNameForInventory].CustomPickupMessage;
                    Inventory.GetItemByName(ItemLookup.BonusUpgrades[Item.ItemNameForInventory].LevelUp).Quantity += 1;
                } else {
                    GoldenTrophy.collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                    GoldenTrophy.collectionMessage.text = $"kawngrahJoulA$uhnz!";
                }

                ItemPresentation.PresentItem(GoldenTrophy);
            }

            if (Item.Type == ItemTypes.RELIC) {

                Item RelicItem = Inventory.GetItemByName(Item.ItemNameForInventory);
                RelicItem.Quantity += Item.QuantityToGive;
                if (TunicArchipelago.Settings.BonusStatUpgradesEnabled) {
                    Inventory.GetItemByName(ItemLookup.BonusUpgrades[Item.ItemNameForInventory].LevelUp).Quantity += 1;
                }

                // Apply custom pickup text
                RelicItem.collectionMessage = new LanguageLine();
                RelicItem.collectionMessage.text = ItemLookup.BonusUpgrades[Item.ItemNameForInventory].CustomPickupMessage;
                
                ItemPresentation.PresentItem(RelicItem);
                if (SceneManager.GetActiveScene().name == "Overworld Interiors") {
                    SceneLoaderPatches.ToggleOldHouseRelics();
                }
            }

            if (Item.Type == ItemTypes.FOOLTRAP) {
                (NotificationTop, NotificationBottom) = ApplyFoolEffect(networkItem.Player);
                DisplayMessageAnyway = true;
            }

            if(Item.Type == ItemTypes.HEXAGONQUEST) {
                Inventory.GetItemByName("Hexagon Gold").Quantity += 1;
                int GoldHexes = Inventory.GetItemByName("Hexagon Gold").Quantity;

                if (SaveFile.GetInt(AbilityShuffle) == 1) {
                    Dictionary<int, (string, string, string)> hexesForAbilities = new Dictionary<int, (string, string, string)>() {
                        { SaveFile.GetInt(HexagonQuestPrayer), (PrayerUnlocked, PrayerUnlockedTime, ItemLookup.PrayerUnlockedLine) },
                        { SaveFile.GetInt(HexagonQuestHolyCross), (HolyCrossUnlocked, HolyCrossUnlockedTime, ItemLookup.HolyCrossUnlockedLine) },
                        { SaveFile.GetInt(HexagonQuestIceRod), (IceRodUnlocked, IceRodUnlockedTime, ItemLookup.IceRodUnlockedLine) },
                    };
                    if (hexesForAbilities.ContainsKey(GoldHexes)) {
                        SaveFile.SetInt(hexesForAbilities[GoldHexes].Item1, 1);
                        SaveFile.SetFloat(hexesForAbilities[GoldHexes].Item2, SpeedrunData.inGameTime);
                        NotificationBottom = hexesForAbilities[GoldHexes].Item3;
                        DisplayMessageAnyway = true;
                        if (GoldHexes == SaveFile.GetInt(HexagonQuestHolyCross)) {
                            ToggleHolyCrossObjects(true);
                        }
                    }
                }

                ItemPresentation.PresentItem(Inventory.GetItemByName(Item.ItemNameForInventory));
            }

            if (ItemLookup.MajorItems.Contains(Item.Name)) {
                if (Item.Type == ItemTypes.SWORDUPGRADE && SaveFile.GetInt(SwordProgressionEnabled) == 1) {
                    SaveFile.SetFloat($"randomizer Sword Progression {SaveFile.GetInt(SwordProgressionLevel)} time", SpeedrunData.inGameTime);
                }
                if (Item.Type == ItemTypes.PAGE) {
                    SaveFile.SetFloat($"randomizer {Item.Name} 1 time", SpeedrunData.inGameTime);
                } else {
                    SaveFile.SetFloat($"randomizer {Item.Name} {TunicArchipelago.Tracker.ImportantItems[Item.ItemNameForInventory] + 1} time", SpeedrunData.inGameTime);
                }
            }

            if (networkItem.Player != Archipelago.instance.GetPlayerSlot()) {
                var sender = Archipelago.instance.GetPlayerName(networkItem.Player);
                NotificationTop = NotificationTop == "" ? $"\"{sender}\" sehnt yoo  {(TextBuilderPatches.ItemNameToAbbreviation.ContainsKey(ItemName) ? TextBuilderPatches.ItemNameToAbbreviation[ItemName] : "")}  \"{ItemName}!\"" : NotificationTop;
                NotificationBottom = NotificationBottom == "" ? $"Rnt #A nIs\"?\"" : NotificationBottom;
                ShowNotification(NotificationTop, NotificationBottom);
            }

            if (networkItem.Player == Archipelago.instance.GetPlayerSlot() && (TunicArchipelago.Settings.SkipItemAnimations || DisplayMessageAnyway)) {
                NotificationTop = NotificationTop == "" ? $"yoo fownd  {(TextBuilderPatches.ItemNameToAbbreviation.ContainsKey(ItemName) ? TextBuilderPatches.ItemNameToAbbreviation[ItemName] : "")}  \"{ItemName}!\"" : NotificationTop;
                NotificationBottom = NotificationBottom == "" ? $"$oud bE yoosfuhl!" : NotificationBottom;
                ShowNotification(NotificationTop, NotificationBottom);
            }

            if (TunicArchipelago.Settings.HeroPathHintsEnabled) {
                string slotLoc = networkItem.Player.ToString() + ", " + Archipelago.instance.GetLocationName(networkItem.Location);
                foreach (string hintId in Hints.HintStructureScenes.Keys) {
                    if (hintId == slotLoc) {
                        SaveFile.SetInt($"randomizer got {hintId}", 1);
                        string sceneName = SceneManager.GetActiveScene().name;
                        if (sceneName == Hints.HintStructureScenes[hintId]) {
                            Hints.ToggleHintIndicator(sceneName, true);
                        }
                    }
                }
            }

            TunicArchipelago.Tracker.SetCollectedItem(ItemName, true);

            return ItemResult.Success;
        }

        private static (string, string) ApplyFoolEffect(int Player) {
            System.Random Random = new System.Random();
            int FoolType = PlayerCharacterPatches.StungByBee ? Random.Next(21, 100) : Random.Next(100);
            string FoolMessageTop = $"";
            string FoolMessageBottom = $"";
            if (FoolType < 35) {
                SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
                PlayerCharacter.instance.IDamageable_ReceiveDamage(PlayerCharacter.instance.hp / 3, 0, Vector3.zero, 0, 0);
                FoolMessageTop = $"yoo R A \"<#ffd700>FOOL<#ffffff>!!\" [fooltrap]";
                FoolMessageBottom = $"\"(\"it wuhz A swRm uhv <#ffd700>bEz\"...)\"";
                PlayerCharacterPatches.StungByBee = true;
                PlayerCharacter.instance.Flinch(true);
            } else if (FoolType >= 35 && FoolType < 50) {
                PlayerCharacter.ApplyRadiationAsDamageInHP(0f);
                PlayerCharacter.instance.stamina = 0;
                PlayerCharacter.instance.cachedFireController.FireAmount = 3f;
                SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
                FoolMessageTop = $"yoo R A \"<#FF3333>FOOL<#ffffff>!!\" [fooltrap]";
                FoolMessageBottom = $"iz it hawt in hEr?";
                PlayerCharacter.instance.Flinch(true);
            } else if (FoolType >= 50) {
                PlayerCharacter.ApplyRadiationAsDamageInHP(PlayerCharacter.instance.maxhp * .2f);
                SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
                SFX.PlayAudioClipAtFox(PlayerCharacter.standardFreezeSFX);
                PlayerCharacter.instance.AddFreezeTime(3f);
                FoolMessageTop = $"yoo R A \"<#86A5FF>FOOL<#ffffff>!!\" [fooltrap]";
                FoolMessageBottom = $"hahvi^ ahn Is tIm?";
            }

            if (Player != Archipelago.instance.GetPlayerSlot()) {
                FoolMessageTop = $"\"{Archipelago.instance.GetPlayerName(Player)}\" %i^ks {FoolMessageTop}";
            }

            return (FoolMessageTop, FoolMessageBottom);
        }

        public static void ToggleHolyCrossObjects(bool isEnabled) {
            foreach (ToggleObjectBySpell SpellToggle in Resources.FindObjectsOfTypeAll<ToggleObjectBySpell>()) {
                foreach (ToggleObjectBySpell Spell in SpellToggle.gameObject.GetComponents<ToggleObjectBySpell>()) {
                    Spell.enabled = isEnabled;
                }
            }
        }

        public static void PotionCombine_Show_PostFixPatch(PotionCombine __instance) {
            //TunicArchipelago.Tracker.ImportantItems["Flask Container"]++;
            //ItemTracker.SaveTrackerFile();
        }

        public static void ButtonAssignableItem_CheckFreeItemSpell_PostfixPatch(ButtonAssignableItem __instance, ref string s) {
            if (ItemLookup.BombCodes.ContainsKey(s) && StateVariable.GetStateVariableByName(ItemLookup.BombCodes[s]).BoolValue) {
                if (SaveFile.GetInt($"randomizer used free bomb code {s}") == 0) {
                    Archipelago.instance.UpdateDataStorage(ItemLookup.BombCodes[s], true);
                    SaveFile.SetInt($"randomizer used free bomb code {s}", 1);
                    if (TunicArchipelago.Settings.SkipItemAnimations) { 
                        switch(ItemLookup.BombCodes[s]) {
                            case "Granted Firecracker":
                                ShowNotification($"[firecracker] \"Firecracker Granted!\"", $"mAd fruhm slorm, #uh poudi^ #aht gOz boom.");
                                break;
                            case "Granted Firebomb":
                                ShowNotification($"[firebomb] \"Fire Bomb Granted!\"", $"fIur fIur ehvurEwAr, ahnd ow ow ow ow ow.");
                                break;
                            case "Granted Icebomb":
                                ShowNotification($"[icebomb] \"Ice Bomb Granted!\"", $"uhnstAboul powdur mAd fruhm #uh fArE uhv #uh wehst gRdin.");
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }


        public static bool UpgradeAltar_DoOfferingSequence_PrefixPatch(UpgradeAltar __instance, OfferingItem offeringItemToOffer) {
            if (TunicArchipelago.Settings.FasterUpgrades) {
                ShowNotification($"{TextBuilderPatches.SpriteNameToAbbreviation[offeringItemToOffer.icon.name]} \"{offeringItemToOffer.statLabelLocKey}\" wehnt uhp fruhm {offeringItemToOffer.upgradeItemReceived.Quantity-1} [arrow_right] {offeringItemToOffer.upgradeItemReceived.Quantity}!", $"#E Ar ahksehpts yor awfuri^.");
                UpgradeMenu.instance.__Exit();
                return false;
            }

            return true;
        }

        public static void UpgradeAltar_DoOfferingSequence_PostfixPatch(UpgradeAltar __instance) {
            foreach (string LevelUp in ItemLookup.LevelUpItems) {
                TunicArchipelago.Tracker.ImportantItems[LevelUp] = Inventory.GetItemByName(LevelUp).Quantity;
            }
        }

        public static bool FairyCollection_getFairyCount_PrefixPatch(FairyCollection __instance, ref int __result) {
            __result = TunicArchipelago.Tracker.ImportantItems["Fairies"];
            return false;
        }

        private static void ShowNotification(string topLine, string bottomLine) {
            var topLineObject = ScriptableObject.CreateInstance<LanguageLine>();
            topLineObject.text = topLine;

            var bottomLineObject = ScriptableObject.CreateInstance<LanguageLine>();
            bottomLineObject.text = bottomLine;

            var areaData = ScriptableObject.CreateInstance<AreaData>();
            areaData.topLine = topLineObject;
            areaData.bottomLine = bottomLineObject;

            AreaLabel.ShowLabel(areaData);
        }
    }
}
