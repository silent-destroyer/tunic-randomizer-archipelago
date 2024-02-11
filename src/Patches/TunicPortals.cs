using BepInEx.Logging;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TunicArchipelago {
    public class TunicPortals {
        private static ManualLogSource Logger = TunicArchipelago.Logger;
        public static Dictionary<string, PortalCombo> RandomizedPortals = new Dictionary<string, PortalCombo>();

        public class TunicPortal {
            public string SceneName; // the scene the portal is in
            public string Destination; // the vanilla destination scene
            public string DestinationTag; // the vanilla destination tag, aka ID
            public string PortalName; // a human-readable name for the portal
            public string GranularRegion; // a sub-region name, if there is one for that scene. For use in making sure everything can be accessed
            public Dictionary<string, int> RequiredItems; // required items if there is only one item or one set of items required. A string like "scene, destination_tag" counts as an item.
            public List<Dictionary<string, int>> RequiredItemsOr; // required items if there are multiple different possible requirements. A string like "scene, destination_tag" counts as an item.
            public List<string> GivesAccess; // portals that you are given access to by this portal. ex: the dance fox portal to the lower east forest portal in guardhouse 1.
            public Dictionary<string, int> EntryItems; // portals that require items to enter, but not exit from. ex: hero's graves, the yellow prayer portal pads, and the fountain holy cross door in overworld.
            public bool DeadEnd; // portals that are dead ends, like stick house or the gauntlet lower entry.
            public bool PrayerPortal; // portals that require prayer to enter. This is a more convenient version of GivesAccess for prayer portals.
            public bool OneWay; // portals that are one-way, such as the back entrance to monastery and the forest belltower top portal
            public bool IgnoreScene; // portals that cannot reach the center of the region, and as such do not give region access, like the rail between beneath the well and furnace
            public bool SpecialReqs; // portals that have weird rules, basically, where we'll be way more verbose in requireditems or requireditemsor
            public TunicPortal() { }

            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion) {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion, bool prayerPortal = false, bool deadEnd = false, bool oneWay = false, bool ignoreScene = false, bool specialReqs = false) {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
                PrayerPortal = prayerPortal;
                DeadEnd = deadEnd;
                OneWay = oneWay;
                IgnoreScene = ignoreScene;
                SpecialReqs = specialReqs;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion, Dictionary<string, int> entryItems, bool prayerPortal = false, bool deadEnd = false, bool oneWay = false, bool ignoreScene = false, bool specialReqs = false) {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
                EntryItems = entryItems;
                PrayerPortal = prayerPortal;
                DeadEnd = deadEnd;
                OneWay = oneWay;
                IgnoreScene = ignoreScene;
                SpecialReqs = specialReqs;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion, Dictionary<string, int> requiredItems, bool prayerPortal = false, bool deadEnd = false, bool ignoreScene = false, bool specialReqs = false) {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
                RequiredItems = requiredItems;
                PrayerPortal = prayerPortal;
                DeadEnd = deadEnd;
                IgnoreScene = ignoreScene;
                SpecialReqs = specialReqs;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion, List<string> givesAccess, bool ignoreScene = false, bool oneWay = false, bool specialReqs = false) {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
                GivesAccess = givesAccess;
                IgnoreScene = ignoreScene;
                SpecialReqs = specialReqs;
                OneWay = oneWay;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion, Dictionary<string, int> requiredItems) {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
                RequiredItems = requiredItems;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion, Dictionary<string, int> requiredItems, List<string> givesAccess, bool ignoreScene = false, bool specialReqs = false, bool prayerPortal = false) {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
                RequiredItems = requiredItems;
                IgnoreScene = ignoreScene;
                SpecialReqs = specialReqs;
                GivesAccess = givesAccess;
                PrayerPortal = prayerPortal;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion, List<Dictionary<string, int>> requiredItemsOr, bool prayerPortal = false, bool ignoreScene = false, bool specialReqs = false) {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
                RequiredItemsOr = requiredItemsOr;
                PrayerPortal = prayerPortal;
                IgnoreScene = ignoreScene;
                SpecialReqs = specialReqs;
            }

            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion, List<string> givesAccess, List<Dictionary<string, int>> requiredItemsOr, bool prayerPortal = false, bool ignoreScene = false, bool specialReqs = false) {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
                GivesAccess = givesAccess;
                RequiredItemsOr = requiredItemsOr;
                PrayerPortal = prayerPortal;
                IgnoreScene = ignoreScene;
                SpecialReqs = specialReqs;
            }

            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion, Dictionary<string, int> requiredItems, List<string> givesAccess) {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
                RequiredItems = requiredItems;
                GivesAccess = givesAccess;
            }
        }

        public class NewPortal {
            public string Name;
            public string Destination;

            public NewPortal(string name, string destination) {
                Name = name;
                Destination = destination;
            }
        }
        
        public class RegionInfo {
            public string Scene;
            public bool DeadEnd;

            public RegionInfo(string scene, bool deadEnd) {
                Scene = scene;
                DeadEnd = deadEnd;
            }
        }

        public static Dictionary<string, Dictionary<string, List<NewPortal>>> RegionPortalsList = new Dictionary<string, Dictionary<string, List<NewPortal>>> {
            {
                "Overworld Redux",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Overworld",
                        new List<NewPortal> {
                            new NewPortal("Stick House Entrance", "Sword Cave_"),
                            new NewPortal("Windmill Entrance", "Windmill_"),
                            new NewPortal("Well Ladder Entrance", "Sewer_entrance"),
                            new NewPortal("Old House Waterfall Entrance", "Overworld Interiors_under_checkpoint"),
                            new NewPortal("Entrance to Furnace under Windmill", "Furnace_gyro_upper_east"),
                            new NewPortal("Entrance to Furnace from Beach", "Furnace_gyro_lower"),
                            new NewPortal("Caustic Light Cave Entrance", "Overworld Cave_"),
                            new NewPortal("Swamp Lower Entrance", "Swamp Redux 2_conduit"),
                            new NewPortal("Ruined Passage Not-Door Entrance", "Ruins Passage_east"),
                            new NewPortal("Atoll Upper Entrance", "Atoll Redux_upper"),
                            new NewPortal("Atoll Lower Entrance", "Atoll Redux_lower"),
                            new NewPortal("Maze Cave Entrance", "Maze Room_"),
                            new NewPortal("Temple Rafters Entrance", "Temple_rafters"),
                            new NewPortal("Ruined Shop Entrance", "Ruined Shop_"),
                            new NewPortal("Patrol Cave Entrance", "PatrolCave_"),
                            new NewPortal("Hourglass Cave Entrance", "Town Basement_beach"),
                            new NewPortal("Changing Room Entrance", "Changing Room_"),
                            new NewPortal("Cube Cave Entrance", "CubeRoom_"),
                            new NewPortal("Stairs from Overworld to Mountain", "Mountain_"),
                            new NewPortal("Overworld to Fortress", "Fortress Courtyard_"),
                            new NewPortal("Overworld to Quarry Connector", "Darkwoods Tunnel_"),
                            new NewPortal("Dark Tomb Main Entrance", "Crypt Redux_"),
                            new NewPortal("Overworld to Forest Belltower", "Forest Belltower_"),
                            new NewPortal("Secret Gathering Place Entrance", "Waterfall_"),
                        }
                    },
                    {
                        "Overworld Well to Furnace Rail",
                        new List<NewPortal> {
                            new NewPortal("Entrance to Well from Well Rail", "Sewer_west_aqueduct"),
                            new NewPortal("Entrance to Furnace from Well Rail", "Furnace_gyro_upper_north"),
                        }
                    },
                    {
                        "Overworld Old House Door",
                        new List<NewPortal> {
                            new NewPortal("Old House Door Entrance", "Overworld Interiors_house"),
                        }
                    },
                    {
                        "Overworld to West Garden from Furnace",
                        new List<NewPortal> {
                            new NewPortal("Entrance to Furnace near West Garden", "Furnace_gyro_west"),
                            new NewPortal("West Garden Entrance from Furnace", "Archipelagos Redux_lower"),
                        }
                    },
                    {
                        "Overworld Swamp Upper Entry",
                        new List<NewPortal> {
                            new NewPortal("Swamp Upper Entrance", "Swamp Redux 2_wall"),
                        }
                    },
                    {
                        "Overworld Ruined Passage Door",
                        new List<NewPortal> {
                            new NewPortal("Ruined Passage Door Entrance", "Ruins Passage_west"),
                        }
                    },
                    {
                        "Overworld Special Shop Entry",
                        new List<NewPortal> {
                            new NewPortal("Special Shop Entrance", "ShopSpecial_"),
                        }
                    },
                    {
                        "Overworld Belltower",
                        new List<NewPortal> {
                            new NewPortal("West Garden Entrance near Belltower", "Archipelagos Redux_upper"),
                        }
                    },
                    {
                        "Overworld West Garden Laurels Entry",
                        new List<NewPortal> {
                            new NewPortal("West Garden Laurels Entrance", "Archipelagos Redux_lowest"),
                        }
                    },
                    {
                        "Overworld Temple Door",
                        new List<NewPortal> {
                            new NewPortal("Temple Door Entrance", "Temple_main"),
                        }
                    },
                    {
                        "Overworld Fountain Cross Door",
                        new List<NewPortal> {
                            new NewPortal("Fountain HC Door Entrance", "Town_FiligreeRoom_"),
                        }
                    },
                    {
                        "Overworld Southeast Cross Door",
                        new List<NewPortal> {
                            new NewPortal("Southeast HC Door Entrance", "EastFiligreeCache_"),
                        }
                    },
                    {
                        "Overworld Town Portal",
                        new List<NewPortal> {
                            new NewPortal("Town to Far Shore", "Transit_teleporter_town"),
                        }
                    },
                    {
                        "Overworld Spawn Portal",
                        new List<NewPortal> {
                            new NewPortal("Spawn to Far Shore", "Transit_teleporter_starting island"),
                        }
                    },
                }
            },
            {
                "Waterfall",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Secret Gathering Place",
                        new List<NewPortal> {
                            new NewPortal("Secret Gathering Place Exit", "Overworld Redux_"),
                        }
                    },
                }
            },
            {
                "Windmill",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Windmill",
                        new List<NewPortal> {
                            new NewPortal("Windmill Exit", "Overworld Redux_"),
                            new NewPortal("Windmill Shop", "Shop_"),
                        }
                    },
                }
            },
            {
                "Overworld Interiors",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Old House Front",
                        new List<NewPortal> {
                            new NewPortal("Old House Door Exit", "Overworld Redux_house"),
                            new NewPortal("Old House to Glyph Tower", "g_elements_"),
                        }
                    },
                    {
                        "Old House Back",
                        new List<NewPortal> {
                            new NewPortal("Old House Waterfall Exit", "Overworld Redux_under_checkpoint"),
                        }
                    },
                }
            },
            {
                "g_elements",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Relic Tower",
                        new List<NewPortal> {
                            new NewPortal("Glyph Tower Exit", "Overworld Interiors_"),
                        }
                    },
                }
            },
            {
                "Changing Room",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Changing Room",
                        new List<NewPortal> {
                            new NewPortal("Changing Room Exit", "Overworld Redux_"),
                        }
                    },
                }
            },
            {
                "Town_FiligreeRoom",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Fountain Cross Room",
                        new List<NewPortal> {
                            new NewPortal("Fountain HC Room Exit", "Overworld Redux_"),
                        }
                    },
                }
            },
            {
                "CubeRoom",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Cube Cave",
                        new List<NewPortal> {
                            new NewPortal("Cube Cave Exit", "Overworld Redux_"),
                        }
                    },
                }
            },
            {
                "PatrolCave",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Patrol Cave",
                        new List<NewPortal> {
                            new NewPortal("Guard Patrol Cave Exit", "Overworld Redux_"),
                        }
                    },
                }
            },
            {
                "Ruined Shop",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Ruined Shop",
                        new List<NewPortal> {
                            new NewPortal("Ruined Shop Exit", "Overworld Redux_"),
                        }
                    },
                }
            },
            {
                "Furnace",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Furnace Fuse",
                        new List<NewPortal> {
                            new NewPortal("Furnace Exit towards Well", "Overworld Redux_gyro_upper_north"),
                        }
                    },
                    {
                        "Furnace Walking Path",
                        new List<NewPortal> {
                            new NewPortal("Furnace Exit to Dark Tomb", "Crypt Redux_"),
                            new NewPortal("Furnace Exit towards West Garden", "Overworld Redux_gyro_west"),
                        }
                    },
                    {
                        "Furnace Ladder Area",
                        new List<NewPortal> {
                            new NewPortal("Furnace Exit to Beach", "Overworld Redux_gyro_lower"),
                            new NewPortal("Furnace Exit under Windmill", "Overworld Redux_gyro_upper_east"),
                        }
                    },
                }
            },
            {
                "Sword Cave",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Stick House",
                        new List<NewPortal> {
                            new NewPortal("Stick House Exit", "Overworld Redux_"),
                        }
                    },
                }
            },
            {
                "Ruins Passage",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Ruined Passage",
                        new List<NewPortal> {
                            new NewPortal("Ruined Passage Not-Door Exit", "Overworld Redux_east"),
                            new NewPortal("Ruined Passage Door Exit", "Overworld Redux_west"),
                        }
                    },
                }
            },
            {
                "EastFiligreeCache",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Southeast Cross Room",
                        new List<NewPortal> {
                            new NewPortal("Southeast HC Room Exit", "Overworld Redux_"),
                        }
                    },
                }
            },
            {
                "Overworld Cave",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Caustic Light Cave",
                        new List<NewPortal> {
                            new NewPortal("Caustic Light Cave Exit", "Overworld Redux_"),
                        }
                    },
                }
            },
            {
                "Maze Room",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Maze Cave",
                        new List<NewPortal> {
                            new NewPortal("Maze Cave Exit", "Overworld Redux_"),
                        }
                    },
                }
            },
            {
                "Town Basement",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Hourglass Cave",
                        new List<NewPortal> {
                            new NewPortal("Hourglass Cave Exit", "Overworld Redux_beach"),
                        }
                    },
                }
            },
            {
                "ShopSpecial",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Special Shop",
                        new List<NewPortal> {
                            new NewPortal("Special Shop Exit", "Overworld Redux_"),
                        }
                    },
                }
            },
            {
                "Temple",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Sealed Temple Rafters",
                        new List<NewPortal> {
                            new NewPortal("Temple Rafters Exit", "Overworld Redux_rafters"),
                        }
                    },
                    {
                        "Sealed Temple",
                        new List<NewPortal> {
                            new NewPortal("Temple Door Exit", "Overworld Redux_main"),
                        }
                    },
                }
            },
            {
                "Sewer",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Beneath the Well Front",
                        new List<NewPortal> {
                            new NewPortal("Well Ladder Exit", "Overworld Redux_entrance"),
                        }
                    },
                    {
                        "Beneath the Well Back",
                        new List<NewPortal> {
                            new NewPortal("Well to Well Boss", "Sewer_Boss_"),
                            new NewPortal("Well Exit towards Furnace", "Overworld Redux_west_aqueduct"),
                        }
                    },
                }
            },
            {
                "Sewer_Boss",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Well Boss",
                        new List<NewPortal> {
                            new NewPortal("Well Boss to Well", "Sewer_"),
                        }
                    },
                    {
                        "Dark Tomb Checkpoint",
                        new List<NewPortal> {
                            new NewPortal("Checkpoint to Dark Tomb", "Crypt Redux_"),
                        }
                    },
                }
            },
            {
                "Crypt Redux",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Dark Tomb Entry Point",
                        new List<NewPortal> {
                            new NewPortal("Dark Tomb to Overworld", "Overworld Redux_"),
                            new NewPortal("Dark Tomb to Checkpoint", "Sewer_Boss_"),
                        }
                    },
                    {
                        "Dark Tomb Dark Exit",
                        new List<NewPortal> {
                            new NewPortal("Dark Tomb to Furnace", "Furnace_"),
                        }
                    },
                }
            },
            {
                "Archipelagos Redux",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "West Garden",
                        new List<NewPortal> {
                            new NewPortal("West Garden Exit near Hero's Grave", "Overworld Redux_lower"),
                            new NewPortal("West Garden to Magic Dagger House", "archipelagos_house_"),
                            new NewPortal("West Garden Shop", "Shop_"),
                        }
                    },
                    {
                        "West Garden after Boss",
                        new List<NewPortal> {
                            new NewPortal("West Garden Exit after Boss", "Overworld Redux_upper"),
                        }
                    },
                    {
                        "West Garden Laurels Exit",
                        new List<NewPortal> {
                            new NewPortal("West Garden Laurels Exit", "Overworld Redux_lowest"),
                        }
                    },
                    {
                        "West Garden Hero's Grave",
                        new List<NewPortal> {
                            new NewPortal("West Garden Hero's Grave", "RelicVoid_teleporter_relic plinth"),
                        }
                    },
                    {
                        "West Garden Portal",
                        new List<NewPortal> {
                            new NewPortal("West Garden to Far Shore", "Transit_teleporter_archipelagos_teleporter"),
                        }
                    },
                }
            },
            {
                "archipelagos_house",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Magic Dagger House",
                        new List<NewPortal> {
                            new NewPortal("Magic Dagger House Exit", "Archipelagos Redux_"),
                        }
                    },
                }
            },
            {
                "Atoll Redux",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Ruined Atoll",
                        new List<NewPortal> {
                            new NewPortal("Atoll Upper Exit", "Overworld Redux_upper"),
                            new NewPortal("Atoll Shop", "Shop_"),
                            new NewPortal("Frog Stairs Eye Entrance", "Frog Stairs_eye"),
                        }
                    },
                    {
                        "Ruined Atoll Lower Entry Area",
                        new List<NewPortal> {
                            new NewPortal("Atoll Lower Exit", "Overworld Redux_lower"),
                        }
                    },
                    {
                        "Ruined Atoll Portal",
                        new List<NewPortal> {
                            new NewPortal("Atoll to Far Shore", "Transit_teleporter_atoll"),
                            new NewPortal("Atoll Statue Teleporter", "Library Exterior_"),
                        }
                    },
                    {
                        "Ruined Atoll Frog Mouth",
                        new List<NewPortal> {
                            new NewPortal("Frog Stairs Mouth Entrance", "Frog Stairs_mouth"),
                        }
                    },
                }
            },
            {
                "Frog Stairs",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Frog's Domain Entry",
                        new List<NewPortal> {
                            new NewPortal("Frog Stairs Eye Exit", "Atoll Redux_eye"),
                            new NewPortal("Frog Stairs Mouth Exit", "Atoll Redux_mouth"),
                            new NewPortal("Frog Stairs to Frog's Domain's Entrance", "frog cave main_Entrance"),
                            new NewPortal("Frog Stairs to Frog's Domain's Exit", "frog cave main_Exit"),
                        }
                    },
                }
            },
            {
                "frog cave main",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Frog's Domain",
                        new List<NewPortal> {
                            new NewPortal("Frog's Domain Ladder Exit", "Frog Stairs_Entrance"),
                        }
                    },
                    {
                        "Frog's Domain Back",
                        new List<NewPortal> {
                            new NewPortal("Frog's Domain Orb Exit", "Frog Stairs_Exit"),
                        }
                    },
                }
            },
            {
                "Library Exterior",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Library Exterior Tree",
                        new List<NewPortal> {
                            new NewPortal("Library Exterior Tree", "Atoll Redux_"),
                        }
                    },
                    {
                        "Library Exterior Ladder",
                        new List<NewPortal> {
                            new NewPortal("Library Exterior Ladder", "Library Hall_"),
                        }
                    },
                }
            },
            {
                "Library Hall",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Library Hall",
                        new List<NewPortal> {
                            new NewPortal("Library Hall Bookshelf Exit", "Library Exterior_"),
                            new NewPortal("Library Hall to Rotunda", "Library Rotunda_"),
                        }
                    },
                    {
                        "Library Hero's Grave",
                        new List<NewPortal> {
                            new NewPortal("Library Hero's Grave", "RelicVoid_teleporter_relic plinth"),
                        }
                    },
                }
            },
            {
                "Library Rotunda",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Library Rotunda",
                        new List<NewPortal> {
                            new NewPortal("Library Rotunda Lower Exit", "Library Hall_"),
                            new NewPortal("Library Rotunda Upper Exit", "Library Lab_"),
                        }
                    },
                }
            },
            {
                "Library Lab",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Library Lab Lower",
                        new List<NewPortal> {
                            new NewPortal("Library Lab to Rotunda", "Library Rotunda_"),
                        }
                    },
                    {
                        "Library Portal",
                        new List<NewPortal> {
                            new NewPortal("Library to Far Shore", "Transit_teleporter_library teleporter"),
                        }
                    },
                    {
                        "Library Lab",
                        new List<NewPortal> {
                            new NewPortal("Library Lab to Librarian Arena", "Library Arena_"),
                        }
                    },
                }
            },
            {
                "Library Arena",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Library Arena",
                        new List<NewPortal> {
                            new NewPortal("Librarian Arena Exit", "Library Lab_"),
                        }
                    },
                }
            },
            {
                "East Forest Redux",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "East Forest",
                        new List<NewPortal> {
                            new NewPortal("Forest to Belltower", "Forest Belltower_"),
                            new NewPortal("Forest Guard House 1 Lower Entrance", "East Forest Redux Laddercave_lower"),
                            new NewPortal("Forest Guard House 1 Gate Entrance", "East Forest Redux Laddercave_gate"),
                            new NewPortal("Forest Guard House 2 Lower Entrance", "East Forest Redux Interior_lower"),
                            new NewPortal("Forest Guard House 2 Upper Entrance", "East Forest Redux Interior_upper"),
                            new NewPortal("Forest Grave Path Lower Entrance", "Sword Access_lower"),
                            new NewPortal("Forest Grave Path Upper Entrance", "Sword Access_upper"),
                        }
                    },
                    {
                        "East Forest Dance Fox Spot",
                        new List<NewPortal> {
                            new NewPortal("Forest Dance Fox Outside Doorway", "East Forest Redux Laddercave_upper"),
                        }
                    },
                    {
                        "East Forest Portal",
                        new List<NewPortal> {
                            new NewPortal("Forest to Far Shore", "Transit_teleporter_forest teleporter"),
                        }
                    },
                }
            },
            {
                "East Forest Redux Laddercave",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Guard House 1 West",
                        new List<NewPortal> {
                            new NewPortal("Guard House 1 Dance Fox Exit", "East Forest Redux_upper"),
                            new NewPortal("Guard House 1 Lower Exit", "East Forest Redux_lower"),
                        }
                    },
                    {
                        "Guard House 1 East",
                        new List<NewPortal> {
                            new NewPortal("Guard House 1 Upper Forest Exit", "East Forest Redux_gate"),
                            new NewPortal("Guard House 1 to Guard Captain Room", "Forest Boss Room_"),
                        }
                    },
                }
            },
            {
                "Sword Access",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Forest Grave Path Upper",
                        new List<NewPortal> {
                            new NewPortal("Forest Grave Path Upper Exit", "East Forest Redux_upper"),
                        }
                    },
                    {
                        "Forest Grave Path Main",
                        new List<NewPortal> {
                            new NewPortal("Forest Grave Path Lower Exit", "East Forest Redux_lower"),
                        }
                    },
                    {
                        "Forest Hero's Grave",
                        new List<NewPortal> {
                            new NewPortal("East Forest Hero's Grave", "RelicVoid_teleporter_relic plinth"),
                        }
                    },
                }
            },
            {
                "East Forest Redux Interior",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Guard House 2",
                        new List<NewPortal> {
                            new NewPortal("Guard House 2 Lower Exit", "East Forest Redux_lower"),
                            new NewPortal("Guard House 2 Upper Exit", "East Forest Redux_upper"),
                        }
                    },
                }
            },
            {
                "Forest Boss Room",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Forest Boss Room",
                        new List<NewPortal> {
                            new NewPortal("Guard Captain Room Non-Gate Exit", "East Forest Redux Laddercave_"),
                            new NewPortal("Guard Captain Room Gate Exit", "Forest Belltower_"),
                        }
                    },
                }
            },
            {
                "Forest Belltower",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Forest Belltower Main",
                        new List<NewPortal> {
                            new NewPortal("Forest Belltower to Fortress", "Fortress Courtyard_"),
                            new NewPortal("Forest Belltower to Overworld", "Overworld Redux_"),
                        }
                    },
                    {
                        "Forest Belltower Lower",
                        new List<NewPortal> {
                            new NewPortal("Forest Belltower to Forest", "East Forest Redux_"),
                        }
                    },
                    {
                        "Forest Belltower Upper",
                        new List<NewPortal> {
                            new NewPortal("Forest Belltower to Guard Captain Room", "Forest Boss Room_"),
                        }
                    },
                }
            },
            {
                "Fortress Courtyard",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Fortress Courtyard",
                        new List<NewPortal> {
                            new NewPortal("Fortress Courtyard to Fortress Grave Path Lower", "Fortress Reliquary_Lower"),
                            new NewPortal("Fortress Courtyard to Fortress Interior", "Fortress Main_Big Door"),
                        }
                    },
                    {
                        "Fortress Courtyard Upper",
                        new List<NewPortal> {
                            new NewPortal("Fortress Courtyard to Fortress Grave Path Upper", "Fortress Reliquary_Upper"),
                            new NewPortal("Fortress Courtyard to East Fortress", "Fortress East_"),
                        }
                    },
                    {
                        "Fortress Exterior near cave",
                        new List<NewPortal> {
                            new NewPortal("Fortress Courtyard to Beneath the Earth", "Fortress Basement_"),
                            new NewPortal("Fortress Courtyard Shop", "Shop_"),
                        }
                    },
                    {
                        "Fortress Exterior from East Forest",
                        new List<NewPortal> {
                            new NewPortal("Fortress Courtyard to Forest Belltower", "Forest Belltower_"),
                        }
                    },
                    {
                        "Fortress Exterior from Overworld",
                        new List<NewPortal> {
                            new NewPortal("Fortress Courtyard to Overworld", "Overworld Redux_"),
                        }
                    },
                }
            },
            {
                "Fortress Basement",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Beneath the Vault Back",
                        new List<NewPortal> {
                            new NewPortal("Beneath the Earth to Fortress Interior", "Fortress Main_"),
                        }
                    },
                    {
                        "Beneath the Vault Front",
                        new List<NewPortal> {
                            new NewPortal("Beneath the Earth to Fortress Courtyard", "Fortress Courtyard_"),
                        }
                    },
                }
            },
            {
                "Fortress Main",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Eastern Vault Fortress",
                        new List<NewPortal> {
                            new NewPortal("Fortress Interior Main Exit", "Fortress Courtyard_Big Door"),
                            new NewPortal("Fortress Interior to Beneath the Earth", "Fortress Basement_"),
                            new NewPortal("Fortress Interior Shop", "Shop_"),
                            new NewPortal("Fortress Interior to East Fortress Upper", "Fortress East_upper"),
                            new NewPortal("Fortress Interior to East Fortress Lower", "Fortress East_lower"),
                        }
                    },
                    {
                        "Eastern Vault Fortress Gold Door",
                        new List<NewPortal> {
                            new NewPortal("Fortress Interior to Siege Engine Arena", "Fortress Arena_"),
                        }
                    },
                }
            },
            {
                "Fortress East",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Fortress East Shortcut Lower",
                        new List<NewPortal> {
                            new NewPortal("East Fortress to Interior Lower", "Fortress Main_lower"),
                        }
                    },
                    {
                        "Fortress East Shortcut Upper",
                        new List<NewPortal> {
                            new NewPortal("East Fortress to Courtyard", "Fortress Courtyard_"),
                            new NewPortal("East Fortress to Interior Upper", "Fortress Main_upper"),
                        }
                    },
                }
            },
            {
                "Fortress Reliquary",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Fortress Grave Path",
                        new List<NewPortal> {
                            new NewPortal("Fortress Grave Path Lower Exit", "Fortress Courtyard_Lower"),
                            new NewPortal("Fortress Hero's Grave", "RelicVoid_teleporter_relic plinth"),
                        }
                    },
                    {
                        "Fortress Grave Path Upper",
                        new List<NewPortal> {
                            new NewPortal("Fortress Grave Path Upper Exit", "Fortress Courtyard_Upper"),
                        }
                    },
                    {
                        "Fortress Grave Path Dusty Entrance",
                        new List<NewPortal> {
                            new NewPortal("Fortress Grave Path Dusty Entrance", "Dusty_"),
                        }
                    },
                }
            },
            {
                "Dusty",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Fortress Leaf Piles",
                        new List<NewPortal> {
                            new NewPortal("Dusty Exit", "Fortress Reliquary_"),
                        }
                    },
                }
            },
            {
                "Fortress Arena",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Fortress Arena",
                        new List<NewPortal> {
                            new NewPortal("Siege Engine Arena to Fortress", "Fortress Main_"),
                        }
                    },
                    {
                        "Fortress Arena Portal",
                        new List<NewPortal> {
                            new NewPortal("Fortress to Far Shore", "Transit_teleporter_spidertank"),
                        }
                    },
                }
            },
            {
                "Mountain",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Lower Mountain Stairs",
                        new List<NewPortal> {
                            new NewPortal("Stairs to Top of the Mountain", "Mountaintop_"),
                        }
                    },
                    {
                        "Lower Mountain",
                        new List<NewPortal> {
                            new NewPortal("Mountain to Quarry", "Quarry Redux_"),
                            new NewPortal("Mountain to Overworld", "Overworld Redux_"),
                        }
                    },
                }
            },
            {
                "Mountaintop",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Top of the Mountain",
                        new List<NewPortal> {
                            new NewPortal("Top of the Mountain Exit", "Mountain_"),
                        }
                    },
                }
            },
            {
                "Darkwoods Tunnel",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Quarry Connector",
                        new List<NewPortal> {
                            new NewPortal("Quarry Connector to Overworld", "Overworld Redux_"),
                            new NewPortal("Quarry Connector to Quarry", "Quarry Redux_"),
                        }
                    },
                }
            },
            {
                "Quarry Redux",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Quarry Entry",
                        new List<NewPortal> {
                            new NewPortal("Quarry to Overworld Exit", "Darkwoods Tunnel_"),
                            new NewPortal("Quarry Shop", "Shop_"),
                        }
                    },
                    {
                        "Quarry Monastery Entry",
                        new List<NewPortal> {
                            new NewPortal("Quarry to Monastery Front", "Monastery_front"),
                        }
                    },
                    {
                        "Monastery Rope",
                        new List<NewPortal> {
                            new NewPortal("Quarry to Monastery Back", "Monastery_back"),
                        }
                    },
                    {
                        "Quarry Back",
                        new List<NewPortal> {
                            new NewPortal("Quarry to Mountain", "Mountain_"),
                        }
                    },
                    {
                        "Lower Quarry Zig Door",
                        new List<NewPortal> {
                            new NewPortal("Quarry to Ziggurat", "ziggurat2020_0_"),
                        }
                    },
                    {
                        "Quarry Portal",
                        new List<NewPortal> {
                            new NewPortal("Quarry to Far Shore", "Transit_teleporter_quarry teleporter"),
                        }
                    },
                }
            },
            {
                "Monastery",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Monastery Back",
                        new List<NewPortal> {
                            new NewPortal("Monastery Rear Exit", "Quarry Redux_back"),
                        }
                    },
                    {
                        "Monastery Front",
                        new List<NewPortal> {
                            new NewPortal("Monastery Front Exit", "Quarry Redux_front"),
                        }
                    },
                    {
                        "Monastery Hero's Grave",
                        new List<NewPortal> {
                            new NewPortal("Monastery Hero's Grave", "RelicVoid_teleporter_relic plinth"),
                        }
                    },
                }
            },
            {
                "ziggurat2020_0",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Rooted Ziggurat Entry",
                        new List<NewPortal> {
                            new NewPortal("Ziggurat Entry Hallway to Ziggurat Upper", "ziggurat2020_1_"),
                            new NewPortal("Ziggurat Entry Hallway to Quarry", "Quarry Redux_"),
                        }
                    },
                }
            },
            {
                "ziggurat2020_1",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Rooted Ziggurat Upper Entry",
                        new List<NewPortal> {
                            new NewPortal("Ziggurat Upper to Ziggurat Entry Hallway", "ziggurat2020_0_"),
                        }
                    },
                    {
                        "Rooted Ziggurat Upper Back",
                        new List<NewPortal> {
                            new NewPortal("Ziggurat Upper to Ziggurat Tower", "ziggurat2020_2_"),
                        }
                    },
                }
            },
            {
                "ziggurat2020_2",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Rooted Ziggurat Middle Top",
                        new List<NewPortal> {
                            new NewPortal("Ziggurat Tower to Ziggurat Upper", "ziggurat2020_1_"),
                        }
                    },
                    {
                        "Rooted Ziggurat Middle Bottom",
                        new List<NewPortal> {
                            new NewPortal("Ziggurat Tower to Ziggurat Lower", "ziggurat2020_3_"),
                        }
                    },
                }
            },
            {
                "ziggurat2020_3",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Rooted Ziggurat Lower Front",
                        new List<NewPortal> {
                            new NewPortal("Ziggurat Lower to Ziggurat Tower", "ziggurat2020_2_"),
                        }
                    },
                    {
                        "Rooted Ziggurat Portal Room Entrance",
                        new List<NewPortal> {
                            new NewPortal("Ziggurat Portal Room Entrance", "ziggurat2020_FTRoom_"),
                        }
                    },
                }
            },
            {
                "ziggurat2020_FTRoom",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Rooted Ziggurat Portal Room Exit",
                        new List<NewPortal> {
                            new NewPortal("Ziggurat Portal Room Exit", "ziggurat2020_3_"),
                        }
                    },
                    {
                        "Rooted Ziggurat Portal",
                        new List<NewPortal> {
                            new NewPortal("Ziggurat to Far Shore", "Transit_teleporter_ziggurat teleporter"),
                        }
                    },
                }
            },
            {
                "Swamp Redux 2",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Swamp",
                        new List<NewPortal> {
                            new NewPortal("Swamp Lower Exit", "Overworld Redux_conduit"),
                            new NewPortal("Swamp Shop", "Shop_"),
                        }
                    },
                    {
                        "Swamp to Cathedral Main Entrance",
                        new List<NewPortal> {
                            new NewPortal("Swamp to Cathedral Main Entrance", "Cathedral Redux_main"),
                        }
                    },
                    {
                        "Swamp to Cathedral Treasure Room",
                        new List<NewPortal> {
                            new NewPortal("Swamp to Cathedral Secret Legend Room Entrance", "Cathedral Redux_secret"),
                        }
                    },
                    {
                        "Back of Swamp",
                        new List<NewPortal> {
                            new NewPortal("Swamp to Gauntlet", "Cathedral Arena_"),
                        }
                    },
                    {
                        "Back of Swamp Laurels Area",
                        new List<NewPortal> {
                            new NewPortal("Swamp Upper Exit", "Overworld Redux_wall"),
                        }
                    },
                    {
                        "Swamp Hero's Grave",
                        new List<NewPortal> {
                            new NewPortal("Swamp Hero's Grave", "RelicVoid_teleporter_relic plinth"),
                        }
                    },
                }
            },
            {
                "Cathedral Redux",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Cathedral",
                        new List<NewPortal> {
                            new NewPortal("Cathedral Main Exit", "Swamp Redux 2_main"),
                            new NewPortal("Cathedral Elevator", "Cathedral Arena_"),
                        }
                    },
                    {
                        "Cathedral Secret Legend Room",
                        new List<NewPortal> {
                            new NewPortal("Cathedral Secret Legend Room Exit", "Swamp Redux 2_secret"),
                        }
                    },
                }
            },
            {
                "Cathedral Arena",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Cathedral Gauntlet Exit",
                        new List<NewPortal> {
                            new NewPortal("Gauntlet to Swamp", "Swamp Redux 2_"),
                        }
                    },
                    {
                        "Cathedral Gauntlet Checkpoint",
                        new List<NewPortal> {
                            new NewPortal("Gauntlet Elevator", "Cathedral Redux_"),
                            new NewPortal("Gauntlet Shop", "Shop_"),
                        }
                    },
                }
            },
            {
                "RelicVoid",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Hero Relic - Fortress",
                        new List<NewPortal> {
                            new NewPortal("Hero's Grave to Fortress", "Fortress Reliquary_teleporter_relic plinth"),
                        }
                    },
                    {
                        "Hero Relic - Quarry",
                        new List<NewPortal> {
                            new NewPortal("Hero's Grave to Monastery", "Monastery_teleporter_relic plinth"),
                        }
                    },
                    {
                        "Hero Relic - West Garden",
                        new List<NewPortal> {
                            new NewPortal("Hero's Grave to West Garden", "Archipelagos Redux_teleporter_relic plinth"),
                        }
                    },
                    {
                        "Hero Relic - East Forest",
                        new List<NewPortal> {
                            new NewPortal("Hero's Grave to East Forest", "Sword Access_teleporter_relic plinth"),
                        }
                    },
                    {
                        "Hero Relic - Library",
                        new List<NewPortal> {
                            new NewPortal("Hero's Grave to Library", "Library Hall_teleporter_relic plinth"),
                        }
                    },
                    {
                        "Hero Relic - Swamp",
                        new List<NewPortal> {
                            new NewPortal("Hero's Grave to Swamp", "Swamp Redux 2_teleporter_relic plinth"),
                        }
                    },
                }
            },
            {
                "Transit",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Far Shore to West Garden",
                        new List<NewPortal> {
                            new NewPortal("Far Shore to West Garden", "Archipelagos Redux_teleporter_archipelagos_teleporter"),
                        }
                    },
                    {
                        "Far Shore to Library",
                        new List<NewPortal> {
                            new NewPortal("Far Shore to Library", "Library Lab_teleporter_library teleporter"),
                        }
                    },
                    {
                        "Far Shore to Quarry",
                        new List<NewPortal> {
                            new NewPortal("Far Shore to Quarry", "Quarry Redux_teleporter_quarry teleporter"),
                        }
                    },
                    {
                        "Far Shore to East Forest",
                        new List<NewPortal> {
                            new NewPortal("Far Shore to East Forest", "East Forest Redux_teleporter_forest teleporter"),
                        }
                    },
                    {
                        "Far Shore to Fortress",
                        new List<NewPortal> {
                            new NewPortal("Far Shore to Fortress", "Fortress Arena_teleporter_spidertank"),
                        }
                    },
                    {
                        "Far Shore",
                        new List<NewPortal> {
                            new NewPortal("Far Shore to Atoll", "Atoll Redux_teleporter_atoll"),
                            new NewPortal("Far Shore to Ziggurat", "ziggurat2020_FTRoom_teleporter_ziggurat teleporter"),
                            new NewPortal("Far Shore to Heir", "Spirit Arena_teleporter_spirit arena"),
                            new NewPortal("Far Shore to Town", "Overworld Redux_teleporter_town"),
                        }
                    },
                    {
                        "Far Shore to Spawn",
                        new List<NewPortal> {
                            new NewPortal("Far Shore to Spawn", "Overworld Redux_teleporter_starting island"),
                        }
                    },
                }
            },
            {
                "Spirit Arena",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Spirit Arena",
                        new List<NewPortal> {
                            new NewPortal("Heir Arena Exit", "Transit_teleporter_spirit arena"),
                        }
                    },
                }
            },
            {
                "Purgatory",
                new Dictionary<string, List<NewPortal>> {
                    {
                        "Purgatory",
                        new List<NewPortal> {
                            new NewPortal("Purgatory Bottom Exit", "Purgatory_bottom"),
                            new NewPortal("Purgatory Top Exit", "Purgatory_top"),
                        }
                    },
                }
            },
        };

        public static Dictionary<string, RegionInfo> RegionDict = new Dictionary<string, RegionInfo> {
            {
                "Overworld",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Holy Cross",
                new RegionInfo("Fake", true)
            },
            {
                "Overworld Belltower",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Swamp Upper Entry",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Special Shop Entry",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld West Garden Laurels Entry",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld to West Garden from Furnace",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Well to Furnace Rail",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Ruined Passage Door",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Old House Door",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Southeast Cross Door",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Fountain Cross Door",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Temple Door",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Town Portal",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Overworld Spawn Portal",
                new RegionInfo("Overworld Redux", false)
            },
            {
                "Stick House",
                new RegionInfo("Sword Cave", true)
            },
            {
                "Windmill",
                new RegionInfo("Windmill", false)
            },
            {
                "Old House Back",
                new RegionInfo("Overworld Interiors", false)
            },
            {
                "Old House Front",
                new RegionInfo("Overworld Interiors", false)
            },
            {
                "Relic Tower",
                new RegionInfo("g_elements", true)
            },
            {
                "Furnace Fuse",
                new RegionInfo("Furnace", false)
            },
            {
                "Furnace Ladder Area",
                new RegionInfo("Furnace", false)
            },
            {
                "Furnace Walking Path",
                new RegionInfo("Furnace", false)
            },
            {
                "Secret Gathering Place",
                new RegionInfo("Waterfall", true)
            },
            {
                "Changing Room",
                new RegionInfo("Changing Room", true)
            },
            {
                "Patrol Cave",
                new RegionInfo("PatrolCave", true)
            },
            {
                "Ruined Shop",
                new RegionInfo("Ruined Shop", true)
            },
            {
                "Ruined Passage",
                new RegionInfo("Ruins Passage", false)
            },
            {
                "Special Shop",
                new RegionInfo("ShopSpecial", true)
            },
            {
                "Caustic Light Cave",
                new RegionInfo("Overworld Cave", true)
            },
            {
                "Maze Cave",
                new RegionInfo("Maze Room", true)
            },
            {
                "Cube Cave",
                new RegionInfo("CubeRoom", true)
            },
            {
                "Southeast Cross Room",
                new RegionInfo("EastFiligreeCache", true)
            },
            {
                "Fountain Cross Room",
                new RegionInfo("Town_FiligreeRoom", true)
            },
            {
                "Hourglass Cave",
                new RegionInfo("Town Basement", true)
            },
            {
                "Sealed Temple",
                new RegionInfo("Temple", false)
            },
            {
                "Sealed Temple Rafters",
                new RegionInfo("Temple", false)
            },
            {
                "Forest Belltower Upper",
                new RegionInfo("Forest Belltower", false)
            },
            {
                "Forest Belltower Main",
                new RegionInfo("Forest Belltower", false)
            },
            {
                "Forest Belltower Lower",
                new RegionInfo("Forest Belltower", false)
            },
            {
                "East Forest",
                new RegionInfo("East Forest Redux", false)
            },
            {
                "East Forest Dance Fox Spot",
                new RegionInfo("East Forest Redux", false)
            },
            {
                "East Forest Portal",
                new RegionInfo("East Forest Redux", false)
            },
            {
                "Guard House 1 East",
                new RegionInfo("East Forest Redux Laddercave", false)
            },
            {
                "Guard House 1 West",
                new RegionInfo("East Forest Redux Laddercave", false)
            },
            {
                "Guard House 2",
                new RegionInfo("East Forest Redux Interior", false)
            },
            {
                "Forest Boss Room",
                new RegionInfo("Forest Boss Room", false)
            },
            {
                "Forest Grave Path Main",
                new RegionInfo("Sword Access", false)
            },
            {
                "Forest Grave Path Upper",
                new RegionInfo("Sword Access", false)
            },
            {
                "Forest Grave Path by Grave",
                new RegionInfo("Sword Access", false)
            },
            {
                "Forest Hero's Grave",
                new RegionInfo("Sword Access", false)
            },
            {
                "Dark Tomb Entry Point",
                new RegionInfo("Crypt Redux", false)
            },
            {
                "Dark Tomb Main",
                new RegionInfo("Crypt Redux", false)
            },
            {
                "Dark Tomb Dark Exit",
                new RegionInfo("Crypt Redux", false)
            },
            {
                "Dark Tomb Checkpoint",
                new RegionInfo("Sewer_Boss", false)
            },
            {
                "Well Boss",
                new RegionInfo("Sewer_Boss", false)
            },
            {
                "Beneath the Well Front",
                new RegionInfo("Sewer", false)
            },
            {
                "Beneath the Well Main",
                new RegionInfo("Sewer", false)
            },
            {
                "Beneath the Well Back",
                new RegionInfo("Sewer", false)
            },
            {
                "West Garden",
                new RegionInfo("Archipelagos Redux", false)
            },
            {
                "West Garden Portal",
                new RegionInfo("Archipelagos Redux", true)
            },
            {
                "West Garden Portal Item",
                new RegionInfo("Archipelagos Redux", true)
            },
            {
                "West Garden Laurels Exit",
                new RegionInfo("Archipelagos Redux", false)
            },
            {
                "West Garden after Boss",
                new RegionInfo("Archipelagos Redux", false)
            },
            {
                "West Garden Hero's Grave",
                new RegionInfo("Archipelagos Redux", false)
            },
            {
                "Magic Dagger House",
                new RegionInfo("archipelagos_house", true)
            },
            {
                "Ruined Atoll",
                new RegionInfo("Atoll Redux", false)
            },
            {
                "Ruined Atoll Lower Entry Area",
                new RegionInfo("Atoll Redux", false)
            },
            {
                "Ruined Atoll Frog Mouth",
                new RegionInfo("Atoll Redux", false)
            },
            {
                "Ruined Atoll Portal",
                new RegionInfo("Atoll Redux", false)
            },
            {
                "Frog's Domain Entry",
                new RegionInfo("Frog Stairs", false)
            },
            {
                "Frog's Domain",
                new RegionInfo("frog cave main", false)
            },
            {
                "Frog's Domain Back",
                new RegionInfo("frog cave main", false)
            },
            {
                "Library Exterior Tree",
                new RegionInfo("Library Exterior", false)
            },
            {
                "Library Exterior Ladder",
                new RegionInfo("Library Exterior", false)
            },
            {
                "Library Hall",
                new RegionInfo("Library Hall", false)
            },
            {
                "Library Hero's Grave",
                new RegionInfo("Library Hall", false)
            },
            {
                "Library Rotunda",
                new RegionInfo("Library Rotunda", false)
            },
            {
                "Library Lab",
                new RegionInfo("Library Lab", false)
            },
            {
                "Library Lab Lower",
                new RegionInfo("Library Lab", false)
            },
            {
                "Library Portal",
                new RegionInfo("Library Lab", false)
            },
            {
                "Library Arena",
                new RegionInfo("Library Arena", true)
            },
            {
                "Fortress Exterior from East Forest",
                new RegionInfo("Fortress Courtyard", false)
            },
            {
                "Fortress Exterior from Overworld",
                new RegionInfo("Fortress Courtyard", false)
            },
            {
                "Fortress Exterior near cave",
                new RegionInfo("Fortress Courtyard", false)
            },
            {
                "Fortress Courtyard",
                new RegionInfo("Fortress Courtyard", false)
            },
            {
                "Fortress Courtyard Upper",
                new RegionInfo("Fortress Courtyard", false)
            },
            {
                "Beneath the Vault Front",
                new RegionInfo("Fortress Basement", false)
            },
            {
                "Beneath the Vault Back",
                new RegionInfo("Fortress Basement", false)
            },
            {
                "Eastern Vault Fortress",
                new RegionInfo("Fortress Main", false)
            },
            {
                "Eastern Vault Fortress Gold Door",
                new RegionInfo("Fortress Main", false)
            },
            {
                "Fortress East Shortcut Upper",
                new RegionInfo("Fortress East", false)
            },
            {
                "Fortress East Shortcut Lower",
                new RegionInfo("Fortress East", false)
            },
            {
                "Fortress Grave Path",
                new RegionInfo("Fortress Reliquary", false)
            },
            {
                "Fortress Grave Path Upper",
                new RegionInfo("Fortress Reliquary", true)
            },
            {
                "Fortress Grave Path Dusty Entrance",
                new RegionInfo("Fortress Reliquary", false)
            },
            {
                "Fortress Hero's Grave",
                new RegionInfo("Fortress Reliquary", false)
            },
            {
                "Fortress Leaf Piles",
                new RegionInfo("Dusty", true)
            },
            {
                "Fortress Arena",
                new RegionInfo("Fortress Arena", false)
            },
            {
                "Fortress Arena Portal",
                new RegionInfo("Fortress Arena", false)
            },
            {
                "Lower Mountain",
                new RegionInfo("Mountain", false)
            },
            {
                "Lower Mountain Stairs",
                new RegionInfo("Mountain", false)
            },
            {
                "Top of the Mountain",
                new RegionInfo("Mountaintop", true)
            },
            {
                "Quarry Connector",
                new RegionInfo("Darkwoods Tunnel", false)
            },
            {
                "Quarry Entry",
                new RegionInfo("Quarry Redux", false)
            },
            {
                "Quarry",
                new RegionInfo("Quarry Redux", false)
            },
            {
                "Quarry Portal",
                new RegionInfo("Quarry Redux", false)
            },
            {
                "Quarry Back",
                new RegionInfo("Quarry Redux", false)
            },
            {
                "Quarry Monastery Entry",
                new RegionInfo("Quarry Redux", false)
            },
            {
                "Monastery Front",
                new RegionInfo("Monastery", false)
            },
            {
                "Monastery Back",
                new RegionInfo("Monastery", false)
            },
            {
                "Monastery Hero's Grave",
                new RegionInfo("Monastery", false)
            },
            {
                "Monastery Rope",
                new RegionInfo("Quarry Redux", false)
            },
            {
                "Lower Quarry",
                new RegionInfo("Quarry Redux", false)
            },
            {
                "Lower Quarry Zig Door",
                new RegionInfo("Quarry Redux", false)
            },
            {
                "Rooted Ziggurat Entry",
                new RegionInfo("ziggurat2020_0", false)
            },
            {
                "Rooted Ziggurat Upper Entry",
                new RegionInfo("ziggurat2020_1", false)
            },
            {
                "Rooted Ziggurat Upper Front",
                new RegionInfo("ziggurat2020_1", false)
            },
            {
                "Rooted Ziggurat Upper Back",
                new RegionInfo("ziggurat2020_1", false)
            },
            {
                "Rooted Ziggurat Middle Top",
                new RegionInfo("ziggurat2020_2", false)
            },
            {
                "Rooted Ziggurat Middle Bottom",
                new RegionInfo("ziggurat2020_2", false)
            },
            {
                "Rooted Ziggurat Lower Front",
                new RegionInfo("ziggurat2020_3", false)
            },
            {
                "Rooted Ziggurat Lower Back",
                new RegionInfo("ziggurat2020_3", false)
            },
            {
                "Rooted Ziggurat Portal Room Entrance",
                new RegionInfo("ziggurat2020_3", false)
            },
            {
                "Rooted Ziggurat Portal",
                new RegionInfo("ziggurat2020_FTRoom", false)
            },
            {
                "Rooted Ziggurat Portal Room Exit",
                new RegionInfo("ziggurat2020_FTRoom", false)
            },
            {
                "Swamp",
                new RegionInfo("Swamp Redux 2", false)
            },
            {
                "Swamp to Cathedral Treasure Room",
                new RegionInfo("Swamp Redux 2", false)
            },
            {
                "Swamp to Cathedral Main Entrance",
                new RegionInfo("Swamp Redux 2", false)
            },
            {
                "Back of Swamp",
                new RegionInfo("Swamp Redux 2", false)
            },
            {
                "Swamp Hero's Grave",
                new RegionInfo("Swamp Redux 2", false)
            },
            {
                "Back of Swamp Laurels Area",
                new RegionInfo("Swamp Redux 2", false)
            },
            {
                "Cathedral",
                new RegionInfo("Cathedral Redux", false)
            },
            {
                "Cathedral Secret Legend Room",
                new RegionInfo("Cathedral Redux", true)
            },
            {
                "Cathedral Gauntlet Checkpoint",
                new RegionInfo("Cathedral Arena", false)
            },
            {
                "Cathedral Gauntlet",
                new RegionInfo("Cathedral Arena", false)
            },
            {
                "Cathedral Gauntlet Exit",
                new RegionInfo("Cathedral Arena", false)
            },
            {
                "Far Shore",
                new RegionInfo("Transit", false)
            },
            {
                "Far Shore to Spawn",
                new RegionInfo("Transit", false)
            },
            {
                "Far Shore to East Forest",
                new RegionInfo("Transit", false)
            },
            {
                "Far Shore to Quarry",
                new RegionInfo("Transit", false)
            },
            {
                "Far Shore to Fortress",
                new RegionInfo("Transit", false)
            },
            {
                "Far Shore to Library",
                new RegionInfo("Transit", false)
            },
            {
                "Far Shore to West Garden",
                new RegionInfo("Transit", false)
            },
            {
                "Hero Relic - Fortress",
                new RegionInfo("RelicVoid", true)
            },
            {
                "Hero Relic - Quarry",
                new RegionInfo("RelicVoid", true)
            },
            {
                "Hero Relic - West Garden",
                new RegionInfo("RelicVoid", true)
            },
            {
                "Hero Relic - East Forest",
                new RegionInfo("RelicVoid", true)
            },
            {
                "Hero Relic - Library",
                new RegionInfo("RelicVoid", true)
            },
            {
                "Hero Relic - Swamp",
                new RegionInfo("RelicVoid", true)
            },
            {
                "Purgatory",
                new RegionInfo("Purgatory", false)
            },
            {
                "Shop Entrance 1",
                new RegionInfo("Shop", true)
            },
            {
                "Shop Entrance 2",
                new RegionInfo("Shop", true)
            },
            {
                "Shop Entrance 3",
                new RegionInfo("Shop", true)
            },
            {
                "Shop Entrance 4",
                new RegionInfo("Shop", true)
            },
            {
                "Shop Entrance 5",
                new RegionInfo("Shop", true)
            },
            {
                "Shop Entrance 6",
                new RegionInfo("Shop", true)
            },
            {
                "Shop",
                new RegionInfo("Shop", true)
            },
            {
                "Spirit Arena",
                new RegionInfo("Spirit Arena", true)
            },
            {
                "Spirit Arena Victory",
                new RegionInfo("Spirit Arena", true)
            },
        };

        public static Dictionary<string, Dictionary<string, List<List<string>>>> TraversalReqs = new Dictionary<string, Dictionary<string, List<List<string>>>> {
            {
                "Overworld",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Overworld Belltower",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "Overworld Swamp Upper Entry",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "Overworld Special Shop Entry",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "Overworld West Garden Laurels Entry",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "Overworld Southeast Cross Door",
                        new List<List<string>> {
                            new List<string> {
                                "21",
                            },
                        }
                    },
                    {
                        "Overworld Ruined Passage Door",
                        new List<List<string>> {
                            new List<string> {
                                "Key",
                            },
                            new List<string> {
                                "Hyperdash", "nmg",
                            },
                        }
                    },
                    {
                        "Overworld Temple Door",
                        new List<List<string>> {
                            new List<string> {
                                "26", "Techbow", "Wand", "Stundagger", "nmg",
                            },
                            new List<string> {
                                "Techbow", "Forest Belltower Upper", "nmg",
                            },
                            new List<string> {
                                "Stick", "Forest Belltower Upper", "Overworld Belltower",
                            },
                            new List<string> {
                                "Techbow", "Forest Belltower Upper", "Overworld Belltower",
                            },
                        }
                    },
                    {
                        "Overworld Fountain Cross Door",
                        new List<List<string>> {
                            new List<string> {
                                "21",
                            },
                            new List<string> {
                                "26", "Techbow", "Wand", "Stundagger", "nmg",
                            },
                        }
                    },
                    {
                        "Overworld Town Portal",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                    {
                        "Overworld Spawn Portal",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                    {
                        "Overworld Well to Furnace Rail",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "Overworld Old House Door",
                        new List<List<string>> {
                            new List<string> {
                                "Key (House)",
                            },
                            new List<string> {
                                "Stundagger", "Wand", "nmg",
                            },
                        }
                    },
                }
            },
            {
                "Old House Front",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Old House Back",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Old House Back",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Old House Front",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash", "nmg",
                            },
                        }
                    },
                }
            },
            {
                "Furnace Fuse",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Furnace Ladder Area",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "Furnace Ladder Area",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Furnace Fuse",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "Furnace Walking Path",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                }
            },
            {
                "Furnace Walking Path",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Furnace Ladder Area",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "Sealed Temple",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Sealed Temple Rafters",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Sealed Temple Rafters",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Sealed Temple",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "East Forest",
                new Dictionary<string, List<List<string>>> {
                    {
                        "East Forest Dance Fox Spot",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "26", "nmg",
                            },
                        }
                    },
                    {
                        "East Forest Portal",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                }
            },
            {
                "East Forest Dance Fox Spot",
                new Dictionary<string, List<List<string>>> {
                    {
                        "East Forest",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "26", "nmg",
                            },
                        }
                    },
                }
            },
            {
                "East Forest Portal",
                new Dictionary<string, List<List<string>>> {
                    {
                        "East Forest",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Forest Grave Path Main",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Forest Grave Path Upper",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                    {
                        "Forest Grave Path by Grave",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Forest Grave Path Upper",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Forest Grave Path Main",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "26", "nmg",
                            },
                        }
                    },
                }
            },
            {
                "Forest Grave Path by Grave",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Forest Hero's Grave",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                    {
                        "Forest Grave Path Main",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash", "nmg",
                            },
                        }
                    },
                }
            },
            {
                "Forest Hero's Grave",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Forest Grave Path by Grave",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Beneath the Well Front",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Beneath the Well Main",
                        new List<List<string>> {
                            new List<string> {
                                "Stick",
                            },
                            new List<string> {
                                "Techbow",
                            },
                        }
                    },
                }
            },
            {
                "Beneath the Well Back",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Beneath the Well Main",
                        new List<List<string>> {
                            new List<string> {
                                "Stick",
                            },
                            new List<string> {
                                "Techbow",
                            },
                        }
                    },
                }
            },
            {
                "Beneath the Well Main",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Beneath the Well Front",
                        new List<List<string>> {
                        }
                    },
                    {
                        "Beneath the Well Back",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Well Boss",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Dark Tomb Checkpoint",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Dark Tomb Checkpoint",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Well Boss",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash", "nmg",
                            },
                        }
                    },
                }
            },
            {
                "Dark Tomb Entry Point",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Dark Tomb Main",
                        new List<List<string>> {
                            new List<string> {
                                "Lantern",
                            },
                        }
                    },
                }
            },
            {
                "Dark Tomb Main",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Dark Tomb Dark Exit",
                        new List<List<string>> {
                        }
                    },
                    {
                        "Dark Tomb Entry Point",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Dark Tomb Dark Exit",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Dark Tomb Main",
                        new List<List<string>> {
                            new List<string> {
                                "Lantern",
                            },
                        }
                    },
                }
            },
            {
                "West Garden",
                new Dictionary<string, List<List<string>>> {
                    {
                        "West Garden Laurels Exit",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "West Garden After Boss",
                        new List<List<string>> {
                            new List<string> {
                                "Sword",
                            },
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "West Garden Hero's Grave",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                    {
                        "West Garden Portal Item",
                        new List<List<string>> {
                            new List<string> {
                                "26", "Wand", "Stundagger", "Techbow", "nmg",
                            },
                        }
                    },
                }
            },
            {
                "West Garden Laurels Exit",
                new Dictionary<string, List<List<string>>> {
                    {
                        "West Garden",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "West Garden After Boss",
                new Dictionary<string, List<List<string>>> {
                    {
                        "West Garden",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "West Garden Portal Item",
                new Dictionary<string, List<List<string>>> {
                    {
                        "West Garden",
                        new List<List<string>> {
                            new List<string> {
                                "26", "Wand", "Stundagger", "Techbow", "nmg",
                            },
                        }
                    },
                    {
                        "West Garden Portal",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash", "12", "West Garden",
                            },
                        }
                    },
                }
            },
            {
                "West Garden Portal",
                new Dictionary<string, List<List<string>>> {
                    {
                        "West Garden Portal Item",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "West Garden Hero's Grave",
                new Dictionary<string, List<List<string>>> {
                    {
                        "West Garden",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Ruined Atoll",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Ruined Atoll Lower Entry Area",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "Ruined Atoll Frog Mouth",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Wand",
                            },
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "Ruined Atoll Portal",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                }
            },
            {
                "Ruined Atoll Lower Entry Area",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Ruined Atoll",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Wand",
                            },
                        }
                    },
                }
            },
            {
                "Ruined Atoll Frog Mouth",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Ruined Atoll",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Wand",
                            },
                        }
                    },
                }
            },
            {
                "Ruined Atoll Portal",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Ruined Atoll",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Frog's Domain",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Frog's Domain Back",
                        new List<List<string>> {
                            new List<string> {
                                "Wand",
                            },
                        }
                    },
                }
            },
            {
                "Library Exterior Ladder",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Exterior Tree",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash", "12",
                            },
                            new List<string> {
                                "Wand", "12",
                            },
                        }
                    },
                }
            },
            {
                "Library Exterior Tree",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Exterior Ladder",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Wand",
                            },
                        }
                    },
                }
            },
            {
                "Library Hall",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Hero's Grave",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                }
            },
            {
                "Library Hero's Grave",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Hall",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Library Lab Lower",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Lab",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Wand",
                            },
                        }
                    },
                }
            },
            {
                "Library Lab",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Lab Lower",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                    {
                        "Library Portal",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                }
            },
            {
                "Library Portal",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Library Lab",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Fortress Exterior from East Forest",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress Exterior from Overworld",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Wand",
                            },
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "Fortress Courtyard Upper",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "Fortress Exterior near cave",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "Fortress Courtyard",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                }
            },
            {
                "Fortress Exterior from Overworld",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress Exterior from East Forest",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                    {
                        "Fortress Exterior near cave",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "Fortress Courtyard",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "26", "Techbow", "Wand", "Stundagger", "nmg",
                            },
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                }
            },
            {
                "Fortress Exterior near cave",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress Exterior from Overworld",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "Fortress Courtyard",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "Fortress Courtyard Upper",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                }
            },
            {
                "Fortress Courtyard",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress Courtyard Upper",
                        new List<List<string>> {
                            new List<string> {
                                "26", "Techbow", "Wand", "Stundagger", "nmg",
                            },
                        }
                    },
                    {
                        "Fortress Exterior from Overworld",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "Fortress Courtyard Upper",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress Courtyard",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Beneath the Vault Front",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Beneath the Vault Back",
                        new List<List<string>> {
                            new List<string> {
                                "Lantern",
                            },
                        }
                    },
                }
            },
            {
                "Beneath the Vault Back",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Beneath the Vault Front",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Fortress East Shortcut Lower",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress East Shortcut Upper",
                        new List<List<string>> {
                            new List<string> {
                                "26", "Techbow", "Wand", "Stundagger", "nmg",
                            },
                        }
                    },
                }
            },
            {
                "Fortress East Shortcut Upper",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress East Shortcut Lower",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Eastern Vault Fortress",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Eastern Vault Fortress Gold Door",
                        new List<List<string>> {
                            new List<string> {
                                "Wand", "Stundagger", "nmg",
                            },
                            new List<string> {
                                "12", "Fortress Exterior from Overworld", "Beneath the Vault Back", "Fortress Courtyard Upper",
                            },
                        }
                    },
                }
            },
            {
                "Eastern Vault Fortress Gold Door",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Eastern Vault Fortress",
                        new List<List<string>> {
                            new List<string> {
                                "Wand", "Stundagger", "nmg",
                            },
                        }
                    },
                }
            },
            {
                "Fortress Grave Path",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress Hero's Grave",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                    {
                        "Fortress Grave Path Dusty Entrance",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "Fortress Grave Path Upper",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress Grave Path",
                        new List<List<string>> {
                            new List<string> {
                                "26", "Stundagger", "Techbow", "Wand", "nmg",
                            },
                        }
                    },
                }
            },
            {
                "Fortress Grave Path Dusty Entrance",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress Grave Path",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "Fortress Hero's Grave",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress Grave Path",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Fortress Arena",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress Arena Portal",
                        new List<List<string>> {
                            new List<string> {
                                "12", "Fortress Exterior from Overworld", "Beneath the Vault Back", "Eastern Vault Fortress",
                            },
                        }
                    },
                }
            },
            {
                "Fortress Arena Portal",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Fortress Arena",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Lower Mountain",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Lower Mountain Stairs",
                        new List<List<string>> {
                            new List<string> {
                                "21",
                            },
                        }
                    },
                }
            },
            {
                "Lower Mountain Stairs",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Lower Mountain",
                        new List<List<string>> {
                            new List<string> {
                                "21",
                            },
                        }
                    },
                }
            },
            {
                "Monastery Back",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Monastery Front",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash", "nmg",
                            },
                        }
                    },
                    {
                        "Monastery Hero's Grave",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                }
            },
            {
                "Monastery Hero's Grave",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Monastery Back",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Monastery Front",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Monastery Back",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Quarry Entry",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Quarry Portal",
                        new List<List<string>> {
                            new List<string> {
                                "12", "Quarry Connector",
                            },
                        }
                    },
                    {
                        "Quarry",
                        new List<List<string>> {
                            new List<string> {
                                "Techbow",
                            },
                            new List<string> {
                                "Sword",
                            },
                        }
                    },
                }
            },
            {
                "Quarry Monastery Entry",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Quarry",
                        new List<List<string>> {
                            new List<string> {
                                "Techbow",
                            },
                            new List<string> {
                                "Sword",
                            },
                        }
                    },
                    {
                        "Quarry Back",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                    {
                        "Monastery Rope",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                }
            },
            {
                "Quarry Back",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Quarry",
                        new List<List<string>> {
                            new List<string> {
                                "Techbow",
                            },
                            new List<string> {
                                "Sword",
                            },
                        }
                    },
                    {
                        "Quarry Monastery Entry",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "Quarry",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Lower Quarry",
                        new List<List<string>> {
                            new List<string> {
                                "Mask",
                            },
                        }
                    },
                    {
                        "Quarry Entry",
                        new List<List<string>> {
                        }
                    },
                    {
                        "Quarry Back",
                        new List<List<string>> {
                        }
                    },
                    {
                        "Quarry Monastery Entry",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Monastery Rope",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Quarry Back",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Rooted Ziggurat Upper Entry",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Rooted Ziggurat Upper Front",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Rooted Ziggurat Upper Front",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Rooted Ziggurat Upper Back",
                        new List<List<string>> {
                            new List<string> {
                                "Sword",
                            },
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "Rooted Ziggurat Upper Back",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Rooted Ziggurat Upper Front",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "Rooted Ziggurat Middle Top",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Rooted Ziggurat Middle Bottom",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Rooted Ziggurat Lower Front",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Rooted Ziggurat Lower Back",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Sword", "12",
                            },
                        }
                    },
                }
            },
            {
                "Rooted Ziggurat Lower Back",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Rooted Ziggurat Lower Front",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "Rooted Ziggurat Portal Room Entrance",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                }
            },
            {
                "Rooted Ziggurat Portal Room Entrance",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Rooted Ziggurat Lower Back",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Rooted Ziggurat Portal Room Exit",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Rooted Ziggurat Portal",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                }
            },
            {
                "Rooted Ziggurat Portal",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Rooted Ziggurat Portal Room Exit",
                        new List<List<string>> {
                            new List<string> {
                                "12", "Rooted Ziggurat Lower Back",
                            },
                        }
                    },
                }
            },
            {
                "Swamp",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Swamp to Cathedral Main Entrance",
                        new List<List<string>> {
                            new List<string> {
                                "12", "Hyperdash",
                            },
                            new List<string> {
                                "Stundagger", "Wand", "nmg",
                            },
                        }
                    },
                    {
                        "Swamp to Cathedral Treasure Room",
                        new List<List<string>> {
                            new List<string> {
                                "21",
                            },
                        }
                    },
                    {
                        "Back of Swamp",
                        new List<List<string>> {
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                }
            },
            {
                "Swamp to Cathedral Treasure Room Entrance",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Swamp",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Swamp to Cathedral Main Entrance",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Swamp",
                        new List<List<string>> {
                            new List<string> {
                                "Stundagger", "Wand", "nmg",
                            },
                        }
                    },
                }
            },
            {
                "Back of Swamp",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Back of Swamp Laurels Area",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                            new List<string> {
                                "Ladder Storage",
                            },
                        }
                    },
                    {
                        "Swamp Hero's Grave",
                        new List<List<string>> {
                            new List<string> {
                                "12",
                            },
                        }
                    },
                }
            },
            {
                "Back of Swamp Laurels Area",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Back of Swamp",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                    {
                        "Swamp",
                        new List<List<string>> {
                            new List<string> {
                                "26", "Wand", "Techbow", "Stundagger", "nmg",
                            },
                        }
                    },
                }
            },
            {
                "Swamp Hero's Grave",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Back of Swamp",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Cathedral Gauntlet Checkpoint",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Cathedral Gauntlet",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Cathedral Gauntlet",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Cathedral Gauntlet Exit",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "Cathedral Gauntlet Exit",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Cathedral Gauntlet",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "Far Shore",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Far Shore to Spawn",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                    {
                        "Far Shore to East Forest",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                    {
                        "Far Shore to Quarry",
                        new List<List<string>> {
                            new List<string> {
                                "12", "Quarry Connector", "Quarry",
                            },
                        }
                    },
                    {
                        "Far Shore to Library",
                        new List<List<string>> {
                            new List<string> {
                                "12", "Library Lab",
                            },
                        }
                    },
                    {
                        "Far Shore to West Garden",
                        new List<List<string>> {
                            new List<string> {
                                "12", "West Garden",
                            },
                        }
                    },
                    {
                        "Far Shore to Fortress",
                        new List<List<string>> {
                            new List<string> {
                                "12", "Fortress Exterior from Overworld", "Beneath the Vault Back", "Eastern Vault Fortress",
                            },
                        }
                    },
                }
            },
            {
                "Far Shore to Spawn",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Far Shore",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "Far Shore to East Forest",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Far Shore",
                        new List<List<string>> {
                            new List<string> {
                                "Hyperdash",
                            },
                        }
                    },
                }
            },
            {
                "Far Shore to Quarry",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Far Shore",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Far Shore to Library",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Far Shore",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Far Shore to West Garden",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Far Shore",
                        new List<List<string>> {
                        }
                    },
                }
            },
            {
                "Far Shore to Fortress",
                new Dictionary<string, List<List<string>>> {
                    {
                        "Far Shore",
                        new List<List<string>> {
                        }
                    },
                }
            },

        };

        // this is a big list of every portal in the game, along with their access requirements
        // a portal without access requirements just means you can get to the center of the region from that portal and vice versa
        public static Dictionary<string, List<TunicPortal>> PortalList = new Dictionary<string, List<TunicPortal>>
        {
            {
                "Overworld Redux",
                new List<TunicPortal> {
                    new TunicPortal("Sword Cave", "", "Stick House Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Windmill", "", "Windmill Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Sewer", "entrance", "Well Ladder Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Sewer", "west_aqueduct", "Entrance to Well from Well Rail", granularRegion: "Overworld Well to Furnace Rail", ignoreScene: true, givesAccess: new List<string> { "Overworld Redux, Furnace_gyro_upper_north" }, requiredItems: new Dictionary<string, int> { { "Overworld Redux, Furnace_gyro_upper_north", 1 } }),
                    new TunicPortal("Overworld Interiors", "house", "Old House Entry Door", granularRegion: "Overworld Not First Steps", requiredItems: new Dictionary<string, int> { {"Key (House)", 1} }), // make this match actual item name
                    new TunicPortal("Overworld Interiors", "under_checkpoint", "Old House Waterfall Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Furnace", "gyro_upper_north", "Entrance to Furnace from Well Rail", granularRegion: "Overworld Well to Furnace Rail", ignoreScene: true, givesAccess: new List<string> { "Overworld Redux, Sewer_west_aqueduct" }, requiredItems: new Dictionary<string, int> { { "Overworld Redux, Sewer_west_aqueduct", 1 } }),
                    new TunicPortal("Furnace", "gyro_upper_east", "Entrance to Furnace under Windmill", granularRegion: "Overworld"),
                    new TunicPortal("Furnace", "gyro_west", "Entrance to Furnace near West Garden", granularRegion: "Overworld Not First Steps", ignoreScene: true, givesAccess: new List<string> {"Overworld Redux, Archipelagos Redux_lower"}, requiredItems: new Dictionary<string, int> { { "Overworld Redux, Archipelagos Redux_lower", 1 } }),
                    new TunicPortal("Furnace", "gyro_lower", "Entrance to Furnace from Beach", granularRegion: "Overworld"),
                    new TunicPortal("Overworld Cave", "", "Caustic Light Cave Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Swamp Redux 2", "wall", "Swamp Upper Entrance", granularRegion: "Overworld Not First Steps", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1} }),
                    new TunicPortal("Swamp Redux 2", "conduit", "Swamp Lower Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Ruins Passage", "east", "Ruined Passage Not-Door Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Ruins Passage", "west", "Ruined Passage Door Entrance", granularRegion: "Overworld Not First Steps", requiredItems: new Dictionary<string, int> { { "Key", 2 } }), // and access to any overworld portal, but we start in overworld so no need to put it here
                    new TunicPortal("Atoll Redux", "upper", "Atoll Upper Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Atoll Redux", "lower", "Atoll Lower Entrance", granularRegion: "Overworld"),
                    new TunicPortal("ShopSpecial", "", "Special Shop Entrance", granularRegion: "Overworld Not First Steps", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1} }),
                    new TunicPortal("Maze Room", "", "Maze Cave Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Archipelagos Redux", "upper", "West Garden Entrance near Belltower", granularRegion: "Overworld Not First Steps", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 } }),
                    new TunicPortal("Archipelagos Redux", "lower", "West Garden Entrance from Furnace", granularRegion: "Overworld Not First Steps", ignoreScene: true, givesAccess: new List<string> {"Overworld Redux, Furnace_gyro_west"}, requiredItems: new Dictionary<string, int> {{"Overworld Redux, Furnace_gyro_west", 1}}),
                    new TunicPortal("Archipelagos Redux", "lowest", "West Garden Laurels Entrance", granularRegion: "Overworld Not First Steps", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 } }),
                    new TunicPortal("Temple", "main", "Temple Door Entrance", granularRegion: "Overworld Not First Steps", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Forest Belltower, Forest Boss Room_", 1 }, { "Overworld Redux, Archipelagos Redux_upper", 1 }, { "Stick", 1 } }, new Dictionary<string, int> { { "Forest Belltower, Forest Boss Room_", 1 }, { "Overworld Redux, Archipelagos Redux_upper", 1 }, { "Techbow", 1 } }, new Dictionary<string, int> { { "Forest Belltower, Forest Boss Room_", 1 }, { "Hyperdash", 1 }, { "Stick", 1 } }, new Dictionary<string, int> { { "Forest Belltower, Forest Boss Room_", 1 }, { "Hyperdash", 1 }, { "Techbow", 1 } } }),
                    new TunicPortal("Temple", "rafters", "Temple Rafters Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Ruined Shop", "", "Ruined Shop Entrance", granularRegion: "Overworld"),
                    new TunicPortal("PatrolCave", "", "Patrol Cave Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Town Basement", "beach", "Hourglass Cave Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Changing Room", "", "Changing Room Entrance", granularRegion: "Overworld"),
                    new TunicPortal("CubeRoom", "", "Cube Cave Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Mountain", "", "Stairs from Overworld to Mountain", granularRegion: "Overworld"),
                    new TunicPortal("Fortress Courtyard", "", "Overworld to Fortress", granularRegion: "Overworld"),
                    new TunicPortal("Town_FiligreeRoom", "", "Fountain HC Door Entrance", granularRegion: "Overworld Ability", entryItems: new Dictionary<string, int> { { "21", 1 } }), // this is entry items because when you exit from this portal, you end up in front of the door
                    new TunicPortal("EastFiligreeCache", "", "Southeast HC Door Entrance", granularRegion: "Overworld Ability", requiredItems: new Dictionary<string, int> { { "21", 1 } }), // this is required items because when you exit from this portal, you end up behind the door
                    new TunicPortal("Darkwoods Tunnel", "", "Overworld to Quarry Connector", granularRegion: "Overworld"),
                    new TunicPortal("Crypt Redux", "", "Dark Tomb Main Entrance", granularRegion: "Overworld"),
                    new TunicPortal("Forest Belltower", "", "Overworld to Forest Belltower", granularRegion: "Overworld"),
                    new TunicPortal("Transit", "teleporter_town", "Town to Far Shore", granularRegion: "Overworld Ability", prayerPortal: true),
                    new TunicPortal("Transit", "teleporter_starting island", "Spawn to Far Shore", granularRegion: "Overworld Ability", prayerPortal: true),
                    new TunicPortal("Waterfall", "", "Secret Gathering Place Entrance", granularRegion: "Overworld"),

                    // new TunicPortal("_", "", "Portal"), // ?
                    // new TunicPortal("Forest Belltower_", "showfloordemo2022", "Portal (12)"), // ?
                    // new TunicPortal("DEMO_altEnd_", "", "_Portal (Secret Demo End)"), // ?
                }
            },
            {
                "Waterfall", // fairy cave
                new List<TunicPortal> {
                    new TunicPortal("Overworld Redux", "", "Secret Gathering Place Exit", granularRegion: "Waterfall", deadEnd: true),
                }
            },
            {
                "Windmill",
                new List<TunicPortal> {
                    new TunicPortal("Overworld Redux", "", "Windmill Exit", granularRegion: "Windmill"),
                    new TunicPortal("Shop", "", "Windmill Shop", granularRegion: "Windmill"),
                }
            },
            {
                "Overworld Interiors", // House in town
                new List<TunicPortal> {
                    new TunicPortal("Overworld Redux", "house", "Old House Door Exit", granularRegion: "Old House Front"),
                    new TunicPortal("g_elements", "", "Old House to Glyph Tower", granularRegion: "Old House Front"),
                    new TunicPortal("Overworld Redux", "under_checkpoint", "Old House Waterfall Exit", granularRegion: "Old House Back", ignoreScene: true, requiredItems: new Dictionary<string, int> { { "Overworld Interiors, Overworld Redux_house", 1 } }), // since you get access to the center of a region from either portal, only one of these two is actually needed

                    // new TunicPortal("Archipelagos Redux_", "", "_ShowfloorDemo2022 Portal"), // unused and disabled
                }
            },
            {
                "g_elements", // Relic tower
                new List<TunicPortal> {
                    new TunicPortal("Overworld Interiors", "", "Glyph Tower Exit", granularRegion: "g_elements", deadEnd: true),
                }
            },
            {
                "Changing Room",
                new List<TunicPortal> {
                    new TunicPortal("Overworld Redux", "", "Changing Room Exit", granularRegion: "Changing Room", deadEnd: true),
                }
            },
            {
                "Town_FiligreeRoom", // the one next to the fountain
                new List<TunicPortal> {
                    new TunicPortal("Overworld Redux", "", "Fountain HC Room Exit", granularRegion: "Town_FiligreeRoom", deadEnd: true),
                }
            },
            {
                "CubeRoom",
                new List<TunicPortal> {
                    new TunicPortal("Overworld Redux", "", "Cube Cave Exit", granularRegion: "CubeRoom", deadEnd: true),
                }
            },
            {
                "PatrolCave",
                new List<TunicPortal> {
                    new TunicPortal("Overworld Redux", "", "Guard Patrol Cave Exit", granularRegion: "PatrolCave", deadEnd: true),
                }
            },
            {
                "Ruined Shop",
                new List<TunicPortal> {
                    new TunicPortal("Overworld Redux", "", "Ruined Shop Exit", granularRegion: "Ruined Shop", deadEnd: true),
                }
            },
            {
                "Furnace", // Under the west belltower
                // I'm calling the "center" of this region the space accessible by the windmill and beach
                new List<TunicPortal> {
                    new TunicPortal("Overworld Redux", "gyro_upper_north", "Furnace Exit towards Well", granularRegion: "Furnace", requiredItems: new Dictionary<string, int> { {"Hyperdash", 1} }),
                    new TunicPortal("Crypt Redux", "", "Furnace Exit to Dark Tomb", granularRegion: "Furnace", requiredItems: new Dictionary<string, int> { {"Hyperdash", 1} }, givesAccess: new List<string> {"Furnace, Overworld Redux_gyro_west"}),
                    new TunicPortal("Overworld Redux", "gyro_west", "Furnace Exit towards West Garden", granularRegion: "Furnace", requiredItems : new Dictionary<string, int> { {"Hyperdash", 1} }, givesAccess : new List<string> {"Furnace, Crypt Redux_"}),
                    new TunicPortal("Overworld Redux", "gyro_lower", "Furnace Exit to Beach", granularRegion: "Furnace"),
                    new TunicPortal("Overworld Redux", "gyro_upper_east", "Furnace Exit under Windmill", granularRegion: "Furnace"),
                }
            },
            {
                "Sword Cave", // Stick house
                new List<TunicPortal> {
                    new TunicPortal("Overworld Redux", "", "Stick House Exit", granularRegion: "Sword Cave", deadEnd: true),
                }
            },
            {
                "Ruins Passage", // That little hallway with the key door near the start in Overworld
                new List<TunicPortal> {
                    new TunicPortal("Overworld Redux", "east", "Ruined Passage Not-Door Exit", granularRegion: "Ruined Passage"),
                    new TunicPortal("Overworld Redux", "west", "Ruined Passage Door Exit", granularRegion: "Ruined Passage"),
                }
            },
            {
                "EastFiligreeCache", // The holy cross room with the 3 chests near swamp entrance
                new List<TunicPortal> {
                    new TunicPortal("Overworld Redux", "", "Southeast HC Room Exit", granularRegion: "EastFiligreeCache", deadEnd: true),
                }
            },
            {
                "Overworld Cave", // East beach, next to swamp entrance, rotating lights room
                new List<TunicPortal> {
                    new TunicPortal("Overworld Redux", "", "Caustic Light Cave Exit", granularRegion: "Overworld Cave", deadEnd: true),
                }
            },
            {
                "Maze Room", // Invisible maze
                new List<TunicPortal> {
                    new TunicPortal("Overworld Redux", "", "Maze Cave Exit", granularRegion: "Maze Room", deadEnd: true),
                }
            },
            {
                "Town Basement", // Hourglass cave
                new List<TunicPortal> {
                    new TunicPortal("Overworld Redux", "beach", "Hourglass Cave Exit", granularRegion: "Town Basement", deadEnd: true), // yes, it has a tag even though it doesn't need one
                }
            },
            {
                "ShopSpecial", // Special shop, laurel across from that platform between east forest and fortress
                new List<TunicPortal> {
                    new TunicPortal("Overworld Redux", "", "Special Shop Exit", granularRegion: "ShopSpecial", deadEnd: true),
                }
            },
            {
                "Temple", // Where you put the hexes
                new List<TunicPortal> {
                    new TunicPortal("Overworld Redux", "rafters", "Temple Rafters Exit", granularRegion: "Temple"),
                    new TunicPortal("Overworld Redux", "main", "Temple Door Exit", granularRegion: "Temple"),
                }
            },
            {
                "Sewer", // Beneath the Well
                new List<TunicPortal> {
                    new TunicPortal("Overworld Redux", "entrance", "Well Ladder Exit", granularRegion: "Sewer"),
                    new TunicPortal("Sewer_Boss", "", "Well to Well Boss", granularRegion: "Sewer"),
                    new TunicPortal("Overworld Redux", "west_aqueduct", "Well Exit towards Furnace", granularRegion: "Sewer"),
                }
            },
            {
                "Sewer_Boss", // Boss room in the Beneath the Well
                new List<TunicPortal> {
                    new TunicPortal("Sewer", "", "Well Boss to Well", granularRegion: "Sewer_Boss"),
                    new TunicPortal("Crypt Redux", "", "Checkpoint to Dark Tomb", granularRegion: "Sewer_Boss", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Sewer_Boss, Sewer_", 1 } }, new Dictionary<string, int> { { "Hyperdash", 1 } } }),
                }
            },
            {
                "Crypt Redux",
                new List<TunicPortal> {
                    new TunicPortal("Overworld Redux", "", "Dark Tomb to Overworld", granularRegion: "Crypt Redux"),
                    new TunicPortal("Furnace", "", "Dark Tomb to Furnace", granularRegion: "Crypt Redux", requiredItems: new Dictionary<string, int> { {"Lantern", 1} }),
                    new TunicPortal("Sewer_Boss", "", "Dark Tomb to Checkpoint", granularRegion: "Crypt Redux"),
                }
            },
            {
                "Archipelagos Redux", // West Garden
                new List<TunicPortal> {
                    new TunicPortal("Overworld Redux", "lower", "West Garden Exit near Hero's Grave", granularRegion: "West Garden"),
                    new TunicPortal("archipelagos_house", "", "West Garden to Magic Dagger House", granularRegion: "West Garden"),
                    new TunicPortal("Overworld Redux", "upper", "West Garden after Boss", granularRegion: "West Garden", ignoreScene: true, requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Sword", 1 }, { "Archipelagos Redux, Overworld Redux_lower", 1 } }, new Dictionary<string, int> { { "Hyperdash", 1 }, {"Archipelagos Redux", 1 } } }),
                    new TunicPortal("Shop", "", "West Garden Shop", granularRegion: "West Garden"), // there's two of these, one is unused and disabled
                    new TunicPortal("Overworld Redux", "lowest", "West Garden Laurels Exit", granularRegion: "West Garden", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 } }),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "West Garden Hero's Grave", granularRegion: "West Garden", prayerPortal: true), // Hero grave
                    new TunicPortal("Transit", "teleporter_archipelagos_teleporter", "West Garden to Far Shore", granularRegion: "West Garden Portal", prayerPortal: true, deadEnd: true), // Portal to the thing behind dagger house
                }
            },
            {
                "archipelagos_house", // Magic Dagger house in West Garden
                new List<TunicPortal> {
                    new TunicPortal("Archipelagos Redux", "", "Magic Dagger House Exit", granularRegion: "Magic Dagger House", deadEnd: true),
                }
            },
            {
                "Atoll Redux",
                new List<TunicPortal> {
                    new TunicPortal("Frog Stairs", "eye", "Frog Stairs Eye Entrance", granularRegion: "Atoll"),
                    new TunicPortal("Library Exterior", "", "Atoll Statue Teleporter", granularRegion: "Atoll", prayerPortal: true),
                    new TunicPortal("Overworld Redux", "upper", "Atoll Upper Exit", granularRegion: "Atoll"),
                    new TunicPortal("Overworld Redux", "lower", "Atoll Lower Exit", granularRegion: "Atoll", requiredItems: new Dictionary<string, int> {{"Hyperdash", 1}}),
                    new TunicPortal("Frog Stairs", "mouth", "Frog Stairs Mouth Entrance", granularRegion: "Atoll", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Wand", 1 } }, new Dictionary<string, int> { { "Hyperdash", 1 } } }),
                    new TunicPortal("Shop", "", "Atoll Shop", granularRegion: "Atoll"),
                    new TunicPortal("Transit", "teleporter_atoll", "Atoll to Far Shore", granularRegion: "Atoll", prayerPortal: true),
                    // new TunicPortal("Forest Lake_", "teleporter", "Portal"), // Unused portal, same spot as library portal
                }
            },
            {
                "Frog Stairs", // Entrance to frog's domain
                new List<TunicPortal> {
                    new TunicPortal("Atoll Redux", "mouth", "Frog Stairs Mouth Exit", granularRegion: "Frog Stairs"),
                    new TunicPortal("frog cave main", "Exit", "Frog Stairs to Frog's Domain's Exit", granularRegion: "Frog Stairs"),
                    new TunicPortal("Atoll Redux", "eye", "Frog Stairs Eye Exit", granularRegion: "Frog Stairs"),
                    new TunicPortal("frog cave main", "Entrance", "Frog Stairs to Frog's Domain's Entrance", granularRegion: "Frog Stairs"),
                }
            },
            {
                "frog cave main", // Frog's domain, yes it's lowercase
                new List<TunicPortal> {
                    new TunicPortal("Frog Stairs", "Exit", "Frog's Domain Orb Exit", granularRegion: "Frog's Domain Back", ignoreScene: true, requiredItems: new Dictionary<string, int> { { "Wand", 1 }, { "frog cave main, Frog Stairs_Entrance", 1 } }),
                    new TunicPortal("Frog Stairs", "Entrance", "Frog's Domain Ladder Exit", granularRegion: "Frog's Domain Front", ignoreScene: true, oneWay: true),
                }
            },
            {
                "Library Exterior",
                new List<TunicPortal> {
                    new TunicPortal("Library Hall", "", "Library Exterior Ladder", granularRegion: "Library Exterior", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Hyperdash", 1 } }, new Dictionary<string, int> { { "Wand", 1} } }),
                    new TunicPortal("Atoll Redux", "", "Library Exterior Tree", granularRegion: "Library Exterior", requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Hyperdash", 1 }, { "12", 1 } }, new Dictionary<string, int> { { "Wand", 1}, { "12", 1 } } }),
                }
            },
            {
                "Library Hall", // Entry area with hero grave
                new List<TunicPortal> {
                    new TunicPortal("Library Rotunda", "", "Library Hall to Rotunda", granularRegion: "Library Hall"),
                    new TunicPortal("Library Exterior", "", "Library Hall Bookshelf Exit", granularRegion: "Library Hall"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "Library Hero's Grave", granularRegion: "Library Hall", prayerPortal: true),
                }
            },
            {
                "Library Rotunda", // The circular room with the ladder
                new List<TunicPortal> {
                    new TunicPortal("Library Hall", "", "Library Rotunda Lower Exit", granularRegion: "Library Rotunda"),
                    new TunicPortal("Library Lab", "", "Library Rotunda Upper Exit", granularRegion: "Library Rotunda"),
                }
            },
            {
                "Library Lab",
                new List<TunicPortal> {
                    new TunicPortal("Library Arena", "", "Library Lab to Librarian Arena", granularRegion: "Library Lab", ignoreScene: true, requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Hyperdash", 1 }, { "Library Lab", 1 } }, new Dictionary<string, int> { { "Wand", 1}, {"Library Lab", 1 } } }),
                    new TunicPortal("Library Rotunda", "", "Library Lab to Rotunda", granularRegion: "Library Lab", ignoreScene: true, requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Hyperdash", 1 }, { "Library Lab", 1 } }, new Dictionary<string, int> { { "Wand", 1}, {"Library Lab, Library Rotunda_", 1 } } }),
                    new TunicPortal("Transit", "teleporter_library teleporter", "Library to Far Shore", granularRegion: "Library Lab", ignoreScene: true, prayerPortal: true, requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Hyperdash", 1 }, { "Library Lab, Library Rotunda_", 1 } }, new Dictionary<string, int> { { "Wand", 1}, {"Library Lab, Library Rotunda_", 1 } }, new Dictionary<string, int> { { "Library Lab", 1 } } }),
                }
            },
            {
                "Library Arena",
                new List<TunicPortal> {
                    new TunicPortal("Library Lab", "", "Librarian Arena Exit", granularRegion: "Library Arena", deadEnd: true),
                }
            },
            {
                "East Forest Redux",
                new List<TunicPortal> {
                    new TunicPortal("Sword Access", "lower", "Forest Grave Path Lower Entrance", granularRegion: "East Forest"),
                    new TunicPortal("East Forest Redux Laddercave", "upper", "Forest Dance Fox Outside Doorway", granularRegion: "East Forest", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 } } ),
                    new TunicPortal("East Forest Redux Interior", "lower", "Forest Guard House 2 Lower Entrance", granularRegion: "East Forest"),
                    new TunicPortal("East Forest Redux Laddercave", "gate", "Forest Guard House 1 Gate Entrance", granularRegion: "East Forest"),
                    new TunicPortal("Sword Access", "upper", "Forest Grave Path Upper Entrance", granularRegion: "East Forest"),
                    new TunicPortal("East Forest Redux Interior", "upper", "Forest Guard House 2 Upper Entrance", granularRegion: "East Forest"),
                    new TunicPortal("East Forest Redux Laddercave", "lower", "Forest Guard House 1 Lower Entrance", granularRegion: "East Forest"),
                    new TunicPortal("Forest Belltower", "", "Forest to Belltower", granularRegion: "East Forest"),
                    new TunicPortal("Transit", "teleporter_forest teleporter", "Forest to Far Shore", granularRegion: "East Forest", prayerPortal: true),
                }
            },
            {
                "East Forest Redux Laddercave", // the place with the two ladders that leads to the boss room
                new List<TunicPortal> {
                    new TunicPortal("East Forest Redux", "upper", "Guard House 1 Dance Fox Exit", "Laddercave", ignoreScene: true, givesAccess: new List<string> { "East Forest Redux Laddercave, East Forest Redux_upper" }, requiredItems: new Dictionary<string, int> { { "East Forest Redux Laddercave, East Forest Redux_gate", 1 } }), // making the upper ones the "center" for easier logic writing
                    new TunicPortal("East Forest Redux", "lower", "Guard House 1 Lower Exit", "Laddercave", ignoreScene: true, givesAccess: new List<string> { "East Forest Redux Laddercave, East Forest Redux_lower" }, requiredItems: new Dictionary<string, int> { { "East Forest Redux Laddercave, East Forest Redux_gate", 1 } }),
                    new TunicPortal("East Forest Redux", "gate", "Guard House 1 Upper Forest Exit", "Laddercave"),
                    new TunicPortal("Forest Boss Room", "", "Guard House 1 to Guard Captain Room", "Laddercave"),
                }
            },
            {
                "Sword Access", // East forest hero grave area
                new List<TunicPortal> {
                    new TunicPortal("East Forest Redux", "upper", "Forest Grave Path Upper Exit", granularRegion: "Sword Access", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 } }),
                    new TunicPortal("East Forest Redux", "lower", "Forest Grave Path Lower Exit", granularRegion: "Sword Access"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "East Forest Hero's Grave", granularRegion: "Sword Access Back", ignoreScene: true, prayerPortal: true, requiredItems: new Dictionary<string, int> { {"Sword Access, East Forest Redux_lower", 1 } }), // Can't open the gate from behind
                    
                    // new TunicPortal("Forest 1_", "lower", "Portal (1)"),
                    // new TunicPortal("Forest 1_", "", "Portal"),
                    // new TunicPortal("Forest 1_", "upper", "Portal (2)"),
                }
            },
            {
                "East Forest Redux Interior", // Guardhouse 2
                new List<TunicPortal> {
                    new TunicPortal("East Forest Redux", "lower", "Guard House 2 Lower Exit", "Guardhouse 2"),
                    new TunicPortal("East Forest Redux", "upper", "Guard House 2 Upper Exit", "Guardhouse 2"),
                }
            },
            {
                "Forest Boss Room",
                new List<TunicPortal> {
                    new TunicPortal("East Forest Redux Laddercave", "", "Guard Captain Room Non-Gate Exit", "Forest Boss"), // entering it from behind puts you in the room, not behind the gate
                    new TunicPortal("Forest Belltower", "", "Guard Captain Room Gate Exit", "Forest Boss"),

                    // new TunicPortal("Archipelagos Redux_", "showfloordemo2022", "Portal (2)"),
                }
            },
            {
                "Forest Belltower",
                new List<TunicPortal> {
                    new TunicPortal("Fortress Courtyard", "", "Forest Belltower to Fortress", granularRegion: "Forest Belltower Main"),
                    new TunicPortal("East Forest Redux", "", "Forest Belltower to Forest", granularRegion: "Forest Belltower Lower"),
                    new TunicPortal("Overworld Redux", "", "Forest Belltower to Overworld", granularRegion: "Forest Belltower Main"),
                    new TunicPortal("Forest Boss Room", "", "Forest Belltower to Guard Captain Room", granularRegion: "Forest Belltower Upper", ignoreScene: true, oneWay: true),
                }
            },
            {
                "Fortress Courtyard", // Outside the fortress, the area connected to east forest and overworld. Center of the area is on the fortress-side of the bridge
                new List<TunicPortal> {
                    new TunicPortal("Fortress Reliquary", "Lower", "Fortress Courtyard to Fortress Grave Path Lower", granularRegion: "Fortress Courtyard"),
                    new TunicPortal("Fortress Reliquary", "Upper", "Fortress Courtyard to Fortress Grave Path Upper", granularRegion: "Fortress Courtyard Upper", ignoreScene: true, oneWay: true, givesAccess: new List<string> { "Fortress Courtyard, Fortress East_" }),
                    new TunicPortal("Fortress Main", "Big Door", "Fortress Courtyard to Fortress Interior", granularRegion: "Fortress Courtyard"),
                    new TunicPortal("Fortress East", "", "Fortress Courtyard to East Fortress", granularRegion: "Fortress Courtyard Upper", ignoreScene: true, oneWay: true, givesAccess: new List<string> { "Fortress Courtyard, Fortress Reliquary_Upper" }),
                    new TunicPortal("Fortress Basement", "", "Fortress Courtyard to Beneath the Earth", granularRegion: "Fortress Courtyard", ignoreScene: true, requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "12", 1 }, { "Fortress Courtyard, Overworld Redux_", 1 } }, new Dictionary<string, int> { { "Hyperdash", 1 }, { "Fortress Courtyard", 1 } }, new Dictionary<string, int> { {"Fortress Courtyard, Shop_", 1 } } }),
                    new TunicPortal("Forest Belltower", "", "Fortress Courtyard to Forest Belltower", granularRegion: "Fortress Courtyard", requiredItems: new Dictionary<string, int>{ { "Hyperdash", 1 } }),
                    new TunicPortal("Overworld Redux", "", "Fortress Courtyard to Overworld", granularRegion: "Fortress Courtyard", ignoreScene: true, requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { {"Hyperdash", 1}, { "Fortress Courtyard", 1 } }, new Dictionary<string, int> { {"Fortress Courtyard, Fortress East_", 1} }, new Dictionary<string, int> { { "Wand", 1 }, { "Fortress Courtyard, Forest Belltower_", 1 } } }), // remember, required items is just what you need to get to the center of a region -- prayer only gets you to the shop and beneath the earth
                    new TunicPortal("Shop", "", "Fortress Courtyard Shop", granularRegion: "Fortress Courtyard", ignoreScene: true, requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "12", 1 }, { "Fortress Courtyard, Overworld Redux_", 1 } }, new Dictionary<string, int> { { "Hyperdash", 1 }, { "Fortress Courtyard", 1 } }, new Dictionary<string, int> { {"Fortress Courtyard, Fortress Basement_", 1 } } }),

                    // new TunicPortal("Overworld Redux_", "", "Portal (4)"), // unused and disabled
                }
            },
            {
                "Fortress Basement", // Under the fortress
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Main", "", "Beneath the Earth to Fortress Interior", "Fortress Basement"),
                    new TunicPortal("Fortress Courtyard", "", "Beneath the Earth to Fortress Courtyard", "Fortress Basement"),
                }
            },
            {
                "Fortress Main", // Inside the fortress
                new List<TunicPortal> {
                    new TunicPortal("Fortress Courtyard", "Big Door", "Fortress Interior Main Exit", "Fortress Main"),
                    new TunicPortal("Fortress Basement", "", "Fortress Interior to Beneath the Earth", "Fortress Main"),
                    new TunicPortal("Fortress Arena", "", "Fortress Interior to Siege Engine Arena", "Fortress Main", requiredItems: new Dictionary<string, int> { { "12", 1 }, {"Fortress Courtyard, Overworld Redux_", 1}, { "Fortress Courtyard, Fortress Reliquary_upper", 1 }, {"Fortress Main, Fortress Courtyard_Big Door", 1 } }), // requires that one prayer thing to be down
                    new TunicPortal("Shop", "", "Fortress Interior Shop", "Fortress Main"),
                    new TunicPortal("Fortress East", "upper", "Fortress Interior to East Fortress Upper", "Fortress Main"),
                    new TunicPortal("Fortress East", "lower", "Fortress Interior to East Fortress Lower", "Fortress Main"),
                }
            },
            {
                "Fortress East", // that tiny area with the couple mages up high, and the ladder in the lower right
                new List<TunicPortal> {
                    new TunicPortal("Fortress Main", "lower", "East Fortress to Interior Lower", granularRegion: "Fortress East Lower", ignoreScene: true, requiredItems: new Dictionary<string, int> { { "Fortress East, Fortress Main_upper", 1} }),
                    new TunicPortal("Fortress Courtyard", "", "East Fortress to Courtyard", granularRegion: "Fortress East"),
                    new TunicPortal("Fortress Main", "upper", "East Fortress to Interior Upper", granularRegion: "Fortress East"),
                }
            },
            {
                "Fortress Reliquary", // Where the grave is
                new List<TunicPortal> {
                    new TunicPortal("Fortress Courtyard", "Lower", "Fortress Grave Path Lower Exit", granularRegion: "Fortress Grave Path"),
                    new TunicPortal("Dusty", "", "Fortress Grave Path Dusty Entrance", granularRegion: "Fortress Grave Path", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 } }),
                    new TunicPortal("Fortress Courtyard", "Upper", "Fortress Grave Path Upper Exit", granularRegion: "Fortress Grave Path Upper", deadEnd: true),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "Fortress Hero's Grave", granularRegion: "Fortress Grave Path", prayerPortal: true),
                }
            },
            {
                "Dusty", // broom
                new List<TunicPortal> {
                    new TunicPortal("Fortress Reliquary", "", "Dusty Exit", "Dusty", deadEnd: true),
                }
            },
            {
                "Fortress Arena", // Where the boss is
                new List<TunicPortal> {
                    new TunicPortal("Fortress Main", "", "Siege Engine Arena to Fortress", "Fortress Arena"),
                    new TunicPortal("Transit", "teleporter_spidertank", "Fortress to Far Shore", "Fortress Arena", entryItems: new Dictionary<string, int> { { "12", 1 }, { "Fortress Basement, Fortress Main_", 1 }, {"Fortress Main, Fortress Courtyard_", 1}, { "Fortress Courtyard, Fortress Main_", 1 } }),
                    // new TunicPortal("Fortress Main_", "", "Portal"), // There's two of these, one is disabled
                }
            },
            {
                "Mountain",
                new List<TunicPortal> {
                    new TunicPortal("Mountaintop", "", "Stairs to Top of the Mountain", "Mountain", requiredItems: new Dictionary<string, int> { { "21", 1 } }),
                    new TunicPortal("Quarry Redux", "", "Mountain to Quarry", "Mountain"),
                    new TunicPortal("Overworld Redux", "", "Mountain to Overworld", "Mountain"),
                }
            },
            {
                "Mountaintop",
                new List<TunicPortal> {
                    new TunicPortal("Mountain", "", "Top of the Mountain Exit", "Mountaintop", deadEnd: true),
                }
            },
            {
                "Darkwoods Tunnel", // connector between overworld and quarry
                new List<TunicPortal> {
                    new TunicPortal("Overworld Redux", "", "Quarry Connector to Overworld", "Darkwoods"),
                    new TunicPortal("Quarry Redux", "", "Quarry Connector to Quarry", "Darkwoods"),
                }
            },
            {
                "Quarry Redux",
                new List<TunicPortal> {
                    new TunicPortal("Darkwoods Tunnel", "", "Quarry to Overworld Exit", granularRegion: "Quarry"),
                    new TunicPortal("Shop", "", "Quarry Shop", granularRegion: "Quarry"),
                    new TunicPortal("Monastery", "front", "Quarry to Monastery Front", granularRegion: "Quarry"),
                    new TunicPortal("Monastery", "back", "Quarry to Monastery Back", granularRegion: "Monastery Rope", ignoreScene: true, oneWay: true),
                    new TunicPortal("Mountain", "", "Quarry to Mountain", granularRegion: "Quarry"),
                    new TunicPortal("ziggurat2020_0", "", "Quarry to Ziggurat", granularRegion: "Quarry", entryItems: new Dictionary<string, int> { { "Wand", 1 }, { "Darkwood Tunnel, Quarry Redux_", 1 }, { "12", 1 } }),
                    new TunicPortal("Transit", "teleporter_quarry teleporter", "Quarry to Far Shore", granularRegion: "Quarry", prayerPortal: true, entryItems: new Dictionary<string, int> { { "Wand", 1 }, { "Darkwood Tunnel, Quarry Redux_", 1 }, { "12", 1 } }),
                }
            },
            {
                "Monastery",
                new List<TunicPortal> {
                    new TunicPortal("Quarry Redux", "back", "Monastery Rear Exit", "Monastery"),
                    new TunicPortal("Quarry Redux", "front", "Monastery Front Exit", "Monastery"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "Monastery Hero's Grave", "Monastery", prayerPortal: true),

                    // new TunicPortal("Quarry_", "lower", "Portal (1)"), // Unused portal, disabled, and very high up
                }
            },
            {
                "ziggurat2020_0", // Zig entrance hallway
                new List<TunicPortal> {
                    new TunicPortal("ziggurat2020_1", "", "Zig Entry Hallway to Zig Upper", "Zig 0"),
                    new TunicPortal("Quarry Redux", "", "Zig Entry Hallway to Quarry", "Zig 0"),
                }
            },
            {
                "ziggurat2020_1", // Upper zig
                new List<TunicPortal> {
                    // new TunicPortal("ziggurat2020_3", "zig2_skip", "Zig Skip"), // the elevator skip to lower zig, put a secret here later
                    new TunicPortal("ziggurat2020_0", "", "Zig Upper to Zig Entry", granularRegion: "Zig 1 Top", ignoreScene: true, oneWay: true),
                    new TunicPortal("ziggurat2020_2", "", "Zig Upper to Zig Tower", granularRegion: "Zig 1 Bottom", deadEnd: true, ignoreScene: true, requiredItems: new Dictionary<string, int>{{"ziggurat2020_1, ziggurat2020_0_", 1}}),
                }
            },
            {
                "ziggurat2020_2", // Zig intermediate elevator
                new List<TunicPortal> {
                    new TunicPortal("ziggurat2020_1", "", "Zig Tower to Zig Upper", granularRegion: "Zig 2 Top", ignoreScene: true, oneWay: true),
                    new TunicPortal("ziggurat2020_3", "", "Zig Tower to Zig Lower", granularRegion: "Zig 2 Bottom", deadEnd: true, ignoreScene: true, requiredItems: new Dictionary<string, int>{{"ziggurat2020_2, ziggurat2020_1_", 1}}),
                }
            },
            {
                "ziggurat2020_3", // Lower zig, center is designated as before the prayer spot with the two cube minibosses
                new List<TunicPortal> {
                    new TunicPortal("ziggurat2020_FTRoom", "", "Zig Portal Room Entrance", granularRegion: "Zig 3", ignoreScene: true, prayerPortal: true, requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { {"Hyperdash", 1 }, { "ziggurat2020_3, ziggurat2020_2_", 1 } }, new Dictionary<string, int> { { "12", 1 }, { "ziggurat2020_3, ziggurat2020_2_", 1 } } }), // Prayer portal room
                    // new TunicPortal("ziggurat2020_1", "zig2_skip", "Zig Skip Exit"), // the elevator skip to lower zig
                    new TunicPortal("ziggurat2020_2", "", "Zig Lower to Zig Tower", granularRegion: "Zig 3"),
                }
            },
            {
                "ziggurat2020_FTRoom", // The room with the prayer portal
                new List<TunicPortal> {
                    new TunicPortal("ziggurat2020_3", "", "Zig Portal Room Exit", "Zig Portal Room", ignoreScene: true, requiredItems: new Dictionary<string, int> { { "12", 1 }, { "ziggurat2020_3, ziggurat2020_FTRoom", 1 } }),
                    new TunicPortal("Transit", "teleporter_ziggurat teleporter", "Zig to Far Shore", "Zig Portal Room", prayerPortal: true),
                }
            },
            {
                "Swamp Redux 2",
                new List<TunicPortal> {
                    new TunicPortal("Overworld Redux", "conduit", "Swamp Lower Exit", granularRegion: "Swamp Front"),
                    new TunicPortal("Cathedral Redux", "main", "Swamp to Cathedral Main Entrance", granularRegion: "Swamp Front", requiredItems: new Dictionary<string, int> { { "12", 1 }, { "Hyperdash", 1 }, { "Overworld Redux, Swamp Redux 2_wall", 1 } } ),
                    new TunicPortal("Cathedral Redux", "secret", "Swamp to Cathedral Secret Legend Room Entrance", granularRegion: "Swamp Front", requiredItems: new Dictionary<string, int> { { "21", 1 } }),
                    new TunicPortal("Cathedral Arena", "", "Swamp to Gauntlet", granularRegion: "Swamp Back", ignoreScene: true, requiredItemsOr: new List<Dictionary<string, int>> { new Dictionary<string, int> { { "Hyperdash", 1 }, {"Swamp Redux 2, Overworld Redux_wall", 1 } }, new Dictionary<string, int> { { "Swamp Redux 2, RelicVoid_teleporter_relic plinth", 1 } } }),
                    new TunicPortal("Shop", "", "Swamp Shop", granularRegion: "Swamp Front"),
                    new TunicPortal("Overworld Redux", "wall", "Swamp Upper Exit", granularRegion: "Swamp Back", ignoreScene: true, requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 }, { "Swamp Redux 2, Cathedral Arena_", 1 } }),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "Swamp Hero's Grave", granularRegion: "Swamp Back", ignoreScene: true, prayerPortal: true, requiredItems: new Dictionary<string, int> { { "Swamp Redux 2, Cathedral Arena_", 1 } }),
                }
            },
            {
                "Cathedral Redux",
                new List<TunicPortal> {
                    new TunicPortal("Swamp Redux 2", "main", "Cathedral Main Exit", granularRegion: "Cathedral"),
                    new TunicPortal("Cathedral Arena", "", "Cathedral Elevator", granularRegion: "Cathedral"),
                    new TunicPortal("Swamp Redux 2", "secret", "Cathedral Secret Legend Room Exit", granularRegion: "Cathedral Secret Legend", ignoreScene: true, deadEnd: true), // only one chest, just use item access rules for it
                }
            },
            {
                "Cathedral Arena", // Gauntlet
                new List<TunicPortal> {
                    new TunicPortal("Swamp Redux 2", "", "Gauntlet to Swamp", granularRegion: "Gauntlet Bottom", ignoreScene: true, deadEnd: true, requiredItems: new Dictionary<string, int>{{"Cathedral Arena, Cathedral Redux_", 1}, {"Hyperdash", 1}}),
                    new TunicPortal("Cathedral Redux", "", "Gauntlet Elevator", granularRegion: "Gauntlet Top", ignoreScene: true, givesAccess: new List<string> {"Cathedral Arena, Shop_"}, requiredItems: new Dictionary<string, int>{{"Cathedral Arena, Shop_", 1}}),
                    new TunicPortal("Shop", "", "Gauntlet Shop", granularRegion: "Gauntlet Top", ignoreScene: true, givesAccess: new List<string> {"Cathedral Arena, Swamp Redux 2_"}, requiredItems: new Dictionary<string, int>{{"Cathedral Arena, Cathedral Redux_", 1}}), // we love gauntlet shop
                }
            },
            {
                "Shop", // Every shop is just this region
                new List<TunicPortal> {
                    new TunicPortal("Previous Region", "", "Shop Portal", granularRegion: "Shop"),
                }
            },
            {
                "RelicVoid", // Hero relic area
                new List<TunicPortal> {
                    new TunicPortal("Fortress Reliquary", "teleporter_relic plinth", "Hero's Grave to Fortress", "RelicVoid", ignoreScene: true, deadEnd: true),
                    new TunicPortal("Monastery", "teleporter_relic plinth", "Hero's Grave to Monastery", "RelicVoid", ignoreScene: true, deadEnd: true),
                    new TunicPortal("Archipelagos Redux", "teleporter_relic plinth", "Hero's Grave to West Garden", "RelicVoid", ignoreScene: true, deadEnd: true),
                    new TunicPortal("Sword Access", "teleporter_relic plinth", "Hero's Grave to East Forest", "RelicVoid", ignoreScene: true, deadEnd: true),
                    new TunicPortal("Library Hall", "teleporter_relic plinth", "Hero's Grave to Library", "RelicVoid", ignoreScene: true, deadEnd: true),
                    new TunicPortal("Swamp Redux 2", "teleporter_relic plinth", "Hero's Grave to Swamp", "RelicVoid", ignoreScene: true, deadEnd: true),
                }
            },
            {
                "Transit", // Teleporter hub
                new List<TunicPortal> {
                    new TunicPortal("Archipelagos Redux", "teleporter_archipelagos_teleporter", "Far Shore to West Garden", "Transit", entryItems: new Dictionary<string, int>{ { "12", 1 }, { "Archipelagos Redux, Overworld Redux_lower", 1} }),
                    new TunicPortal("Library Lab", "teleporter_library teleporter", "Far Shore to Library", "Transit", entryItems: new Dictionary<string, int>{ { "12", 1 }, { "Library Lab, Library Arena_", 1} }),
                    new TunicPortal("Quarry Redux", "teleporter_quarry teleporter", "Far Shore to Quarry", "Transit", entryItems: new Dictionary<string, int>{ { "12", 1 }, { "Quarry Redux, Darkwoods Tunnel_", 1 }, {"Darkwoods Tunnel, Quarry Redux_", 1 }, { "Wand", 1 } }),
                    new TunicPortal("East Forest Redux", "teleporter_forest teleporter", "Far Shore to East Forest", "Transit", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 } }),
                    new TunicPortal("Fortress Arena", "teleporter_spidertank", "Far Shore to Fortress", "Transit", entryItems: new Dictionary<string, int> { { "12", 1 }, { "Fortress Basement, Fortress Main_", 1 }, {"Fortress Courtyard, Overworld Redux_", 1}, { "Fortress Courtyard, Fortress Main_", 1 } }),
                    new TunicPortal("Atoll Redux", "teleporter_atoll", "Far Shore to Atoll", "Transit"),
                    new TunicPortal("ziggurat2020_FTRoom", "teleporter_ziggurat teleporter", "Far Shore to Zig", "Transit"),
                    new TunicPortal("Spirit Arena", "teleporter_spirit arena", "Far Shore to Heir", "Transit"),
                    new TunicPortal("Overworld Redux", "teleporter_town", "Far Shore to Town", "Transit"),
                    new TunicPortal("Overworld Redux", "teleporter_starting island", "Far Shore to Spawn", "Transit", requiredItems: new Dictionary<string, int> { { "Hyperdash", 1 } }),

                    // new TunicPortal("Transit_", "teleporter_", "Portal"), // Unused portal, far away and not enabled
                }
            },
            {
                "Spirit Arena", // Heir fight
                new List<TunicPortal> {
                    new TunicPortal("Transit", "teleporter_spirit arena", "Heir Arena Exit", "Heir Arena", deadEnd: true),
                }
            },
            {
                "Purgatory", // Second save hallway
                new List<TunicPortal> {
                    new TunicPortal("Purgatory", "bottom", "Purgatory Bottom Exit", "Purgatory"),
                    new TunicPortal("Purgatory", "top", "Purgatory Top Exit", "Purgatory"),
                }
            },
        };

        public static void ShuffleList<T>(IList<T> list, int seed) {
            var rng = new System.Random(seed);
            int n = list.Count;

            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        // function to see if we're placing a lock before its key, since doing that can possibly self-lock
        public static bool LockBeforeKey(Portal checkPortal) {
            if (checkPortal.SceneDestinationTag == "Overworld Redux, Temple_main") {
                // check if the belltower upper has been placed yet, if not then reshuffle the two plus portals list (since this list is gonna be the bigger one)
                int i = 0;
                foreach (Portal portal in deadEndPortals) {
                    if (portal.SceneDestinationTag == "Forest Belltower, Forest Boss Room_") {
                        i++;
                        break;
                    }
                }
                if (i == 1) { return true; }
            } else if (checkPortal.SceneDestinationTag == "Fortress Main, Fortress Arena_") {
                // check if none of the portals that lead to the necessary fuses have been placed
                int i = 0;
                int j = 0;
                int k = 0;
                foreach (Portal portal in twoPlusPortals) {
                    if (portal.SceneDestinationTag == "Fortress Courtyard, Fortress Reliquary_upper"
                        || portal.SceneDestinationTag == "Fortress Courtyard, Fortress East_") { i++; }
                    if (portal.Scene == "Fortress Basement") { j++; }
                    if (portal.Scene == "Fortress Main") { k++; }
                }
                if (i == 2 || j == 2 || k == 6) { return true; }
            } else if (checkPortal.SceneDestinationTag == "Fortress Arena, Transit_teleporter_spidertank"
                  || checkPortal.SceneDestinationTag == "Transit, Fortress Arena_teleporter_spidertank") {
                // check if none of the portals that lead to the necessary fuses have been placed
                int i = 0;
                int j = 0;
                int k = 0;
                foreach (Portal portal in twoPlusPortals) {
                    if (portal.Scene == "Fortress Courtyard") { i++; }
                    if (portal.Scene == "Fortress Basement") { j++; }
                    if (portal.Scene == "Fortress Main") { k++; }
                }
                if (i == 8 || j == 2 || k == 6) { return true; }
            } else if (checkPortal.SceneDestinationTag == "Swamp Redux 2, Cathedral Redux_main") {
                int i = 0;
                foreach (Portal portal in twoPlusPortals) {
                    if (portal.SceneDestinationTag == "Swamp Redux 2, Overworld Redux_conduit"
                        || portal.SceneDestinationTag == "Swamp Redux 2, Shop_"
                        || portal.SceneDestinationTag == "Swamp Redux 2, Cathedral Redux_secret") { i++; }
                }
                if (i == 3) { return true; }
            } else if (checkPortal.SceneDestinationTag == "ziggurat2020_FTRoom, ziggurat2020_3") {
                int i = 0;
                foreach (Portal portal in twoPlusPortals) {
                    if (portal.Scene == "ziggurat2020_3") { i++; }
                }
                if (i == 2) { return true; }
            } else if (checkPortal.SceneDestinationTag == "Quarry Redux, Transit_teleporter_quarry teleporter") {
                int i = 0;
                foreach (Portal portal in twoPlusPortals) {
                    if (portal.Scene == "Darkwoods Tunnel") { i++; }
                }
                if (i == 2) { return true; }
            } else if (checkPortal.SceneDestinationTag == "Transit, Quarry Redux_teleporter_quarry teleporter") {
                int i = 0;
                int j = 0;
                foreach (Portal portal in twoPlusPortals) {
                    if (portal.Scene == "Darkwoods Tunnel") { i++; }
                    if (portal.Scene == "Quarry Redux") { j++; }
                }
                if (i == 2 || j == 7) { return true; }
            } else if (checkPortal.SceneDestinationTag == "Transit, Library Lab_teleporter_library teleporter") {
                int i = 0;
                foreach (Portal portal in twoPlusPortals) {
                    if (portal.Scene == "Library Lab") { i++; }
                }
                if (i == 3) { return true; }
            } else if (checkPortal.SceneDestinationTag == "Transit, Archipelagos Redux_teleporter_archipelagos_teleporter") {
                int i = 0;
                foreach (Portal portal in twoPlusPortals) {
                    if (portal.Scene == "Archipelagos Redux") { i++; }
                }
                if (i == 7) { return true; }
            }
            return false;
        }

        // if we have some granular regions, we get another one. This is for one-way connections, basically
        // so that we don't unnecessarily force the back of house to be connected to a non-dead-end, for example
        public static List<string> AddDependentRegions(string region) {
            List<string> regions = new List<string>();
            // idk if we need to clear it
            regions.Clear();
            regions.Add(region);

            if (region == "Old House Front") {
                regions.Add("Old House Back");
            } else if (region == "Frog's Domain Front") {
                regions.Add("Frog's Domain Back");
            } else if (region == "Sword Access") {
                regions.Add("Sword Access Back");
            } else if (region == "Forest Belltower Upper") {
                regions.Add("Forest Belltower Main");
                regions.Add("Forest Belltower Lower");
            } else if (region == "Forest Beltower Main") {
                regions.Add("Forest Belltower Lower");
            } else if (region == "Fortress Courtyard Upper") {
                regions.Add("Fortress Courtyard");
            } else if (region == "Fortress East") {
                regions.Add("Fortress East Lower");
            } else if (region == "Monastery Rope") {
                regions.Add("Quarry");
            } else if (region == "Zig 1 Top") {
                regions.Add("Zig 1 Bottom");
            } else if (region == "Zig 2 Top") {
                regions.Add("Zig 2 Bottom");
            } else if (region == "Gauntlet Top") {
                regions.Add("Gauntlet Bottom");
            }

            return regions;
        }

        // making a separate lists for portals connected to one, two, or three+ regions, to be populated by the foreach coming up next
        public static List<Portal> deadEndPortals = new List<Portal>();
        public static List<Portal> twoPlusPortals = new List<Portal>();
        // create a list of all portals with their information loaded in, just a slightly expanded version of the above to include destinations
        public static void RandomizePortals(int seed) {
            RandomizedPortals.Clear();

            // separate the portals into their respective lists
            foreach (KeyValuePair<string, Dictionary<string, List<NewPortal>>> scene_group in RegionPortalsList) {
                string scene_name = scene_group.Key;
                foreach (KeyValuePair<string, List<NewPortal>> region_group in scene_group.Value) {
                    string region_name = region_group.Key;
                    List<NewPortal> region_portals = region_group.Value;
                    foreach (NewPortal portal in region_portals) {
                        Portal newPortal = new Portal(name: portal.Name, destination: portal.Destination, scene: scene_name, region: region_name);
                        if (RegionDict[region_name].DeadEnd == true) {
                            deadEndPortals.Add(newPortal);
                        } else {
                            twoPlusPortals.Add(newPortal);
                        }
                    }
                }
            }
            if (SaveFile.GetInt("randomizer ER fixed shop") == 1) {
                foreach (Portal portal in twoPlusPortals) {
                    if (portal.SceneDestinationTag == "Overworld Redux, Windmill_") {
                        twoPlusPortals.Remove(portal);
                        break;
                    }
                }
            }

            // making a list of accessible regions that will be updated as we gain access to more regions
            List<string> accessibleRegions = new List<string>();
            accessibleRegions.Clear();

            // just picking a static start region for now, can modify later if we want to do random start location
            string start_region = "Overworld";
            accessibleRegions.Add(start_region);

            int comboNumber = 0;

            // This might be way too much shuffling -- was done to not favor connecting new regions to the first regions added to the list
            // create a portal combo for every region in the threePlusRegions list, so that every region can now be accessed (ignoring rules for now)
            // todo: make it add regions to the list based on previously gotten regions
            while (accessibleRegions.Count < 57) {
                ShuffleList(twoPlusPortals, seed);
                // later on, start by making the first several portals into shop portals
                Portal portal1 = null;
                Portal portal2 = null;
                foreach (Portal portal in twoPlusPortals) {
                    // find a portal in a region we can't access yet
                    if (LockBeforeKey(portal) == false && !accessibleRegions.Contains(portal.Region)) {
                        portal1 = portal;
                    }
                }
                if (portal1 == null) { Logger.LogInfo("something messed up in portal pairing for portal 1"); }
                twoPlusPortals.Remove(portal1);
                ShuffleList(twoPlusPortals, seed);
                foreach (Portal secondPortal in twoPlusPortals) {
                    if (LockBeforeKey(secondPortal) == false && accessibleRegions.Contains(secondPortal.Region)) {
                        portal2 = secondPortal;
                        twoPlusPortals.Remove(secondPortal);
                        break;
                    }
                }
                if (portal2 == null) { Logger.LogInfo("something messed up in portal pairing for portal 2"); }
                // add the portal combo to the randomized portals list
                RandomizedPortals.Add(comboNumber.ToString(), new PortalCombo(portal1, portal2));
                foreach (string region in AddDependentRegions(portal1.Region)) {
                    if (!accessibleRegions.Contains(region)) {
                        accessibleRegions.Add(region);
                    }
                }
                comboNumber++;
            }

            // since the dead ends only have one exit, we just append them 1 to 1 to a random portal in the two plus list
            ShuffleList(deadEndPortals, seed);
            ShuffleList(twoPlusPortals, seed);
            while (deadEndPortals.Count > 0) {
                if (LockBeforeKey(twoPlusPortals[0]) == true) { ShuffleList(twoPlusPortals, seed); } else {
                    comboNumber++;
                    RandomizedPortals.Add(comboNumber.ToString(), new PortalCombo(deadEndPortals[0], twoPlusPortals[0]));
                    deadEndPortals.RemoveAt(0);
                    twoPlusPortals.RemoveAt(0);
                }
            }
            List<string> shopRegionList = new List<string>();
            int shopCount = 6;
            if (SaveFile.GetInt("randomizer ER fixed shop") == 1) {
                shopCount = 1;
                Portal windmillPortal = new Portal(name: "Windmill Entrance", destination: "Windmill", scene: "Overworld Redux", region: "Overworld");
                Portal shopPortal = new Portal(destination: "Previous Region", tag: "", name: "Shop portal", scene: "Shop", region: "Shop");
                RandomizedPortals.Add("fixedshop", new PortalCombo(windmillPortal, shopPortal));
                shopRegionList.Add("Overworld Redux");
            }
            int regionNumber = 0;
            while (shopCount > 0) {
                // manually making a portal for the shop, because it has some special properties
                Portal shopPortal = new Portal(destination: "Previous Region", tag: "", name: "Shop portal", scene: "Shop", region: "Shop");
                // check that a shop has not already been added to this region, since two shops in the same region causes problems
                if (!shopRegionList.Contains(twoPlusPortals[regionNumber].Scene)) {
                    comboNumber++;
                    shopRegionList.Add(twoPlusPortals[regionNumber].Scene);
                    RandomizedPortals.Add(comboNumber.ToString(), new PortalCombo(twoPlusPortals[regionNumber], shopPortal));
                    twoPlusPortals.RemoveAt(regionNumber);
                    shopCount--;
                } else {
                    regionNumber++;
                }
                if (regionNumber == twoPlusPortals.Count - 1) {
                    Logger.LogInfo("too many shops, not enough regions, add more shops");
                }
            }

            // now we have every region accessible
            // the twoPlusPortals list still has items left in it, so now we pair them off
            while (twoPlusPortals.Count > 1) {
                // I don't think the LockBeforeKey check can lead to an infinite loop?
                if (LockBeforeKey(twoPlusPortals[0]) == true || LockBeforeKey(twoPlusPortals[1]) == true) { ShuffleList(twoPlusPortals, seed); } else {
                    comboNumber++;
                    RandomizedPortals.Add(comboNumber.ToString(), new PortalCombo(twoPlusPortals[0], twoPlusPortals[1]));
                    twoPlusPortals.RemoveAt(1); // I could do removeat0 twice, but I don't like how that looks
                    twoPlusPortals.RemoveAt(0);
                }
            }
            if (twoPlusPortals.Count == 1) {
                // if this triggers, increase or decrease shop count by 1
                Logger.LogInfo("one extra dead end remaining alone, rip. It's " + twoPlusPortals[0].Name);
            }

            // todo: figure out why the quarry portal isn't working right
            //Portal betaQuarryPortal = new Portal(destination: "Darkwoods", tag: "", name: "Beta Quarry", scene: "Quarry", region: "Quarry", requiredItems: new Dictionary<string, int>(), givesAccess: new List<string>(), deadEnd: true, prayerPortal: false, oneWay: false, ignoreScene: false);
            //Portal zigSkipPortal = new Portal(destination: "ziggurat2020_3", tag: "zig2_skip", name: "Zig Skip", scene: "ziggurat2020_1", region: "Zig 1", requiredItems: new Dictionary<string, int>(), givesAccess: new List<string>(), deadEnd: true, prayerPortal: false, oneWay: false, ignoreScene: false);
            //RandomizedPortals.Add("zigsecret", new PortalCombo(betaQuarryPortal, zigSkipPortal));
        }

        public static void CreatePortalPairs(Dictionary<string, string> APPortalStrings) {
            RandomizedPortals.Clear();
            List<Portal> portalsList = new List<Portal>();
            int comboNumber = 0;

            // turn the TunicPortals into Portals (so we can get the scene name in)
            foreach (KeyValuePair<string, List<TunicPortal>> region_group in PortalList) {
                string region_name = region_group.Key;
                List<TunicPortal> region_portals = region_group.Value;
                foreach (TunicPortal portal in region_portals) {
                    Portal newPortal = new Portal(destination: portal.Destination, tag: portal.DestinationTag, name: portal.PortalName, scene: region_name, region: portal.GranularRegion, requiredItems: portal.RequiredItems, requiredItemsOr: portal.RequiredItemsOr, entryItems: portal.EntryItems, givesAccess: portal.GivesAccess, deadEnd: portal.DeadEnd, prayerPortal: portal.PrayerPortal, oneWay: portal.OneWay, ignoreScene: portal.IgnoreScene);
                    portalsList.Add(newPortal);
                }
            }

            // make the PortalCombo dictionary
            foreach (KeyValuePair<string, string> stringPair in APPortalStrings) {
                string portal1SDT = stringPair.Key;
                string portal2SDT = stringPair.Value;
                Portal portal1 = null;
                Portal portal2 = null;

                foreach (Portal portal in portalsList) {
                    if (portal1SDT == portal.SceneDestinationTag) {
                        portal1 = portal;
                    }
                    if (portal2SDT == portal.SceneDestinationTag) {
                        portal2 = portal;
                    }
                }
                PortalCombo portalCombo = new PortalCombo(portal1, portal2);
                RandomizedPortals.Add(comboNumber.ToString(), portalCombo);
                comboNumber++;
            }
        }

        // a function to apply the randomized portal list to portals during onSceneLoaded
        // todo: get silent to show me how to reference an object instead of feeding portalPairs back into this
        public static void ModifyPortals(Scene loadingScene) {
            var Portals = Resources.FindObjectsOfTypeAll<ScenePortal>();
            foreach (var portal in Portals) {
                // go through the list of randomized portals and see if either the first or second portal matches the one we're looking at
                foreach (KeyValuePair<string, PortalCombo> portalCombo in RandomizedPortals) {
                    string comboTag = portalCombo.Key;
                    Portal portal1 = portalCombo.Value.Portal1;
                    Portal portal2 = portalCombo.Value.Portal2;

                    if (portal1.Scene == loadingScene.name && portal1.Tag == portal.id && portal1.Destination == portal.destinationSceneName) {
                        if (portal2.Scene == "Shop") {
                            portal.destinationSceneName = portal2.Scene;
                            portal.id = "";
                            portal.optionalIDToSpawnAt = "";
                            portal.name = portal2.Name;
                        } else {
                            portal.destinationSceneName = portal2.Scene;
                            portal.id = comboTag;
                            portal.optionalIDToSpawnAt = comboTag + comboTag + comboTag + comboTag; // quadrupling since doubling and tripling can have overlaps
                            portal.name = portal2.Name;
                        }
                        break;
                    }

                    if (portal2.Scene == loadingScene.name && portal2.Tag == portal.id && portal2.Destination == portal.destinationSceneName) {
                        if (portal1.Scene == "Shop") {
                            portal.destinationSceneName = portal1.Scene;
                            portal.id = "";
                            portal.optionalIDToSpawnAt = "";
                            portal.name = portal1.Name;
                        } else {
                            portal.destinationSceneName = portal1.Scene;
                            portal.id = comboTag + comboTag + comboTag + comboTag;
                            portal.optionalIDToSpawnAt = comboTag; // quadrupling since doubling and tripling can have overlaps
                            portal.name = portal2.Name;
                        }
                        break;
                    }
                }
            }
        }

        // this is for use in PlayerCharacterPatches. Will need to refactor later if we do random player spawn
        // todo: get silent to show me how to reference an object instead of feeding portalPairs back into this
        public static void AltModifyPortals() {
            var Portals = Resources.FindObjectsOfTypeAll<ScenePortal>();
            foreach (var portal in Portals) {
                // go through the list of randomized portals and see if either the first or second portal matches the one we're looking at
                foreach (KeyValuePair<string, PortalCombo> portalCombo in RandomizedPortals) {
                    string comboTag = portalCombo.Key;
                    Portal portal1 = portalCombo.Value.Portal1;
                    Portal portal2 = portalCombo.Value.Portal2;
                    if (portal1.Scene == "Overworld Redux" && portal1.Tag == portal.id && portal1.Destination == portal.destinationSceneName) {
                        if (portal2.Scene == "Shop") {
                            portal.destinationSceneName = portal2.Scene;
                            portal.id = "";
                            portal.optionalIDToSpawnAt = "";
                        } else {
                            portal.destinationSceneName = portal2.Scene;
                            portal.id = comboTag;
                            portal.optionalIDToSpawnAt = comboTag + comboTag + comboTag + comboTag; // quadrupling since doubling and tripling can have overlaps
                        }
                        break;
                    }

                    if (portal2.Scene == "Overworld Redux" && portal2.Tag == portal.id && portal2.Destination == portal.destinationSceneName) {
                        if (portal1.Scene == "Shop") {
                            portal.destinationSceneName = portal1.Scene;
                            portal.id = "";
                            portal.optionalIDToSpawnAt = "";
                        } else {
                            portal.destinationSceneName = portal1.Scene;
                            portal.id = comboTag + comboTag + comboTag + comboTag;
                            portal.optionalIDToSpawnAt = comboTag; // quadrupling since doubling and tripling can have overlaps
                        }
                        break;
                    }
                }
            }
        }

        public static void MarkPortals() {
            var Portals = Resources.FindObjectsOfTypeAll<ScenePortal>();

            foreach (var portal in Portals) {
                if (portal.FullID == PlayerCharacterSpawn.portalIDToSpawnAt) {
                    foreach (KeyValuePair<string, PortalCombo> portalCombo in TunicPortals.RandomizedPortals) {
                        if (portal.name == portalCombo.Value.Portal1.Name || portal.name == portalCombo.Value.Portal2.Name) {
                            SaveFile.SetInt("randomizer entered portal " + portalCombo.Value.Portal1.Name, 1);
                            SaveFile.SetInt("randomizer entered portal " + portalCombo.Value.Portal2.Name, 1);
                        }
                    }
                }
            }
        }

    }
}
