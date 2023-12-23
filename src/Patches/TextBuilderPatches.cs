using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace TunicArchipelago {
    public class TextBuilderPatches {

        public static string CustomImageToDisplay = "";

        public static Dictionary<string, int> CustomSpriteIndices = new Dictionary<string, int>() {
            { "[stick]", 110 },
            { "[realsword]", 111 },
            { "[wand]", 112 },
            { "[dagger]", 113 },
            { "[orb]", 114 },
            { "[shield]", 115 },
            { "[gun]", 116 },
            { "[hourglass]", 117 },
            { "[lantern]", 118 },
            { "[laurels]", 119 },
            { "[coin]", 120 },
            { "[trinket]", 121 },
            { "[square]", 122 },
            { "[fairy]", 123 },
            { "[mayor]", 124 },
            { "[book]", 125 },
            { "[att]", 126 },
            { "[def]", 127 },
            { "[potion]", 128 },
            { "[hp]", 129 },
            { "[sp]", 130 },
            { "[mp]", 131 },
            { "[yellowkey]", 132 },
            { "[housekey]", 133 },
            { "[vaultkey]", 134 },
            { "[firecracker]", 135 },
            { "[firebomb]", 136 },
            { "[icebomb]", 137 },
            { "[hpberry]", 138 },
            { "[mpberry]", 139 },
            { "[pepper]", 140 },
            { "[ivy]", 141 },
            { "[lure]", 142 },
            { "[effigy]", 143 },
            { "[flask]", 144 },
            { "[shard]", 145 },
            { "[dath]", 146 },
            { "[torch]", 147 },
            { "[triangle]", 148 },
            { "[realmoney]", 149 },
            { "[anklet]", 150 },
            { "[perfume]", 151 },
            { "[mufflingbell]", 152 },
            { "[rtsr]", 153 },
            { "[aurasgem]", 154 },
            { "[invertedash]", 155 },
            { "[bonecard]", 156 },
            { "[luckycup]", 157 },
            { "[glasscannon]", 158 },
            { "[daggerstrap]", 159 },
            { "[louderecho]", 160 },
            { "[magicecho]", 161 },
            { "[bracer]", 162 },
            { "[tincture]", 163 },
            { "[btsr]", 164 },
            { "[scavengermask]", 165 },
            { "[customimage]", 166 },
        };

        public static Dictionary<string, string> CustomSpriteIcons = new Dictionary<string, string>() {
            { "[stick]", "Inventory items_stick" },
            { "[realsword]", "Inventory items_sword" },
            { "[wand]", "Inventory items_techbow" },
            { "[dagger]", "Inventory items_stundagger" },
            { "[orb]", "Inventory items_forcewand" },
            { "[shield]", "Inventory items_shield" },
            { "[gun]", "Inventory items_shotgun" },
            { "[hourglass]", "Inventory items_hourglass" },
            { "[lantern]", "Inventory items_lantern" },
            { "[laurels]", "Inventory items_cape" },
            { "[coin]", "Inventory items_coin question mark" },
            { "[trinket]", "Inventory items_trinketcard" },
            { "[square]", "Inventory items_trinketslot" },
            { "[fairy]", "Inventory items_fairy" },
            { "[mayor]", "Inventory items_trophy" },
            { "[book]", "Inventory items_book" },
            { "[att]", "Inventory items_offering_tooth" },
            { "[def]", "Inventory items_offering_effigy" },
            { "[potion]", "Inventory items_offering_ash" },
            { "[hp]", "Inventory items_offering_flower" },
            { "[sp]", "Inventory items_offering_feather" },
            { "[mp]", "Inventory items_offering_orb" },
            { "[yellowkey]", "Inventory items_key" },
            { "[housekey]", "Inventory items 3_keySpecial" },
            { "[vaultkey]", "Inventory items_vault key" },
            { "[firecracker]", "Inventory items_firecracker" },
            { "[firebomb]", "Inventory items_firebomb" },
            { "[icebomb]", "Inventory items_icebomb" },
            { "[hpberry]", "Inventory items_berry" },
            { "[mpberry]", "Inventory items_berry_blue" },
            { "[pepper]", "Inventory items_pepper" },
            { "[ivy]", "Inventory items_ivy" },
            { "[lure]", "Inventory items_bait" },
            { "[effigy]", "Inventory items_piggybank" },
            { "[flask]", "Inventory items_potion" },
            { "[shard]", "Inventory items 3_shard" },
            { "[dath]", "Inventory items_dash stone" },
            { "[torch]", "Inventory items_torch" },
            { "[triangle]", "Inventory items_money triangle" },
            { "[realmoney]", "game gui_money_icon" },
            { "[anklet]", "trinkets 1_anklet" },
            { "[perfume]", "trinkets 1_laurel" },
            { "[mufflingbell]", "trinkets 1_bell" },
            { "[rtsr]", "trinkets 1_RTSR" },
            { "[aurasgem]", "trinkets 2_aurasgem" },
            { "[invertedash]", "trinkets 1_MP Flasks" },
            { "[bonecard]", "trinkets 2_bone" },
            { "[luckycup]", "trinkets 1_heart" },
            { "[glasscannon]", "trinkets 1_glasadagger" },
            { "[daggerstrap]", "trinkets 1_dagger" },
            { "[louderecho]", "trinkets 1_ghost" },
            { "[magicecho]", "trinkets 1_bloodstain MP" },
            { "[bracer]", "trinkets 1_shield" },
            { "[tincture]", "trinkets 1_glass" },
            { "[btsr]", "trinkets 1_BTSR" },
            { "[scavengermask]", "trinkets 1_mask" },
            { "[customimage]", "Inventory items_sword2" },
        };

        public static Dictionary<string, string> SpriteNameToAbbreviation = new Dictionary<string, string>();

        public static Dictionary<string, string> ItemNameToAbbreviation = new Dictionary<string, string>() {
            // Consumables
            { "Firecracker", "[firecracker]" },
            { "Firecracker x2", "[firecracker]" },
            { "Firecracker x3", "[firecracker]" },
            { "Firecracker x4", "[firecracker]" },
            { "Firecracker x5", "[firecracker]" },
            { "Firecracker x6", "[firecracker]" },
            { "Fire Bomb", "[firebomb]" },
            { "Fire Bomb x2", "[firebomb]" },
            { "Fire Bomb x3", "[firebomb]" },
            { "Ice Bomb", "[icebomb]" },
            { "Ice Bomb x2", "[icebomb]" },
            { "Ice Bomb x3", "[icebomb]" },
            { "Ice Bomb x5", "[icebomb]" },
            { "Lure", "[lure]" },
            { "Lure x2", "[lure]" },
            { "Pepper", "[pepper]" },
            { "Pepper x2", "[pepper]" },
            { "Ivy", "[ivy]" },
            { "Ivy x3", "[ivy]" },
            { "Effigy", "[effigy]" },
            { "HP Berry", "[hpberry]" },
            { "HP Berry x2", "[hpberry]" },
            { "HP Berry x3", "[hpberry]" },
            { "MP Berry", "[mpberry]" },
            { "MP Berry x2", "[mpberry]" },
            { "MP Berry x3", "[mpberry]" },
            // Fairy
            { "Fairy", "[fairy]" },
            // Regular Items
            { "Stick", "[stick]" },
            { "Sword", "[realsword]" },
            { "Sword Upgrade", "[realsword]" },
            { "Magic Wand", "[wand]" },
            { "Magic Dagger", "[dagger]" },
            { "Magic Orb", "[orb]" },
            { "Hero's Laurels", "[laurels]" },
            { "Lantern", "[lantern]" },
            { "Gun", "[gun]" },
            { "Shield", "[shield]" },
            { "Dath Stone", "[dath]" },
            { "Hourglass", "[hourglass]" },
            { "Old House Key", "[housekey]" },
            { "Key", "[yellowkey]" },
            { "Fortress Vault Key", "[vaultkey]" },
            { "Flask Shard", "[shard]" },
            { "Potion Flask", "[flask]" },
            { "Golden Coin", "[coin]" },
            { "Card Slot", "[square]" },
            { "Red Questagon", "[customimage]" },
            { "Green Questagon", "[customimage]" },
            { "Blue Questagon", "[customimage]" },
            { "Gold Questagon", "[customimage]" },
            // Upgrades and Relics
            { "ATT Offering", "[att]" },
            { "DEF Offering", "[def]" },
            { "Potion Offering", "[potion]" },
            { "HP Offering", "[hp]" },
            { "MP Offering", "[mp]" },
            { "SP Offering", "[sp]" },
            { "Hero Relic - ATT", "[customimage]" },
            { "Hero Relic - DEF", "[customimage]" },
            { "Hero Relic - POTION", "[customimage]" },
            { "Hero Relic - HP", "[customimage]" },
            { "Hero Relic - SP", "[customimage]" },
            { "Hero Relic - MP", "[customimage]" },
            // Trinket Cards
            { "Orange Peril Ring", "[rtsr]" },
            { "Tincture", "[tincture]" },
            { "Scavenger Mask", "[scavengermask]" },
            { "Cyan Peril Ring", "[btsr]" },
            { "Bracer", "[bracer]" },
            { "Dagger Strap", "[daggerstrap]" },
            { "Inverted Ash", "[invertedash]" },
            { "Lucky Cup", "[luckycup]" },
            { "Magic Echo", "[magicecho]" },
            { "Anklet", "[anklet]" },
            { "Muffling Bell", "[mufflingbell]" },
            { "Glass Cannon", "[glasscannon]" },
            { "Perfume", "[perfume]" },
            { "Louder Echo", "[louderecho]" },
            { "Aura's Gem", "[aurasgem]" },
            { "Bone Card", "[bonecard]" },
            // Golden Trophies
            { "Mr Mayor", "[customimage]" },
            { "Secret Legend", "[customimage]" },
            { "Sacred Geometry", "[customimage]" },
            { "Vintage", "[customimage]" },
            { "Just Some Pals", "[customimage]" },
            { "Regal Weasel", "[customimage]" },
            { "Spring Falls", "[customimage]" },
            { "Power Up", "[customimage]" },
            { "Back To Work", "[customimage]" },
            { "Phonomath", "[customimage]" },
            { "Dusty", "[customimage]" },
            { "Forever Friend", "[customimage]" },
            // Fool Trap
            { "Fool Trap", "[customimage]" },
            // Money
            { "Money x1", "[realmoney]" },
            { "Money x10", "[realmoney]" },
            { "Money x15", "[realmoney]" },
            { "Money x16", "[realmoney]" },
            { "Money x20", "[realmoney]" },
            { "Money x25", "[realmoney]" },
            { "Money x30", "[realmoney]" },
            { "Money x32", "[realmoney]" },
            { "Money x40", "[realmoney]" },
            { "Money x48", "[realmoney]" },
            { "Money x50", "[realmoney]" },
            { "Money x64", "[realmoney]" },
            { "Money x100", "[realmoney]" },
            { "Money x128", "[realmoney]" },
            { "Money x200", "[realmoney]" },
            { "Money x255", "[realmoney]" },
            // Pages
            { "Pages 0-1", "[book]" },
            { "Pages 2-3", "[book]" },
            { "Pages 4-5", "[book]" },
            { "Pages 6-7", "[book]" },
            { "Pages 8-9", "[book]" },
            { "Pages 10-11", "[book]" },
            { "Pages 12-13", "[book]" },
            { "Pages 14-15", "[book]" },
            { "Pages 16-17", "[book]" },
            { "Pages 18-19", "[book]" },
            { "Pages 20-21", "[book]" },
            { "Pages 22-23", "[book]" },
            { "Pages 24-25 (Prayer)", "[book]" },
            { "Pages 26-27", "[book]" },
            { "Pages 28-29", "[book]" },
            { "Pages 30-31", "[book]" },
            { "Pages 32-33", "[book]" },
            { "Pages 34-35", "[book]" },
            { "Pages 36-37", "[book]" },
            { "Pages 38-39", "[book]" },
            { "Pages 40-41", "[book]" },
            { "Pages 42-43 (Holy Cross)", "[book]" },
            { "Pages 44-45", "[book]" },
            { "Pages 46-47", "[book]" },
            { "Pages 48-49", "[book]" },
            { "Pages 50-51", "[book]" },
            { "Pages 52-53 (Ice Rod)", "[book]" },
            { "Pages 54-55", "[book]" },
            // Non-Tunic Item
            { "Archipelago Item", "[customimage]"}
        };

        public static void SetupCustomGlyphSprites() {
            List<string> cartouches = Parser.cartouche.ToList();
            if (!cartouches.Contains("[torch]")) {
                cartouches.AddRange(CustomSpriteIndices.Keys);
                cartouches.Add("[filler]");
                Parser.cartouche = cartouches.ToArray();
                List<Sprite> sprites = SpriteBuilder.spriteResources.ToList();
                foreach (string Icon in CustomSpriteIcons.Keys) {
                    sprites.Add(ModelSwaps.FindSprite(CustomSpriteIcons[Icon]));
                    SpriteNameToAbbreviation.Add(CustomSpriteIcons[Icon], Icon);
                }
                sprites.Add(Inventory.GetItemByName("Homeward Bone Statue").icon);
                SpriteBuilder.spriteResources = sprites.ToArray();
            }
        }

        public static bool Parser_findSymbol_PrefixPatch(ref string s, ref int __result) {
            if (CustomSpriteIndices.ContainsKey(s)) {
                __result = CustomSpriteIndices[s];
                return false;
            }
            return true;
        }

        public static void SpriteBuilder_rebuild_PostfixPatch(SpriteBuilder __instance) {
            foreach (SpriteRenderer renderer in __instance.gameObject.transform.GetComponentsInChildren<SpriteRenderer>(true)) {
                if (renderer.sprite != null && CustomSpriteIcons.Values.ToList().Contains(renderer.sprite.name)) {
                    renderer.material = ModelSwaps.FindMaterial("UI Add");
                    if (renderer.sprite.name.Contains("trinkets ")) {
                        renderer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                        renderer.material = ModelSwaps.FindMaterial("UI-trinket");
                        GameObject backing = new GameObject("backing");
                        backing.AddComponent<SpriteRenderer>().sprite = ModelSwaps.FindSprite("trinkets 2_backing");
                        backing.transform.parent = renderer.transform.parent;
                        backing.transform.localPosition = renderer.transform.localPosition;
                        backing.transform.localScale = renderer.transform.localScale * 0.9f;
                        backing.layer = renderer.gameObject.layer;
                    } else if (renderer.sprite.name == "Inventory items_sword") {
                        if (SaveFile.GetInt("randomizer sword progression enabled") == 1) {
                            switch (NPCDialogue.instance.isActiveAndEnabled ? SaveFile.GetInt("randomizer sword progression level")+1 : SaveFile.GetInt("randomizer sword progression level")) {
                                case 1:
                                    renderer.sprite = ModelSwaps.FindSprite("Inventory items_stick");
                                    break;
                                case 2:
                                    renderer.sprite = ModelSwaps.FindSprite("Inventory items_sword");
                                    break;
                                case 3:
                                    renderer.gameObject.AddComponent<RawImage>().texture = ModelSwaps.SecondSwordImage.GetComponent<RawImage>().texture;
                                    renderer.enabled = false;
                                    break;
                                case 4:
                                case 5:
                                    renderer.gameObject.AddComponent<RawImage>().texture = ModelSwaps.ThirdSwordImage.GetComponent<RawImage>().texture;
                                    renderer.enabled = false;
                                    break;
                                default:
                                    renderer.sprite = ModelSwaps.FindSprite("Inventory items_sword");
                                    break;
                            }
                        }
                    } else if (renderer.sprite.name == "Inventory items_sword2" && ModelSwaps.CustomItemImages.ContainsKey(CustomImageToDisplay)) {
                        if (renderer.gameObject != null && ModelSwaps.CustomItemImages[CustomImageToDisplay] != null
                            && ModelSwaps.CustomItemImages[CustomImageToDisplay].GetComponent<RawImage>() != null && renderer.GetComponent<RawImage>() == null) {

                            renderer.gameObject.AddComponent<RawImage>().texture = ModelSwaps.CustomItemImages[CustomImageToDisplay].GetComponent<RawImage>().texture;
                            renderer.gameObject.GetComponent<RawImage>().material = renderer.material;
                            if (CustomImageToDisplay == "Fool Trap") {
                                renderer.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
                                renderer.transform.localScale *= 0.75f;
                            }
                            if (CustomImageToDisplay == "Archipelago Item") {
                                renderer.transform.localScale *= 0.85f;
                            }
                            renderer.enabled = false;
                        }
                    } else if (renderer.sprite.name == "game gui_money_icon") {
                        renderer.transform.localScale *= 1.25f;
                    }

                    renderer.transform.localScale *= 0.85f;
                }
            }
        }
    }
}
