﻿using Archipelago.MultiClient.Net.Enums;
using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static TunicArchipelago.SaveFlags;

namespace TunicArchipelago {
    public class ModelSwaps {
        public static ManualLogSource Logger = TunicArchipelago.Logger;

        public static Dictionary<string, Sprite> Cards = new Dictionary<string, Sprite>();
        public static Dictionary<string, GameObject> Items = new Dictionary<string, GameObject>();
        public static Dictionary<string, GameObject> Chests = new Dictionary<string, GameObject>();
        public static GameObject SecondSword = null;
        public static GameObject ThirdSword = null;
        public static GameObject PagePickup = null;
        public static bool SwappedThisSceneAlready = false;
        public static GameObject SecondSwordImage;
        public static GameObject ThirdSwordImage;
        public static GameObject HexagonGoldImage;
        public static GameObject TuncTitleImage;
        public static GameObject TuncImage;
        public static GameObject GardenKnightVoid;
        public static GameObject MoneySfx;
        public static GameObject FairyAnimation;
        public static GameObject IceFlask;

        public static GameObject RedKeyMaterial;
        public static GameObject GreenKeyMaterial;
        public static GameObject BlueKeyMaterial;
        public static GameObject HeroRelicMaterial;

        public static Dictionary<string, GameObject> CustomItemImages = new Dictionary<string, GameObject>();

        public static bool DathStonePresentationAlreadySetup = false;
        public static bool OldHouseKeyPresentationAlreadySetup = false;
        public static bool SwordPresentationsAlreadySetup = false;
        public static GameObject GlowEffect;
        public static GameObject SpecialKeyPopup;

        public static List<string> ShopItemIDs = new List<string>() {
            "Potion (First) [Shop]",
            "Potion (West Garden) [Shop]",
            "Trinket Coin 1 (day) [Shop]",
            "Trinket Coin 2 (night) [Shop]"
        };
        public static List<string> ShopGameObjectIDs = new List<string>() {
            "Shop/Item Holder/Potion (First)/rotation/potion",
            "Shop/Item Holder/Potion (West Garden)/rotation/potion",
            "Shop/Item Holder/Trinket Coin 1 (day)/rotation/Trinket Coin",
            "Shop/Item Holder/Trinket Coin 2 (night)/rotation/Trinket Coin"
        };

        public static void InitializeItems() {
            GameObject ItemRoot = Resources.FindObjectsOfTypeAll<GameObject>().Where(Item => Item.name == "User Rotation Root").ToList()[0];
            Items["Firecracker"] = ItemRoot.transform.GetChild(3).GetChild(0).gameObject;
            Items["Firebomb"] = ItemRoot.transform.GetChild(21).GetChild(0).gameObject;
            Items["Ice Bomb"] = ItemRoot.transform.GetChild(2).GetChild(0).gameObject;

            Items["Flask Shard"] = ItemRoot.transform.GetChild(22).gameObject;
            Items["Flask Container"] = ItemRoot.transform.GetChild(10).gameObject;

            Items["Berry_HP"] = ItemRoot.transform.GetChild(1).gameObject;
            Items["Berry_MP"] = ItemRoot.transform.GetChild(25).gameObject;
            Items["Pepper"] = ItemRoot.transform.GetChild(5).gameObject;
            Items["Ivy"] = ItemRoot.transform.GetChild(45).gameObject;
            Items["Bait"] = ItemRoot.transform.GetChild(0).GetChild(0).gameObject;

            Items["Piggybank L1"] = ItemRoot.transform.GetChild(6).gameObject;

            Items["Trinket Coin"] = ItemRoot.transform.GetChild(29).gameObject;
            Items["Trinket Card"] = ItemRoot.transform.GetChild(47).gameObject;
            Items["Trinket Slot"] = ItemRoot.transform.GetChild(30).gameObject;

            Items["Sword"] = GameObject.Instantiate(ItemRoot.transform.GetChild(9).gameObject);
            Items["Sword Progression"] = GameObject.Instantiate(ItemRoot.transform.GetChild(9).gameObject);
            Items["Sword"].SetActive(false);
            Items["Sword Progression"].SetActive(false);
            GameObject.DontDestroyOnLoad(Items["Sword"]);
            GameObject.DontDestroyOnLoad(Items["Sword Progression"]);
            Items["Stick"] = ItemRoot.transform.GetChild(8).gameObject;
            Items["Shield"] = ItemRoot.transform.GetChild(7).gameObject;
            Items["Stundagger"] = ItemRoot.transform.GetChild(16).gameObject;
            Items["Techbow"] = ItemRoot.transform.GetChild(11).gameObject;
            Items["Wand"] = ItemRoot.transform.GetChild(41).gameObject;
            Items["Shotgun"] = ItemRoot.transform.GetChild(46).gameObject;
            Items["SlowmoItem"] = ItemRoot.transform.GetChild(26).gameObject;
            Items["Lantern"] = ItemRoot.transform.GetChild(17).gameObject;
            Items["Hyperdash"] = ItemRoot.transform.GetChild(40).gameObject;

            Items["Upgrade Offering - Attack - Tooth"] = ItemRoot.transform.GetChild(15).gameObject;
            Items["Upgrade Offering - DamageResist - Effigy"] = ItemRoot.transform.GetChild(13).gameObject;
            Items["Upgrade Offering - Health HP - Flower"] = ItemRoot.transform.GetChild(18).gameObject;
            Items["Upgrade Offering - Magic MP - Mushroom"] = ItemRoot.transform.GetChild(19).gameObject;
            Items["Upgrade Offering - PotionEfficiency Swig - Ash"] = ItemRoot.transform.GetChild(12).gameObject;
            Items["Upgrade Offering - Stamina SP - Feather"] = ItemRoot.transform.GetChild(14).gameObject;

            Items["GoldenTrophy_1"] = ItemRoot.transform.GetChild(31).gameObject;
            Items["GoldenTrophy_2"] = ItemRoot.transform.GetChild(32).gameObject;
            Items["GoldenTrophy_3"] = ItemRoot.transform.GetChild(33).gameObject;
            Items["GoldenTrophy_4"] = ItemRoot.transform.GetChild(34).gameObject;
            Items["GoldenTrophy_5"] = ItemRoot.transform.GetChild(35).gameObject;
            Items["GoldenTrophy_6"] = ItemRoot.transform.GetChild(36).gameObject;
            Items["GoldenTrophy_7"] = ItemRoot.transform.GetChild(37).gameObject;
            Items["GoldenTrophy_8"] = ItemRoot.transform.GetChild(38).gameObject;
            Items["GoldenTrophy_9"] = ItemRoot.transform.GetChild(39).gameObject;
            Items["GoldenTrophy_10"] = ItemRoot.transform.GetChild(42).gameObject;
            Items["GoldenTrophy_11"] = ItemRoot.transform.GetChild(43).gameObject;
            Items["GoldenTrophy_12"] = ItemRoot.transform.GetChild(44).gameObject;

            Items["Key"] = ItemRoot.transform.GetChild(4).gameObject;
            Items["Vault Key (Red)"] = ItemRoot.transform.GetChild(23).gameObject;

            Items["Hexagon Red"] = ItemRoot.transform.GetChild(24).GetChild(0).gameObject;
            Items["Hexagon Green"] = ItemRoot.transform.GetChild(27).GetChild(0).gameObject;
            Items["Hexagon Blue"] = ItemRoot.transform.GetChild(28).GetChild(0).gameObject;

            RedKeyMaterial = new GameObject();
            RedKeyMaterial.AddComponent<MeshRenderer>().materials = new Material[] { Resources.FindObjectsOfTypeAll<Material>().Where(Material => Material.name == "Questagon (1) R").ToList()[0] };
            GameObject.DontDestroyOnLoad(RedKeyMaterial);
            GreenKeyMaterial = new GameObject();
            GreenKeyMaterial.AddComponent<MeshRenderer>().materials = new Material[] { Resources.FindObjectsOfTypeAll<Material>().Where(Material => Material.name == "Questagon (2) G").ToList()[0] };
            GameObject.DontDestroyOnLoad(GreenKeyMaterial);
            BlueKeyMaterial = new GameObject();
            BlueKeyMaterial.AddComponent<MeshRenderer>().materials = new Material[] { Resources.FindObjectsOfTypeAll<Material>().Where(Material => Material.name == "Questagon (3) B").ToList()[0] };
            GameObject.DontDestroyOnLoad(BlueKeyMaterial);
            Items["Hexagon Gold"] = GameObject.Instantiate(ItemRoot.transform.GetChild(28).GetChild(0).gameObject);
            Items["Hexagon Gold"].GetComponent<MeshRenderer>().material = Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().material;
            Items["Hexagon Gold"].GetComponent<MeshRenderer>().materials[0] = Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().material;
            Items["Hexagon Gold"].SetActive(false);
            GameObject.DontDestroyOnLoad(Items["Hexagon Gold"]);

            Items["Dath Stone"] = GameObject.Instantiate(ItemRoot.transform.GetChild(28).GetChild(0).gameObject);
            Items["Dath Stone"].SetActive(false);
            Material DathStoneMaterial = Items["Lantern"].GetComponent<MeshRenderer>().material;
            DathStoneMaterial.color = new UnityEngine.Color(1, 1, 1);
            Items["Dath Stone"].GetComponent<MeshRenderer>().material = DathStoneMaterial;
            Items["Dath Stone"].GetComponent<MeshRenderer>().materials[0] = DathStoneMaterial;
            GameObject.Destroy(Items["Dath Stone"].GetComponent<Rotate>());
            GameObject.DontDestroyOnLoad(Items["Dath Stone"]);

            Items["money small"] = GameObject.Instantiate(Resources.FindObjectsOfTypeAll<ItemPickup>().Where(ItemPickup => ItemPickup.name == "Coin Pickup 1(Clone)").ToList()[0].gameObject.transform.GetChild(0).gameObject);
            Items["money medium"] = GameObject.Instantiate(Resources.FindObjectsOfTypeAll<ItemPickup>().Where(ItemPickup => ItemPickup.name == "Coin Pickup 2(Clone)").ToList()[0].gameObject.transform.GetChild(0).gameObject);
            Items["money large"] = GameObject.Instantiate(Resources.FindObjectsOfTypeAll<ItemPickup>().Where(ItemPickup => ItemPickup.name == "Coin Pickup 3(Clone)").ToList()[0].gameObject.transform.GetChild(0).gameObject);
            Items["money large"].transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            GameObject.Destroy(Items["money small"].GetComponent<Rotate>());
            GameObject.Destroy(Items["money medium"].GetComponent<Rotate>());
            GameObject.Destroy(Items["money large"].GetComponent<Rotate>());
            GameObject.DontDestroyOnLoad(Items["money small"]);
            GameObject.DontDestroyOnLoad(Items["money medium"]);
            GameObject.DontDestroyOnLoad(Items["money large"]);
            MoneySfx = new GameObject("moneysfx");
            MoneySfx.AddComponent<FMODUnity.StudioEventEmitter>().EventReference = Resources.FindObjectsOfTypeAll<ItemPickup>().Where(ItemPickup => ItemPickup.name == "Coin Pickup 1(Clone)").ToList()[0].pickupSFX;
            GameObject.DontDestroyOnLoad(MoneySfx);
            Items["money small"].SetActive(false);
            Items["money medium"].SetActive(false);
            Items["money large"].SetActive(false);
            MoneySfx.SetActive(false);

            PaletteEditor.ToonFox = new GameObject("toon fox");
            GameObject.DontDestroyOnLoad(PaletteEditor.ToonFox);
            PaletteEditor.RegularFox = new GameObject("regular fox");
            PaletteEditor.RegularFox.AddComponent<MeshRenderer>().material = FindMaterial("fox redux");
            GameObject.DontDestroyOnLoad(PaletteEditor.RegularFox);
            PaletteEditor.GhostFox = new GameObject("ghost fox");
            PaletteEditor.GhostFox.AddComponent<MeshRenderer>().material = FindMaterial("ghost material");
            GameObject.DontDestroyOnLoad(PaletteEditor.GhostFox);

            foreach (TrinketItem TrinketItem in Resources.FindObjectsOfTypeAll<TrinketItem>()) {
                Cards[TrinketItem.name] = TrinketItem.CardGraphic;
            }

            SetupOldHouseKeyItemPresentation();
            Items["Key (House)"] = ItemRoot.transform.GetChild(48).gameObject;
            SetupDathStoneItemPresentation();
            LoadTexture();
            InitializeExtras();
        }

        public static void CreateOtherWorldItemBlocks() {

            Items[$"Other World {ItemFlags.Advancement}"] = new GameObject("other world");
            Items[$"Other World {ItemFlags.Advancement}"].AddComponent<SpriteRenderer>().sprite = FindSprite("trinkets 1_slot_grey");

            GameObject Front = GameObject.Instantiate(Items[$"Other World {ItemFlags.Advancement}"]);
            Front.transform.localPosition = new Vector3(0f, 0f, -1.9f); 
            GameObject Top = GameObject.Instantiate(Items[$"Other World {ItemFlags.Advancement}"]);
            Top.transform.localPosition = new Vector3(0f, 1.9f, 0f);
            Top.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
            GameObject Bottom = GameObject.Instantiate(Items[$"Other World {ItemFlags.Advancement}"]);
            Bottom.transform.localPosition = new Vector3(0, -1.9f, 0f);
            Bottom.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
            GameObject Left = GameObject.Instantiate(Items[$"Other World {ItemFlags.Advancement}"]);
            Left.transform.localPosition = new Vector3(-1.9f, 0f, 0f);
            Left.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
            GameObject Right = GameObject.Instantiate(Items[$"Other World {ItemFlags.Advancement}"]);
            Right.transform.localPosition = new Vector3(1.9f, 0f, 0f);
            Right.transform.localEulerAngles = new Vector3(0f, 270f, 0f);
            GameObject Back = GameObject.Instantiate(Items[$"Other World {ItemFlags.Advancement}"]);
            Back.transform.localPosition = new Vector3(0f, 0f, 1.9f);
            Back.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            GameObject Cube = new GameObject("cube");
            Cube.AddComponent<MeshFilter>().mesh = GameObject.Find("Group/Jelly Cube/Jelly Cube").GetComponent<SkinnedMeshRenderer>().sharedMesh;
            Cube.AddComponent<MeshRenderer>().materials = Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
            Cube.transform.localScale = new Vector3(1.89f, 1.89f, 1.89f);
            Cube.transform.localPosition = new Vector3(0f, -1.9f, 0f);
            Top.transform.parent = Items[$"Other World {ItemFlags.Advancement}"].transform;
            Bottom.transform.parent = Items[$"Other World {ItemFlags.Advancement}"].transform;
            Left.transform.parent = Items[$"Other World {ItemFlags.Advancement}"].transform;
            Right.transform.parent = Items[$"Other World {ItemFlags.Advancement}"].transform;
            Back.transform.parent = Items[$"Other World {ItemFlags.Advancement}"].transform;
            Front.transform.parent = Items[$"Other World {ItemFlags.Advancement}"].transform;
            Cube.transform.parent = Items[$"Other World {ItemFlags.Advancement}"].transform;
            Items[$"Other World {ItemFlags.Advancement}"].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            Items[$"Other World {ItemFlags.Advancement}"].SetActive(false);

            Items[$"Other World {ItemFlags.None}"] = GameObject.Instantiate(Items[$"Other World {ItemFlags.Advancement}"]);
            Items[$"Other World {ItemFlags.None}"].transform.GetChild(6).gameObject.GetComponent<MeshRenderer>().materials = GameObject.Find("Group/Jelly Cube/Jelly Cube").GetComponent<CreatureMaterialManager>().originalMaterials;
            Items[$"Other World {ItemFlags.None}"].transform.GetChild(6).gameObject.GetComponent<MeshRenderer>().material.color = new UnityEngine.Color(0, 0.5f, 0, 1);

            Items[$"Other World {ItemFlags.NeverExclude}"] = GameObject.Instantiate(Items[$"Other World {ItemFlags.Advancement}"]);
            Items[$"Other World {ItemFlags.NeverExclude}"].transform.GetChild(6).gameObject.GetComponent<MeshRenderer>().materials = GameObject.Find("Group/Jelly Cube/Jelly Cube").GetComponent<CreatureMaterialManager>().originalMaterials;
            Items[$"Other World {ItemFlags.NeverExclude}"].transform.GetChild(6).gameObject.GetComponent<MeshRenderer>().material.color = new UnityEngine.Color(0, 0.15f, 1, 1);

            Items[$"Other World {ItemFlags.Trap}"] = GameObject.Instantiate(Items[$"Other World {ItemFlags.Advancement}"]);

            GameObject.DontDestroyOnLoad(Items[$"Other World {ItemFlags.None}"]);
            GameObject.DontDestroyOnLoad(Items[$"Other World {ItemFlags.Advancement}"]);
            GameObject.DontDestroyOnLoad(Items[$"Other World {ItemFlags.NeverExclude}"]);
            GameObject.DontDestroyOnLoad(Items[$"Other World {ItemFlags.Trap}"]);

            GameObject.Destroy(Items[$"Other World {ItemFlags.None}"].GetComponent<SpriteRenderer>());
            GameObject.Destroy(Items[$"Other World {ItemFlags.Advancement}"].GetComponent<SpriteRenderer>());
            GameObject.Destroy(Items[$"Other World {ItemFlags.NeverExclude}"].GetComponent<SpriteRenderer>());
            GameObject.Destroy(Items[$"Other World {ItemFlags.Trap}"].GetComponent<SpriteRenderer>());
        }

        public static void InitializeExtras() {
            InitializeChestType("Fairy");
            InitializeChestType("GoldenTrophy");
            InitializeChestType("Normal");

            List<PagePickup> PagePickups = Resources.FindObjectsOfTypeAll<PagePickup>().ToList();
            if (PagePickups.Count > 0) {
                PagePickup = GameObject.Instantiate(PagePickups[0].gameObject.transform.GetChild(2).gameObject);
                PagePickup.SetActive(false);
                GameObject.DontDestroyOnLoad(PagePickup);
            }
        }

        public static void SetupGlowEffect() {
            GlowEffect = GameObject.Instantiate(Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Night Glow").ToList()[0]);
            GameObject.Destroy(GlowEffect.GetComponent<StatefulActive>());
            GlowEffect.SetActive(false);
            GameObject.DontDestroyOnLoad(GlowEffect);
        }

        public static void InitializeSecondSword() {
            GameObject Temp = Resources.FindObjectsOfTypeAll<GameObject>().Where(Obj => Obj.name == "Sword").ToList()[0];
            SecondSword = new GameObject("second sword");
            SecondSword.AddComponent<MeshFilter>().mesh = Temp.GetComponent<MeshFilter>().mesh;
            SecondSword.AddComponent<MeshRenderer>().materials = Temp.GetComponent<MeshRenderer>().materials;
            SecondSword.SetActive(false);
            GameObject.DontDestroyOnLoad(SecondSword);
        }

        public static void InitializeThirdSword() {
            GameObject Temp = GameObject.Find("_NON-BOSSFIGHT ROOT/elderfox_sword graphic");
            ThirdSword = new GameObject("third sword");
            ThirdSword.AddComponent<MeshFilter>().mesh = Temp.GetComponent<MeshFilter>().mesh;
            ThirdSword.AddComponent<MeshRenderer>().materials = Temp.GetComponent<MeshRenderer>().materials;
            ThirdSword.SetActive(false);
            GameObject.DontDestroyOnLoad(ThirdSword);
        }

        public static void InitializeHeroRelics() {
            GameObject ItemRoot = Resources.FindObjectsOfTypeAll<GameObject>().Where(Item => Item.name == "User Rotation Root").ToList()[0];

            Items["Relic - Hero Sword"] = SetupRelicItemPresentation(ItemRoot.transform, 15, "Relic - Hero Sword");
            Items["Relic - Hero Crown"] = SetupRelicItemPresentation(ItemRoot.transform, 13, "Relic - Hero Crown");
            Items["Relic - Hero Pendant HP"] = SetupRelicItemPresentation(ItemRoot.transform, 18, "Relic - Hero Pendant HP");
            Items["Relic - Hero Pendant MP"] = SetupRelicItemPresentation(ItemRoot.transform, 19, "Relic - Hero Pendant MP");
            Items["Relic - Hero Water"] = SetupRelicItemPresentation(ItemRoot.transform, 12, "Relic - Hero Water");
            Items["Relic - Hero Pendant SP"] = SetupRelicItemPresentation(ItemRoot.transform, 14, "Relic - Hero Pendant SP");

            List<ItemPresentationGraphic> newipgs = new List<ItemPresentationGraphic>() { };
            foreach (ItemPresentationGraphic ipg in ItemPresentation.instance.itemGraphics) {
                newipgs.Add(ipg);
            }
            newipgs.Add(Items["Relic - Hero Sword"].GetComponent<ItemPresentationGraphic>());
            newipgs.Add(Items["Relic - Hero Crown"].GetComponent<ItemPresentationGraphic>());
            newipgs.Add(Items["Relic - Hero Pendant HP"].GetComponent<ItemPresentationGraphic>());
            newipgs.Add(Items["Relic - Hero Pendant MP"].GetComponent<ItemPresentationGraphic>());
            newipgs.Add(Items["Relic - Hero Water"].GetComponent<ItemPresentationGraphic>());
            newipgs.Add(Items["Relic - Hero Pendant SP"].GetComponent<ItemPresentationGraphic>());
            ItemPresentation.instance.itemGraphics = newipgs.ToArray();
        }

        private static GameObject SetupRelicItemPresentation(Transform parent, int index, string itemname) {
            GameObject relic = GameObject.Instantiate(parent.GetChild(index).gameObject);
            relic.transform.parent = parent;
            relic.SetActive(false);
            relic.transform.localPosition = Vector3.zero;
            relic.GetComponent<ItemPresentationGraphic>().items = new Item[] { Inventory.GetItemByName(itemname) };

            Material RelicMaterial = FindMaterial("ghost material_offerings");

            if (relic.GetComponent<MeshRenderer>() != null) {
                relic.GetComponent<MeshRenderer>().material = RelicMaterial;
            }

            for (int i = 0; i < relic.transform.childCount; i++) {
                relic.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = RelicMaterial;
            }
            GameObject.DontDestroyOnLoad(relic);
            return relic;
        }

        public static void InitializeChestType(string ChestType) {
            if (!Chests.ContainsKey(ChestType)) {
                List<Chest> SceneChests = new List<Chest>();
                if (ChestType == "Normal") {
                    SceneChests = Resources.FindObjectsOfTypeAll<Chest>().Where(Chest => Chest.gameObject.name != "Chest: Fairy" && !Chest.gameObject.name.Contains("Chest: Golden Trophy")).ToList();
                } else {
                    SceneChests = Resources.FindObjectsOfTypeAll<Chest>().Where(Chest => Chest.gameObject.name.Contains($"Chest: {ChestType}")).ToList();
                }
                if (SceneChests.Count > 0) {
                    Chests[ChestType] = GameObject.Instantiate(SceneChests[0].gameObject.transform.GetChild(1).gameObject);
                    Il2CppReferenceArray<Material> ChestMaterials = Chests[ChestType].GetComponent<SkinnedMeshRenderer>().materials;
                    GameObject.Destroy(Chests[ChestType].GetComponent<SkinnedMeshRenderer>());
                    Chests[ChestType].AddComponent<MeshRenderer>().materials = ChestMaterials;
                    Chests[ChestType].AddComponent<MeshFilter>().mesh = Resources.FindObjectsOfTypeAll<Mesh>().Where(Mesh => Mesh.name == "chest").ToList()[0];
                    Chests[ChestType].AddComponent<FMODUnity.StudioEventEmitter>().EventReference = SceneChests[0].gameObject.GetComponent<FMODUnity.StudioEventEmitter>().EventReference;
                    Chests[ChestType].SetActive(false);
                    GameObject.DontDestroyOnLoad(Chests[ChestType]);
                    if (ChestType == "Fairy") {
                        FairyAnimation = GameObject.Instantiate(SceneChests[0].transform.parent.GetChild(1).gameObject);
                        FairyAnimation.SetActive(false);
                        GameObject.DontDestroyOnLoad(FairyAnimation);
                    }
                }
            }
        }

        public static void SwapItemsInScene() {

            if (IceFlask == null) {
                if (Resources.FindObjectsOfTypeAll<BombFlask>().Where(bomb => bomb.name == "Ice flask").Count() > 0) {
                    IceFlask = Resources.FindObjectsOfTypeAll<BombFlask>().Where(bomb => bomb.name == "Ice flask").ToList()[0].transform.GetChild(0).gameObject;
                    Items["Ice Bomb"] = IceFlask;
                }
            }

            CheckCollectedItemFlags();

            if (SceneLoaderPatches.SceneName == "Shop") {
                SetupShopItems();
            } else {
                if (TunicArchipelago.Settings.ShowItemsEnabled) {
                    foreach (ItemPickup ItemPickup in Resources.FindObjectsOfTypeAll<ItemPickup>()) {
                        if (ItemPickup != null && ItemPickup.itemToGive != null) {
                            string checkId = $"{ItemPickup.itemToGive.name} [{SceneLoaderPatches.SceneName}]";
                            if(ItemLookup.ItemList.ContainsKey(checkId)) {
                                if (ItemPickup.itemToGive.name == "Hexagon Red") {
                                    SetupRedHexagonPlinth();
                                } else if (ItemPickup.itemToGive.name == "Hexagon Blue") {
                                    SetupBlueHexagonPlinth();
                                } else if (ItemPickup.itemToGive.name != "MoneySmall") {
                                    SetupItemPickup(ItemPickup);
                                }
                            }
                        }
                    }

                    foreach (PagePickup PagePickup in Resources.FindObjectsOfTypeAll<PagePickup>()) {
                        string ItemId = $"{PagePickup.pageName} [{SceneLoaderPatches.SceneName}]";
                        if(ItemLookup.ItemList.ContainsKey(ItemId)) {
                            SetupPagePickup(PagePickup);
                        }
                    }

                    foreach (HeroRelicPickup HeroRelicPickup in Resources.FindObjectsOfTypeAll<HeroRelicPickup>()) {
                        SetupHeroRelicPickup(HeroRelicPickup);
                    }
                }
                if (TunicArchipelago.Settings.ChestsMatchContentsEnabled) {
                    foreach (Chest Chest in Resources.FindObjectsOfTypeAll<Chest>()) {
                        ApplyChestTexture(Chest);
                    }
                }

                if (SceneLoaderPatches.SceneName == "Fortress Arena") {
                    SwapSiegeEngineCrown();
                }
            }
            ItemLookup.Items["Fool Trap"].QuantityToGive = 1;
            SwappedThisSceneAlready = true;
        }

        public static void ApplyChestTexture(Chest Chest) {
            if (Chest != null) {

                int Player = Archipelago.instance.GetPlayerSlot();

                string ItemId = Chest.chestID == 0 ? $"{SceneLoaderPatches.SceneName}-{Chest.transform.position.ToString()} [{SceneLoaderPatches.SceneName}]" : $"{Chest.chestID} [{SceneLoaderPatches.SceneName}]";
                if (ItemLookup.ItemList.ContainsKey(ItemId)) {
                    ArchipelagoItem APItem = ItemLookup.ItemList[ItemId];
                    if (Archipelago.instance.GetPlayerGame(APItem.Player) != "Tunic" || !ItemLookup.Items.ContainsKey(APItem.ItemName)) {
                        Chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials = Chests["Normal"].GetComponent<MeshRenderer>().materials;
                        Chest.GetComponent<FMODUnity.StudioEventEmitter>().EventReference = Chests["Normal"].GetComponent<FMODUnity.StudioEventEmitter>().EventReference;
                        Chest.GetComponent<FMODUnity.StudioEventEmitter>().Lookup();
                        ApplyAPChestTexture(Chest, APItem);
                        return;
                    }
                    ItemData Item = ItemLookup.Items[APItem.ItemName];
                    if (Item.Type == ItemTypes.FAIRY) {
                        Chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials = Chests["Fairy"].GetComponent<MeshRenderer>().materials;
                        Chest.GetComponent<FMODUnity.StudioEventEmitter>().EventReference = Chests["Fairy"].GetComponent<FMODUnity.StudioEventEmitter>().EventReference;
                    } else if (Item.Type == ItemTypes.GOLDENTROPHY) {
                        Chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials = Chests["GoldenTrophy"].GetComponent<MeshRenderer>().materials;
                        Chest.GetComponent<FMODUnity.StudioEventEmitter>().EventReference = Chests["GoldenTrophy"].GetComponent<FMODUnity.StudioEventEmitter>().EventReference;
                    } else if (Item.ItemNameForInventory == "Hyperdash") {
                        Chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials = Chests["Hyperdash"].GetComponent<MeshRenderer>().materials;
                        Chest.GetComponent<FMODUnity.StudioEventEmitter>().EventReference = Chests["Hyperdash"].GetComponent<FMODUnity.StudioEventEmitter>().EventReference;
                    } else if (Item.ItemNameForInventory.Contains("Hexagon") && Item.Type != ItemTypes.HEXAGONQUEST) {
                        Material[] Mats = new Material[] { Items[Item.ItemNameForInventory].GetComponent<MeshRenderer>().material, Items[Item.ItemNameForInventory].GetComponent<MeshRenderer>().material };
                        Chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials = Mats;
                        Chest.GetComponent<FMODUnity.StudioEventEmitter>().EventReference = Chests["Normal"].GetComponent<FMODUnity.StudioEventEmitter>().EventReference;
                    } else {
                        Chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials = Chests["Normal"].GetComponent<MeshRenderer>().materials;
                        Chest.GetComponent<FMODUnity.StudioEventEmitter>().EventReference = Chests["Normal"].GetComponent<FMODUnity.StudioEventEmitter>().EventReference;
                    }
                }
            }
        }

        public static void ApplyAPChestTexture(Chest chest, ArchipelagoItem APItem) {

            GameObject ChestTop = new GameObject("sprite");
            ChestTop.transform.parent = chest.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0);
            ChestTop.AddComponent<SpriteRenderer>().sprite = FindSprite("trinkets 1_slot_grey");
            ChestTop.transform.localPosition = new Vector3(0f, 0.1f, 1.2f);
            ChestTop.transform.localEulerAngles = new Vector3(57f, 180f, 0f);
            ChestTop.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            ItemFlags flag = APItem.Classification;
            if (flag == ItemFlags.Trap) {
                flag = new List<ItemFlags>() { ItemFlags.Advancement, ItemFlags.NeverExclude, ItemFlags.None }[new System.Random().Next(3)];
            }

            if (flag == ItemFlags.None) {
                chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials[0].color = new UnityEngine.Color(0f, 0.75f, 0f, 1f);
            } else if(flag == ItemFlags.NeverExclude) {
                chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials[0].color = new UnityEngine.Color(0f, 0.5f, 0.75f, 1f);
            } else if(flag == ItemFlags.Advancement) {
                chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials = new Material[] {
                    ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().material,
                    chest.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials[1]
                };
            }
            
            if(APItem.Classification == ItemFlags.Trap) {
                ChestTop.transform.localEulerAngles = new Vector3(57f, 180f, 180f);
            }
        }

        public static void CheckCollectedItemFlags() {
            string Scene = SceneManager.GetActiveScene().name;
            if (TunicArchipelago.Settings.CollectReflectsInWorld) {
                foreach (PagePickup PagePickup in Resources.FindObjectsOfTypeAll<PagePickup>()) {
                    if (PagePickup != null && PagePickup.pageName != null) {
                        string PageId = $"{PagePickup.pageName} [{Scene}]";
                        if (SaveFile.GetInt($"randomizer {PageId} was collected") == 1) {
                            GameObject.Destroy(PagePickup.gameObject);
                        }
                    }
                }


                foreach (ItemPickup ItemPickup in Resources.FindObjectsOfTypeAll<ItemPickup>()) {
                    if (ItemPickup != null && ItemPickup.itemToGive != null) {
                        string ItemId = $"{ItemPickup.itemToGive.name} [{Scene}]";
                        if (SaveFile.GetInt($"randomizer {ItemId} was collected") == 1) {
                            GameObject.Destroy(ItemPickup.gameObject);
                        }
                    }
                }

                foreach (HeroRelicPickup RelicPickup in Resources.FindObjectsOfTypeAll<HeroRelicPickup>()) {
                    if (RelicPickup != null && RelicPickup.name != null) {
                        string RelicId = $"{RelicPickup.name} [{Scene}]";
                        if (SaveFile.GetInt($"randomizer {RelicId} was collected") == 1) {
                            GameObject.Destroy(RelicPickup.gameObject);
                        }
                    }
                }

                foreach (ShopItem ShopItem in Resources.FindObjectsOfTypeAll<ShopItem>()) {
                    if (ShopItem != null) {
                        string ShopId = $"{ShopItem.name} [{Scene}]";
                        if (SaveFile.GetInt($"randomizer {ShopId} was collected") == 1) {
                            ShopItem.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        public static void SetupItemPickup(ItemPickup ItemPickup) {
            
            if (ItemPickup != null && ItemPickup.itemToGive != null) {
                string ItemId = $"{ItemPickup.itemToGive.name} [{SceneLoaderPatches.SceneName}]";

                int Player = Archipelago.instance.GetPlayerSlot();

                if (ItemLookup.ItemList.ContainsKey(ItemId)) { 
                    ArchipelagoItem Item = ItemLookup.ItemList[ItemId];

                    if (Locations.CheckedLocations[ItemId] || SaveFile.GetInt($"{ItemPatches.SaveFileCollectedKey} {ItemId}") == 1) {
                        return;
                    }

                    for (int i = 0; i < ItemPickup.transform.childCount; i++) {
                        ItemPickup.transform.GetChild(i).gameObject.SetActive(false);
                    }
                    if (ItemPickup.GetComponent<MeshFilter>() != null) {
                        GameObject.Destroy(ItemPickup.GetComponent<MeshFilter>());
                        GameObject.Destroy(ItemPickup.GetComponent<MeshRenderer>());
                    }


                    GameObject NewItem = SetupItemBase(ItemPickup.transform, Item);

                    TransformData TransformData;

                    if (ItemPositions.SpecificItemPlacement.ContainsKey(ItemPickup.itemToGive.name)) {
                        if (ItemPickup.itemToGive.name == "Stundagger" && SceneLoaderPatches.SceneName == "archipelagos_house") {
                            GameObject.Find("lanterndagger/").transform.localRotation = new Quaternion(0, 0, 0, 1);
                        }
                        if (ItemPickup.itemToGive.name == "Key" || ItemPickup.itemToGive.name == "Key (House)") {
                            NewItem.transform.parent.localRotation = SceneLoaderPatches.SceneName == "Overworld Redux" ? new Quaternion(0f, 0f, 0f, 0f) : new Quaternion(0f, 0.7071f, 0f, 0.7071f);
                        }
                        if (Archipelago.instance.GetPlayerGame(Item.Player) != "Tunic" || !ItemLookup.Items.ContainsKey(Item.ItemName)) {
                            TransformData = ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name]["Other World"];
                        } else {
                            ItemData ItemData = ItemLookup.Items[Item.ItemName];
                            if (ItemData.ItemNameForInventory.Contains("Trinket - ") || ItemData.ItemNameForInventory == "Mask") {
                                TransformData = ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name]["Trinket Card"];
                            } else if (ItemData.Type == ItemTypes.PAGE || ItemData.Type == ItemTypes.FAIRY) {
                                TransformData = ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name][Enum.GetName(typeof(ItemTypes), ItemData.Type)];
                            } else if (ItemData.Type == ItemTypes.MONEY || ItemData.Type == ItemTypes.FOOLTRAP) {
                                if (ItemData.QuantityToGive < 30) {
                                    TransformData = ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name]["money small"];
                                } else if (ItemData.QuantityToGive >= 30 && ItemData.QuantityToGive < 100) {
                                    TransformData = ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name]["money medium"];
                                } else {
                                    TransformData = ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name]["money large"];
                                }
                            } else if (ItemData.Type == ItemTypes.SWORDUPGRADE && (SaveFile.GetInt(SwordProgressionEnabled) == 1) && Item.Player == Archipelago.instance.GetPlayerSlot()) {
                                int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);
                                TransformData = ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name].ContainsKey($"Sword Progression {SwordLevel}") ? ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name][$"Sword Progression {SwordLevel}"] : ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name][ItemData.ItemNameForInventory];
                            } else {
                                TransformData = ItemPositions.SpecificItemPlacement[ItemPickup.itemToGive.name][ItemData.ItemNameForInventory];
                            }
                        }

                    } else {
                        TransformData = new TransformData(Vector3.one, Quaternion.identity, Vector3.one);
                    }

                    if (ItemPickup.itemToGive.name == "Sword") {
                        ItemPickup.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
                    }
                    NewItem.transform.localPosition = TransformData.pos;
                    NewItem.transform.localRotation = TransformData.rot;
                    NewItem.transform.localScale = TransformData.scale;
                    NewItem.transform.parent.gameObject.SetActive(true);

                    NewItem.SetActive(true);
                }
            }
        }

        public static void SetupPagePickup(PagePickup PagePickup) {
            if (PagePickup != null) {
                GameObject Page = PagePickup.gameObject.transform.GetChild(2).GetChild(0).gameObject;
                string ItemId = $"{PagePickup.pageName} [{SceneLoaderPatches.SceneName}]";
                if (ItemLookup.ItemList.ContainsKey(ItemId)) {
                    if (Locations.CheckedLocations[ItemId] || SaveFile.GetInt($"{ItemPatches.SaveFileCollectedKey} {ItemId}") == 1) {
                        GameObject.Destroy(PagePickup.gameObject);
                        return;
                    }
                    int Player = Archipelago.instance.GetPlayerSlot();
                    ArchipelagoItem APItem = ItemLookup.ItemList[ItemId];

                    if (APItem.Player == Player && ItemLookup.Items.ContainsKey(APItem.ItemName) && ItemLookup.Items[APItem.ItemName].Type == ItemTypes.PAGE) {
                        return;
                    }

                    Page.transform.localScale = Vector3.one;
                    GameObject.Destroy(Page.transform.GetChild(1));
                    GameObject.Destroy(Page.GetComponent<MeshFilter>());
                    GameObject.Destroy(Page.GetComponent<MeshRenderer>());

                    for (int i = 1; i < Page.transform.childCount; i++) {
                        Page.transform.GetChild(i).gameObject.SetActive(false);
                    }

                    GameObject NewItem = SetupItemBase(Page.transform, APItem);

                    Page.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    TransformData TransformData;
                    if (Archipelago.instance.GetPlayerGame(APItem.Player) != "Tunic" || !ItemLookup.Items.ContainsKey(APItem.ItemName)) {
                        TransformData = ItemPositions.Techbow["Other World"];
                        PagePickup.transform.GetChild(2).GetComponent<Rotate>().eulerAnglesPerSecond = new Vector3(0f, 45f, 0f);
                    } else {
                        ItemData Item = ItemLookup.Items[APItem.ItemName];

                        if (Item.Type == ItemTypes.TRINKET) {
                            TransformData = ItemPositions.Techbow["Trinket Card"];
                        } else if (Item.Type == ItemTypes.MONEY || Item.Type == ItemTypes.FOOLTRAP) {
                            if (Item.QuantityToGive < 30) {
                                TransformData = ItemPositions.Techbow["money small"];
                            } else if (Item.QuantityToGive >= 30 && Item.QuantityToGive < 100) {
                                TransformData = ItemPositions.Techbow["money medium"];
                            } else {
                                TransformData = ItemPositions.Techbow["money large"];
                            }
                        } else if (Item.Type == ItemTypes.SWORDUPGRADE && (SaveFile.GetInt(SwordProgressionEnabled) == 1) && APItem.Player == Archipelago.instance.GetPlayerSlot()) {
                            int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);
                            TransformData = ItemPositions.Techbow.ContainsKey($"Sword Progression {SwordLevel}") ? ItemPositions.Techbow[$"Sword Progression {SwordLevel}"] : ItemPositions.Techbow[Item.ItemNameForInventory];
                        } else {
                            TransformData = ItemPositions.Techbow.ContainsKey(Item.ItemNameForInventory) ? ItemPositions.Techbow[Item.ItemNameForInventory] : ItemPositions.Techbow[Enum.GetName(typeof(ItemTypes), Item.Type)];
                        }
                    }

                    NewItem.transform.localPosition = TransformData.pos;
                    NewItem.transform.localRotation = TransformData.rot;
                    NewItem.transform.localScale = TransformData.scale;
                    NewItem.SetActive(true);
                    Page.transform.GetChild(1).gameObject.SetActive(false);
                    Page.transform.GetChild(0).gameObject.GetComponent<ParticleSystemRenderer>().material.color = UnityEngine.Color.cyan;
                    PagePickup.optionalPickupPrompt = ScriptableObject.CreateInstance<LanguageLine>();
                    PagePickup.optionalPickupPrompt.text = $"tAk Ituhm?";
                }
            }
        }


        public static GameObject SetupItemBase(Transform Parent, ArchipelagoItem APItem) {
            GameObject NewItem;
            int Player = Archipelago.instance.GetPlayerSlot();
            if (Archipelago.instance.GetPlayerGame(APItem.Player) != "Tunic" || !ItemLookup.Items.ContainsKey(APItem.ItemName)) {
                ItemFlags flag = APItem.Classification;
                if (flag == ItemFlags.Trap) {
                    flag = new List<ItemFlags>() { ItemFlags.Advancement, ItemFlags.NeverExclude, ItemFlags.None}[new System.Random().Next(3)];
                }
                NewItem = GameObject.Instantiate(Items[$"Other World {flag}"], Parent.transform.position, Parent.transform.rotation);
                if (APItem.Classification == ItemFlags.Trap) {
                    for (int i = 2; i < 6; i++) {
                        Vector3 flipped = NewItem.transform.GetChild(i).localEulerAngles;
                        NewItem.transform.GetChild(i).gameObject.transform.localEulerAngles = new Vector3(180, flipped.y, flipped.z);
                    }
                }
            } else {
                ItemData Item = ItemLookup.Items[APItem.ItemName];

                if (Item.Type == ItemTypes.TRINKET) {
                    NewItem = GameObject.Instantiate(Items["Trinket Card"], Parent.transform.position, Parent.transform.rotation);
                    NewItem.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Cards[Item.ItemNameForInventory];
                } else if (Item.Type == ItemTypes.MONEY || Item.Type == ItemTypes.FOOLTRAP) {
                    if (Item.Type == ItemTypes.FOOLTRAP) {
                        Item.QuantityToGive = new System.Random().Next(150);
                    }
                    NewItem = MoneyObject(Parent, Item.QuantityToGive);
                } else if (Item.Type == ItemTypes.PAGE) {
                    NewItem = GameObject.Instantiate(PagePickup, Parent.transform.position, Parent.transform.rotation);
                } else if (Item.Type == ItemTypes.FAIRY) {
                    NewItem = GameObject.Instantiate(Chests["Fairy"], Parent.transform.position, Parent.transform.rotation);
                } else if (Item.Type == ItemTypes.SWORDUPGRADE && (SaveFile.GetInt(SwordProgressionEnabled) == 1) && APItem.Player == Archipelago.instance.GetPlayerSlot()) {
                    NewItem = SwordProgressionObject(Parent);
                } else {
                    NewItem = GameObject.Instantiate(Items[Item.ItemNameForInventory], Parent.transform.position, Parent.transform.rotation);
                }
            }
            NewItem.transform.parent = Parent.transform;
            NewItem.layer = 0;
            for (int i = 0; i < NewItem.transform.childCount; i++) {
                NewItem.transform.GetChild(i).gameObject.layer = 0;
            }
            NewItem.SetActive(true);
            return NewItem;
        }

        private static GameObject MoneyObject(Transform Parent, int Quantity) {
            if (Quantity < 30) {
                return GameObject.Instantiate(Items["money small"], Parent.transform.position, Parent.transform.rotation);
            } else if (Quantity >= 30 && Quantity < 100) {
                return GameObject.Instantiate(Items["money medium"], Parent.transform.position, Parent.transform.rotation);
            } else {
                return GameObject.Instantiate(Items["money large"], Parent.transform.position, Parent.transform.rotation);
            }
        }

        private static GameObject SwordProgressionObject(Transform Parent) {
            int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);
            GameObject NewItem;
            if (SwordLevel == 0) {
                NewItem = GameObject.Instantiate(Items["Stick"], Parent.transform.position, Parent.transform.rotation);
            } else if (SwordLevel == 1) {
                NewItem = GameObject.Instantiate(Items["Sword"], Parent.transform.position, Parent.transform.rotation);
            } else if (SwordLevel == 2) {
                NewItem = GameObject.Instantiate(SecondSword, Parent.transform.position, Parent.transform.rotation);
            } else if (SwordLevel == 3) {
                NewItem = GameObject.Instantiate(ThirdSword, Parent.transform.position, Parent.transform.rotation);
            } else {
                NewItem = GameObject.Instantiate(Items["Sword"], Parent.transform.position, Parent.transform.rotation);
            }
            return NewItem;
        }

        public static void SetupRedHexagonPlinth() {
            GameObject Plinth = GameObject.Find("_Hexagon Plinth Assembly/hexagon plinth/PRISM/questagon");
            string ItemId = "Hexagon Red [Fortress Arena]";
            int Player = Archipelago.instance.GetPlayerSlot();
            if (Plinth != null && ItemLookup.ItemList.ContainsKey(ItemId)) {
                ArchipelagoItem Item = ItemLookup.ItemList[ItemId];
                if (Item.Player == Player && ItemLookup.Items.ContainsKey(Item.ItemName) && ItemLookup.Items[Item.ItemName].ItemNameForInventory == "Hexagon Red") {
                    return;
                }
                if (Plinth.GetComponent<MeshFilter>() != null) {
                    GameObject.Destroy(Plinth.GetComponent<MeshRenderer>());
                    GameObject.Destroy(Plinth.GetComponent<MeshFilter>());
                }

                for (int i = 0; i < Plinth.transform.childCount; i++) {
                    Plinth.transform.GetChild(i).gameObject.SetActive(false);
                }

                GameObject NewItem = SetupItemBase(Plinth.transform, Item);

                TransformData TransformData;
                if (Archipelago.instance.GetPlayerGame(Item.Player) != "Tunic" || !ItemLookup.Items.ContainsKey(Item.ItemName)) {
                    TransformData = ItemPositions.HexagonRed["Other World"];
                } else {
                    ItemData HexagonItem = ItemLookup.Items[Item.ItemName];
                    if (HexagonItem.Type == ItemTypes.TRINKET) {
                        TransformData = ItemPositions.HexagonRed["Trinket Card"];
                    } else if (HexagonItem.Type == ItemTypes.MONEY || HexagonItem.Type == ItemTypes.FOOLTRAP) {
                        if (HexagonItem.QuantityToGive < 30) {
                            TransformData = ItemPositions.HexagonRed["money small"];
                        } else if (HexagonItem.QuantityToGive >= 30 && HexagonItem.QuantityToGive < 100) {
                            TransformData = ItemPositions.HexagonRed["money medium"];
                        } else {
                            TransformData = ItemPositions.HexagonRed["money large"];
                        }
                    } else if (HexagonItem.Type == ItemTypes.SWORDUPGRADE && (SaveFile.GetInt(SwordProgressionEnabled) == 1) && Item.Player == Archipelago.instance.GetPlayerSlot()) {
                        int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);
                        TransformData = ItemPositions.HexagonRed.ContainsKey($"Sword Progression {SwordLevel}") ? ItemPositions.HexagonRed[$"Sword Progression {SwordLevel}"] : ItemPositions.HexagonRed[HexagonItem.ItemNameForInventory];
                    } else {
                        TransformData = ItemPositions.HexagonRed.ContainsKey(HexagonItem.ItemNameForInventory) ? ItemPositions.HexagonRed[HexagonItem.ItemNameForInventory] : ItemPositions.HexagonRed[Enum.GetName(typeof(ItemTypes), HexagonItem.Type)];
                    }
                }

                NewItem.transform.localPosition = TransformData.pos;
                NewItem.transform.localRotation = TransformData.rot;
                NewItem.transform.localScale = TransformData.scale;
                Plinth.transform.localPosition = new Vector3(-1.4f, -8.7f, -1.5f);
                Plinth.transform.localScale = Vector3.one;
                NewItem.SetActive(true);
            }
        }

        public static void SetupBlueHexagonPlinth() {
            GameObject Plinth = GameObject.Find("_Plinth/turn off when taken/questagon");
            string ItemId = "Hexagon Blue [ziggurat2020_3]";
            int Player = Archipelago.instance.GetPlayerSlot();

            if (Plinth != null && ItemLookup.ItemList.ContainsKey(ItemId)) {
                ArchipelagoItem Item = ItemLookup.ItemList[ItemId];
                if (Item.Player == Player && ItemLookup.Items.ContainsKey(Item.ItemName) && ItemLookup.Items[Item.ItemName].ItemNameForInventory == "Hexagon Blue") {
                    return;
                }
                if (Plinth.GetComponent<MeshFilter>() != null) {
                    GameObject.Destroy(Plinth.GetComponent<MeshRenderer>());
                    GameObject.Destroy(Plinth.GetComponent<MeshFilter>());
                }

                for (int i = 0; i < Plinth.transform.childCount; i++) {
                    Plinth.transform.GetChild(i).gameObject.SetActive(false);
                }

                GameObject NewItem = SetupItemBase(Plinth.transform, Item);

                TransformData TransformData;
                if (Archipelago.instance.GetPlayerGame(Item.Player) != "Tunic" || !ItemLookup.Items.ContainsKey(Item.ItemName)) {
                    TransformData = ItemPositions.HexagonRed["Other World"];
                } else {
                    ItemData HexagonItem = ItemLookup.Items[Item.ItemName];
                    if (HexagonItem.Type == ItemTypes.TRINKET) {
                        TransformData = ItemPositions.HexagonRed["Trinket Card"];
                    } else if (HexagonItem.Type == ItemTypes.MONEY || HexagonItem.Type == ItemTypes.FOOLTRAP) {
                        if (HexagonItem.QuantityToGive < 30) {
                            TransformData = ItemPositions.HexagonRed["money small"];
                        } else if (HexagonItem.QuantityToGive >= 30 && HexagonItem.QuantityToGive < 100) {
                            TransformData = ItemPositions.HexagonRed["money medium"];
                        } else {
                            TransformData = ItemPositions.HexagonRed["money large"];
                        }
                    } else if (HexagonItem.Type == ItemTypes.SWORDUPGRADE && (SaveFile.GetInt(SwordProgressionEnabled) == 1) && Item.Player == Archipelago.instance.GetPlayerSlot()) {
                        int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);
                        TransformData = ItemPositions.HexagonRed.ContainsKey($"Sword Progression {SwordLevel}") ? ItemPositions.HexagonRed[$"Sword Progression {SwordLevel}"] : ItemPositions.HexagonRed[HexagonItem.ItemNameForInventory];
                    } else {
                        TransformData = ItemPositions.HexagonRed.ContainsKey(HexagonItem.ItemNameForInventory) ? ItemPositions.HexagonRed[HexagonItem.ItemNameForInventory] : ItemPositions.HexagonRed[Enum.GetName(typeof(ItemTypes), HexagonItem.Type)];
                    }
                }

                NewItem.transform.localPosition = TransformData.pos;
                NewItem.transform.localRotation = TransformData.rot;
                NewItem.transform.localScale = TransformData.scale;
                Plinth.transform.localPosition = new Vector3(-0.1f, 4.7f, -1.6f);
                Plinth.transform.localScale = Vector3.one;
                NewItem.SetActive(true);
            }
        }

        public static void SwapSiegeEngineCrown() {
            GameObject VaultKey = GameObject.Find("Spidertank/Spidertank_skeleton/root/thorax/vault key graphic");

            string ItemId = "Vault Key (Red) [Fortress Arena]";

            if (VaultKey != null && ItemLookup.ItemList.ContainsKey(ItemId)) {
                ArchipelagoItem Item = ItemLookup.ItemList[ItemId];

                int Player = Archipelago.instance.GetPlayerSlot();

                if (Item.Player == Player && ItemLookup.Items.ContainsKey(Item.ItemName) && ItemLookup.Items[Item.ItemName].ItemNameForInventory == "Vault Key (Red)") {
                    return;
                }
                if (VaultKey.GetComponent<MeshFilter>() != null) {
                    GameObject.Destroy(VaultKey.GetComponent<MeshRenderer>());
                    GameObject.Destroy(VaultKey.GetComponent<MeshFilter>());
                }

                for (int i = 0; i < VaultKey.transform.childCount; i++) {
                    VaultKey.transform.GetChild(i).gameObject.SetActive(false);
                }

                GameObject NewItem = SetupItemBase(VaultKey.transform, Item);

                TransformData TransformData;
                if (Archipelago.instance.GetPlayerGame(Item.Player) != "Tunic" || !ItemLookup.Items.ContainsKey(Item.ItemName)) {
                    TransformData = ItemPositions.VaultKeyRed["Other World"];
                } else {
                    ItemData VaultKeyItem = ItemLookup.Items[Item.ItemName];
                    if (VaultKeyItem.Type == ItemTypes.TRINKET) {
                        TransformData = ItemPositions.VaultKeyRed["Trinket Card"];
                    } else if (VaultKeyItem.Type == ItemTypes.MONEY || VaultKeyItem.Type == ItemTypes.FOOLTRAP) {
                        if (VaultKeyItem.QuantityToGive < 30) {
                            TransformData = ItemPositions.VaultKeyRed["money small"];
                        } else if (VaultKeyItem.QuantityToGive >= 30 && VaultKeyItem.QuantityToGive < 100) {
                            TransformData = ItemPositions.VaultKeyRed["money medium"];
                        } else {
                            TransformData = ItemPositions.VaultKeyRed["money large"];
                        }
                    } else if (VaultKeyItem.Type == ItemTypes.SWORDUPGRADE && (SaveFile.GetInt(SwordProgressionEnabled) == 1) && Item.Player == Archipelago.instance.GetPlayerSlot()) {
                        int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);
                        TransformData = ItemPositions.VaultKeyRed.ContainsKey($"Sword Progression {SwordLevel}") ? ItemPositions.VaultKeyRed[$"Sword Progression {SwordLevel}"] : ItemPositions.VaultKeyRed[VaultKeyItem.ItemNameForInventory];
                    } else {
                        TransformData = ItemPositions.VaultKeyRed.ContainsKey(VaultKeyItem.ItemNameForInventory) ? ItemPositions.VaultKeyRed[VaultKeyItem.ItemNameForInventory] : ItemPositions.VaultKeyRed[Enum.GetName(typeof(ItemTypes), VaultKeyItem.Type)];
                    }
                }

                NewItem.transform.localPosition = TransformData.pos;
                NewItem.transform.localRotation = TransformData.rot;
                NewItem.transform.localScale = TransformData.scale;
                NewItem.SetActive(true);
            }

        }

        public static void SetupShopItems() {

            int Player = Archipelago.instance.GetPlayerSlot();

            for (int i = 0; i < ShopItemIDs.Count; i++) {
                if (!Locations.CheckedLocations[ShopItemIDs[i]]) {
                    
                    GameObject ItemHolder = GameObject.Find(ShopGameObjectIDs[i]);
                    GameObject NewItem;

                    if (ItemHolder.name.Contains("Trinket Coin")) {
                        ItemHolder.transform.rotation = new Quaternion(.7071f, 0f, 0f, -.7071f);
                    }
                    // Destroy original coin child meshes if they exist;
                    for (int j = 0; j < ItemHolder.transform.childCount; j++) {
                        GameObject.Destroy(ItemHolder.transform.GetChild(j).gameObject);
                    }
                    for (int j = 1; j < ItemHolder.transform.parent.childCount; j++) {
                        GameObject.Destroy(ItemHolder.transform.parent.GetChild(j).gameObject);
                    }
                    ArchipelagoItem APItem = ItemLookup.ItemList[ShopItemIDs[i]];

                    NewItem = SetupItemBase(ItemHolder.transform, APItem);

                    TransformData TransformData;
                    if (Archipelago.instance.GetPlayerGame(APItem.Player) != "Tunic" || !ItemLookup.Items.ContainsKey(APItem.ItemName)) {
                        TransformData = ItemPositions.Shop["Other World"];
                    } else {
                        ItemData Item = ItemLookup.Items[APItem.ItemName];
                        if (Item.Type == ItemTypes.TRINKET) {
                            TransformData = ItemPositions.Shop["Trinket Card"];
                        } else if (Item.Type == ItemTypes.MONEY || Item.Type == ItemTypes.FOOLTRAP) {
                            if (Item.QuantityToGive < 30) {
                                TransformData = ItemPositions.Shop["money small"];
                            } else if (Item.QuantityToGive >= 30 && Item.QuantityToGive < 100) {
                                TransformData = ItemPositions.Shop["money medium"];
                            } else {
                                TransformData = ItemPositions.Shop["money large"];
                            }
                        } else if (Item.Type == ItemTypes.SWORDUPGRADE && (SaveFile.GetInt(SwordProgressionEnabled) == 1) && APItem.Player == Archipelago.instance.GetPlayerSlot()) {
                            int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);
                            TransformData = ItemPositions.Shop.ContainsKey($"Sword Progression {SwordLevel}") ? ItemPositions.Shop[$"Sword Progression {SwordLevel}"] : ItemPositions.Shop[Item.ItemNameForInventory];
                        } else {
                            TransformData = ItemPositions.Shop.ContainsKey(Item.ItemNameForInventory) ? ItemPositions.Shop[Item.ItemNameForInventory] : ItemPositions.Shop[Enum.GetName(typeof(ItemTypes), Item.Type)];
                        }

                        if (Item.Type == ItemTypes.PAGE) {
                            GameObject.Destroy(NewItem.GetComponent<Rotate>());
                            GameObject.Destroy(NewItem.transform.GetChild(0).GetChild(0).gameObject);
                        }
                    }

                    NewItem.transform.parent = ItemHolder.transform.parent;
                    NewItem.transform.localPosition = TransformData.pos;
                    NewItem.transform.localRotation = TransformData.rot;
                    NewItem.transform.localScale = TransformData.scale;
                    NewItem.SetActive(true);

                    NewItem.layer = 12;
                    NewItem.transform.localPosition = Vector3.zero;
                    for (int j = 0; j < NewItem.transform.childCount; j++) {
                        NewItem.transform.GetChild(j).gameObject.layer = 12;
                    }

                    if (Archipelago.instance.GetPlayerGame(APItem.Player) == "Tunic" && APItem.ItemName.Contains("Ice Bomb")) {
                        NewItem.transform.GetChild(0).gameObject.SetActive(false);
                    }

                    ItemHolder.SetActive(false);
                }
            }
        }

        public static void SetupHeroRelicPickup(HeroRelicPickup HeroRelicPickup) {
            string ItemId = $"{HeroRelicPickup.name} [{SceneLoaderPatches.SceneName}]";
            if (ItemLookup.ItemList.ContainsKey(ItemId)) {

                int Player = Archipelago.instance.GetPlayerSlot();

                for (int i = 0; i < HeroRelicPickup.transform.childCount; i++) {
                    HeroRelicPickup.transform.GetChild(i).gameObject.SetActive(false);
                }
                if (HeroRelicPickup.GetComponent<MeshFilter>() != null) {
                    GameObject.Destroy(HeroRelicPickup.GetComponent<MeshFilter>());
                    GameObject.Destroy(HeroRelicPickup.GetComponent<MeshRenderer>());
                }

                ArchipelagoItem APItem = ItemLookup.ItemList[ItemId];
                GameObject NewItem = SetupItemBase(HeroRelicPickup.transform, APItem);

                if (Archipelago.instance.GetPlayerGame(APItem.Player) != "Tunic" || !ItemLookup.Items.ContainsKey(APItem.ItemName)) {
                    if (NewItem.GetComponent<Rotate>() == null) {
                        NewItem.AddComponent<Rotate>().eulerAnglesPerSecond = new Vector3(0f, 45f, 0f);
                    }
                    NewItem.transform.localRotation = Quaternion.identity;
                    NewItem.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                } else {
                    ItemData Item = ItemLookup.Items[APItem.ItemName];

                    NewItem.transform.localRotation = ItemPositions.ItemPickupRotations.ContainsKey(Item.ItemNameForInventory) ? ItemPositions.ItemPickupRotations[Item.ItemNameForInventory] : Quaternion.Euler(0, 0, 0);
                    NewItem.transform.localPosition = ItemPositions.ItemPickupPositions.ContainsKey(Item.ItemNameForInventory) ? ItemPositions.ItemPickupPositions[Item.ItemNameForInventory] : Vector3.zero;
                    if (Item.Type == ItemTypes.SWORDUPGRADE && SaveFile.GetInt(SwordProgressionEnabled) == 1 && APItem.Player == Archipelago.instance.GetPlayerSlot()) {
                        int SwordLevel = SaveFile.GetInt(SwordProgressionLevel);
                        TransformData TransformData = ItemPositions.Techbow.ContainsKey($"Sword Progression {SwordLevel}") ? ItemPositions.Techbow[$"Sword Progression {SwordLevel}"] : ItemPositions.Techbow[Item.ItemNameForInventory];
                        NewItem.transform.localPosition = TransformData.pos;
                        NewItem.transform.localRotation = TransformData.rot;
                        NewItem.transform.localScale = TransformData.scale;
                    }
                    NewItem.transform.localScale *= 2;
                    if ((Item.Type == ItemTypes.MONEY || Item.Type == ItemTypes.FOOLTRAP) && Item.QuantityToGive >= 100) {
                        NewItem.transform.localScale *= 3;
                    }
                    if (Item.Type == ItemTypes.FAIRY) {
                        NewItem.transform.localScale = Vector3.one;
                    }
                    if (NewItem.GetComponent<Rotate>() == null) {
                        NewItem.AddComponent<Rotate>().eulerAnglesPerSecond = (Item.ItemNameForInventory == "Relic - Hero Water" || Item.ItemNameForInventory == "Upgrade Offering - PotionEfficiency Swig - Ash" || Item.ItemNameForInventory == "Techbow") ? new Vector3(0f, 0f, 25f) : new Vector3(0f, 25f, 0f);
                        if (Item.ItemNameForInventory == "Sword Progression" && (SaveFile.GetInt(SwordProgressionLevel) == 0 || SaveFile.GetInt(SwordProgressionLevel) == 3)) {
                            NewItem.GetComponent<Rotate>().eulerAnglesPerSecond = new Vector3(0f, 0f, 25f);
                        }
                    }
                }

                NewItem.SetActive(true);
            }
        }

        public static void SetupHexagonQuest() {
            try {
                Items["Hexagon Blue"].GetComponent<MeshRenderer>().materials = Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().materials;
                Inventory.GetItemByName("Hexagon Blue").collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                Inventory.GetItemByName("Hexagon Blue").collectionMessage.text = $"    #uh sEl wEkinz\"...\"";
            } catch (Exception e) {
                Logger.LogInfo(e.Message);
            }
        }

        public static void RestoreOriginalHexagons() {
            try {
                Items["Hexagon Red"].GetComponent<MeshRenderer>().materials = RedKeyMaterial.GetComponent<MeshRenderer>().materials;
                Items["Hexagon Green"].GetComponent<MeshRenderer>().materials = GreenKeyMaterial.GetComponent<MeshRenderer>().materials;
                Items["Hexagon Blue"].GetComponent<MeshRenderer>().materials = BlueKeyMaterial.GetComponent<MeshRenderer>().materials;
                Inventory.GetItemByName("Hexagon Blue").collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                Inventory.GetItemByName("Hexagon Blue").collectionMessage.text = $"fownd ahn Itehm!";
            } catch (Exception e) {
                Logger.LogInfo(e.Message);
            }
        }

        public static void SetupOldHouseKeyItemPresentation() {
            if (!OldHouseKeyPresentationAlreadySetup) {
                try {
                    Resources.FindObjectsOfTypeAll<ItemPresentationGraphic>().Where(item => item.gameObject.name == "key twist")
                        .ToList()[0].gameObject.GetComponent<ItemPresentationGraphic>().items = new Item[] { Inventory.GetItemByName("Key") };
                    GameObject housekey = GameObject.Instantiate(Resources.FindObjectsOfTypeAll<ItemPresentationGraphic>().Where(item => item.gameObject.name == "key twist (special)")
                        .ToList()[0].gameObject);
                    housekey.SetActive(false);
                    housekey.transform.parent = Resources.FindObjectsOfTypeAll<ItemPresentationGraphic>().Where(item => item.gameObject.name == "key twist (special)")
                        .ToList()[0].gameObject.transform.parent;
                    housekey.transform.localPosition = new Vector3(-0.071f, -0.123f, 0f);
                    housekey.GetComponent<ItemPresentationGraphic>().items = new Item[] { Inventory.GetItemByName("Key (House)") };
                    GameObject.DontDestroyOnLoad(housekey);
                    List<ItemPresentationGraphic> newipgs = new List<ItemPresentationGraphic>() { };
                    foreach (ItemPresentationGraphic ipg in ItemPresentation.instance.itemGraphics) {
                        newipgs.Add(ipg);
                    }
                    newipgs.Add(housekey.GetComponent<ItemPresentationGraphic>());
                    ItemPresentation.instance.itemGraphics = newipgs.ToArray();
                    OldHouseKeyPresentationAlreadySetup = true;
                } catch (Exception e) { 
                    
                }
            }
        }

        public static void SetupCustomSwordItemPresentations() {
            if (!SwordPresentationsAlreadySetup) {
                try {
                    GameObject SwordPresentation = Resources.FindObjectsOfTypeAll<GameObject>().Where(Item => Item.name == "User Rotation Root").ToList()[0].transform.GetChild(9).gameObject;
                    GameObject LibrarianSword = GameObject.Instantiate(SwordPresentation);
                    LibrarianSword.transform.parent = SwordPresentation.transform.parent;
                    LibrarianSword.GetComponent<MeshFilter>().mesh = SecondSword.GetComponent<MeshFilter>().mesh;
                    LibrarianSword.GetComponent<MeshRenderer>().materials = SecondSword.GetComponent<MeshRenderer>().materials;
                    LibrarianSword.transform.localScale = new Vector3(0.25f, 0.2f, 0.25f);
                    LibrarianSword.transform.localRotation = new Quaternion(-0.2071f, -0.1216f, 0.3247f, -0.9148f);
                    LibrarianSword.transform.localPosition = SwordPresentation.transform.localPosition;
                    LibrarianSword.SetActive(false);
                    GameObject.DontDestroyOnLoad(LibrarianSword);

                    GameObject HeirSword = GameObject.Instantiate(SwordPresentation);
                    HeirSword.transform.parent = SwordPresentation.transform.parent;
                    HeirSword.GetComponent<MeshFilter>().mesh = ThirdSword.GetComponent<MeshFilter>().mesh;
                    HeirSword.GetComponent<MeshRenderer>().materials = ThirdSword.GetComponent<MeshRenderer>().materials;
                    HeirSword.transform.localScale = new Vector3(0.175f, 0.175f, 0.175f);
                    HeirSword.transform.localRotation = new Quaternion(-0.6533f, 0.2706f, -0.2706f, 0.6533f);
                    HeirSword.transform.localPosition = SwordPresentation.transform.localPosition;
                    HeirSword.SetActive(false);
                    GameObject.DontDestroyOnLoad(HeirSword);

                    LibrarianSword.GetComponent<ItemPresentationGraphic>().items = new List<Item>() { Inventory.GetItemByName("Librarian Sword") }.ToArray();
                    HeirSword.GetComponent<ItemPresentationGraphic>().items = new List<Item>() { Inventory.GetItemByName("Heir Sword") }.ToArray();

                    List<ItemPresentationGraphic> newipgs = ItemPresentation.instance.itemGraphics.ToList();
                    newipgs.Add(LibrarianSword.GetComponent<ItemPresentationGraphic>());
                    newipgs.Add(HeirSword.GetComponent<ItemPresentationGraphic>());
                    ItemPresentation.instance.itemGraphics = newipgs.ToArray();

                    SwordPresentationsAlreadySetup = true;
                } catch (Exception e) {
                }
            }
        }


        public static void SetupDathStoneItemPresentation() {
            if (!DathStonePresentationAlreadySetup) {
                try {
                    GameObject KeySpecial = Resources.FindObjectsOfTypeAll<ItemPresentationGraphic>().Where(Item => Item.gameObject.name == "key twist (special)").ToList()[0].gameObject;

                    Inventory.GetItemByName("Key Special").collectionMessage = ScriptableObject.CreateInstance<LanguageLine>();
                    Inventory.GetItemByName("Key Special").collectionMessage.text = $"dah% stOn\"!?\"";

                    if (KeySpecial.GetComponent<MeshFilter>() != null) {
                        GameObject.Destroy(KeySpecial.GetComponent<MeshRenderer>());
                        GameObject.Destroy(KeySpecial.GetComponent<MeshFilter>());
                    }
                    if (KeySpecial.GetComponent<SpriteRenderer>() == null) {
                        KeySpecial.AddComponent<SpriteRenderer>().sprite = Inventory.GetItemByName("Dash Stone").Icon;
                        KeySpecial.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
                        KeySpecial.GetComponent<SpriteRenderer>().material = Resources.FindObjectsOfTypeAll<Material>().Where(mat => mat.name == "UI Add").ToList()[0];
                    }

                    GameObject Torch = new GameObject("torch");
                    Torch.AddComponent<SpriteRenderer>().sprite = FindSprite("Inventory items_torch");
                    Torch.GetComponent<SpriteRenderer>().material = FindMaterial("UI Add");
                    Torch.layer = KeySpecial.layer;
                    Torch.transform.parent = KeySpecial.transform;
                    Torch.transform.localPosition = new Vector3(0.7f, 0.2f, 0f);
                    Torch.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
                    Torch.transform.localEulerAngles = Vector3.zero;

                    GameObject Plus = new GameObject("plus");
                    Plus.AddComponent<SpriteRenderer>().sprite = FindSprite("game gui_plus sign");
                    Plus.GetComponent<SpriteRenderer>().material = FindMaterial("UI Add");
                    Plus.layer = KeySpecial.layer;
                    Plus.transform.parent = KeySpecial.transform;
                    Plus.transform.localPosition = new Vector3(0.55f, 0.2f, 0f);
                    Plus.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                    Plus.transform.localEulerAngles = Vector3.zero;

                    Torch.SetActive(SaveFile.GetInt("randomizer entrance rando enabled") == 0);
                    Plus.SetActive(SaveFile.GetInt("randomizer entrance rando enabled") == 0);

                    KeySpecial.transform.localScale = Vector3.one;
                    KeySpecial.transform.localPosition = Vector3.zero;
                    KeySpecial.transform.localRotation = Quaternion.identity;
                    DathStonePresentationAlreadySetup = true;
                } catch (Exception e) {
                }
            } else {
                try {
                    GameObject KeySpecial = Resources.FindObjectsOfTypeAll<ItemPresentationGraphic>().Where(Item => Item.gameObject.name == "key twist (special)").ToList()[0].gameObject;

                    for (int i = 0; i < KeySpecial.transform.childCount; i++) {
                        KeySpecial.transform.GetChild(i).gameObject.SetActive(SaveFile.GetInt("randomizer entrance rando enabled") == 0);
                    }
                } catch (Exception e) {

                }
            }

        }

        public static void AddNewShopItems() {
            try {
                if (IceFlask == null) {
                    if (Resources.FindObjectsOfTypeAll<BombFlask>().Where(bomb => bomb.name == "Ice flask").Count() > 0) {
                        IceFlask = Resources.FindObjectsOfTypeAll<BombFlask>().Where(bomb => bomb.name == "Ice flask").ToList()[0].transform.GetChild(0).gameObject;
                        Items["Ice Bomb"] = IceFlask;
                    }
                }
                GameObject IceBombs = GameObject.Instantiate(GameObject.Find("Shop/Item Holder/Firebombs/"));
                GameObject Pepper = GameObject.Instantiate(GameObject.Find("Shop/Item Holder/Ivy/"));
                IceBombs.name = "Ice Bombs";
                Pepper.name = "Pepper";
                IceBombs.transform.parent = GameObject.Find("Shop/Item Holder/Firebombs/").transform.parent;
                Pepper.transform.parent = GameObject.Find("Shop/Item Holder/Ivy/").transform.parent;

                IceBombs.GetComponent<ShopItem>().itemToGive = Inventory.GetItemByName("Ice Bomb");
                IceBombs.GetComponent<ShopItem>().price = 200;
                Pepper.GetComponent<ShopItem>().itemToGive = Inventory.GetItemByName("Pepper");
                Pepper.GetComponent<ShopItem>().price = 130;
                
                for (int i = 0; i < 3; i++) {
                    GameObject.Destroy(IceBombs.transform.GetChild(0).GetChild(i).GetChild(0).gameObject);
                    if (IceFlask != null) { 
                        GameObject newIceBomb = GameObject.Instantiate(IceFlask);
                        newIceBomb.transform.position = IceBombs.transform.GetChild(0).GetChild(i).position;
                        newIceBomb.transform.localEulerAngles = IceBombs.transform.GetChild(0).GetChild(i).localEulerAngles;
                        GameObject.Destroy(IceBombs.transform.GetChild(0).GetChild(i).gameObject);
                        newIceBomb.transform.parent = IceBombs.transform.GetChild(0);
                        newIceBomb.transform.GetChild(0).gameObject.SetActive(false);
                        newIceBomb.transform.localScale = Vector3.one;
                    } else {
                        IceBombs.transform.GetChild(0).GetChild(i).GetComponent<MeshFilter>().mesh = Items["Ice Bomb"].GetComponent<MeshFilter>().mesh;
                        IceBombs.transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>().materials = Items["Ice Bomb"].GetComponent<MeshRenderer>().materials;
                        IceBombs.transform.GetChild(0).GetChild(i).localScale = Vector3.one;
                    }
                }

                Pepper.transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh = Items["Pepper"].GetComponent<MeshFilter>().mesh;
                Pepper.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().materials = Items["Pepper"].GetComponent<MeshRenderer>().materials;
                Pepper.transform.GetChild(0).GetChild(0).localScale = new Vector3(1.2f, 1.2f, 1.2f);

                List<ShopItem> items = ShopManager.cachedShopItems != null ? ShopManager.cachedShopItems.ToList() : new List<ShopItem>();
                items.Add(IceBombs.GetComponent<ShopItem>());
                items.Add(Pepper.GetComponent<ShopItem>());
                ShopManager.cachedShopItems = items.ToArray();
            } catch (Exception e) {
                Logger.LogError("Failed to create permanent ice bomb and/or pepper items in the shop.");
            }
        }

        public static void ShopManager_entrySequence_MoveNext_PostfixPatch(ShopManager._entrySequence_d__14 __instance, ref bool __result) {
            if (SceneManager.GetActiveScene().name == "Shop" && __instance._f_5__2 > 0.5f && __instance._f_5__2 < 0.6f) {
                for (int i = 0; i < 3; i++) {
                    if (IceFlask != null) {
                        try {
                            __instance.__4__this.transform.GetChild(0).GetChild(10).GetChild(0).GetChild(i).GetChild(0).gameObject.SetActive(true);
                            foreach (ShopItem shopItem in ShopManager.cachedShopItems) {
                                if (ShopItemIDs.Contains($"{shopItem.name} [Shop]") && !Locations.CheckedLocations[$"{shopItem.name} [Shop]"]
                                    && ItemLookup.ItemList[$"{shopItem.name} [Shop]"].ItemName.Contains("Ice Bomb") && Archipelago.instance.GetPlayerGame(ItemLookup.ItemList[$"{shopItem.name} [Shop]"].Player) == "Tunic") {
                                    shopItem.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(true);
                                }
                            }
                        } catch (Exception e) {

                        }
                    }
                }
            }
        }

        public static void LoadTexture() {

            Texture2D GoldHexTexture = new Texture2D(160, 160, TextureFormat.DXT1, false);
            ImageConversion.LoadImage(GoldHexTexture, Convert.FromBase64String(ImageData.GoldHex));
            Texture2D SecondSwordTexture = new Texture2D(160, 160, TextureFormat.DXT1, false);
            ImageConversion.LoadImage(SecondSwordTexture, Convert.FromBase64String(ImageData.SecondSword));
            Texture2D ThirdSwordTexture = new Texture2D(160, 160, TextureFormat.DXT1, false);
            ImageConversion.LoadImage(ThirdSwordTexture, Convert.FromBase64String(ImageData.ThirdSword));
            Texture2D TuncTitleTexture = new Texture2D(1400, 742, TextureFormat.DXT1, false);
            ImageConversion.LoadImage(TuncTitleTexture, Convert.FromBase64String(ImageData.TuncTitle));
            Texture2D TuncTexture = new Texture2D(148, 148, TextureFormat.DXT1, false);
            ImageConversion.LoadImage(TuncTexture, Convert.FromBase64String(ImageData.Tunc));

            Material ImageMaterial = FindMaterial("UI Add");
            HexagonGoldImage = new GameObject("hexagon gold");
            HexagonGoldImage.AddComponent<RawImage>().texture = GoldHexTexture;
            HexagonGoldImage.GetComponent<RawImage>().material = ImageMaterial;
            GameObject.DontDestroyOnLoad(HexagonGoldImage);

            SecondSwordImage = new GameObject("second sword");
            SecondSwordImage.AddComponent<RawImage>().texture = SecondSwordTexture;
            SecondSwordImage.GetComponent<RawImage>().material = ImageMaterial;
            GameObject.DontDestroyOnLoad(SecondSwordImage);

            ThirdSwordImage = new GameObject("third sword");
            ThirdSwordImage.AddComponent<RawImage>().texture = ThirdSwordTexture;
            ThirdSwordImage.GetComponent<RawImage>().material = ImageMaterial;
            GameObject.DontDestroyOnLoad(ThirdSwordImage);

            TuncTitleImage = new GameObject("tunc title logo");
            TuncTitleImage.AddComponent<RawImage>().texture = TuncTitleTexture;
            TuncTitleImage.GetComponent<RawImage>().material = ImageMaterial;
            GameObject.DontDestroyOnLoad(TuncTitleImage);

            TuncImage = new GameObject("TUNC");
            TuncImage.AddComponent<RawImage>().texture = TuncTexture;
            //TuncImage.GetComponent<RawImage>().material = ImageMaterial;
            GameObject.DontDestroyOnLoad(TuncImage);


            CustomItemImages.Add("Mr Mayor", CreateImageObject(ImageData.MrMayor, ImageMaterial));
            CustomItemImages.Add("Secret Legend", CreateImageObject(ImageData.SecretLegend, ImageMaterial));
            CustomItemImages.Add("Sacred Geometry", CreateImageObject(ImageData.SacredGeometry, ImageMaterial));
            CustomItemImages.Add("Vintage", CreateImageObject(ImageData.Vintage, ImageMaterial));
            CustomItemImages.Add("Just Some Pals", CreateImageObject(ImageData.JustSomePals, ImageMaterial));
            CustomItemImages.Add("Regal Weasel", CreateImageObject(ImageData.RegalWeasel, ImageMaterial));
            CustomItemImages.Add("Spring Falls", CreateImageObject(ImageData.SpringFalls, ImageMaterial));
            CustomItemImages.Add("Power Up", CreateImageObject(ImageData.PowerUp, ImageMaterial));
            CustomItemImages.Add("Back To Work", CreateImageObject(ImageData.BackToWork, ImageMaterial));
            CustomItemImages.Add("Phonomath", CreateImageObject(ImageData.Phonomath, ImageMaterial));
            CustomItemImages.Add("Dusty", CreateImageObject(ImageData.Dusty, ImageMaterial));
            CustomItemImages.Add("Forever Friend", CreateImageObject(ImageData.ForeverFriend, ImageMaterial));
            CustomItemImages.Add("Red Questagon", CreateImageObject(ImageData.RedQuestagon, ImageMaterial));
            CustomItemImages.Add("Green Questagon", CreateImageObject(ImageData.GreenQuestagon, ImageMaterial));
            CustomItemImages.Add("Blue Questagon", CreateImageObject(ImageData.BlueQuestagon, ImageMaterial));
            CustomItemImages.Add("Gold Questagon", CreateImageObject(ImageData.GoldHex, ImageMaterial));
            CustomItemImages.Add("Hero Relic - ATT", CreateImageObject(ImageData.HeroRelicATT, ImageMaterial));
            CustomItemImages.Add("Hero Relic - DEF", CreateImageObject(ImageData.HeroRelicDef, ImageMaterial));
            CustomItemImages.Add("Hero Relic - POTION", CreateImageObject(ImageData.HeroRelicPotion, ImageMaterial));
            CustomItemImages.Add("Hero Relic - HP", CreateImageObject(ImageData.HeroRelicHP, ImageMaterial));
            CustomItemImages.Add("Hero Relic - SP", CreateImageObject(ImageData.HeroRelicSP, ImageMaterial));
            CustomItemImages.Add("Hero Relic - MP", CreateImageObject(ImageData.HeroRelicMP, ImageMaterial));
            CustomItemImages.Add("Fool Trap", CreateImageObject(ImageData.TinyFox, ImageMaterial));
            CustomItemImages.Add("Archipelago Item", CreateImageObject(ImageData.ArchipelagoItem, ImageMaterial, 128, 128));
        }

        public static GameObject CreateImageObject(string ImageData, Material imgMaterial, int Width = 160, int Height = 160) {

            Texture2D Texture = new Texture2D(Width, Height, TextureFormat.DXT1, false);
            ImageConversion.LoadImage(Texture, Convert.FromBase64String(ImageData));

            GameObject obj = new GameObject();
            obj.AddComponent<RawImage>().texture = Texture;
            obj.GetComponent<RawImage>().material = imgMaterial;
            GameObject.DontDestroyOnLoad(obj);
            return obj;
        }

        public static Material FindMaterial(string MaterialName) {
            List<Material> Material = Resources.FindObjectsOfTypeAll<Material>().Where(Mat => Mat.name == MaterialName).ToList();
            if (Material != null && Material.Count > 0) {
                return Material[0];
            } else {
                return null;
            }
        }

        public static Sprite FindSprite(string SpriteName) {
            List<Sprite> Sprites = Resources.FindObjectsOfTypeAll<Sprite>().Where(Sprite => Sprite.name == SpriteName).ToList();
            if (Sprites != null && Sprites.Count > 0) {
                return Sprites[0];
            } else {
                return null;
            }
        }
    }
}
