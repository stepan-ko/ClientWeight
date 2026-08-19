using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Desktop.Settings
{
    public partial class AppSettings : ObservableObject
    {
        [ObservableProperty] private string _modbusHost = "10.6.173.231";
        [ObservableProperty] private string _modbusLocalHost = "10.6.173.230";
        [ObservableProperty] private int _modbusPort = 502;
        [ObservableProperty] private byte _modbusUnitId = 1;
        [ObservableProperty] private int _reconnectDelaySeconds = 5;

    }


}
