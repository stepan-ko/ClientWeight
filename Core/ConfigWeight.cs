using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weight
{
    public class ConfigWeight
    {
        public string Host { get; set; } = "192.168.5.11";
        public int Port { get; set; } = 502;        
        public int StatusReadIntervalMs { get; set; } = 50;
        public int OrderReadIntervalMs { get; set; } = 1000;
    }
}
