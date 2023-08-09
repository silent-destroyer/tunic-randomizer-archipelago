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
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

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
            if (__instance.chestID == 19) {
                if (__instance.transform.position.ToString() == "(8.8, 0.0, 9.9)") {
                    __result = SaveFile.GetInt("randomizer picked up 19 [Sword Cave]") == 1;
                } else {
                    __result = SaveFile.GetInt("randomizer picked up 19 [Forest Belltower]") == 1;
                }
                return false;
            }
            if (__instance.chestID == 5) {
                __result = SaveFile.GetInt("randomizer picked up 5 [Overworld Redux]") == 1;
                return false;
            }
            string FairyId = $"{__instance.gameObject.scene.name}-{__instance.transform.position.ToString()}";
            if (ItemLookup.FairyLookup.ContainsKey(FairyId)) {
                __result = SaveFile.GetInt($"randomizer opened fairy chest {FairyId}") == 1;
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
                __instance.confirmPurchaseFormattedLanguageLine.text = $"bI for {Price} [money]?";

                if (ShopItem.Player != Archipelago.instance.GetPlayerSlot()) {
                    __instance.confirmPurchaseFormattedLanguageLine.text = GhostHints.WordWrapString($"bI for {Price} [money]?\n\"({Archipelago.instance.GetPlayerName(ShopItem.Player).ToUpper().Replace(" ", "\" \"")}'S {ShopItem.ItemName.ToUpper().Replace($" ", $"\" \"")})\"");
                    Logger.LogInfo(GhostHints.WordWrapString($"bI for {Price} [money]?\n\"({Archipelago.instance.GetPlayerName(ShopItem.Player).ToUpper().Replace(" ", "\" \"")}'S {ShopItem.ItemName.ToUpper().Replace($" ", $"\" \"")})\""));
                }
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
                GameObject.Find("_GameGUI(Clone)/PauseMenu/") != null || GameObject.Find("_OptionsGUI(Clone)") != null || PlayerCharacter.instance.IsDead) {
                return ItemResult.TemporaryFailure;
            }

            if (!ItemLookup.Items.ContainsKey(ItemName)) {
                return ItemResult.PermanentFailure;
            }

            ItemData Item = ItemLookup.Items[ItemName];
            string LocationId = Archipelago.instance.integration.session.Locations.GetLocationNameFromId(networkItem.Location);
            
            if (Item.Type == ItemTypes.MONEY) {
                int AmountToGive = Item.QuantityToGive;

                RandomizerSettings.FoolTrapOption FoolOption = TunicArchipelago.Settings.FoolTrapIntensity;

                if ((FoolOption == RandomizerSettings.FoolTrapOption.NORMAL && AmountToGive < 20) ||
                    (FoolOption == RandomizerSettings.FoolTrapOption.DOUBLE && AmountToGive <= 20) ||
                    (FoolOption == RandomizerSettings.FoolTrapOption.ONSLAUGHT && AmountToGive <= 30)) {
                    ApplyFoolEffect();
                } else {
                    Dictionary<string, int> OriginalShopPrices = new Dictionary<string, int>() {
                    { "Shop - Potion 1", 300 },
                    { "Shop - Potion 2", 1000 },
                    { "Shop - Coin 1", 999 },
                    { "Shop - Coin 2", 999 }
                };

                    // If buying your own money item from the shop, increase amount rewarded
                    if (OriginalShopPrices.ContainsKey(LocationId) && (networkItem.Player == Archipelago.instance.GetPlayerSlot())) {
                        AmountToGive += OriginalShopPrices[LocationId];
                    }

                    CoinSpawner.SpawnCoins(AmountToGive, PlayerCharacter.instance.transform.position);
                }

            }

            if (Item.Type == ItemTypes.INVENTORY || Item.Type == ItemTypes.TRINKET) {
                Item InventoryItem = Inventory.GetItemByName(Item.ItemNameForInventory);
                InventoryItem.Quantity += Item.QuantityToGive;
                ItemPresentation.PresentItem(InventoryItem, Item.QuantityToGive);
            }

            if (Item.Type == ItemTypes.SWORD) {

                if (SaveFile.GetInt("randomizer sword progression enabled") == 1) {
                    int SwordLevel = SaveFile.GetInt("randomizer sword progression level");
                    SwordProgression.UpgradeSword(SwordLevel+1);
                } else {
                    Item InventoryItem = Inventory.GetItemByName(Item.ItemNameForInventory);
                    InventoryItem.collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                    InventoryItem.collectionMessage.text = $"fownd ahn Itehm!";

                    InventoryItem.Quantity += Item.QuantityToGive;
                    ItemPresentation.PresentItem(InventoryItem);
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
                if (SaveFile.GetInt("randomizer shuffled abilities") == 1) {
                    Dictionary<string, string> pagesForAbilities = new Dictionary<string, string>() {
                        { "12", "randomizer prayer unlocked" },
                        { "21", "randomizer holy cross unlocked" },
                        { "26", "randomizer ice rod unlocked" },
                    };
                    if (pagesForAbilities.ContainsKey(Item.ItemNameForInventory)) {
                        PageDisplayPatches.ShowAbilityUnlock = true;
                        PageDisplayPatches.AbilityUnlockPage = Item.ItemNameForInventory;
                        SaveFile.SetInt(pagesForAbilities[Item.ItemNameForInventory], 1);
                        SaveFile.SetFloat($"{pagesForAbilities[Item.ItemNameForInventory]} time", SpeedrunData.inGameTime);
                        if(Item.ItemNameForInventory == "21") {
                            foreach (ToggleObjectBySpell SpellToggle in Resources.FindObjectsOfTypeAll<ToggleObjectBySpell>()) {
                                SpellToggle.gameObject.GetComponent<ToggleObjectBySpell>().enabled = true;
                            }
                        }
                    }
                }

                PageDisplay.ShowPage(int.Parse(Item.ItemNameForInventory, CultureInfo.InvariantCulture));
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
                Item ItemToPresent = Inventory.GetItemByName(ItemLookup.HeroRelicLookup[Item.ItemNameForInventory].ItemPresentedOnCollection);

                // Apply custom pickup text
                LanguageLine OriginalText = ItemToPresent.CollectionMessage;
                ItemToPresent.collectionMessage = new LanguageLine();
                ItemToPresent.collectionMessage.text = ItemLookup.BonusUpgrades[Item.ItemNameForInventory].CustomPickupMessage;
                
                ItemPresentation.PresentItem(ItemToPresent);

                // Revert pickup text for regular stat upgrades
                ItemToPresent.collectionMessage = OriginalText;
            }

            if (Item.Type == ItemTypes.SPECIAL) {
                Inventory.GetItemByName("Homeward Bone Statue").Quantity += Item.QuantityToGive;
                ItemPresentation.PresentItem(Inventory.GetItemByName("Key Special"));
            }

            if(Item.Type == ItemTypes.HEXAGONQUEST) {
                SaveFile.SetInt("randomizer inventory quantity Hexagon Gold", SaveFile.GetInt("randomizer inventory quantity Hexagon Gold") + 1);

                int GoldHexes = SaveFile.GetInt("randomizer inventory quantity Hexagon Gold");

                if (GoldHexes == SaveFile.GetInt("randomizer hexagon quest prayer requirement")) {
                    SaveFile.SetInt("randomizer prayer unlocked", 1);
                    SaveFile.SetFloat("randomizer prayer unlocked time", SpeedrunData.inGameTime);
                    ShowNotification($"\"PRAYER Unlocked\"", $"Jahnuhl yor wizduhm, rooin sEkur");
                }
                if (GoldHexes == SaveFile.GetInt("randomizer hexagon quest holy cross requirement"))
                {
                    SaveFile.SetInt("randomizer holy cross unlocked", 1);
                    SaveFile.SetFloat("randomizer holy cross unlocked time", SpeedrunData.inGameTime);
                    ShowNotification($"\"HOLY CROSS Unlocked\"", $"sEk wuht iz rItfuhlE yorz");
                    foreach (ToggleObjectBySpell SpellToggle in Resources.FindObjectsOfTypeAll<ToggleObjectBySpell>()) {
                        SpellToggle.gameObject.GetComponent<ToggleObjectBySpell>().enabled = true;
                    }
                }
                if (GoldHexes == SaveFile.GetInt("randomizer hexagon quest ice rod requirement")) {
                    SaveFile.SetInt("randomizer ice rod unlocked", 1);
                    SaveFile.SetFloat("randomizer ice rod unlocked time", SpeedrunData.inGameTime);
                    ShowNotification($"\"ICE ROD Unlocked\"", $"#A wOnt nO wuht hit #ehm");
                }
                ItemPresentation.PresentItem(Inventory.GetItemByName("Hexagon Blue"));
            }

            if (ItemLookup.MajorItems.Contains(Item.Name)) {
                if (Item.Type == ItemTypes.SWORD && SaveFile.GetInt("randomizer sword progression enabled") == 1) {
                    SaveFile.SetFloat($"randomizer Sword Progression {SaveFile.GetInt("randomizer sword progression level")} time", SpeedrunData.inGameTime);
                }
                if (Item.Type == ItemTypes.PAGE) {
                    SaveFile.SetFloat($"randomizer {Item.Name} 1 time", SpeedrunData.inGameTime);
                } else {
                    SaveFile.SetFloat($"randomizer {Item.Name} {TunicArchipelago.Tracker.ImportantItems[Item.ItemNameForInventory] + 1} time", SpeedrunData.inGameTime);
                }
            }

            TunicArchipelago.Tracker.SetCollectedItem(ItemName, true);

            return ItemResult.Success;
        }

        private static void ApplyFoolEffect() {
            System.Random Random = new System.Random();
            int FoolType = PlayerCharacterPatches.StungByBee ? Random.Next(21, 100) : Random.Next(100);
            if (FoolType < 20) {
                SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
                PlayerCharacter.instance.IDamageable_ReceiveDamage(PlayerCharacter.instance.hp / 3, 0, Vector3.zero, 0, 0);
                ShowNotification($"yoo R A \"<#ffd700>FOOL<#ffffff>!!\"", $"\"(\"it wuhz A swRm uhv <#ffd700>bEz\"...)\"");
                //GenericMessage.ShowMessage($"yoo R A \"<#ffd700>FOOL<#ffffff>!!\"\n\"(\"it wuhz A swRm uhv <#ffd700>bEz\"...)\"");
                PlayerCharacterPatches.StungByBee = true;
                PlayerCharacter.instance.Flinch(true);
            } else if (FoolType >= 20 && FoolType < 50) {
                PlayerCharacter.ApplyRadiationAsDamageInHP(0f);
                PlayerCharacter.instance.stamina = 0;
                PlayerCharacter.instance.cachedFireController.FireAmount = 3f;
                SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
                ShowNotification($"yoo R A \"<#FF3333>FOOL<#ffffff>!!\"", $"iz it hawt in hEr?");
                //GenericMessage.ShowMessage($"yoo R A \"<#FF3333>FOOL<#ffffff>!!\"");
                PlayerCharacter.instance.Flinch(true);
            } else if (FoolType >= 50) {
                PlayerCharacter.ApplyRadiationAsDamageInHP(PlayerCharacter.instance.maxhp * .2f);
                SFX.PlayAudioClipAtFox(PlayerCharacter.instance.bigHurtSFX);
                SFX.PlayAudioClipAtFox(PlayerCharacter.standardFreezeSFX);
                PlayerCharacter.instance.AddFreezeTime(3f);
                ShowNotification($"yoo R A \"<#86A5FF>FOOL<#ffffff>!!\"", $"hahvi^ ahn Is tIm?");
                GenericMessage.ShowMessage($"yoo R A \"<#86A5FF>FOOL<#ffffff>!!\"");
            }
        }

        public static void PotionCombine_Show_PostFixPatch(PotionCombine __instance) {
            //TunicArchipelago.Tracker.ImportantItems["Flask Container"]++;
            //ItemTracker.SaveTrackerFile();
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
