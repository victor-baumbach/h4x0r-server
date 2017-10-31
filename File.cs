using System;

namespace h4x0r
{
    class File
    {
        public File()
        {
            FileType = Type.Generic;
            Version = 0;
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
			NoiseWall,
			QuantumGate,

            // Icebreaker
            NodeAnalyser,
            Cracker,
			Fracter,
			ProxyBypasser,
			FeedbackLoop,

            // Utility
            Tracer,
            Tracekill,
			VirtualIntelligence,
			ArtificialIntelligence,
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
