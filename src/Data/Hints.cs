using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using static TunicArchipelago.GhostHints;

namespace TunicArchipelago {
    public class Hints {
        public static string LocationDescriptionsJson = "{\"5 [Overworld Redux]\":\"[East] Between ladders near Ruined Passage\",\"55 [Archipelagos Redux]\":\"[North] Obscured Beneath Hero's Memorial\",\"217 [Archipelagos Redux]\":\"[North] Behind Holy Cross Door\",\"west_garden [Archipelagos Redux]\":\"[North] Page Pickup\",\"256 [Archipelagos Redux]\":\"[North] Across From Page Pickup\",\"280 [Archipelagos Redux]\":\"[West] In Flooded Walkway\",\"283 [Archipelagos Redux]\":\"[West] Past Flooded Walkway\",\"57 [Archipelagos Redux]\":\"[West Highlands] Upper Left Walkway\",\"253 [Archipelagos Redux]\":\"[Central Lowlands] Passage Beneath Bridge\",\"56 [Archipelagos Redux]\":\"[Central Lowlands] Chest Near Shortcut Bridge\",\"Archipelagos Redux-(-396.3, 1.4, 42.3) [Archipelagos Redux]\":\"[West Lowlands] Tree Holy Cross Chest\",\"58 [Archipelagos Redux]\":\"[Central Lowlands] Chest Beneath Save Point\",\"206 [Archipelagos Redux]\":\"[Central Lowlands] Chest Beneath Faeries\",\"94 [Archipelagos Redux]\":\"[South Highlands] Secret Chest Beneath Fuse\",\"111 [Archipelagos Redux]\":\"[Southeast Lowlands] Outside Cave\",\"archipelagos_night [Archipelagos Redux]\":\"[East Lowlands] Page Behind Ice Dagger House\",\"257 [Archipelagos Redux]\":\"[Central Lowlands] Below Left Walkway\",\"59 [Archipelagos Redux]\":\"[Central Highlands] Behind Guard Captain\",\"Archipelagos Redux-(-236.0, 8.0, 86.3) [Archipelagos Redux]\":\"[Central Highlands] Holy Cross (Blue Lines)\",\"223 [Archipelagos Redux]\":\"[Central Highlands] Top of Ladder Before Boss\",\"93 [Archipelagos Redux]\":\"[Central Highlands] After Garden Knight\",\"Stundagger [archipelagos_house]\":\"[Southeast Lowlands] Ice Dagger Pickup\",\"72 [Atoll Redux]\":\"[North] From Lower Overworld Entrance\",\"67 [Atoll Redux]\":\"[South] Upper Floor On Bricks\",\"218 [Atoll Redux]\":\"[South] Upper Floor On Power Line\",\"219 [Atoll Redux]\":\"[South] Chest Near Big Crabs\",\"76 [Atoll Redux]\":\"[Southeast] Chest Near Fuse\",\"66 [Atoll Redux]\":\"[North] Obscured Beneath Bridge\",\"220 [Atoll Redux]\":\"[North] Guarded By Bird\",\"287 [Atoll Redux]\":\"[Northwest] Bombable Wall\",\"69 [Atoll Redux]\":\"[Northwest] Behind Envoy\",\"1010 [Atoll Redux]\":\"[West] Near Kevin Block\",\"70 [Atoll Redux]\":\"[Southwest] Obscured Behind Fuse\",\"68 [Atoll Redux]\":\"[South] Near Birds\",\"Key [Atoll Redux]\":\"[Northeast] Key Pickup\",\"73 [Atoll Redux]\":\"[East] Locked Room Lower Chest\",\"71 [Atoll Redux]\":\"[East] Locked Room Upper Chest\",\"221 [Atoll Redux]\":\"[Northeast] Chest Beneath Brick Walkway\",\"75 [Atoll Redux]\":\"[Northeast] Chest On Brick Walkway\",\"999 [Cathedral Arena]\":\"Gauntlet Reward\",\"1002 [Cathedral Redux]\":\"Secret Legend Trophy Chest\",\"240 [Cathedral Redux]\":\"[1F] Library\",\"244 [Cathedral Redux]\":\"[1F] Library Secret\",\"236 [Cathedral Redux]\":\"[1F] Guarded By Lasers\",\"237 [Cathedral Redux]\":\"[1F] Near Spikes\",\"243 [Cathedral Redux]\":\"[2F] Bird Room Secret\",\"238 [Cathedral Redux]\":\"[2F] Bird Room\",\"239 [Cathedral Redux]\":\"[2F] Entryway Upper Walkway\",\"241 [Cathedral Redux]\":\"[2F] Library\",\"242 [Cathedral Redux]\":\"[2F] Guarded By Lasers\",\"60 [Changing Room]\":\"Normal Chest\",\"52 [Crypt Redux]\":\"Skulls Chest\",\"213 [Crypt Redux]\":\"Spike Maze Upper Walkway\",\"53 [Crypt Redux]\":\"Spike Maze Near Stairs\",\"210 [Crypt Redux]\":\"Spike Maze Near Exit\",\"54 [Crypt Redux]\":\"1st Laser Room Obscured\",\"212 [Crypt Redux]\":\"1st Laser Room\",\"211 [Crypt Redux]\":\"2nd Laser Room\",\"CubeRoom-(321.1, 3.0, 217.0) [CubeRoom]\":\"Holy Cross Chest\",\"1011 [Dusty]\":\"Secret Chest\",\"286 [East Forest Redux]\":\"Bombable Wall\",\"25 [East Forest Redux]\":\"Near Telescope\",\"24 [East Forest Redux]\":\"Near Save Point\",\"forest [East Forest Redux]\":\"Page On Teleporter\",\"23 [East Forest Redux]\":\"From Guardhouse 1 Chest\",\"East Forest Redux-(104.0, 16.0, 61.0) [East Forest Redux]\":\"Dancing Fox Spirit Holy Cross\",\"26 [East Forest Redux]\":\"Spider Chest\",\"248 [East Forest Redux]\":\"Beneath Spider Chest\",\"East Forest Redux-(164.0, -25.0, -56.0) [East Forest Redux]\":\"Golden Obelisk Holy Cross\",\"284 [East Forest Redux]\":\"Lower Grapple Chest\",\"281 [East Forest Redux]\":\"Lower Dash Chest\",\"1006 [East Forest Redux]\":\"Ice Rod Grapple Chest\",\"21 [East Forest Redux]\":\"Above Save Point\",\"22 [East Forest Redux]\":\"Above Save Point Obscured\",\"29 [East Forest Redux Interior]\":\"Upper Floor\",\"30 [East Forest Redux Interior]\":\"Bottom Floor Secret\",\"27 [East Forest Redux Laddercave]\":\"Upper Floor Obscured\",\"28 [East Forest Redux Laddercave]\":\"Upper Floor\",\"270 [EastFiligreeCache]\":\"Chest 3\",\"271 [EastFiligreeCache]\":\"Chest 2\",\"272 [EastFiligreeCache]\":\"Chest 1\",\"forest shortcut [Forest Belltower]\":\"Page Pickup\",\"205 [Forest Belltower]\":\"Obscured Beneath Bell Bottom Floor\",\"20 [Forest Belltower]\":\"After Guard Captain\",\"204 [Forest Belltower]\":\"Obscured Near Bell Top Floor\",\"19 [Forest Belltower]\":\"Near Save Point\",\"Vault Key (Red) [Fortress Arena]\":\"Siege Engine/Vault Key Pickup\",\"Hexagon Red [Fortress Arena]\":\"Hexagon Red\",\"63 [Fortress Basement]\":\"Obscured Behind Waterfall\",\"61 [Fortress Basement]\":\"Bridge\",\"62 [Fortress Basement]\":\"Cell Chest 1\",\"65 [Fortress Basement]\":\"Cell Chest 2\",\"64 [Fortress Basement]\":\"Back Room Chest\",\"86 [Fortress Courtyard]\":\"From West Belltower\",\"96 [Fortress Courtyard]\":\"Below Walkway\",\"88 [Fortress Courtyard]\":\"Near Fuse\",\"87 [Fortress Courtyard]\":\"Chest Near Cave\",\"spidercave [Fortress Courtyard]\":\"Page Near Cave\",\"112 [Fortress East]\":\"Chest Near Slimes\",\"fortress [Fortress Main]\":\"[West Wing] Page Pickup\",\"83 [Fortress Main]\":\"[West Wing] Dark Room Chest 1\",\"84 [Fortress Main]\":\"[West Wing] Dark Room Chest 2\",\"Fortress Main-(-75.0, -1.0, 17.0) [Fortress Main]\":\"[West Wing] Candles Holy Cross\",\"85 [Fortress Main]\":\"[East Wing] Bombable Wall\",\"113 [Fortress Reliquary]\":\"Upper Walkway\",\"114 [Fortress Reliquary]\":\"Chest Right of Grave\",\"115 [Fortress Reliquary]\":\"Obscured Chest Left of Grave\",\"Wand [frog cave main]\":\"Magic Orb Pickup\",\"77 [frog cave main]\":\"Above Vault\",\"82 [frog cave main]\":\"Side Room Grapple Secret\",\"81 [frog cave main]\":\"Side Room Chest\",\"80 [frog cave main]\":\"Side Room Secret Passage\",\"78 [frog cave main]\":\"Main Room Top Floor\",\"279 [frog cave main]\":\"Grapple Above Hot Tub\",\"79 [frog cave main]\":\"Main Room Bottom Floor\",\"222 [frog cave main]\":\"Near Vault\",\"259 [frog cave main]\":\"Slorm Room\",\"276 [frog cave main]\":\"Escape Chest\",\"Lantern [Furnace]\":\"Lantern Pickup\",\"92 [Furnace]\":\"Chest\",\"Hexagon Green [Library Arena]\":\"Hexagon Green\",\"Library Hall-(133.3, 10.0, -43.2) [Library Hall]\":\"Holy Cross Chest\",\"library_2 [Library Lab]\":\"Page 1\",\"library_3 [Library Lab]\":\"Page 2\",\"library_1 [Library Lab]\":\"Page 3\",\"226 [Library Lab]\":\"Chest By Shrine 1\",\"225 [Library Lab]\":\"Chest By Shrine 2\",\"227 [Library Lab]\":\"Chest By Shrine 3\",\"228 [Library Lab]\":\"Behind Chalkboard by Fuse\",\"216 [Maze Room]\":\"Maze Room Chest\",\"Maze Room-(1.0, 0.0, -1.0) [Maze Room]\":\"Maze Room Holy Cross\",\"200 [Monastery]\":\"Monastery Chest\",\"mountain [Mountain]\":\"Page Before Door\",\"final [Mountaintop]\":\"Page At The Peak\",\"Overworld Cave-(-90.4, 515.0, -738.9) [Overworld Cave]\":\"Holy Cross Chest\",\"89 [Overworld Interiors]\":\"Normal Chest\",\"Overworld Interiors-(-28.0, 27.0, -50.5) [Overworld Interiors]\":\"Holy Cross Chest\",\"Shield [Overworld Interiors]\":\"Shield Pickup\",\"under_overworld [Overworld Interiors]\":\"Holy Cross Door Page\",\"1013 [Overworld Redux]\":\"[South] Starting Platform Holy Cross\",\"8 [Overworld Redux]\":\"[Central] Chest Across From Well\",\"11 [Overworld Redux]\":\"[West] Obscured Near Well\",\"1 [Overworld Redux]\":\"[West] Obscured Behind Windmill\",\"1003 [Overworld Redux]\":\"[West] Windmill Holy Cross\",\"Key [Overworld Redux]\":\"[West] Key Pickup\",\"12 [Overworld Redux]\":\"[Central] Bombable Wall\",\"90 [Overworld Redux]\":\"[East] Chest In Trees\",\"255 [Overworld Redux]\":\"[Southeast] Chest Near Swamp\",\"15 [Overworld Redux]\":\"[East] Chest Near Pots\",\"Overworld Redux-(90.4, 36.0, -122.1) [Overworld Redux]\":\"[East] Weathervane Holy Cross\",\"overworld post-forest [Overworld Redux]\":\"[East] Page Near Secret Shop\",\"Overworld Redux-(64.5, 44.0, -40.0) [Overworld Redux]\":\"[Northeast] Flowers Holy Cross\",\"6 [Overworld Redux]\":\"[Northeast] Chest Above Patrol Cave\",\"Techbow [Overworld Redux]\":\"[Northwest] Fire Wand Pickup\",\"stonehenge_reward [Overworld Redux]\":\"[Northwest] Golden Obelisk Page\",\"16 [Overworld Redux]\":\"[Northwest] Chest Near Golden Obelisk\",\"9 [Overworld Redux]\":\"[Northwest] Chest Near Quarry Gate\",\"13 [Overworld Redux]\":\"[Northwest] Chest Near Turret\",\"207 [Overworld Redux]\":\"[Northwest] Shadowy Corner Chest\",\"1008 [Overworld Redux]\":\"[West] Windchimes Holy Cross\",\"town_upper [Overworld Redux]\":\"[West] Page On Teleporter\",\"Overworld Redux-(-132.0, 28.0, -55.5) [Overworld Redux]\":\"[West] Moss Wall Holy Cross\",\"91 [Overworld Redux]\":\"[West] Chest Behind Moss Wall\",\"overworld_dash [Overworld Redux]\":\"[Southwest] Fountain Page\",\"Overworld Redux-(-83.0, 20.0, -117.5) [Overworld Redux]\":\"[Southwest] Fountain Holy Cross\",\"2 [Overworld Redux]\":\"[Southwest] Chest Guarded By Turret\",\"209 [Overworld Redux]\":\"[Southwest] Grapple Chest Over Walkway\",\"Key (House) [Overworld Redux]\":\"[Southwest] Key Pickup\",\"17 [Overworld Redux]\":\"[Southwest] South Chest Near Guard\",\"208 [Overworld Redux]\":\"[Southwest] Obscured In Tunnel To Beach\",\"273 [Overworld Redux]\":\"[Southwest] Beach Chest Near Flowers\",\"Overworld Redux-(-52.0, 2.0, -174.8) [Overworld Redux]\":\"[Southwest] Flowers Holy Cross\",\"1004 [Overworld Redux]\":\"[Southwest] Haiku Holy Cross\",\"7 [Overworld Redux]\":\"[Southwest] Beach Chest Beneath Guard\",\"4 [Overworld Redux]\":\"[Southwest] Tunnel Guarded By Turret\",\"18 [Overworld Redux]\":\"[Southwest] West Beach Guarded By Turret\",\"267 [Overworld Redux]\":\"[Southwest] West Beach Guarded By Turret 2\",\"beach [Overworld Redux]\":\"[South] Beach Page\",\"285 [Overworld Redux]\":\"[Southwest] Bombable Wall Near Fountain\",\"10 [Overworld Redux]\":\"[South] Beach Chest\",\"cathedral [Overworld Redux]\":\"[Southeast] Page on Pillar by Swamp\",\"tablet [Overworld Redux]\":\"[Northwest] Page on Pillar by Dark Tomb\",\"town_well [Overworld Redux]\":\"[Northwest] Page By Well\",\"245 [Overworld Redux]\":\"[Northwest] Chest Beneath Quarry Gate\",\"14 [Overworld Redux]\":\"[West] Near Gardens Entrance\",\"258 [Overworld Redux]\":\"[Southwest] From West Garden\",\"3 [Overworld Redux]\":\"[West] Chest After Bell\",\"266 [Overworld Redux]\":\"[East] Grapple Chest\",\"214 [PatrolCave]\":\"Normal Chest\",\"PatrolCave-(74.0, 46.0, 24.0) [PatrolCave]\":\"Holy Cross Chest\",\"Quarry Redux-(0.7, 68.0, 84.7) [Quarry Redux]\":\"[Back Entrance] Bushes Holy Cross\",\"126 [Quarry Redux]\":\"[East] Obscured Near Telescope\",\"133 [Quarry Redux]\":\"[Central] Obscured Below Entry Walkway\",\"116 [Quarry Redux]\":\"[Back Entrance] Chest\",\"128 [Quarry Redux]\":\"[Back Entrance] Obscured Behind Wall\",\"288 [Quarry Redux]\":\"[West] Upper Area Bombable Wall\",\"127 [Quarry Redux]\":\"[West] Upper Area Near Waterfall\",\"120 [Quarry Redux]\":\"[West] Near Shooting Range\",\"265 [Quarry Redux]\":\"[West] Shooting Range Secret Path\",\"121 [Quarry Redux]\":\"[West] Below Shooting Range\",\"130 [Quarry Redux]\":\"[West] Lower Area Below Bridge\",\"131 [Quarry Redux]\":\"[West] Lower Area Isolated Chest\",\"262 [Quarry Redux]\":\"[West] Lower Area After Bridge\",\"122 [Quarry Redux]\":\"[Lowlands] Below Broken Ladder\",\"129 [Quarry Redux]\":\"[Lowlands] Upper Walkway\",\"132 [Quarry Redux]\":\"[Lowlands] Near Elevator\",\"123 [Quarry Redux]\":\"[Central] Below Entry Walkway\",\"117 [Quarry Redux]\":\"[Central] Near Shortcut Ladder\",\"224 [Quarry Redux]\":\"[East] Near Bridge\",\"289 [Quarry Redux]\":\"[East] Bombable Wall\",\"118 [Quarry Redux]\":\"[East] Near Telescope\",\"268 [Quarry Redux]\":\"[Central] Obscured Behind Staircase\",\"125 [Quarry Redux]\":\"[East] Obscured Beneath Scaffolding\",\"124 [Quarry Redux]\":\"[East] Obscured Near Winding Staircase\",\"119 [Quarry Redux]\":\"[East] Upper Floor\",\"250 [Quarry Redux]\":\"[Central] Above Ladder\",\"282 [Quarry Redux]\":\"[Central] Above Ladder Dash Chest\",\"134 [Quarry Redux]\":\"[Central] Top Floor Overhang\",\"Relic PIckup (6) Sword) [RelicVoid]\":\"Tooth Relic\",\"Relic PIckup (5) (MP) [RelicVoid]\":\"Mushroom Relic\",\"Relic PIckup (4) (water) [RelicVoid]\":\"Ash Relic\",\"Relic PIckup (3) (HP) [RelicVoid]\":\"Flowers Relic\",\"Relic PIckup (2) (Crown) [RelicVoid]\":\"Effigy Relic\",\"Relic PIckup (1) (SP) [RelicVoid]\":\"Feathers Relic\",\"35 [Ruined Shop]\":\"Chest 1\",\"36 [Ruined Shop]\":\"Chest 2\",\"37 [Ruined Shop]\":\"Chest 3\",\"first [Ruins Passage]\":\"Page Pickup\",\"1001 [Ruins Passage]\":\"Holy Cross Chest\",\"40 [Sewer]\":\"[Entryway] Chest\",\"43 [Sewer]\":\"[Entryway] Obscured Behind Waterfall\",\"sewer [Sewer]\":\"[Second Room] Page\",\"49 [Sewer]\":\"[Second Room] Obscured Behind Waterfall\",\"47 [Sewer]\":\"[Back Corridor] Right Secret\",\"48 [Sewer]\":\"[Back Corridor] Left Secret\",\"41 [Sewer]\":\"[Third Room] Beneath Platform Chest\",\"42 [Sewer]\":\"[Third Room] Tentacle Chest\",\"46 [Sewer]\":\"[Second Room] Underwater Chest\",\"50 [Sewer]\":\"[Side Room] Chest By Pots\",\"44 [Sewer]\":\"[Save Room] Upper Floor Chest 1\",\"45 [Sewer]\":\"[Save Room] Upper Floor Chest 2\",\"51 [Sewer]\":\"[Side Room] Chest By Phrends\",\"264 [Sewer]\":\"[Powered Secret Room] Chest\",\"below_crypt [Sewer_Boss]\":\"[Passage To Dark Tomb] Page Pickup\",\"Potion (First) [Shop]\":\"Potion 1\",\"Potion (West Garden) [Shop]\":\"Potion 2\",\"Trinket Coin 1 (day) [Shop]\":\"Coin 1\",\"Trinket Coin 2 (night) [Shop]\":\"Coin 2\",\"skullcave [ShopSpecial]\":\"Secret Page Pickup\",\"105 [Swamp Redux 2]\":\"[Entrance] Above Entryway\",\"235 [Swamp Redux 2]\":\"[Entrance] Obscured Inside Watchtower\",\"254 [Swamp Redux 2]\":\"[Entrance] North Small Island\",\"246 [Swamp Redux 2]\":\"[Entrance] South Near Fence\",\"249 [Swamp Redux 2]\":\"[South Graveyard] Chest Near Graves\",\"247 [Swamp Redux 2]\":\"[South Graveyard] Guarded By Big Skeleton\",\"108 [Swamp Redux 2]\":\"[South Graveyard] Guarded By Tentacles\",\"104 [Swamp Redux 2]\":\"[South Graveyard] Obscured Beneath Telescope\",\"98 [Swamp Redux 2]\":\"[South Graveyard] Above Big Skeleton\",\"107 [Swamp Redux 2]\":\"[South Graveyard] Upper Walkway On Pedestal\",\"97 [Swamp Redux 2]\":\"[South Graveyard] Upper Walkway Dash Chest\",\"1005 [Swamp Redux 2]\":\"[South Graveyard] 4 Orange Skulls\",\"103 [Swamp Redux 2]\":\"[South Graveyard] Obscured Behind Ridge\",\"278 [Swamp Redux 2]\":\"[Central] Obscured Behind Northern Mountain\",\"99 [Swamp Redux 2]\":\"[Central] Beneath Memorial\",\"101 [Swamp Redux 2]\":\"[Central] Near Ramps Up\",\"106 [Swamp Redux 2]\":\"[Central] South Secret Passage\",\"109 [Swamp Redux 2]\":\"[Upper Graveyward] Near Telescope\",\"100 [Swamp Redux 2]\":\"[Upper Graveyward] Obscured Behind Hill\",\"102 [Swamp Redux 2]\":\"[Upper Graveyward] Near Shield Fleemers\",\"277 [Swamp Redux 2]\":\"[Outside Cathedral] Obscured Behind Memorial\",\"110 [Swamp Redux 2]\":\"[Outside Cathedral] Near Moonlight Bridge Door\",\"32 [Sword Access]\":\"Obscured Chest\",\"31 [Sword Access]\":\"Above Gate\",\"Sword [Sword Access]\":\"Sword Pickup\",\"1009 [Sword Access]\":\"Holy Cross Code by Grave\",\"33 [Sword Access]\":\"Upper Walkway\",\"19 [Sword Cave]\":\"Stick Chest\",\"Temple-(14.0, 0.1, 42.4) [Temple]\":\"Holy Cross Chest\",\"temple [Temple]\":\"Page Pickup\",\"95 [Town Basement]\":\"Hourglass Chest\",\"Town Basement-(-202.0, 28.0, 150.0) [Town Basement]\":\"Holy Cross Chest\",\"town_filigree [Town_FiligreeRoom]\":\"Page Pickup\",\"FT_Island [Transit]\":\"Page Pickup\",\"1012 [Transit]\":\"Secret Chest\",\"Well Reward (3 Coins) [Trinket Well]\":\"3 Coins\",\"Well Reward (6 Coins) [Trinket Well]\":\"6 Coins\",\"Well Reward (10 Coins) [Trinket Well]\":\"10 Coins\",\"Well Reward (15 Coins) [Trinket Well]\":\"15 Coins\",\"Waterfall-(-47.0, 45.0, 10.0) [Waterfall]\":\"Holy Cross Chest\",\"waterfall [Waterfall]\":\"10 Fairy Reward\",\"1007 [Waterfall]\":\"20 Fairy Reward\",\"274 [ziggurat2020_1]\":\"Near Bridge Switch\",\"275 [ziggurat2020_1]\":\"Beneath Bridge To Administrator\",\"229 [ziggurat2020_2]\":\"Inside Tower\",\"230 [ziggurat2020_3]\":\"Near Corpses\",\"231 [ziggurat2020_3]\":\"Spider Ambush\",\"234 [ziggurat2020_3]\":\"Guarded By Double Turrets\",\"261 [ziggurat2020_3]\":\"Guarded By Double Turrets 2 \",\"260 [ziggurat2020_3]\":\"After 2nd Double Turret Chest\",\"232 [ziggurat2020_3]\":\"Left Of Checkpoint Before Fuse\",\"233 [ziggurat2020_3]\":\"After Guarded Fuse\",\"Hexagon Blue [ziggurat2020_3]\":\"Hexagon Blue\"}";

        public static Dictionary<string, string> HintLocations = new Dictionary<string, string>() {
            {"Overworld Redux (-7.0, 12.0, -136.4)", "Mailbox"},
            {"Archipelagos Redux (-170.0, 11.0, 152.5)", "West Garden Relic"},
            {"Swamp Redux 2 (92.0, 7.0, 90.8)", "Swamp Relic"},
            {"Sword Access (28.5, 5.0, -190.0)", "East Forest Relic"},
            {"Library Hall (131.0, 19.0, -8.5)", "Library Relic"},
            {"Monastery (-6.0, 26.0, 180.5)", "Monastery Relic"},
            {"Fortress Reliquary (198.5, 5.0, -40.0)", "Fortress Relic"},
            {"Temple (14.0, -0.5, 49.0)", "Temple Statue"}
        };

        public static Dictionary<string, string> HintMessages = new Dictionary<string, string>();

        public static void PopulateHints() {
            HintMessages.Clear();
            System.Random random = new System.Random(SaveFile.GetInt("seed"));
            string Hint = "";
            string Scene = "";
            string Prefix = "";
            List<char> Vowels = new List<char>() { 'A', 'E', 'I', 'O', 'U' };

            int Player = Archipelago.instance.GetPlayerSlot();

            ArchipelagoHint Lantern = Locations.MajorItemLocations["Lantern"];
            Scene = Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[Lantern.Location]].Location.SceneName].ToUpper();
            Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
            if (Lantern.Player == Player) {
                Hint = $"lehjehnd sehz {Prefix} \"{Scene.ToUpper()}\"\nwil hehlp yoo \"<#00FFFF>LIGHT THE WAY<#ffffff>...\"";
            } else {
                Hint = $"lehjehnd sehz \"{Archipelago.instance.GetPlayerName((int)Lantern.Player).ToUpper()}'S WORLD\"\nwil hehlp yoo \"<#00FFFF>LIGHT THE WAY<#ffffff>...\"";
            }
            HintMessages.Add("Mailbox", Hint);

            ArchipelagoHint Hyperdash = Locations.MajorItemLocations["Hero's Laurels"];
            Hint = $"lehjehnd sehz <#FF00FF>suhm%i^ ehkstruhordinArE<#FFFFFF>\nuhwAts yoo in ";
            if (Hyperdash.Player == Player) {
                Scene = Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[Hyperdash.Location]].Location.SceneName].ToUpper();
                Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                Hint += $"{Prefix} \"{Scene}...\"";
            } else {
                Hint += $"\"{Archipelago.instance.GetPlayerName((int)Hyperdash.Player).ToUpper()}'S WORLD...\"";  
            }
            HintMessages.Add("Temple Statue", Hint);

            List<string> HintItems = new List<string>() { "Magic Wand", "Magic Orb", "Magic Dagger" };
            if (SaveFile.GetInt("randomizer shuffled abilities") == 1) {
                HintItems.Add("Pages 24-25 (Prayer)");
                HintItems.Add("Pages 42-43 (Holy Cross)");
                HintItems.Remove("Magic Dagger");
            }
            List<string> HintGraves = new List<string>() { "East Forest Relic", "Fortress Relic", "West Garden Relic" };
            while (HintGraves.Count > 0) {
                string HintItem = HintItems[random.Next(HintItems.Count)];
                ArchipelagoHint ItemHint = Locations.MajorItemLocations[HintItem];
                string HintGrave = HintGraves[random.Next(HintGraves.Count)];

                if (ItemHint.Player == Player) {
                    Scene = Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[ItemHint.Location]].Location.SceneName].ToUpper();
                    if (HintItem == "Pages 24-25 (Prayer)" && Scene == "Fortress Relic") {
                        continue;
                    }
                    Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                    Hint = $"lehjehnd sehz {Prefix} \"{Scene}\"";
                } else {
                    Hint = $"lehjehnd sehz \"{Archipelago.instance.GetPlayerName((int)ItemHint.Player).ToUpper()}'S WORLD...\"";
                }
                Hint += $"\niz lOkAtid awn #uh \"<#ffd700>PATH OF THE HERO<#ffffff>...\"";
                Hints.HintMessages.Add(HintGrave, Hint);

                HintItems.Remove(HintItem);
                HintGraves.Remove(HintGrave);
            }

            List<string> Hexagons;
            Dictionary<string, string> HexagonColors = new Dictionary<string, string>() { { "Red Hexagon", "<#FF3333>" }, { "Green Hexagon", "<#33FF33>" }, { "Blue Hexagon", "<#3333FF>" }, { "Gold Hexagon", "<#ffd700>" } };
            if (SaveFile.GetString("randomizer game mode") == "HEXAGONQUEST") {
                Hexagons = new List<string>() { "Gold Hexagon", "Gold Hexagon", "Gold Hexagon" };
            } else {
                Hexagons = new List<string>() { "Red Hexagon", "Green Hexagon", "Blue Hexagon" };
            }

            List<string> HexagonHintGraves = new List<string>() { "Swamp Relic", "Library Relic", "Monastery Relic" };
            for (int i = 0; i < 3; i++) {
                string Hexagon = Hexagons[random.Next(Hexagons.Count)];
                string HexagonHintArea = HexagonHintGraves[random.Next(HexagonHintGraves.Count)];
                ArchipelagoHint HexHint = Locations.MajorItemLocations[Hexagon];

                if (HexHint.Player == Player) {
                    Scene = Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[Hyperdash.Location]].Location.SceneName].ToUpper();
                    Prefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                    Hint = $"#A sA {Prefix} \"{Scene.ToUpper()}\" iz \nwAr #uh {HexagonColors[Hexagon]}kwehstuhgawn [hexagram]<#FFFFFF> iz fownd\"...\"";
                } else {
                    Hint = $"#A sA \"{Archipelago.instance.GetPlayerName((int)HexHint.Player).ToUpper()}'S WORLD\" iz \nwAr #uh {HexagonColors[Hexagon]}kwehstuhgawn [hexagram]<#FFFFFF> iz fownd\"...\"";
                }
                
                Hints.HintMessages.Add(HexagonHintArea, Hint);

                Hexagons.Remove(Hexagon);
                HexagonHintGraves.Remove(HexagonHintArea);
            }
        }

        public static string WordWrapString(string Hint) {
            string formattedHint = "";

            int length = 40;
            foreach (string split in Hint.Split(' ')) {
                string split2 = split;
                if (split.StartsWith($"\"") && !split.EndsWith($"\"")) {
                    split2 += $"\"";
                } else if (split.EndsWith($"\"") && !split.StartsWith($"\"")) {
                    split2 = $"\"{split2}";
                }
                if ((formattedHint + split2).Length < length) {
                    formattedHint += split2 + " ";
                } else {
                    formattedHint += split2 + "\n";
                    length += 40;
                }
            }

            return formattedHint;
        }

    }
}
