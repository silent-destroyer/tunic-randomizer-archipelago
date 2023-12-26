using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TunicArchipelago {
    public class ItemPresentationPatches {
        private static ManualLogSource Logger = TunicArchipelago.Logger;

        public static bool DathStonePresentationAlreadyCreated = false;

        public static bool ItemPresentation_presentItem_PrefixPatch(ItemPresentation __instance) {
            if (TunicArchipelago.Settings.SkipItemAnimations) {
                return false;
            }
            return true;
        }

        public static void SetupHexagonQuestItemPresentation() {
            try {
                GameObject HexagonRoot = ModelSwaps.Items["Hexagon Blue"].transform.parent.gameObject;
                GameObject GoldHexagon = GameObject.Instantiate(HexagonRoot);
                GoldHexagon.transform.parent = HexagonRoot.transform.parent;
                GoldHexagon.transform.GetChild(0).GetComponent<MeshRenderer>().material = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().material;
                GoldHexagon.transform.GetChild(0).GetComponent<MeshRenderer>().materials[0] = ModelSwaps.Items["GoldenTrophy_2"].GetComponent<MeshRenderer>().material; GoldHexagon.SetActive(false);
                GoldHexagon.transform.localPosition = Vector3.zero;
                GameObject.DontDestroyOnLoad(GoldHexagon);

                GoldHexagon.GetComponent<ItemPresentationGraphic>().items = new List<Item>() { Inventory.GetItemByName("Hexagon Gold") }.ToArray();

                List<ItemPresentationGraphic> newipgs = ItemPresentation.instance.itemGraphics.ToList();
                newipgs.Add(GoldHexagon.GetComponent<ItemPresentationGraphic>());
                ItemPresentation.instance.itemGraphics = newipgs.ToArray();

                ModelSwaps.Items["Hexagon Gold"] = GoldHexagon.transform.GetChild(0).gameObject;
            } catch (Exception e) {
                Logger.LogError("Setup Hexagon Quest: " + e.Message);
            }
        }

        public static void SetupOldHouseKeyItemPresentation() {
            try {
                Resources.FindObjectsOfTypeAll<ItemPresentationGraphic>().Where(item => item.gameObject.name == "key twist")
                    .ToList()[0].gameObject.GetComponent<ItemPresentationGraphic>().items = new Item[] { Inventory.GetItemByName("Key") };
                GameObject housekey = GameObject.Instantiate(Resources.FindObjectsOfTypeAll<ItemPresentationGraphic>().Where(item => item.gameObject.name == "key twist (special)")
                    .ToList()[0].gameObject);
                housekey.name = "old house key";
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
            } catch (Exception e) {
                Logger.LogError("Setup Old House Key Item Presentation: " + e.Message);
            }
        }

        public static void SetupCustomSwordItemPresentations() {
            try {
                GameObject SwordPresentation = Resources.FindObjectsOfTypeAll<GameObject>().Where(Item => Item.name == "User Rotation Root").ToList()[0].transform.GetChild(9).gameObject;
                GameObject LibrarianSword = GameObject.Instantiate(SwordPresentation);
                LibrarianSword.transform.parent = SwordPresentation.transform.parent;
                LibrarianSword.GetComponent<MeshFilter>().mesh = ModelSwaps.SecondSword.GetComponent<MeshFilter>().mesh;
                LibrarianSword.GetComponent<MeshRenderer>().materials = ModelSwaps.SecondSword.GetComponent<MeshRenderer>().materials;
                LibrarianSword.transform.localScale = new Vector3(0.25f, 0.2f, 0.25f);
                LibrarianSword.transform.localRotation = new Quaternion(-0.2071f, -0.1216f, 0.3247f, -0.9148f);
                LibrarianSword.transform.localPosition = SwordPresentation.transform.localPosition;
                LibrarianSword.SetActive(false);
                GameObject.DontDestroyOnLoad(LibrarianSword);

                GameObject HeirSword = GameObject.Instantiate(SwordPresentation);
                HeirSword.transform.parent = SwordPresentation.transform.parent;
                HeirSword.GetComponent<MeshFilter>().mesh = ModelSwaps.ThirdSword.GetComponent<MeshFilter>().mesh;
                HeirSword.GetComponent<MeshRenderer>().materials = ModelSwaps.ThirdSword.GetComponent<MeshRenderer>().materials;
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

            } catch (Exception e) {
                Logger.LogError("Setup Custom Sword Item Presentation: " + e.Message);
            }
        }


        public static void SetupDathStoneItemPresentation() {
            try {
                GameObject KeySpecial = Resources.FindObjectsOfTypeAll<ItemPresentationGraphic>().Where(Item => Item.gameObject.name == "key twist (special)").ToList()[0].gameObject;
                GameObject DathStone = GameObject.Instantiate(KeySpecial);

                DathStone.transform.parent = KeySpecial.transform.parent;
                DathStone.name = "dath stone";
                DathStone.layer = KeySpecial.layer;

                if (DathStone.GetComponent<MeshFilter>() != null) {
                    GameObject.Destroy(DathStone.GetComponent<MeshRenderer>());
                    GameObject.Destroy(DathStone.GetComponent<MeshFilter>());
                }
                GameObject DathSprite = new GameObject("dath stone sprite");
                DathSprite.AddComponent<SpriteRenderer>().sprite = Inventory.GetItemByName("Dash Stone").icon;
                DathSprite.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
                DathSprite.GetComponent<SpriteRenderer>().material = Resources.FindObjectsOfTypeAll<Material>().Where(mat => mat.name == "UI Add").ToList()[0];
                DathSprite.transform.parent = DathStone.transform;
                DathSprite.layer = KeySpecial.layer;
                DathSprite.transform.localPosition = Vector3.zero;
                DathSprite.transform.localScale = Vector3.one;
                DathSprite.transform.localEulerAngles = Vector3.zero;

                GameObject Torch = new GameObject("torch");
                Torch.AddComponent<SpriteRenderer>().sprite = ModelSwaps.FindSprite("Randomizer items_Torch redux");
                Torch.GetComponent<SpriteRenderer>().material = ModelSwaps.FindMaterial("UI Add");
                Torch.layer = KeySpecial.layer;
                Torch.transform.parent = DathStone.transform;
                Torch.transform.localPosition = new Vector3(0.7f, 0.2f, 0f);
                Torch.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
                Torch.transform.localEulerAngles = Vector3.zero;

                GameObject Plus = new GameObject("plus");
                Plus.AddComponent<SpriteRenderer>().sprite = ModelSwaps.FindSprite("game gui_plus sign");
                Plus.GetComponent<SpriteRenderer>().material = ModelSwaps.FindMaterial("UI Add");
                Plus.layer = KeySpecial.layer;
                Plus.transform.parent = DathStone.transform;
                Plus.transform.localPosition = new Vector3(0.55f, 0.2f, 0f);
                Plus.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                Plus.transform.localEulerAngles = Vector3.zero;

                Torch.SetActive(SaveFile.GetInt("randomizer entrance rando enabled") == 0);
                Plus.SetActive(SaveFile.GetInt("randomizer entrance rando enabled") == 0);
                DathStone.GetComponent<ItemPresentationGraphic>().items = new List<Item>() { Inventory.GetItemByName("Dath Stone") }.ToArray();

                List<ItemPresentationGraphic> newipgs = ItemPresentation.instance.itemGraphics.ToList();
                newipgs.Add(DathStone.GetComponent<ItemPresentationGraphic>());
                ItemPresentation.instance.itemGraphics = newipgs.ToArray();

                DathStone.transform.localScale = Vector3.one;
                DathStone.transform.localPosition = Vector3.zero;
                DathStone.transform.localRotation = Quaternion.identity;

                GameObject.DontDestroyOnLoad(DathStone);
                DathStone.SetActive(false);
            } catch (Exception e) {
                Logger.LogError("Setup dath stone presentation error: " + e.Message);
            }
        }

        public static void SwitchDathStonePresentation() {
            try {
                GameObject DathStone = Resources.FindObjectsOfTypeAll<ItemPresentationGraphic>().Where(Item => Item.gameObject.name == "dath stone").ToList()[0].gameObject;

                for (int i = 1; i < DathStone.transform.childCount; i++) {
                    DathStone.transform.GetChild(i).gameObject.SetActive(SaveFile.GetInt("randomizer entrance rando enabled") == 0);
                }
            } catch (Exception e) {
                Logger.LogError("Switch dath stone presentation error: " + e.Message);
            }
        }

    }
}
