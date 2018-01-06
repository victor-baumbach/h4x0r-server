using System.Collections.Generic;

namespace h4x0r
{
    public class BlackSphereSales
    {
        public BlackSphereSales()
        {
            Software = new Dictionary<File.Type, SoftwareEntry>();

            Software[File.Type.Cracker] = new SoftwareEntry("Cracker", File.Type.Cracker, 0);
            Software[File.Type.NodeAnalyser] = new SoftwareEntry("Node analyser", File.Type.NodeAnalyser, 1000);
            Software[File.Type.VirtualIntelligence] = new SoftwareEntry("Virtual intelligence", File.Type.VirtualIntelligence, 1000);
            Software[File.Type.Siphon] = new SoftwareEntry("Siphon", File.Type.Siphon, 5000);
            Software[File.Type.MinerVirus] = new SoftwareEntry("Miner virus", File.Type.MinerVirus, 5000);
            Software[File.Type.Fracter] = new SoftwareEntry("Fracter", File.Type.Fracter, 20000);
            Software[File.Type.ProxyBypasser] = new SoftwareEntry("Proxy bypasser", File.Type.ProxyBypasser, 50000);
            Software[File.Type.SSHTunnel] = new SoftwareEntry("SSH Tunnel", File.Type.SSHTunnel, 100000);
            Software[File.Type.ExploitHTTP] = new SoftwareEntry("HTTP exploit", File.Type.ExploitHTTP, 250000);
            Software[File.Type.ExploitFTP] = new SoftwareEntry("FTP exploit", File.Type.ExploitFTP, 250000);
            Software[File.Type.ExploitSSH] = new SoftwareEntry("SSH exploit", File.Type.ExploitSSH, 250000);
            Software[File.Type.ExploitSMTP] = new SoftwareEntry("SMTP exploit", File.Type.ExploitSMTP, 250000);
            Software[File.Type.ExploitDatabase] = new SoftwareEntry("Database exploit", File.Type.ExploitDatabase, 250000);
            Software[File.Type.FeedbackLoop] = new SoftwareEntry("Feedback loop", File.Type.FeedbackLoop, 500000);
            Software[File.Type.FeedbackVirus] = new SoftwareEntry("Feedback virus", File.Type.FeedbackVirus, 500000);
            Software[File.Type.TraceKill] = new SoftwareEntry("Trace kill", File.Type.TraceKill, 1000000);
        }

        public class SoftwareEntry
        {
            public SoftwareEntry(string name, File.Type type, int price)
            {
                Name = name;
                Type = type;
                Price = price;
            }

            public string Name { get; set; }
            public File.Type Type { get; set; }
            public int Price { get; set; }
        }

        public Dictionary<File.Type, SoftwareEntry> Software;
    }
}
