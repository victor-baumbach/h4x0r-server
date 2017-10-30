using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace h4x0r
{
    class File
    {
        public File()
        {
            FileType = Type.Generic;
            Version = 1;
        }

        public enum Type
        {
            // Generic
            Generic,

            // Ice
            Obfuscator,
            PasswordGate,
            Firewall,
            Service,
            ConnectionSentry,
            Proxy,

            // Icebreaker
            NodeAnalyser,
            Cracker,

            // Utility
            Tracer,
            Tracekill,
        }

        public Type FileType { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
    }
}
