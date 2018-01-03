using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace h4x0r
{
    public partial class Node
    {
        public class Address
        {
            // Generates a random address.
            // Excludes the most common reserved ranges.
            public Address()
            {
                Value = GenerateRandomAddress();
            }

            public Address(string value)
            {
                Value = value;
            }

            private string GenerateRandomAddress()
            {
                if (m_Random == null)
                {
                    m_Random = new Random();
                }

                int[] address =
                {
                    m_Random.Next(1, 255),
                    m_Random.Next(0, 255),
                    m_Random.Next(0, 255),
                    m_Random.Next(1, 255)
                };

                bool valid = true;
                if ((address[0] == 127 && address[1] == 0 && address[2] == 0) || // 127.0.0.*
                    (address[0] == 192 && address[1] == 168) || // 192.168.*.*
                    (address[0] == 255 && address[1] == 255 && address[2] == 255 && address[3] == 255) || // 255.255.255.255
                    (address[0] == 224)) // 224.*.*.*
                {
                    valid = false;
                }

                if (valid)
                {
                    return String.Format("{0}.{1}.{2}.{3}", address[0], address[1], address[2], address[3]);
                }

                return GenerateRandomAddress();
            }

            public string Value { get; set; }

            private static Random m_Random = null;
        }
    }
}
