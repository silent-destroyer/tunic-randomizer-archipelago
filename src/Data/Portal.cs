using BepInEx.Logging;
using System.Collections.Generic;
using UnityEngine.UI;

namespace TunicArchipelago {
    public class Portal {
        //private static ManualLogSource Logger = TunicRandomizer.Logger;
        public string Scene {
            get;
            set;
        }
        public string Destination {
            get;
            set;
        }

        public string Tag {
            get;
            set;
        }

        public string Name {
            get;
            set;
        }

        public string SceneDestinationTag
        {
            get;
            set;
        }

        public Portal(string scene, string destination, string tag, string name)
        {
            Scene = scene;
            Destination = destination;
            Tag = tag;
            Name = name;
            SceneDestinationTag = (Scene + ", " + Destination + "_" + Tag);
        }
    }
}
