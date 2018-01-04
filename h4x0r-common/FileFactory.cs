namespace h4x0r
{
    public class FileFactory
    {
        static public File Create(File.Type type)
        {
            File file = new File();

            switch (type)
            {
                case File.Type.Generic:
                    file.CanBeExecuted = File.Permission.Never;
                    file.CanBeDeleted = File.Permission.Always;
                    break;

                /////////////////////////////////////////////////////////
                // Various types of ICE 
                /////////////////////////////////////////////////////////

                case File.Type.Obfuscator:
                    file.Name = "obfuscator";
                    break;

                case File.Type.PasswordGate:
                    file.Name = "password_gate";
                    break;

                case File.Type.Firewall:
                    file.Name = "firewall";
                    break;

                case File.Type.ConnectionSentry:
                    file.Name = "connection_sentry";
                    break;

                case File.Type.Proxy:
                    // Technically a proxy would actually be a separate server rather than something that can be executed.
                    // For all intents and purposes, it is hidden from the player as a file.
                    file.Name = "proxy_server";
                    file.CanBeExecuted = File.Permission.Never;
                    file.CanBeDeleted = File.Permission.Never;
                    file.MemoryUsage = 0;
                    file.DiskSpaceUsage = 0;
                    file.Visible = false;
                    break;

                case File.Type.NoiseWall:
                    file.Name = "noise_wall";
                    break;

                case File.Type.QuantumGate:
                    file.Name = "quantum_gate";
                    break;

                /////////////////////////////////////////////////////////
                // Icebreakers
                /////////////////////////////////////////////////////////

                case File.Type.NodeAnalyser:
                    file.Name = "node_analyser";
                    break;

                case File.Type.Cracker:
                    file.Name = "cracker";
                    break;

                case File.Type.Fracter:
                    file.Name = "fracter";
                    break;

                case File.Type.ProxyBypasser:
                    file.Name = "proxy_bypasser";
                    break;

                case File.Type.FeedbackLoop:
                    file.Name = "feedback_loop";
                    break;

                /////////////////////////////////////////////////////////
                // Utility
                /////////////////////////////////////////////////////////

                case File.Type.Tracer:
                    file.Name = "tracer";
                    break;

                case File.Type.TraceKill:
                    file.Name = "trace_kill";
                    file.SingleUse = true;
                    break;

                case File.Type.VirtualIntelligence:
                    file.Name = "virtual_intelligence";
                    file.Upgradeable = false;
                    break;

                case File.Type.ArtificialIntelligence:
                    file.Name = "artificial_intelligence";
                    file.Unique = true;
                    break;

                /////////////////////////////////////////////////////////
                // Viruses
                /////////////////////////////////////////////////////////

                case File.Type.MinerVirus:
                    file.Name = "miner_virus";
                    file.CanBeExecuted = File.Permission.RemoteOnly;
                    break;

                case File.Type.FeedbackVirus:
                    file.Name = "feedback_virus";
                    file.CanBeExecuted = File.Permission.RemoteOnly;
                    break;
            }

            return file;
        }
    }
}
