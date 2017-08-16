using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace h4x0r_server
{
    class Account
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UInt64 NodeID { get; set; }
        public Int64 Reputation { get; set; }
        public Int64 Credits { get; set; }
        public bool Banned { get; set; }
    }
}
