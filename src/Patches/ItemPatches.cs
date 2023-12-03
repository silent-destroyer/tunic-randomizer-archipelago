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
            if (__instance.chestID == 0 || __instance.chestID == 5) {
                return false;
            }
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

/*            if (__instance.chestID == 19) {
                if (ActiveScene == "Sword Cave") {
                    __result = SaveFile.GetInt("randomizer picked up 19 [Sword Cave]") == 1;
                } else {
                    __result = SaveFile.GetInt("randomizer picked up 19 [Forest Belltower]") == 1;
                }
                return false;
            }
            if (__instance.chestID == 5) {
                __result = SaveFile.GetInt("randomizer picked up 5 [Overworld Redux]") == 1;
                return false;
            }*/
            string FairyId = $"{__instance.gameObject.scene.name}-{__instance.transform.position.ToString()}";
            if (ItemLookup.FairyLookup.ContainsKey(FairyId)) {
                __result = SaveFile.GetInt($"randomizer opened fairy chest {FairyId}") == 1;
                return false;
            }
            string ChestObjectId = $"{__instance.chestID} [{ActiveScene}]";
            if (Locations.LocationIdToDescription.ContainsKey(ChestObjectId)) {
                __result = SaveFile.GetInt($"randomizer picked up {ChestObjectId}") == 1 || (TunicArchipelago.Settings.CollectReflectsInWorld && SaveFile.GetInt($"randomizer {ChestObjectId} was collected") == 1);
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
                __instance.confirmPurchaseFormattedLanguageLine.text = $"bI for {Price} [money]?\n\t" + GhostHints.WordWrapString($"\"({Archipelago.instance.GetPlayerName(ShopItem.Player).ToUpper().Replace(" ", "\" \"")}'S {ShopItem.ItemName.ToUpper().Replace($" ", $"\" \"")})\"");
            } else {
                __instance.confirmPurchaseFormattedLanguageLine.text = $"bI for {__instance.price} [money]?";
            }

            return true;
        }

        public static bool ShopItem_buy_PrefixPatch(ShopItem __instance) {
            string LocationId = $"{__instance.name} [Shop]";
            if (!Locations.LocationIdToDescription.ContainsKey(LocationId)) {
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
                ItemPresentation.PresentItem(InventoryItem, Item.QuantityToGive);
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
                ShowNotification($"yoo fownd A \"FAIRY!!\"", $"#Ar R stil \"{20 - (TunicArchipelago.Tracker.ImportantItems["Fairies"]+1)}\" lehft too fInd\"!!\"");
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
                    Dictionary<string, string> pagesForAbilities = new Dictionary<string, string>() {
                        { "12", PrayerUnlocked },
                        { "21", HolyCrossUnlocked },
                        { "26", IceRodUnlocked },
                    };
                    if (pagesForAbilities.ContainsKey(Item.ItemNameForInventory)) {
                        PageDisplayPatches.ShowAbilityUnlock = true;
                        PageDisplayPatches.AbilityUnlockPage = Item.ItemNameForInventory;
                        SaveFile.SetInt(pagesForAbilities[Item.ItemNameForInventory], 1);
                        SaveFile.SetFloat($"{pagesForAbilities[Item.ItemNameForInventory]} time", SpeedrunData.inGameTime);
                        if(Item.ItemNameForInventory == "21") {
                            foreach (ToggleObjectBySpell SpellToggle in Resources.FindObjectsOfTypeAll<ToggleObjectBySpell>()) {
                                foreach (ToggleObjectBySpell Spell in SpellToggle.gameObject.GetComponents<ToggleObjectBySpell>()) {
                                    Spell.enabled = true;
                                }
                            }
                        }
                    }
                }
                if (!TunicArchipelago.Settings.SkipItemAnimations) {
                    PageDisplay.ShowPage(int.Parse(Item.ItemNameForInventory, CultureInfo.InvariantCulture));
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
                ApplyFoolEffect(networkItem.Player);
            }

            if (Item.Type == ItemTypes.SPECIAL) {
                Inventory.GetItemByName("Homeward Bone Statue").Quantity += Item.QuantityToGive;
                Inventory.GetItemByName("Torch").Quantity = 1;
                ItemPresentation.PresentItem(Inventory.GetItemByName("Key Special"));
            }

            if(Item.Type == ItemTypes.HEXAGONQUEST) {
                SaveFile.SetInt(GoldHexagonQuantity, SaveFile.GetInt(GoldHexagonQuantity) + 1);

                int GoldHexes = SaveFile.GetInt(GoldHexagonQuantity);

                if (GoldHexes == SaveFile.GetInt(HexagonQuestPrayer)) {
                    SaveFile.SetInt(PrayerUnlocked, 1);
                    SaveFile.SetFloat(PrayerUnlockedTime, SpeedrunData.inGameTime);
                    ShowNotification($"\"PRAYER Unlocked\"", $"Jahnuhl yor wizduhm, rooin sEkur");
                }
                if (GoldHexes == SaveFile.GetInt(HexagonQuestHolyCross)) {
                    SaveFile.SetInt(HolyCrossUnlocked, 1);
                    SaveFile.SetFloat(HolyCrossUnlockedTime, SpeedrunData.inGameTime);
                    ShowNotification($"\"HOLY CROSS Unlocked\"", $"sEk wuht iz rItfuhlE yorz");
                    foreach (ToggleObjectBySpell SpellToggle in Resources.FindObjectsOfTypeAll<ToggleObjectBySpell>()) {
                        foreach (ToggleObjectBySpell Spell in SpellToggle.gameObject.GetComponents<ToggleObjectBySpell>()) {
                            Spell.enabled = true;
                        }
                    }
                }
                if (GoldHexes == SaveFile.GetInt(HexagonQuestIceRod)) {
                    SaveFile.SetInt(IceRodUnlocked, 1);
                    SaveFile.SetFloat(IceRodUnlockedTime, SpeedrunData.inGameTime);
                    ShowNotification($"\"ICE ROD Unlocked\"", $"#A wOnt nO wuht hit #ehm");
                }
                ItemPresentation.PresentItem(Inventory.GetItemByName("Hexagon Blue"));
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

            if (TunicArchipelago.Settings.HeroPathHintsEnabled) {
                string slotLoc = networkItem.Player.ToString() + ", " + Archipelago.instance.GetLocationName(networkItem.Location);
                if (Hints.ForestHintId == slotLoc) {
                    SaveFile.SetInt($"randomizer got {Hints.ForestHintId}", 1);
                    if (SceneManager.GetActiveScene().name == "Sword Access") {
                        GameObject.Find("_Setpieces/RelicPlinth (1)/cathedral_candleflame").SetActive(true);
                    }
                } else if (Hints.FortressHintId == slotLoc) {
                    SaveFile.SetInt($"randomizer got {Hints.FortressHintId}", 1);
                    if (SceneManager.GetActiveScene().name == "Fortress Reliquary") {
                        GameObject.Find("RelicPlinth/cathedral_candleflame").SetActive(true);
                    }
                } else if (Hints.GardenHintId == slotLoc) {
                    SaveFile.SetInt($"randomizer got {Hints.GardenHintId}", 1);
                    if (SceneManager.GetActiveScene().name == "Archipelagos Redux") {
                        GameObject.Find("_Environment Prefabs/RelicPlinth/cathedral_candleflame").SetActive(true);
                        GameObject.Find("_Environment Prefabs/RelicPlinth/Point Light").SetActive(true);
                    }
                } else if (Hints.SwampHintId == slotLoc) {
                    SaveFile.SetInt($"randomizer got {Hints.SwampHintId}", 1);
                    if (SceneManager.GetActiveScene().name == "Swamp Redux 2") {
                        GameObject.Find("_Setpieces Etc/RelicPlinth/cathedral_candleflame").SetActive(true);
                        GameObject.Find("_Setpieces Etc/RelicPlinth/Point Light").SetActive(true);
                    }
                } else if (Hints.LibraryHintId == slotLoc) {
                    SaveFile.SetInt($"randomizer got {Hints.LibraryHintId}", 1);
                    if (SceneManager.GetActiveScene().name == "Library Hall") {
                        GameObject.Find("_Special/RelicPlinth/cathedral_candleflame").SetActive(true);
                        GameObject.Find("_Special/RelicPlinth/Point Light").SetActive(true);
                    }
                } else if (Hints.MonasteryHintId == slotLoc) {
                    SaveFile.SetInt($"randomizer got {Hints.MonasteryHintId}", 1);
                    if (SceneManager.GetActiveScene().name == "Monastery") {
                        GameObject.Find("Root/RelicPlinth (1)/cathedral_candleflame").SetActive(true);
                        GameObject.Find("Root/RelicPlinth (1)/Point Light").SetActive(true);
                    }
                }
            }

            TunicArchipelago.Tracker.SetCollectedItem(ItemName, true);

            return ItemResult.Success;
        }

        private static void ApplyFoolEffect(int Player) {
            System.Random Random = new System.Random();
            int FoolType = PlayerCharacterPatches.StungByBee ? Random.Next(21, 100) : Random.Next(100);
            string FoolMessageTop = $"";
            string FoolMessageBottom = $"";
            if (FoolType < 35) {
                SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
                PlayerCharacter.instance.IDamageable_ReceiveDamage(PlayerCharacter.instance.hp / 3, 0, Vector3.zero, 0, 0);
                FoolMessageTop = $"yoo R A \"<#ffd700>FOOL<#ffffff>!!\"";
                FoolMessageBottom = $"\"(\"it wuhz A swRm uhv <#ffd700>bEz\"...)\"";
                PlayerCharacterPatches.StungByBee = true;
                PlayerCharacter.instance.Flinch(true);
            } else if (FoolType >= 35 && FoolType < 50) {
                PlayerCharacter.ApplyRadiationAsDamageInHP(0f);
                PlayerCharacter.instance.stamina = 0;
                PlayerCharacter.instance.cachedFireController.FireAmount = 3f;
                SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
                FoolMessageTop = $"yoo R A \"<#FF3333>FOOL<#ffffff>!!\"";
                FoolMessageBottom = $"iz it hawt in hEr?";
                PlayerCharacter.instance.Flinch(true);
            } else if (FoolType >= 50) {
                PlayerCharacter.ApplyRadiationAsDamageInHP(PlayerCharacter.instance.maxhp * .2f);
                SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
                SFX.PlayAudioClipAtFox(PlayerCharacter.standardFreezeSFX);
                PlayerCharacter.instance.AddFreezeTime(3f);
                FoolMessageTop = $"yoo R A \"<#86A5FF>FOOL<#ffffff>!!\"";
                FoolMessageBottom = $"hahvi^ ahn Is tIm?";
            }

            if (Player != Archipelago.instance.GetPlayerSlot()) {
                FoolMessageTop = $"\"{Archipelago.instance.GetPlayerName(Player)}\" %i^ks {FoolMessageTop}";
            }
            ShowNotification(FoolMessageTop, FoolMessageBottom);
        }

        public static void PotionCombine_Show_PostFixPatch(PotionCombine __instance) {
            //TunicArchipelago.Tracker.ImportantItems["Flask Container"]++;
            //ItemTracker.SaveTrackerFile();
        }

        public static void ButtonAssignableItem_CheckFreeItemSpell_PostfixPatch(ButtonAssignableItem __instance, ref string s) {
            if (ItemLookup.BombCodes.ContainsKey(s) && StateVariable.GetStateVariableByName(ItemLookup.BombCodes[s]).BoolValue) {
                Archipelago.instance.UpdateDataStorage(ItemLookup.BombCodes[s], true);
            }
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

        public static bool ItemPresentation_presentItem_PrefixPatch(ItemPresentation __instance) {
            if(TunicArchipelago.Settings.SkipItemAnimations) {
                return false;
            }
            return true;
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
