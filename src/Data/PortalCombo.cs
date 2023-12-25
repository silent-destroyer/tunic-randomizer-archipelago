using System.Collections.Generic;

namespace TunicArchipelago {
    public class PortalCombo {
        //private static ManualLogSource Logger = TunicRandomizer.Logger;
        public Portal Portal1 { get; set; }

        public Portal Portal2 { get; set; }

        public List<Portal> Portals { get; set; }

        public PortalCombo() {}

        public PortalCombo(Portal portal1, Portal portal2) {
            Portal1 = portal1;
            Portal2 = portal2;
            Portals = new List<Portal> {
                portal1,
                portal2
            };
        }

    }
}
