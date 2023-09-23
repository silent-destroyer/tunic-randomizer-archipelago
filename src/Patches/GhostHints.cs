using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BepInEx.Logging;
using static TunicArchipelago.SaveFlags;
using UnityEngine.SceneManagement;

namespace TunicArchipelago {

    public class GhostHints {

        public struct ArchipelagoHint {
            public string Item;
            public string Location;
            public long Player;

            public ArchipelagoHint(string item, string location, long player) { 
                Item = item; 
                Location = location; 
                Player = player;
            }
        }

        public class HintGhost {
            public string SceneName;
            public Vector3 Position;
            public Quaternion Rotation;
            public NPC.NPCAnimState AnimState;
            public string Dialogue;
            public string Hint;

            public HintGhost() { }

            public HintGhost(string sceneName, Vector3 position, Quaternion rotation, NPC.NPCAnimState animState, string dialogue) {
                SceneName = sceneName;
                Position = position;
                Rotation = rotation;
                Dialogue = dialogue;
                AnimState = animState;
                Hint = "";
            }
        }

        private static ManualLogSource Logger = TunicArchipelago.Logger;

        public static GameObject GhostFox;

        public static List<char> Vowels = new List<char>() { 'A', 'E', 'I', 'O', 'U' };
        public static List<string> LocationHints = new List<string>();
        public static List<string> ItemHints = new List<string>();
        public static List<string> BarrenAndTreasureHints = new List<string>();

        public static List<HintGhost> HintGhosts = new List<HintGhost>();

        public static Dictionary<string, string> HintableLocationIds = new Dictionary<string, string>() {
            { "1005 [Swamp Redux 2]", "FOUR SKULLS" },
            { "1006 [East Forest Redux]", "EAST FOREST SLIME" },
            { "999 [Cathedral Arena]", "CATHEDRAL GAUNTLET" },
            { "Vault Key (Red) [Fortress Arena]", "SIEGE ENGINE" },
            { "Hexagon Green [Library Arena]", "LIBRARIAN" },
            { "Hexagon Blue [ziggurat2020_3]", "SCAVENGER BOSS" },
            { "Hexagon Red [Fortress Arena]", "VAULT KEY" },
            { "1007 [Waterfall]", "20 FAIRIES" },
            { "Well Reward (10 Coins) [Trinket Well]", "10 COIN TOSSES" },
            { "Well Reward (15 Coins) [Trinket Well]", "15 COIN TOSSES" },
            { "1011 [Dusty]", "PILES OF LEAVES" },
            { "Archipelagos Redux-(-396.3, 1.4, 42.3) [Archipelagos Redux]", "WEST GARDEN TREE" },
            { "final [Mountaintop]", "TOP OF THE MOUNTAIN" }
        };

        public static List<string> HintableItemNames = new List<string>() {
            "Stick",
            "Sword",
            "Sword Upgrade",
            "Gun",
            "Shield",
            "Hourglass",
            "Scavenger Mask",
            "Old House Key",
            "Hero Relic - ATT",
            "Hero Relic - DEF",
            "Hero Relic - POTION",
            "Hero Relic - HP",
            "Hero Relic - SP",
            "Hero Relic - MP",
            "Dath Stone",
        };

        public static List<string> BarrenItemNames = new List<string>() {
            "Firecracker x2",
            "Firecracker x3",
            "Firecracker x4",
            "Firecracker x5",
            "Firecracker x6",
            "Fire Bomb x2",
            "Fire Bomb x3",
            "Ice Bomb x2",
            "Ice Bomb x3",
            "Ice Bomb x5",
            "Pepper x2",
            "Ivy x3",
            "Lure",
            "Lure x2",
            "Effigy",
            "HP Berry",
            "HP Berry x2",
            "HP Berry x3",
            "MP Berry",
            "MP Berry x2",
            "MP Berry x3",
            "Money x1",
            "Money x10",
            "Money x15",
            "Money x16",
            "Money x20",
            "Money x25",
            "Money x30",
            "Money x32",
            "Money x40",
            "Money x48",
            "Money x50",
            "Money x64",
            "Money x100",
            "Money x128",
            "Money x200",
            "Money x255",
        };

        public static Dictionary<string, List<HintGhost>> GhostLocations = new Dictionary<string, List<HintGhost>>() {
            { "Sword Cave", new List<HintGhost>() {
                new HintGhost("Sword Cave", new Vector3(5.1151f, 0.0637f, 12.6657f), new Quaternion(0f, 0.9642988f, 0f, 0.2648164f), NPC.NPCAnimState.SIT, $"its dAnjuris too gO uhlOn, tAk #is hint:"), }
            },
            { "Far Shore", new List<HintGhost>() {
                new HintGhost("Overworld Redux", new Vector3(8.45f, 12.0833f, -204.9243f), new Quaternion(0f, 0.4226183f, 0f, -0.9063078f), NPC.NPCAnimState.IDLE, $"wAr did yoo kuhm fruhm, tInE fawks?"),
                new HintGhost("Transit", new Vector3(-18.6177f, 8.0314f, -81.6153f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, "I stoud awn #aht skwAr ahnd ehndid uhp hEr suhmhow.\nwAr igzahktlE R wE?" ) }
            },
            { "Ruined Passage", new List<HintGhost>() {
                new HintGhost("Ruins Passage", new Vector3(184.1698f, 17.3268f, 40.54981f), new Quaternion(0f, 0.9659258f, 0f, 0.2588191f), NPC.NPCAnimState.TIRED, $"nahp tIm! haw haw haw... geht it?") }
            },
            { "Windmill", new List<HintGhost>() {
                new HintGhost("Windmill", new Vector3(-58.33329f, 54.0833f, -27.8653f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"viziti^ #uh \"SHOPKEEPER\"? doo nawt bE uhlRmd, #A R\nA frehnd.") }
            },
            { "Old House Back", new List<HintGhost>() {
                new HintGhost("Overworld Interiors", new Vector3(11.0359f, 29.0833f, -7.3707f), new Quaternion(0f, 0.8660254f, 0f, -0.5000001f), NPC.NPCAnimState.PRAY, $"nuh%i^ wurks! mAbE #Arz suhm trik too #is dor...") }
            },
            { "Old House Front", new List<HintGhost>() {
                new HintGhost("Overworld Interiors", new Vector3(-24.06613f, 27.39948f, -47.9316f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.TIRED, $"juhst fIv mor minits..."),
                new HintGhost("Overworld Interiors", new Vector3(12.0368f, 21.1446f, -72.81052f), new Quaternion(0f, 0.8660254f, 0f, -0.5000001f), NPC.NPCAnimState.SIT, $"wuht R #Ez pehduhstuhlz for? doo yoo nO?") }
            },
            { "Overworld Above Ruins", new List<HintGhost>() {
               new HintGhost("Overworld Redux", new Vector3(28.53184f, 36.0833f, -108.3734f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.IDLE, $"I wuhz hIdi^ fruhm #uh \"SLIMES,\" buht yoo dOnt louk\nlIk wuhn uhv #ehm."),
               new HintGhost("Overworld Redux", new Vector3(22.3667f, 27.9833f, -126.3728f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"wAr did I lEv %aht kE..."),
               new HintGhost("Overworld Redux", new Vector3(51.20462f, 28.00694f, -129.722f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.SIT, $"I %awt #aht Jehst wuhz ehmptE. how suhspi$is.") }
            },
            { "Early Overworld Spawns", new List<HintGhost>() {
               new HintGhost("Overworld Redux", new Vector3(-9.441f, 43.9363f, -8.4385f), new Quaternion(0f, 0.7069f, 0f, -0.7069f), NPC.NPCAnimState.SIT, $"sEld forehvur? nO... #Ar muhst bE uhnuh#ur wA..."),
               new HintGhost("Overworld Redux", new Vector3(-34.0649f, 37.9833f, -59.2506f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.GAZE, $"sO mehnE roodli^z. Im stAi^ uhp hEr.") }
            },
            { "Inside Temple", new List<HintGhost>() {
                new HintGhost("Temple", new Vector3(7.067f, -0.224f, 59.9285f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.IDLE, $"yur naht uh \"RUIN SEEKER,\" R yoo? mAbE yoo $oud gO\nsuhmwAr ehls."),
                new HintGhost("Temple", new Vector3(0.9350182f, 4.076f, 133.7965f), new Quaternion(0f, 0.8660254f, 0f, 0.5f), NPC.NPCAnimState.GAZE_UP, $"yur guhnuh frE \"THE HEIR\"? iznt #aht... bahd?") }
            },
            { "Ruined Shop", new List<HintGhost>() {
                new HintGhost("Ruined Shop", new Vector3(16.5333f, 8.983299f, -45.60382f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"hehlO. wuht iz yor nAm?---...tuhnk? wuht A strAnj nAm."),
                new HintGhost("Ruined Shop", new Vector3(9.8111f, 8.0833f, -37.52119f), new Quaternion(0f, 0.9659258f, 0f, 0.2588191f), NPC.NPCAnimState.IDLE, $"wehl, if yur nawt bIi^ ehnE%i^..." ) }
            },
            { "West Filigree", new List<HintGhost>() {
                new HintGhost("Town_FiligreeRoom", new Vector3(-79.4348f, 22.0379f, -59.8104f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.PRAY, $"wow, yoo hahv #uh powur uhv #uh \"Holy Cross\"!") }
            },
            { "East Filigree", new List<HintGhost>() {
                new HintGhost("EastFiligreeCache", new Vector3(14.3719f, 0.0167f, -8.8614f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"wAt, how did yoo Opehn #aht dOr?") }
            },
            { "Maze Room", new List<HintGhost>() {
                new HintGhost("Maze Room", new Vector3(3.5129f,-0.1167f,-9.4481f), new Quaternion(0f,0f,0f,1f), NPC.NPCAnimState.IDLE, $"wAt... how kahn yoo wahk in hEr? #Arz nO flOr!" ) }
            },
            { "Changing Room", new List<HintGhost>() {
                new HintGhost("Changing Room", new Vector3(14.9876f, 6.9379f, 14.6771f), new Quaternion(0f, 0.9238796f, 0f, -0.3826834f), NPC.NPCAnimState.PRAY, $"doo yoo %ink #is louks goud awn mE?"),
                new HintGhost("Changing Room", new Vector3(14.9876f, 6.9379f, 14.6771f), new Quaternion(0f, 0.9238796f, 0f, -0.3826834f), NPC.NPCAnimState.DANCE,  $"doo yoo nO sIlehntdistroiur? hE iz kool.") }
            },
            { "Waterfall", new List<HintGhost>() {
                new HintGhost("Waterfall", new Vector3(-41.13461f, 44.9833f, -0.6913f), new Quaternion(0f, 0.6755902f, 0f, -0.7372773f), NPC.NPCAnimState.IDLE, $"doo yoo nO wuht #uh fArEz R sAi^?") }
            },
            { "Hourglass Cave", new List<HintGhost>() {
                new HintGhost("Town Basement", new Vector3(-211.3147f, 1.0833f, 35.7667f), new Quaternion(0f, 0.4226183f, 0f, 0.9063078f), NPC.NPCAnimState.GAZE, $"dOnt gO in #Ar. kahnt yoo rEd %uh sIn?") }
            },
            { "Overworld Cave", new List<HintGhost>() {
                new HintGhost("Overworld Cave", new Vector3(-88.0794f, 515.1076f, -741.0837f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"sO pritE!" ) }
            },
            { "Patrol Cave", new List<HintGhost>() {
                new HintGhost("PatrolCave", new Vector3(80.41302f, 46.0686f, -48.0821f), new Quaternion(0f, 0.9848078f, 0f, -0.1736482f), NPC.NPCAnimState.GAZE, $"#Arz ahlwAz uh sEkrit bEhInd #uh wahturfahl!"),
                new HintGhost("PatrolCave", new Vector3(72.6667f, 46.0686f, 14.9446f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.GAZE, $"klA^ klA^ klA^... duhzint hE ehvur stawp?") }
            },
            { "Cube Room", new List<HintGhost>() {
                new HintGhost("CubeRoom", new Vector3(326.784f, 3.0833f, 207.0065f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.DANCE, $"rIt ahnd uhp! rit ahnd uhp!") }
            },
            { "Furnace", new List<HintGhost>() {
                new HintGhost("Furnace", new Vector3(-131.9886f, 12.0833f, -51.0197f), new Quaternion(0f, 0f, 0f, 1f), NPC.NPCAnimState.GAZE_UP, $"#Ez powur sorsehz... I dOnt truhst #ehm.") }
            },
            { "Golden Obelisk", new List<HintGhost>() {
                new HintGhost("Overworld Redux", new Vector3(-94.5973f, 70.0937f, 36.38749f), new Quaternion(0f, 0f, 0f, 1f), NPC.NPCAnimState.FISHING, $"pEpuhl yoost too wur$ip #is. it rehprEzehnts #uh\n\"Holy Cross.\"") }
            },
            { "Overworld Before Garden", new List<HintGhost>(){
                new HintGhost("Overworld Redux", new Vector3(-146.1464f, 11.6929f, -67.55009f), new Quaternion(0f, 0.3007058f, 0f, 0.9537169f), NPC.NPCAnimState.IDLE, "A vi$is baws blawks #uh wA too #uh behl uhp #Ar.\nbE kArfuhl, it wil kil yoo.") }
            },
            { "West Garden", new List<HintGhost>() {
                new HintGhost("Archipelagos Redux", new Vector3(-290.3334f, 4.0667f, 153.9145f), new Quaternion(0f, 0.9659259f, 0f, -0.2588191f), NPC.NPCAnimState.GAZE, $"wawJ owt for tArE uhp uhhehd. hE wil trI too Jawmp yoo."),
                new HintGhost("Archipelagos Redux", new Vector3(-137.9978f, 2.0781f, 150.5348f), new Quaternion(0f, 0.9238796f, 0f, -0.3826834f), NPC.NPCAnimState.GAZE, $"iz #is pRt uhv suhm%i^ ehls? hmm..."),
                new HintGhost("Archipelagos Redux", new Vector3(-190.6887f, 2.0667f, 126.7101f), new Quaternion(0f, 0.3826835f, 0f, -0.9238795f), NPC.NPCAnimState.FISHING, $"bE kArfuhl if yoo R gOi^ uhp #Ar. #aht mawnstur iz nO jOk."),
                new HintGhost("Archipelagos Redux", new Vector3(-256.3194f, 4.1667f, 168.15f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.FISHING, $"doo #A louk fuhmilyur too yoo?") }
            },
            { "West Bell", new List<HintGhost>() {
                new HintGhost("Overworld Redux", new Vector3(-130.929f, 40.0833f, -51.5f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.SIT, $"its sO kwIeht hEr... goud #i^ nObuhdE rA^ #aht behl.") }
            },
            { "Ice Dagger House", new List<HintGhost>() {
                new HintGhost("archipelagos_house", new Vector3(-201.1842f, 3.1209f, 38.4875f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.PRAY, $"Is too mEt yoo!") }
            },
            { "East Belltower Lower", new List<HintGhost>() {
                new HintGhost("Forest Belltower", new Vector3(500.9258f, 13.9394f, 63.79896f), new Quaternion(0f, 0.9659258f, 0f, 0.2588191f), NPC.NPCAnimState.SIT, $"#uh lahdur brOk ahnd #Ar R ehnehmEz owtsId, buht #is stahJoo\nawfurz sAftE.") }
            },
            { "East Belltower Upper", new List<HintGhost>() {
                new HintGhost("Forest Belltower", new Vector3(500.3264f, 62.012f, 107.5831f), new Quaternion(0f, 0.9238796f, 0f, -0.3826834f), NPC.NPCAnimState.GAZE, $"di^ daw^! doo yoo hahv wuht it tAks?"),
                new HintGhost("Forest Belltower", new Vector3(593.9467f, 14.0052f, 84.43121f), new Quaternion(0f, 0.9238796f, 0f, -0.3826834f), NPC.NPCAnimState.IDLE, $"wow... yoo did it!"),
                new HintGhost("East Forest Redux Laddercave", new Vector3(159.0245f, 17.89421f, 78.52466f), new Quaternion(0f, 0.9238796f, 0f, -0.3826834f), NPC.NPCAnimState.PRAY, $"#Arz uh baws uhhehd... buht hE hahz por vi&uhn.")}
            },
            { "Swamp Redux 2", new List<HintGhost>() {
                new HintGhost("Swamp Redux 2", new Vector3(-47f, 16.0463f, -31.3333f), new Quaternion(0f, 0f, 0f, 1f), NPC.NPCAnimState.GAZE, $"I kahn sE mI hows fruhm hEr!" ),
                new HintGhost("Swamp Redux 2", new Vector3(-90.55162f, 3.0462f, 6.2667f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"suhm%i^ $oud bE hEr rIt? I dOnt rEmehmbur..." ),
                new HintGhost("Swamp Redux 2", new Vector3(-100.5333f, 3.3462f, 25.0965f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"I %ink #is wuhz pRt uhv suhm%i^ ehls wuhns." ) }
            },
            { "Dark Tomb", new List<HintGhost>() {
                new HintGhost("Crypt Redux", new Vector3(-75.8704f, 57.0833f, -56.2025f), new Quaternion(0f, 0.3826835f, 0f, -0.9238795f), NPC.NPCAnimState.GAZE, $"dRk! its sO dRk!"),
                new HintGhost("Sewer_Boss", new Vector3(70.30289f, 9.4138f, -9.387097f), new Quaternion(0f, 0.7660444f, 0f, 0.6427876f), NPC.NPCAnimState.GAZE, $"wuht koud bE bahk #Ar?") }
            },
            { "Fortress Courtyard", new List<HintGhost>() {
                new HintGhost("Fortress Courtyard", new Vector3(-50.54346f, 0.0417f, -36.46348f), new Quaternion(0f, 0.9659259f, 0f, -0.2588191f), NPC.NPCAnimState.GAZE, $"yoo gO furst. spIdurs giv mE #uh krEps..."),
                new HintGhost("Fortress Courtyard", new Vector3(6.967727f, 0.0417f, -74.5881f), new Quaternion(0f, 0.9238796f, 0f, -0.3826834f), NPC.NPCAnimState.IDLE, $"wehl wehl wehl... sawrE."),
                new HintGhost("Fortress Courtyard", new Vector3(7.299674f, 9.0417f, -89.57533f), new Quaternion(0f, 0f, 0f, 1f), NPC.NPCAnimState.FISHING, $"sO mehnE kahndlz! hahpE bur%dA!"),
                new HintGhost("Fortress Courtyard", new Vector3(11.6453f, 4.0203f, -115.355f), new Quaternion(0f, 0.9238796f, 0f, -0.3826834f), NPC.NPCAnimState.GAZE, $"I woud nawt tuhJ #aht wuhn. hoo nOz wuht #Al doo wi%\n#uh powur?") }
            },
            { "Mountain Door", new List<HintGhost>() {
                new HintGhost("Mountain", new Vector3(54.7674f, 41.5568f, 4.4282f), new Quaternion(0f, 0.3826835f, 0f, -0.9238795f), NPC.NPCAnimState.GAZE_UP, $"yoo kahn Opehn #is? uhmAzi^!") }
            },
            { "Atoll Entrance", new List<HintGhost>() {
                new HintGhost("Atoll Redux", new Vector3(-3.5443f, 1.0174f, 120.0543f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"burdsaw^ iz sO rElahksi^. twEt twEt twEt!"),
                new HintGhost("Atoll Redux", new Vector3(4.7f, 16.0776f, 101.9315f), new Quaternion(0f, 0.7071068f, 0f, -0.7071068f), NPC.NPCAnimState.SIT, $"#Ez skwArz...wuht purpis doo #A surv?"),
                new HintGhost("Atoll Redux", new Vector3(0.4395638f, 16.0874f, 64.47866f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.GAZE, $"tIm hahz tAkin ahtOl awn #is plAs.") }
            },
            { "Frog's Domain", new List<HintGhost>() {
                new HintGhost("frog cave main", new Vector3(19.7682f, 9.1943f, -23.3269f), new Quaternion(0f, 1f, 0f, -4.371139E-08f), NPC.NPCAnimState.FISHING, $"I wuhndur wAr #uh kwehstuhgawn iz?"),
                new HintGhost("frog cave main", new Vector3(27.09619f, 9.2581f, -37.28336f), new Quaternion(0f, 0.5000001f, 0f, -0.8660254f), NPC.NPCAnimState.FISHING, $"$hhh. Im hIdi^ fruhm #uh frawgs.") }
            },

        };

        public static void InitializeGhostFox() {
            try {
                GhostFox = GameObject.Instantiate(Resources.FindObjectsOfTypeAll<NPC>().Where(npc => npc.name == "NPC_greeter").ToList()[0].gameObject);
                GameObject.DontDestroyOnLoad(GhostFox);
                GhostFox.SetActive(false);
            } catch (Exception e) {
                Logger.LogInfo("Error initalizing ghost foxes for hints!");
            }
        }

        public static void SpawnHintGhosts(string SceneName) {
            foreach (HintGhost HintGhost in HintGhosts) {
                if (HintGhost.SceneName == SceneName) {
                    GhostFox.GetComponent<NPC>().nPCAnimState = HintGhost.AnimState;
                    GameObject NewGhostFox = GameObject.Instantiate(GhostFox);
                    NewGhostFox.transform.position = HintGhost.Position;
                    NewGhostFox.transform.rotation = HintGhost.Rotation;
                    LanguageLine HintText = ScriptableObject.CreateInstance<LanguageLine>();
                    HintText.text = $"{HintGhost.Dialogue}---{HintGhost.Hint}";
                    NewGhostFox.GetComponent<NPC>().script = HintText;

                    if (PaletteEditor.CelShadingEnabled && PaletteEditor.ToonFox != null) {
                        NewGhostFox.transform.GetChild(2).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = PaletteEditor.ToonFox.GetComponent<MeshRenderer>().material;
                    }

                    NewGhostFox.SetActive(true);
                }
            }
        }

        public static void GenerateHints() {
            HintGhosts.Clear();
            List<string> GhostSpawns = GhostLocations.Keys.ToList();
            List<string> SelectedSpawns = new List<string>();
            System.Random random = new System.Random(SaveFile.GetInt("seed"));
            for (int i = 0; i < 15; i++) {
                string Location = GhostSpawns[random.Next(GhostSpawns.Count)];
                SelectedSpawns.Add(Location);
                GhostSpawns.Remove(Location);
            }
            foreach (string Location in SelectedSpawns) {
                HintGhost HintGhost = GhostLocations[Location][random.Next(GhostLocations[Location].Count)];
                HintGhosts.Add(HintGhost);
            }

            GenerateLocationHints();
            GenerateItemHints();
            GenerateBarrenAndMoneySceneHints();

            List<string> Hints = new List<string>();
            for (int i = 0; i < 5; i++) {
                string LocationHint = LocationHints[random.Next(LocationHints.Count)];
                Hints.Add(LocationHint);
                LocationHints.Remove(LocationHint);
            }
            for (int i = 0; i < 7; i++) {
                if (ItemHints.Count > 0) {
                    string ItemHint = ItemHints[random.Next(ItemHints.Count)];
                    Hints.Add(ItemHint);
                    ItemHints.Remove(ItemHint);
                }
            }
            for (int i = 0; i < 3; i++) {
                if (BarrenAndTreasureHints.Count > 0) {
                    string BarrenHint = BarrenAndTreasureHints[random.Next(BarrenAndTreasureHints.Count)];
                    Hints.Add(BarrenHint);
                    BarrenAndTreasureHints.Remove(BarrenHint);
                }
            }

            while (Hints.Count < 15) {
                if (ItemHints.Count > 0) {
                    string ItemHint = ItemHints[random.Next(ItemHints.Count)];
                    Hints.Add(ItemHint);
                    ItemHints.Remove(ItemHint);
                } else if (LocationHints.Count > 0) {
                    string LocationHint = LocationHints[random.Next(LocationHints.Count)];
                    Hints.Add(LocationHint);
                    LocationHints.Remove(LocationHint);
                } else {
                    break;
                }
            }
            if (Hints.Count < 15) {
                HintGhosts = HintGhosts.Take(Hints.Count).ToList();
            }
            foreach (HintGhost HintGhost in HintGhosts) {
                string Hint = Hints[random.Next(Hints.Count)];
                HintGhost.Hint = Hint;
                Hints.Remove(Hint);
            }
        }
        
        public static void GenerateLocationHints() {
            LocationHints.Clear();
            List<String> HintableLocations = HintableLocationIds.Keys.ToList();
            if(SaveFile.GetInt(KeysBehindBosses) == 1) {
                // Remove boss hints if keys behind bosses is on
                HintableLocations.Remove("Vault Key (Red) [Fortress Arena]");
                HintableLocations.Remove("Hexagon Green [Library Arena]");
                HintableLocations.Remove("Hexagon Blue [ziggurat2020_3]");
            }
            foreach (string Key in HintableLocations) {
                ArchipelagoItem Item = ItemLookup.ItemList[Key];
                string Location = HintableLocationIds[Key];
                string LocationSuffix = Location[Location.Length - 1] == 'S' ? "R" : "iz";
                string ItemPrefix = Item.ItemName.Contains("Money") ? "suhm" : Vowels.Contains(Item.ItemName.ToUpper()[0]) ? "ahn" : "uh";
                string PlayerName = Archipelago.instance.GetPlayerName(Item.Player);
                string Hint = $"bI #uh wA, I hurd #aht \"{HintableLocationIds[Key].Replace(" ", "\" \"")}\" {LocationSuffix} gRdi^ \"{PlayerName.ToUpper().Replace(" ", "\" \"")}'S {Item.ItemName.ToUpper().Replace(" ", "\" \"").Replace("_", "\" \"")}.\"";

                LocationHints.Add(WordWrapString(Hint));
            }
        }

        public static void GenerateItemHints() {
            ItemHints.Clear();

            string Hint = "";

            List<string> HintableItems = new List<string>(HintableItemNames);
            if (SaveFile.GetInt(AbilityShuffle) == 1) {
                HintableItems.Add("Pages 52-53 (Ice Rod)");
            }
            for (int i = 0; i < HintableItems.Count; i++) {
                string Item = HintableItems[i];
                List<ArchipelagoHint> ItemLocations = Locations.MajorItemLocations[Item];
                foreach(ArchipelagoHint HintLocation in ItemLocations) {
                    if (HintLocation.Player == Archipelago.instance.GetPlayerSlot()) {
                        string Scene = HintLocation.Location == "Your Pocket" ? HintLocation.Location.ToUpper() : Locations.SimplifiedSceneNames[Locations.VanillaLocations[Locations.LocationDescriptionToId[HintLocation.Location]].Location.SceneName].ToUpper();
                        string ScenePrefix = Scene == "Trinket Well" ? "%rOi^" : "aht #uh";
                        Hint = $"bI #uh wA, I saw A \"{Item.ToUpper().Replace(" ", "\" \"")}\" #uh lahst tIm I wuhs {ScenePrefix} \"{Scene.Replace(" ", "\" \"")}.\"";

                        ItemHints.Add(WordWrapString(Hint));
                    }
                }
            }
            if (SaveFile.GetInt(HexagonQuestEnabled) == 1 && SaveFile.GetInt(AbilityShuffle) == 1) {
                ItemHints.Add($"bI #uh wA, I hurd #aht \"{SaveFile.GetInt(HexagonQuestPrayer)} GOLD QUESTAGONS\"\nwil grahnt yoo #uh powur uhv \"PRAYER.\"");
                ItemHints.Add($"bI #uh wA, I hurd #aht \"{SaveFile.GetInt(HexagonQuestHolyCross)} GOLD QUESTAGONS\"\nwil grahnt yoo #uh powur uhv #uh \"HOLY CROSS.\"");
                ItemHints.Add($"bI #uh wA, I hurd #aht \"{SaveFile.GetInt(HexagonQuestIceRod)} GOLD QUESTAGONS\"\nwil grahnt yoo #uh #uh powur uhv #uh \"ICE ROD.\"");
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

            return formattedHint.Replace($"\" \"", $" ");
        }

        public static void GenerateBarrenAndMoneySceneHints() {
            BarrenAndTreasureHints.Clear();
            foreach (string Key in Locations.SimplifiedSceneNames.Keys) { 
                HashSet<string> ItemsInScene = new HashSet<string>();
                List<ArchipelagoItem> APItemsInScene = new List<ArchipelagoItem>();
                string Scene = Locations.SimplifiedSceneNames[Key];
                int SceneItemCount = 0;
                int MoneyInScene = 0;
                foreach (string ItemKey in ItemLookup.ItemList.Keys.Where(item => Locations.VanillaLocations[item].Location.SceneName == Key).ToList()) {
                    ArchipelagoItem Item = ItemLookup.ItemList[ItemKey];
                    ItemsInScene.Add(Item.ItemName);
                    APItemsInScene.Add(Item);
                    if (Item.Player == Archipelago.instance.GetPlayerSlot() && ItemLookup.Items[Item.ItemName].Type == ItemTypes.MONEY) {
                        MoneyInScene += ItemLookup.Items[Item.ItemName].QuantityToGive;
                    }
                    SceneItemCount++;
                }

                if (MoneyInScene >= 200 && SceneItemCount < 10) {
                    string ScenePrefix = Vowels.Contains(Scene[0]) ? "#E" : "#uh";
                    BarrenAndTreasureHints.Add($"ahn EzE plAs too fInd A \"LOT OF MONEY\" iz {ScenePrefix}\n\"{Scene.ToUpper()}.\"");
                } else {
                    bool BarrenArea = true;
                    foreach(ArchipelagoItem Item in APItemsInScene) {
                        if (Item.Player != Archipelago.instance.GetPlayerSlot()) {
                            BarrenArea = false;
                            break;
                        }
                        if(!BarrenItemNames.Contains(Item.ItemName)) {
                            BarrenArea = false;
                            break;
                        }
                        if (HintGhosts.Where(HintGhost => HintGhost.SceneName == Key).ToList().Count > 0) {
                            BarrenArea = false;
                            break;
                        }
                    }
                    if(BarrenArea) {
                        string Hint = "";
                        if (Scene.Length > 15) {
                            string[] SceneSplit = Scene.Split(' ');
                            Hint = $"if I wur yoo, I woud uhvoid \"{String.Join(" ", SceneSplit.Take(SceneSplit.Length - 1)).ToUpper()}\"\n\"{SceneSplit[SceneSplit.Length - 1].ToUpper()}.\" #aht plAs iz \"NOT IMPORTANT.\"";
                        } else {
                            Hint = $"if I wur yoo, I woud uhvoid \"{Scene.ToUpper()}.\"\n#aht plAs iz \"NOT IMPORTANT.\"";
                        }
                        BarrenAndTreasureHints.Add(Hint);
                    }
                }

            }
        }

        /*        public static List<ItemData> FindAllRandomizedItemsByName(string ItemName) {
                    List<ItemData> Items = new List<ItemData>();
                    foreach (string Key in ItemRandomizer.ItemList.Keys) {
                        ItemData Item = ItemRandomizer.ItemList[Key];
                        if (Item.Reward.Name == ItemName) {
                            Items.Add(Item);
                        }
                    }
                    return Items;
                }
        */
        public static void ResetGhostHints() {
            HintGhosts.Clear();
            LocationHints.Clear();
            ItemHints.Clear();
            BarrenAndTreasureHints.Clear();
        }

    }
}
