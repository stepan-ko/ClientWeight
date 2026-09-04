using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Weight;
using Weight.Data;

namespace Desktop.Settings
{
    public class ScaleData
    {        
        public OrderData OrderData { get; } = new OrderData();
        public StaticOrderData StaticOrderData { get; } = new StaticOrderData();
        public StatusData StatusData { get; } = new StatusData();
        public MiscStatusData MiscStatusData { get; } = new MiscStatusData();
        public ConfigData ConfigData { get; } = new ConfigData();
    }

}
