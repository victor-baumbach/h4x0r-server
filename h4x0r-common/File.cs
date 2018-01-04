using System;

namespace h4x0r
{
    public class File
    {
        public File()
        {
            FileType = Type.Generic;
            Version = 10;
			CanBeDeleted = Permission.LocalOnly;
			CanBeExecuted = Permission.LocalOnly;
			EncryptionLevel = 0;
			MemoryUsage = 1;
            DiskSpaceUsage = 1;
			Unique = false;
            Visible = true;
            SingleUse = false;
            Upgradeable = true;
        }

        public enum Type
        {
            // Generic
            Generic,

            // Ice
            Obfuscator,
            PasswordGate,
            Firewall,
            ConnectionSentry,
            Proxy,
			NoiseWall,
			QuantumGate,
            ServiceHTTP,
            ServiceFTP,
            ServiceSSH,
            ServiceSMTP,
            ServiceDatabase,

            // Icebreaker
            NodeAnalyser,
            Cracker,
			Fracter,
            SSHTunnel,
			ProxyBypasser,
			FeedbackLoop,
            ExploitHTTP,
            ExploitFTP,
            ExploitSSH,
            ExploitSMTP,
            ExploitDatabase,
            Siphon,

            // Utility
            Tracer,
            TraceKill,
			VirtualIntelligence,
			ArtificialIntelligence,

            // Viruses
            MinerVirus,
            FeedbackVirus
        }
		
		public enum Permission
		{
			Always,
			Never,
			LocalOnly,
			RemoteOnly
		}

        public Type FileType { get; set; }

        public string Name // Base name, like "Calvinist"
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
                UpdateSystemName();
            }
        }

		public string SystemName
        {
            get
            {
                return m_SystemName;
            }
        }

        public int Version
        {
            get
            {
                return m_Version;
            }
            set
            {
                m_Version = value;
                UpdateSystemName();
            }
        }
		
		public Permission CanBeExecuted { get; set; }
		public Permission CanBeDeleted { get; set; }
		public int EncryptionLevel { get; set; }
		public bool Unique { get; set; }
		public int DiskSpaceUsage { get; set; }
        public int MemoryUsage { get; set; }
        public bool Visible { get; set; }
        public bool SingleUse { get; set; }
        public bool Upgradeable { get; set; }

        private void UpdateSystemName()
        {
            if (Version < 1)
            {
                m_SystemName = m_Name.ToLower();
            }
            else
            {
                int majorVersion = Version / 10;
                int minorVersion = Version % 10;
                m_SystemName = String.Format("{0}_{1}_{2}", m_Name.ToLower(), majorVersion, minorVersion);
            }
        }

        private string m_Name;
        private string m_SystemName;
        private int m_Version;
    }
}
