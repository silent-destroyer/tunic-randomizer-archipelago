﻿using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using BepInEx.Logging;

namespace TunicArchipelago
{
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

            public TunicPortal() { }

            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion) {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion, bool prayerPortal = false, bool deadEnd = false, bool oneWay = false, bool ignoreScene = false) {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
                PrayerPortal = prayerPortal;
                DeadEnd = deadEnd;
                OneWay = oneWay;
                IgnoreScene = ignoreScene;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion, Dictionary<string, int> entryItems, bool prayerPortal = false, bool deadEnd = false, bool oneWay = false, bool ignoreScene = false) {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
                EntryItems = entryItems;
                PrayerPortal = prayerPortal;
                DeadEnd = deadEnd;
                OneWay = oneWay;
                IgnoreScene = ignoreScene;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion, Dictionary<string, int> requiredItems, bool prayerPortal = false, bool deadEnd = false, bool ignoreScene = false) {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
                RequiredItems = requiredItems;
                PrayerPortal = prayerPortal;
                DeadEnd = deadEnd;
                IgnoreScene = ignoreScene;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion, List<string> givesAccess, bool ignoreScene = false, bool oneWay = false) {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
                GivesAccess = givesAccess;
                IgnoreScene = ignoreScene;
                OneWay = oneWay;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion, Dictionary<string, int> requiredItems) {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
                RequiredItems = requiredItems;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion, Dictionary<string, int> requiredItems, List<string> givesAccess, bool ignoreScene = false) {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
                RequiredItems = requiredItems;
                IgnoreScene = ignoreScene;
                GivesAccess = givesAccess;
            }
            public TunicPortal(string destination, string destinationTag, string portalName, string granularRegion, List<Dictionary<string, int>> requiredItemsOr, bool prayerPortal = false, bool ignoreScene = false) {
                Destination = destination;
                DestinationTag = destinationTag;
                PortalName = portalName;
                GranularRegion = granularRegion;
                RequiredItemsOr = requiredItemsOr;
                PrayerPortal = prayerPortal;
                IgnoreScene = ignoreScene;
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
                        } else {
                            portal.destinationSceneName = portal2.Scene;
                            portal.id = comboTag;
                            portal.optionalIDToSpawnAt = comboTag + comboTag + comboTag + comboTag; // quadrupling since doubling and tripling can have overlaps
                        }
                        break;
                    }


                    if (portal2.Scene == loadingScene.name && portal2.Tag == portal.id && portal2.Destination == portal.destinationSceneName) {
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

    }
}
