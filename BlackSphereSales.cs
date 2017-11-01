using System;
using System.Collections.Generic;

namespace h4x0r
{
    class BlackSphereSales
    {
        public BlackSphereSales()
        {
            Software = new List<SoftwareEntry>();

            SoftwareEntry nodeAnalyser = new SoftwareEntry();
            nodeAnalyser.Name = "Node analyser";
            nodeAnalyser.Type = File.Type.NodeAnalyser;
            nodeAnalyser.Price = 100;
            nodeAnalyser.Description = "";
            Software.Add(nodeAnalyser);

            SoftwareEntry cracker = new SoftwareEntry();
            cracker.Name = "Cracker";
            cracker.Type = File.Type.Cracker;
            cracker.Price = 500;
            cracker.Description = "";
            Software.Add(cracker);

            SoftwareEntry fracter = new SoftwareEntry();
            fracter.Name = "Fracter";
            fracter.Type = File.Type.Fracter;
            fracter.Price = 10000;
            fracter.Description = "";
            Software.Add(fracter);

            SoftwareEntry proxyBypasser = new SoftwareEntry();
            proxyBypasser.Name = "Proxy bypasser";
            proxyBypasser.Type = File.Type.ProxyBypasser;
            proxyBypasser.Price = 20000;
            proxyBypasser.Description = "";
            Software.Add(proxyBypasser);

            SoftwareEntry feedbackLoop = new SoftwareEntry();
            feedbackLoop.Name = "Feedback loop";
            feedbackLoop.Type = File.Type.FeedbackLoop;
            feedbackLoop.Price = 20000;
            feedbackLoop.Description = "";
            Software.Add(feedbackLoop);

            SoftwareEntry minerVirus = new SoftwareEntry();
            minerVirus.Name = "Miner virus";
            minerVirus.Type = File.Type.MinerVirus;
            minerVirus.Price = 1000;
            minerVirus.Description = "";
            Software.Add(minerVirus);

            SoftwareEntry feedbackVirus = new SoftwareEntry();
            feedbackVirus.Name = "Feedback virus";
            feedbackVirus.Type = File.Type.FeedbackVirus;
            feedbackVirus.Price = 20000;
            feedbackVirus.Description = "";
            Software.Add(feedbackVirus);
        }

        public class SoftwareEntry
        {
            public string Name { get; set; }
            public File.Type Type { get; set; }
            public int Price { get; set; }
            public string Description { get; set; }
        }

        public List<SoftwareEntry> Software;
    }
}
