using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace TunicArchipelago {
    public class ConnectionSettings {
        public string Player {
            get;
            set;
        }

        public string Hostname {
            get;
            set;
        }

        public int Port {
            get;
            set;
        }

        public ConnectionSettings() {
            Player = "Silent";
            Hostname = "localhost";
            Port = 38281;
        }
    }
}
