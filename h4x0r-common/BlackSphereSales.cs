using System.Collections.Generic;

namespace h4x0r
{
    public class BlackSphereSales
    {
        public BlackSphereSales()
        {
            Software = new Dictionary<File.Type, SoftwareEntry>();

            SoftwareEntry nodeAnalyser = new SoftwareEntry();
            nodeAnalyser.Name = "Node analyser";
            nodeAnalyser.Type = File.Type.NodeAnalyser;
            nodeAnalyser.Price = 100;
            Software[File.Type.NodeAnalyser] = nodeAnalyser;

            SoftwareEntry cracker = new SoftwareEntry();
            cracker.Name = "Cracker";
            cracker.Type = File.Type.Cracker;
            cracker.Price = 500;
            Software[File.Type.Cracker] = cracker;

            SoftwareEntry fracter = new SoftwareEntry();
            fracter.Name = "Fracter";
            fracter.Type = File.Type.Fracter;
            fracter.Price = 10000;
            Software[File.Type.Fracter] = fracter;

            SoftwareEntry proxyBypasser = new SoftwareEntry();
            proxyBypasser.Name = "Proxy bypasser";
            proxyBypasser.Type = File.Type.ProxyBypasser;
            proxyBypasser.Price = 20000;
            Software[File.Type.ProxyBypasser] = proxyBypasser;

            SoftwareEntry feedbackLoop = new SoftwareEntry();
            feedbackLoop.Name = "Feedback loop";
            feedbackLoop.Type = File.Type.FeedbackLoop;
            feedbackLoop.Price = 20000;
            Software[File.Type.FeedbackLoop] = feedbackLoop;

            SoftwareEntry minerVirus = new SoftwareEntry();
            minerVirus.Name = "Miner virus";
            minerVirus.Type = File.Type.MinerVirus;
            minerVirus.Price = 1000;
            Software[File.Type.MinerVirus] = minerVirus;

            SoftwareEntry feedbackVirus = new SoftwareEntry();
            feedbackVirus.Name = "Feedback virus";
            feedbackVirus.Type = File.Type.FeedbackVirus;
            feedbackVirus.Price = 20000;
            Software[File.Type.FeedbackVirus] = feedbackVirus;
        }

        public class SoftwareEntry
        {
            public string Name { get; set; }
            public File.Type Type { get; set; }
            public int Price { get; set; }
        }

        public Dictionary<File.Type, SoftwareEntry> Software;
    }
}
