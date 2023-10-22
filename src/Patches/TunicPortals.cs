using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;
using BepInEx.Logging;
using Lib;
using HarmonyLib;
using JetBrains.Annotations;
using System.Globalization;
using HarmonyLib.Tools;

namespace TunicArchipelago
{
    public class TunicPortals {
        private static ManualLogSource Logger = TunicArchipelago.Logger;

        public class TunicPortal
        {
            public string Destination; // the vanilla destination scene
            public string Tag; // the vanilla destination tag, aka ID
            public string Name; // a human-readable name for the portal

            public TunicPortal() { }

            public TunicPortal(string destination, string tag, string name)
            {
                Destination = destination;
                Tag = tag;
                Name = name;
            }
        }

        // this is a big list of every portal in the game, along with their access requirements
        // a portal without access requirements just means you can get to the center of the region from that portal and vice versa
        public static Dictionary<string, List<TunicPortal>> PortalList = new Dictionary<string, List<TunicPortal>>
        {
            {
                "Overworld Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Sword Cave", "", "Stick House Entrance"),
                    new TunicPortal("Windmill", "", "Windmill Entrance"),
                    new TunicPortal("Sewer", "entrance", "Well Ladder Entrance"),
                    new TunicPortal("Sewer", "west_aqueduct", "Entrance to Well from Well Rail"),
                    new TunicPortal("Overworld Interiors", "house", "Old House Entry Door"),
                    new TunicPortal("Overworld Interiors", "under_checkpoint", "Old House Waterfall Entrance"),
                    new TunicPortal("Furnace", "gyro_upper_north", "Entrance to Furnace from Well Rail"),
                    new TunicPortal("Furnace", "gyro_upper_east", "Entrance to Furnace from Windmill"),
                    new TunicPortal("Furnace", "gyro_west", "Entrance to Furnace from West Garden"),
                    new TunicPortal("Furnace", "gyro_lower", "Entrance to Furnace from Beach"),
                    new TunicPortal("Overworld Cave", "", "Rotating Lights Entrance"),
                    new TunicPortal("Swamp Redux 2", "wall", "Swamp Upper Entrance"),
                    new TunicPortal("Swamp Redux 2", "conduit", "Swamp Lower Entrance"),
                    new TunicPortal("Ruins Passage", "east", "Ruined Hall Entrance Not-Door"),
                    new TunicPortal("Ruins Passage", "west", "Ruined Hall Entrance Door"),
                    new TunicPortal("Atoll Redux", "upper", "Atoll Upper Entrance"),
                    new TunicPortal("Atoll Redux", "lower", "Atoll Lower Entrance"),
                    new TunicPortal("ShopSpecial", "", "Special Shop Entrance"),
                    new TunicPortal("Maze Room", "", "Maze Cave Entrance"),
                    new TunicPortal("Archipelagos Redux", "upper", "West Garden Entrance by Belltower"),
                    new TunicPortal("Archipelagos Redux", "lower", "West Garden Entrance by Dark Tomb"),
                    new TunicPortal("Archipelagos Redux", "lowest", "West Garden Laurel Entrance"),
                    new TunicPortal("Temple", "main", "Temple Door Entrance"),
                    new TunicPortal("Temple", "rafters", "Temple Rafters Entrance"),
                    new TunicPortal("Ruined Shop", "", "Ruined Shop Entrance"),
                    new TunicPortal("PatrolCave", "", "Patrol Cave Entrance"),
                    new TunicPortal("Town Basement", "beach", "Hourglass Cave Entrance"),
                    new TunicPortal("Changing Room", "", "Changing Room Entrance"),
                    new TunicPortal("CubeRoom", "", "Cube Room Entrance"),
                    new TunicPortal("Mountain", "", "Stairs from Overworld to Mountain"),
                    new TunicPortal("Fortress Courtyard", "", "Overworld to Fortress"),
                    new TunicPortal("Town_FiligreeRoom", "", "HC Room Entrance next to Changing Room"),
                    new TunicPortal("EastFiligreeCache", "", "Glass Cannon HC Room Entrance"),
                    new TunicPortal("Darkwoods Tunnel", "", "Overworld to Quarry Connector"),
                    new TunicPortal("Crypt Redux", "", "Dark Tomb Main Entrance"),
                    new TunicPortal("Forest Belltower", "", "Overworld to Forest Belltower"),
                    new TunicPortal("Transit", "teleporter_town", "Town Portal"),
                    new TunicPortal("Transit", "teleporter_starting island", "Spawn Portal"),
                    new TunicPortal("Waterfall", "", "Entrance to Fairy Cave"),

                    // new TunicPortal("_", "", "Portal"), // unused, also ???
                    // new TunicPortal("Forest Belltower_", "showfloordemo2022", "Portal (12)"), // unused
                    // new TunicPortal("DEMO_altEnd_", "", "_Portal (Secret Demo End)"), // unused
                }
            },
            {
                "Waterfall", // fairy cave
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Fairy Cave Exit"),
                }
            },
            {
                "Windmill",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Windmill Exit"),
                    new TunicPortal("Shop", "", "Windmill Shop"),
                }
            },
            {
                "Overworld Interiors", // House in town
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "house", "Front Door of Old House Exit"),
                    new TunicPortal("g_elements", "", "Teleport to Secret Treasure Room"),
                    new TunicPortal("Overworld Redux", "under_checkpoint", "Exit from Old House Back Door"),

                    // new TunicPortal("Archipelagos Redux_", "", "_ShowfloorDemo2022 Portal"), // unused and disabled
                }
            },
            {
                "g_elements", // Relic tower
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Interiors", "", "Secret Treasure Room Exit"),
                }
            },
            {
                "Changing Room",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Changing Room Exit"),
                }
            },
            {
                "Town_FiligreeRoom", // the one next to the fountain
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Fountain HC Room Exit"),
                }
            },
            {
                "CubeRoom",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Cube Room Exit"),
                }
            },
            {
                "PatrolCave",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Guard Patrol Cave Exit"),
                }
            },
            {
                "Ruined Shop",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Ruined Shop Exit"),
                }
            },
            {
                "Furnace", // Under the west belltower
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "gyro_upper_north", "Furnace to Well Rail"),
                    new TunicPortal("Crypt Redux", "", "Furnace to Dark Tomb"),
                    new TunicPortal("Overworld Redux", "gyro_west", "Furnace to West Garden"),
                    new TunicPortal("Overworld Redux", "gyro_lower", "Furnace to Beach"),
                    new TunicPortal("Overworld Redux", "gyro_upper_east", "Furnace to Windmill"),
                }
            },
            {
                "Sword Cave", // Stick house
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Stick House Exit"),
                }
            },
            {
                "Ruins Passage", // That little hallway with the key door near the start in Overworld
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "east", "Ruins Passage"),
                    new TunicPortal("Overworld Redux", "west", "Ruins Passage"),
                }
            },
            {
                "EastFiligreeCache", // The holy cross room with the 3 chests near swamp entrance
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Glass Cannon HC Room Exit"),
                }
            },
            {
                "Overworld Cave", // East beach, next to swamp entrance, rotating lights room
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Rotating Lights Exit"),
                }
            },
            {
                "Maze Room", // Invisible maze
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Maze Cave Exit"),
                }
            },
            {
                "Town Basement", // Hourglass cave
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "beach", "Hourglass Cave Exit"), // yes, it has a tag even though it doesn't need one
                }
            },
            {
                "ShopSpecial", // Special shop, laurel across from that platform between east forest and fortress
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Special Shop Exit"),
                }
            },
            {
                "Temple", // Where you put the hexes
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "rafters", "Temple Rafters Exit"),
                    new TunicPortal("Overworld Redux", "main", "Temple Door Exit"),
                }
            },
            {
                "Sewer", // Bottom of the Well
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "entrance", "Well Ladder Exit"),
                    new TunicPortal("Sewer_Boss", "", "Well to Well Boss"),
                    new TunicPortal("Overworld Redux", "west_aqueduct", "Well Rail Exit"),
                }
            },
            {
                "Sewer_Boss", // Boss room in the Bottom of the Well
                new List<TunicPortal>
                {
                    new TunicPortal("Sewer", "", "Well Boss to Well"),
                    new TunicPortal("Crypt Redux", "", "Checkpoint to Dark Tomb"),
                }
            },
            {
                "Crypt Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Dark Tomb to Overworld"),
                    new TunicPortal("Furnace", "", "Dark Tomb to Furnace"),
                    new TunicPortal("Sewer_Boss", "", "Dark Tomb to Checkpoint"),
                }
            },
            {
                "Archipelagos Redux", // West Garden
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "lower", "West Garden towards Dark Tomb"),
                    new TunicPortal("archipelagos_house", "", "Magic Dagger House Entrance"),
                    new TunicPortal("Overworld Redux", "upper", "West Garden after Boss"),
                    new TunicPortal("Shop", "", "West Garden Shop"), // there's two of these, one is unused and disabled
                    new TunicPortal("Overworld Redux", "lowest", "West Garden Laurel Exit"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "West Garden Hero's Grave"), // Hero grave
                    new TunicPortal("Transit", "teleporter_archipelagos_teleporter", "West Garden Portal"), // Portal to the thing behind dagger house
                }
            },
            {
                "archipelagos_house", // Magic Dagger house in West Garden
                new List<TunicPortal>
                {
                    new TunicPortal("Archipelagos Redux", "", "Magic Dagger House Exit"),
                }
            },
            {
                "Atoll Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Frog Stairs", "eye", "Frog Eye Entrance"),
                    new TunicPortal("Library Exterior", "", "Atoll to Library"),
                    new TunicPortal("Overworld Redux", "upper", "Upper Atoll Exit"),
                    new TunicPortal("Overworld Redux", "lower", "Lower Atoll Exit"),
                    new TunicPortal("Frog Stairs", "mouth", "Frog Mouth Entrance"),
                    new TunicPortal("Shop", "", "Atoll Shop"),
                    new TunicPortal("Transit", "teleporter_atoll", "Atoll Portal"),
                    // new TunicPortal("Forest Lake_", "teleporter", "Portal"), // Unused portal, same spot as library portal
                }
            },
            {
                "Frog Stairs", // Entrance to frog's domain
                new List<TunicPortal>
                {
                    new TunicPortal("Atoll Redux", "mouth", "Frog Mouth Exit"),
                    new TunicPortal("frog cave main", "Exit", "Upper Frog to Lower Frog Exit"),
                    new TunicPortal("Atoll Redux", "eye", "Frog Eye Exit"),
                    new TunicPortal("frog cave main", "Entrance", "Upper Frog to Lower Frog Entrance"),
                }
            },
            {
                "frog cave main", // Frog's domain, yes it's lowercase
                new List<TunicPortal>
                {
                    new TunicPortal("Frog Stairs", "Exit", "Lower Frog Orb Exit"),
                    new TunicPortal("Frog Stairs", "Entrance", "Lower Frog Ladder Exit"),
                }
            },
            {
                "Library Exterior",
                new List<TunicPortal>
                {
                    new TunicPortal("Library Hall", "", "Library Entry Ladder"),
                    new TunicPortal("Atoll Redux", "", "Library to Atoll"),
                }
            },
            {
                "Library Hall", // Entry area with hero grave
                new List<TunicPortal>
                {
                    new TunicPortal("Library Rotunda", "", "Lower Library to Rotunda"),
                    new TunicPortal("Library Exterior", "", "Library Bookshelf Exit"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "Library Hero's Grave"),
                }
            },
            {
                "Library Rotunda", // The circular room with the ladder
                new List<TunicPortal>
                {
                    new TunicPortal("Library Hall", "", "Library Rotunda Lower Exit"),
                    new TunicPortal("Library Lab", "", "Library Rotunda Upper Exit"),
                }
            },
            {
                "Library Lab",
                new List<TunicPortal>
                {
                    new TunicPortal("Library Arena", "", "Upper Library to Librarian"),
                    new TunicPortal("Library Rotunda", "", "Upper Library to Rotunda"),
                    new TunicPortal("Transit", "teleporter_library teleporter", "Library Portal"),
                }
            },
            {
                "Library Arena",
                new List<TunicPortal>
                {
                    new TunicPortal("Library Lab", "", "Library Librarian Arena Exit"),
                }
            },
            {
                "East Forest Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Sword Access", "lower", "Forest Grave Path Lower Entrance"),
                    new TunicPortal("East Forest Redux Laddercave", "upper", "Forest Fox Dance Outside Doorway"),
                    new TunicPortal("East Forest Redux Interior", "lower", "Forest Guard House 2 Lower Entrance"),
                    new TunicPortal("East Forest Redux Laddercave", "gate", "Forest Guard House 1 Gate Entrance"),
                    new TunicPortal("Sword Access", "upper", "Forest Grave Path Upper Entrance"),
                    new TunicPortal("East Forest Redux Interior", "upper", "Forest Guard House 2 Upper Entrance"),
                    new TunicPortal("East Forest Redux Laddercave", "lower", "Forest Guard House 1 Lower Entrance"),
                    new TunicPortal("Forest Belltower", "", "Forest to Belltower"),
                    new TunicPortal("Transit", "teleporter_forest teleporter", "Forest Portal"),
                }
            },
            {
                "East Forest Redux Laddercave", // the place with the two ladders that leads to the boss room
                new List<TunicPortal>
                {
                    new TunicPortal("East Forest Redux", "upper", "Guard House 1 Dance Exit"),
                    new TunicPortal("East Forest Redux", "lower", "Guard House 1 Lower Exit"),
                    new TunicPortal("East Forest Redux", "gate", "Guard House 1 Upper Forest Exit"),
                    new TunicPortal("Forest Boss Room", "", "Guard House 1 to Guard Captain Room"),
                }
            },
            {
                "Sword Access", // East forest hero grave area
                new List<TunicPortal>
                {
                    new TunicPortal("East Forest Redux", "upper", "Upper Forest Grave Path Exit"),
                    new TunicPortal("East Forest Redux", "lower", "Lower Forest Grave Path Exit"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "East Forest Hero's Grave"),
                    
                    // new TunicPortal("Forest 1_", "lower", "Portal (1)"),
                    // new TunicPortal("Forest 1_", "", "Portal"),
                    // new TunicPortal("Forest 1_", "upper", "Portal (2)"),
                }
            },
            {
                "East Forest Redux Interior", // Guardhouse 2
                new List<TunicPortal>
                {
                    new TunicPortal("East Forest Redux", "lower", "Guard House 2 Lower Exit"),
                    new TunicPortal("East Forest Redux", "upper", "Guard House 2 Upper Exit"),
                }
            },
            {
                "Forest Boss Room",
                new List<TunicPortal>
                {
                    new TunicPortal("East Forest Redux Laddercave", "", "Guard Captain Room Non-Gate Exit"), // entering it from behind puts you in the room, not behind the gate
                    new TunicPortal("Forest Belltower", "", "Guard Captain Room Gate Exit"),

                    // new TunicPortal("Archipelagos Redux_", "showfloordemo2022", "Portal (2)"),
                }
            },
            {
                "Forest Belltower",
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Courtyard", "", "Forest Belltower to Fortress"),
                    new TunicPortal("East Forest Redux", "", "Forest Belltower to Forest"),
                    new TunicPortal("Overworld Redux", "", "Forest Belltower to Overworld"),
                    new TunicPortal("Forest Boss Room", "", "Forest Belltower to Guard Captain Room"),
                }
            },
            {
                "Fortress Courtyard", // Outside the fortress, the area connected to east forest and overworld. Center of the area is on the fortress-side of the bridge
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Reliquary", "Lower", "Lower Fortress Grave Path Entrance"),
                    new TunicPortal("Fortress Reliquary", "Upper", "Upper Fortress Grave Path Entrance"),
                    new TunicPortal("Fortress Main", "Big Door", "Fortress Courtyard to Fortress Interior"),
                    new TunicPortal("Fortress East", "", "Fortress Courtyard to Fortress East"),
                    new TunicPortal("Fortress Basement", "", "Fortress Courtyard to Beneath the Earth"),
                    new TunicPortal("Forest Belltower", "", "Fortress Courtyard to Forest Belltower"),
                    new TunicPortal("Overworld Redux", "", "Fortress Courtyard to Overworld"),
                    new TunicPortal("Shop", "", "Fortress Courtyard Shop"),

                    // new TunicPortal("Overworld Redux_", "", "Portal (4)"), // unused and disabled
                }
            },
            {
                "Fortress Basement", // Under the fortress
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Main", "", "Beneath the Earth to Fortress Interior"),
                    new TunicPortal("Fortress Courtyard", "", "Beneath the Earth to Fortress Courtyard"),
                }
            },
            {
                "Fortress Main", // Inside the fortress
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Courtyard", "Big Door", "Fortress Main Exit"),
                    new TunicPortal("Fortress Basement", "", "Fortress Interior to Beneath the Earth"),
                    new TunicPortal("Fortress Arena", "", "Fortress Interior to Siege Engine"),
                    new TunicPortal("Shop", "", "Fortress Interior Shop"),
                    new TunicPortal("Fortress East", "upper", "Fortress Interior to East Fortress Upper"),
                    new TunicPortal("Fortress East", "lower", "Fortress Interior to East Fortress Lower"),
                }
            },
            {
                "Fortress East", // that tiny area with the couple mages up high, and the ladder in the lower right
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Main", "lower", "East Fortress to Interior Lower"),
                    new TunicPortal("Fortress Courtyard", "", "East Fortress to Courtyard"),
                    new TunicPortal("Fortress Main", "upper", "East Fortress to Interior Upper"),
                }
            },
            {
                "Fortress Reliquary", // Where the grave is
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Courtyard", "Lower", "Lower Fortress Grave Path Exit"),
                    new TunicPortal("Dusty", "", "Fortress Grave Path Dusty Entrance"),
                    new TunicPortal("Fortress Courtyard", "Upper", "Upper Fortress Grave Path Exit"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "Fortress Hero's Grave"),
                }
            },
            {
                "Fortress Arena", // Where the boss is
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Main", "", "Siege Engine Arena to Fortress"),
                    new TunicPortal("Transit", "teleporter_spidertank", "Fortress Portal"),
                    // new TunicPortal("Fortress Main_", "", "Portal"), // There's two of these, one is disabled
                }
            },
            {
                "Dusty", // broom
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Reliquary", "", "Dusty Exit"),
                }
            },
            {
                "Mountain",
                new List<TunicPortal>
                {
                    new TunicPortal("Mountaintop", "", "Stairs to Top of the Mountain"),
                    new TunicPortal("Quarry Redux", "", "Mountain to Quarry"),
                    new TunicPortal("Overworld Redux", "", "Mountain to Overworld"),
                }
            },
            {
                "Mountaintop",
                new List<TunicPortal>
                {
                    new TunicPortal("Mountain", "", "Top of the Mountain Exit"),
                }
            },
            {
                "Darkwoods Tunnel", // connector between overworld and quarry
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "", "Quarry Connector to Overworld"),
                    new TunicPortal("Quarry Redux", "", "Quarry Connector to Quarry"),
                }
            },
            {
                "Quarry Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Darkwoods Tunnel", "", "Quarry to Overworld Exit"),
                    new TunicPortal("Shop", "", "Quarry Shop"),
                    new TunicPortal("Monastery", "front", "Quarry to Monastery Front"),
                    new TunicPortal("Monastery", "back", "Quarry to Monastery Back"),
                    new TunicPortal("Mountain", "", "Quarry to Mountain"),
                    new TunicPortal("ziggurat2020_0", "", "Quarry Zig Entrance"),
                    new TunicPortal("Transit", "teleporter_quarry teleporter", "Quarry Portal"),
                }
            },
            {
                "Monastery",
                new List<TunicPortal>
                {
                    new TunicPortal("Quarry Redux", "back", "Monastery Rear Exit"),
                    new TunicPortal("Quarry Redux", "front", "Monastery Front Exit"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "Monastery Hero's Grave"),

                    // new TunicPortal("Quarry_", "lower", "Portal (1)"), // Unused portal, disabled, and very high up
                }
            },
            {
                "ziggurat2020_0", // Zig entrance hallway
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_1", "", "Zig Entry Hallway to Zig 1"),
                    new TunicPortal("Quarry Redux", "", "Zig Entry Hallway to Quarry"),
                }
            },
            {
                "ziggurat2020_1", // Upper zig
                new List<TunicPortal>
                {
                    // new TunicPortal("ziggurat2020_3", "zig2_skip", "Zig Skip"), // the elevator skip to lower zig, put a secret here later
                    new TunicPortal("ziggurat2020_0", "", "Zig 1 to Zig Entry"),
                    new TunicPortal("ziggurat2020_2", "", "Zig 1 to Zig 2"),
                }
            },
            {
                "ziggurat2020_2", // Zig intermediate elevator
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_1", "", "Zig 2 to Zig 1"),
                    new TunicPortal("ziggurat2020_3", "", "Zig 2 to Zig 3"),
                }
            },
            {
                "ziggurat2020_3", // Lower zig
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_FTRoom", "", "Zig Portal Room Entrance"), // Prayer portal room
                    // new TunicPortal("ziggurat2020_1", "zig2_skip", "Zig Skip Exit"), // the elevator skip to lower zig
                    new TunicPortal("ziggurat2020_2", "", "Zig 3 to Zig 2"),
                }
            },
            {
                "ziggurat2020_FTRoom", // The room with the prayer portal
                new List<TunicPortal>
                {
                    new TunicPortal("ziggurat2020_3", "", "Zig Portal Room Exit"),
                    new TunicPortal("Transit", "teleporter_ziggurat teleporter", "Zig Portal"),
                }
            },
            {
                "Swamp Redux 2",
                new List<TunicPortal>
                {
                    new TunicPortal("Overworld Redux", "conduit", "Lower Swamp Exit"),
                    new TunicPortal("Cathedral Redux", "main", "Swamp to Cathedral Main Entrance"),
                    new TunicPortal("Cathedral Redux", "secret", "Swamp to Cathedral Treasure Room Entrance"),
                    new TunicPortal("Cathedral Arena", "", "Swamp to Gauntlet"),
                    new TunicPortal("Shop", "", "Swamp Shop"),
                    new TunicPortal("Overworld Redux", "wall", "Upper Swamp Exit"),
                    new TunicPortal("RelicVoid", "teleporter_relic plinth", "Swamp Hero's Grave"),
                }
            },
            {
                "Cathedral Redux",
                new List<TunicPortal>
                {
                    new TunicPortal("Swamp Redux 2", "main", "Cathedral Main Exit"),
                    new TunicPortal("Cathedral Arena", "", "Cathedral Elevator"),
                    new TunicPortal("Swamp Redux 2", "secret", "Cathedral Treasure Room Exit"),
                }
            },
            {
                "Cathedral Arena", // Gauntlet
                new List<TunicPortal>
                {
                    new TunicPortal("Swamp Redux 2", "", "Gauntlet to Swamp"),
                    new TunicPortal("Cathedral Redux", "", "Gauntlet Elevator"),
                    new TunicPortal("Shop", "", "Gauntlet Shop"), // we love gauntlet shop
                }
            },
            {
                "Shop", // Every shop is just this region
                new List<TunicPortal>
                {
                    new TunicPortal("Previous Region", "", "Shop Portal"),
                }
            },
            {
                "RelicVoid", // Hero relic area
                new List<TunicPortal>
                {
                    new TunicPortal("Fortress Reliquary", "teleporter_relic plinth", "Hero Relic to Fortress"),
                    new TunicPortal("Monastery", "teleporter_relic plinth", "Hero Relic to Monastery"),
                    new TunicPortal("Archipelagos Redux", "teleporter_relic plinth", "Hero Relic to West Garden"),
                    new TunicPortal("Sword Access", "teleporter_relic plinth", "Hero Relic to East Forest"),
                    new TunicPortal("Library Hall", "teleporter_relic plinth", "Hero Relic to Library"),
                    new TunicPortal("Swamp Redux 2", "teleporter_relic plinth", "Hero Relic to Swamp"),
                }
            },
            {
                "Transit", // Teleporter hub
                new List<TunicPortal>
                {
                    new TunicPortal("Archipelagos Redux", "teleporter_archipelagos_teleporter", "Far Shore to West Garden"),
                    new TunicPortal("Library Lab", "teleporter_library teleporter", "Far Shore to Library"),
                    new TunicPortal("Quarry Redux", "teleporter_quarry teleporter", "Far Shore to Quarry"),
                    new TunicPortal("East Forest Redux", "teleporter_forest teleporter", "Far Shore to East Forest"),
                    new TunicPortal("Fortress Arena", "teleporter_spidertank", "Far Shore to Fortress"),
                    new TunicPortal("Atoll Redux", "teleporter_atoll", "Far Shore to Atoll"),
                    new TunicPortal("ziggurat2020_FTRoom", "teleporter_ziggurat teleporter", "Far Shore to Zig"),
                    new TunicPortal("Spirit Arena", "teleporter_spirit arena", "Far Shore to Heir"),
                    new TunicPortal("Overworld Redux", "teleporter_town", "Far Shore to Town"),
                    new TunicPortal("Overworld Redux", "teleporter_starting island", "Far Shore to Spawn"),

                    // new TunicPortal("Transit_", "teleporter_", "Portal"), // Unused portal, far away and disabled
                }
            },
            {
                "Spirit Arena", // Heir fight
                new List<TunicPortal>
                {
                    new TunicPortal("Transit", "teleporter_spirit arena", "Heir Arena Exit"),
                }
            },
            {
                "Purgatory", // Second save hallway
                new List<TunicPortal>
                {
                    new TunicPortal("Purgatory", "bottom", "Purgatory Bottom Exit"),
                    new TunicPortal("Purgatory", "top", "Purgatory Top Exit"),
                }
            },
        };

        // this gets populated by slot_data, the strings are the StringDestinationTags for the two portals
        public static Dictionary<string, string> APPortalStrings = new Dictionary<string, string>();
        public static Dictionary<string, PortalCombo> APPortalPairs = new Dictionary<string, PortalCombo>();

        public static void CreatePortalPairs()
        {
            Logger.LogInfo("starting create portal pairs");
            List<Portal> portalsList = new List<Portal>();
            Dictionary<string, PortalCombo> portalPairs = new Dictionary<string, PortalCombo>();
            int comboNumber = 0;

            // turn the TunicPortals into Portals (so we can get the scene name in)
            foreach (KeyValuePair<string, List<TunicPortal>> region_group in PortalList)
            {
                string scene = region_group.Key;
                List<TunicPortal> region_portals = region_group.Value;
                foreach (TunicPortal portal in region_portals)
                {
                    Portal newPortal = new Portal(scene: scene, destination: portal.Destination, tag: portal.Tag, name: portal.Name);
                    if (newPortal.Scene == "ziggurat2020_3")
                    {
                        Logger.LogInfo("zig 3 portal here");
                        Logger.LogInfo(newPortal.SceneDestinationTag);
                    }
                    portalsList.Add(newPortal);
                }
            }

            // make the PortalCombo dictionary
            foreach (KeyValuePair<string, string> stringPair in APPortalStrings)
            {
                string portal1SDT = stringPair.Key;
                string portal2SDT = stringPair.Value;
                Portal portal1 = null;
                Portal portal2 = null;

                foreach (Portal portal in portalsList)
                {
                    if (portal1SDT == portal.SceneDestinationTag)
                    {
                        portal1 = portal; 
                    }
                    if (portal2SDT == portal.SceneDestinationTag)
                    {
                        portal2 = portal;
                    }
                }

                if (portal2 == null)
                {
                    Logger.LogInfo(portal2SDT);
                }
                if (portal1 == null)
                {
                    Logger.LogInfo(portal1SDT);
                }
                PortalCombo portalCombo = new PortalCombo(portal1, portal2);
                portalPairs.Add(comboNumber.ToString(), portalCombo);
                comboNumber++;
            }
            APPortalPairs = portalPairs;
        }

        // a function to apply the randomized portal list to portals during onSceneLoaded
        public static void ModifyPortals(Scene loadingScene)
        {
            Dictionary<string, PortalCombo> portalComboList = APPortalPairs;
            var Portals = Resources.FindObjectsOfTypeAll<ScenePortal>();
            foreach (var portal in Portals)
            {
                // go through the list of randomized portals and see if either the first or second portal matches the one we're looking at
                foreach (KeyValuePair<string, PortalCombo> portalCombo in portalComboList)
                {
                    string comboTag = portalCombo.Key;
                    Portal portal1 = portalCombo.Value.Portal1;
                    Portal portal2 = portalCombo.Value.Portal2;

                    if (portal1.Scene == loadingScene.name && portal1.Tag == portal.id && portal1.Destination == portal.destinationSceneName)
                    {
                        if (portal2.Scene == "Shop")
                        {
                            portal.destinationSceneName = portal2.Scene;
                            portal.id = "";
                            portal.optionalIDToSpawnAt = "";
                        }
                        else
                        {
                            portal.destinationSceneName = portal2.Scene;
                            portal.id = comboTag;
                            portal.optionalIDToSpawnAt = comboTag + comboTag + comboTag + comboTag; // quadrupling since doubling and tripling can have overlaps
                        }
                        break;
                    }


                    if (portal2.Scene == loadingScene.name && portal2.Tag == portal.id && portal2.Destination == portal.destinationSceneName)
                    {
                        if (portal1.Scene == "Shop")
                        {
                            portal.destinationSceneName = portal1.Scene;
                            portal.id = "";
                            portal.optionalIDToSpawnAt = "";
                        }
                        else
                        {
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
        public static void AltModifyPortals()
        {
            Dictionary<string, PortalCombo> portalComboList = APPortalPairs;
            var Portals = Resources.FindObjectsOfTypeAll<ScenePortal>();
            foreach (var portal in Portals)
            {
                // go through the list of randomized portals and see if either the first or second portal matches the one we're looking at
                foreach (KeyValuePair<string, PortalCombo> portalCombo in portalComboList)
                {
                    string comboTag = portalCombo.Key;
                    Portal portal1 = portalCombo.Value.Portal1;
                    Portal portal2 = portalCombo.Value.Portal2;
                    Logger.LogInfo("step 6");
                    Logger.LogInfo("portal.id == " + portal.id);
                    Logger.LogInfo("portal.destinationSceneName == " + portal.destinationSceneName);
                    Logger.LogInfo("portal1.Destination == " + portal1.Destination);
                    Logger.LogInfo("portal1.Tag == " + portal1.Tag);
                    Logger.LogInfo("portal1.Scene == " + portal1.Scene);
                    
                    if (portal1.Scene == "Overworld Redux" && portal1.Tag == portal.id && portal1.Destination == portal.destinationSceneName)
                    {
                        if (portal2.Scene == "Shop")
                        {
                            portal.destinationSceneName = portal2.Scene;
                            portal.id = "";
                            portal.optionalIDToSpawnAt = "";
                        }
                        else
                        {
                            portal.destinationSceneName = portal2.Scene;
                            portal.id = comboTag;
                            portal.optionalIDToSpawnAt = comboTag + comboTag + comboTag + comboTag; // quadrupling since doubling and tripling can have overlaps
                        }
                        break;
                    }

                    if (portal2.Scene == "Overworld Redux" && portal2.Tag == portal.id && portal2.Destination == portal.destinationSceneName)
                    {
                        if (portal1.Scene == "Shop")
                        {
                            portal.destinationSceneName = portal1.Scene;
                            portal.id = "";
                            portal.optionalIDToSpawnAt = "";
                        }
                        else
                        {
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
