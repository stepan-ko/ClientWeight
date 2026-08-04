using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Weight
{
    public class NewSystemDateTime : ObservableObject
    {
                       
        private int _NewSystemTimeHour;
        private int _NewSystemTimeMin;
        private int _NewSystemTimeSec;
        private int _NewSystemTimeDay;
        private int _NewSystemTimeMonth;
        private int _NewSystemTimeYear;
        private string _Comments = "";
        
        public ushort[] GetRegisters()
        {            
            ushort[] result = new ushort[120];
            result[0] = (ushort)NewSystemTimeHour;
            result[1] = (ushort)NewSystemTimeMin;
            result[2] = (ushort)NewSystemTimeSec;
            result[3] = (ushort)NewSystemTimeDay;
            result[4] = (ushort)NewSystemTimeMonth;
            result[5] = (ushort)NewSystemTimeYear;                                
            Lib.StringToReg(Comments, 20).CopyTo(result, 6);                
                
            return result;             
        }
        
        public int NewSystemTimeHour { get => _NewSystemTimeHour; set => this.SetProperty(ref _NewSystemTimeHour, value); }
        public int NewSystemTimeMin { get => _NewSystemTimeMin; set => this.SetProperty(ref _NewSystemTimeMin, value); }
        public int NewSystemTimeSec { get => _NewSystemTimeSec; set => this.SetProperty(ref _NewSystemTimeSec, value); }
        public int NewSystemTimeDay { get => _NewSystemTimeDay; set => this.SetProperty(ref _NewSystemTimeDay, value); }
        public int NewSystemTimeMonth { get => _NewSystemTimeMonth; set => this.SetProperty(ref _NewSystemTimeMonth, value); }
        public int NewSystemTimeYear { get => _NewSystemTimeYear; set => this.SetProperty(ref _NewSystemTimeYear, value); }
        public string Comments { get => _Comments; set => this.SetProperty(ref _Comments, value); }
        

    }
}
