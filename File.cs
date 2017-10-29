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
            Generic,
            Ice,
            Icebreaker,
            Utility
        };

        public enum SubType
        {
            // Generic
            Generic,

            // Ice
            Firewall,
            Service,
            ConnectionSentry,
            PasswordGate,
            Proxy,

            // Icebreaker

            // Utility
            NodeAnalyser
        }

        public Type FileType { get; set; }
        public SubType FileSubType { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
    }
}
