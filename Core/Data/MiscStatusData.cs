using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media;
using ReactiveUI;

namespace Weight
{
    public class MiscStatusData : ReactiveObject
    {
        //---------- Дополнительные Данные (Misc.) -----------
        private int _mBuildupTestWeight;
        private int _mRunningTotalWeight;
        private int _mTotalDraftCounter;
        private int _mGateSetpoint;
        private int _mStatisticsLoggingStatus;
        private uint _mOrderPreCutoff;
        private int _mLGGateState;

        private int _mHourlyTotal;
        private int _mHourlyTotalHour;
        private int _mHourlyTotalMin;
        private int _mHourlyTotalSec;
        private int _mHourlyTotalDay;
        private int _mHourlyTotalMonth;
        private int _mHourlyTotalYear;
        private DateTime _mHourlyTotalDateTime;

        private int _mDailyTotal;
        private int _mDailyTotalSec;
        private int _mDailyTotalMin;
        private int _mDailyTotalHour;
        private int _mDailyTotalDay;
        private int _mDailyTotalMonth;
        private int _mDailyTotalYear;
        private DateTime _mDailyTotalDateTime;

        private ushort _mHWWord1;
        private bool _mHWDBSizeTooBig;
        private bool _mHWLowDiskSpace;
        private bool _mHWADBoardFault;
        private bool _mHWIOBoardFault;
        private bool _mHWCWCBoardFault;
        private bool _mHWLCDFault;
        private bool _mHWNVRAMFault;

        private ushort _mSelShippWord;
        private int _mSelShippBinAB;
        private int _mSelShippBinAutoSwitch;

        /// <summary>
        /// Обновить значения mSelShippBin
        /// </summary>
        private void UpdateSelShipp()
        {
            mSelShippBinAB = (byte)mSelShippWord;
            mSelShippBinAutoSwitch = mSelShippWord >> 8;
        }

        /// <summary>
        /// Обновить Дату/Время счетчика часового
        /// </summary>
        private void UpdateHourlyTotalDateTime()
        {
            mHourlyTotalDateTime = Lib.SetDateTime(mHourlyTotalYear, mHourlyTotalMonth, mHourlyTotalDay, mHourlyTotalHour, mHourlyTotalMin, mHourlyTotalSec);
        }

        /// <summary>
        /// Обновить Дату/Время счетчика суточного
        /// </summary>
        private void UpdateDailyTotalDateTime()
        {
            mDailyTotalDateTime = Lib.SetDateTime(mDailyTotalYear, mDailyTotalMonth, mDailyTotalDay, mDailyTotalHour, mDailyTotalMin, mDailyTotalSec);
        }

        /// <summary>
        /// Обновить значения битов Ошибок HardWare
        /// </summary>
        private void UpdateHWWord1()
        {
            mHWDBSizeTooBig = Lib.GetBitWord(mHWWord1, 9);
            mHWLowDiskSpace = Lib.GetBitWord(mHWWord1, 8);
            mHWADBoardFault = Lib.GetBitWord(mHWWord1, 4);
            mHWIOBoardFault = Lib.GetBitWord(mHWWord1, 3);
            mHWCWCBoardFault = Lib.GetBitWord(mHWWord1, 2);
            mHWLCDFault = Lib.GetBitWord(mHWWord1, 1);
            mHWNVRAMFault = Lib.GetBitWord(mHWWord1, 0);
        }



        public int mBuildupTestWeight { get => _mBuildupTestWeight; set => this.RaiseAndSetIfChanged(ref _mBuildupTestWeight, value); }
        public int mRunningTotalWeight { get => _mRunningTotalWeight; set => this.RaiseAndSetIfChanged(ref _mRunningTotalWeight, value); }
        public int mTotalDraftCounter { get => _mTotalDraftCounter; set => this.RaiseAndSetIfChanged(ref _mTotalDraftCounter, value); }
        public int mGateSetpoint { get => _mGateSetpoint; set => this.RaiseAndSetIfChanged(ref _mGateSetpoint, value); }
        public int mStatisticsLoggingStatus { get => _mStatisticsLoggingStatus; set => this.RaiseAndSetIfChanged(ref _mStatisticsLoggingStatus, value); }
        public uint mOrderPreCutoff { get => _mOrderPreCutoff; set => this.RaiseAndSetIfChanged(ref _mOrderPreCutoff, value); }
        public int mLGGateState { get => _mLGGateState; set => this.RaiseAndSetIfChanged(ref _mLGGateState, value); }
        public int mHourlyTotal { get => _mHourlyTotal; set => this.RaiseAndSetIfChanged(ref _mHourlyTotal, value); }
        public int mHourlyTotalHour
        {
            get => _mHourlyTotalHour;
            set
            {
                if (_mHourlyTotalHour != value) UpdateHourlyTotalDateTime();
                this.RaiseAndSetIfChanged(ref _mHourlyTotalHour, value);
            }
        }
        public int mHourlyTotalMin
        {
            get => _mHourlyTotalMin;
            set
            {
                if (_mHourlyTotalMin != value) UpdateHourlyTotalDateTime();
                this.RaiseAndSetIfChanged(ref _mHourlyTotalMin, value);
            }
        }
        public int mHourlyTotalSec
        {
            get => _mHourlyTotalSec;
            set
            {
                if (_mHourlyTotalSec != value) UpdateHourlyTotalDateTime();
                this.RaiseAndSetIfChanged(ref _mHourlyTotalSec, value);
            }
        }
        public int mHourlyTotalDay
        {
            get => _mHourlyTotalDay;
            set
            {
                if (_mHourlyTotalDay != value) UpdateHourlyTotalDateTime();
                this.RaiseAndSetIfChanged(ref _mHourlyTotalDay, value);
            }
        }
        public int mHourlyTotalMonth
        {
            get => _mHourlyTotalMonth;
            set
            {
                if (_mHourlyTotalMonth != value) UpdateHourlyTotalDateTime();
                this.RaiseAndSetIfChanged(ref _mHourlyTotalMonth, value);
            }
        }
        public int mHourlyTotalYear
        {
            get => _mHourlyTotalYear;
            set
            {
                if (_mHourlyTotalYear != value) UpdateHourlyTotalDateTime();
                this.RaiseAndSetIfChanged(ref _mHourlyTotalYear, value);
            }
        }
        public DateTime mHourlyTotalDateTime { get => _mHourlyTotalDateTime; set => this.RaiseAndSetIfChanged(ref _mHourlyTotalDateTime, value); }

        public int mDailyTotal { get => _mDailyTotal; set => this.RaiseAndSetIfChanged(ref _mDailyTotal, value); }
        public int mDailyTotalHour
        {
            get => _mDailyTotalHour;
            set
            {
                if (_mDailyTotalHour != value) UpdateDailyTotalDateTime();
                this.RaiseAndSetIfChanged(ref _mDailyTotalHour, value);
            }
        }
        public int mDailyTotalMin
        {
            get => _mDailyTotalMin;
            set
            {
                if (_mDailyTotalMin != value) UpdateDailyTotalDateTime();
                this.RaiseAndSetIfChanged(ref _mDailyTotalMin, value);
            }
        }
        public int mDailyTotalSec
        {
            get => _mDailyTotalSec;
            set
            {
                if (_mDailyTotalSec != value) UpdateDailyTotalDateTime();
                this.RaiseAndSetIfChanged(ref _mDailyTotalSec, value);
            }
        }
        public int mDailyTotalDay
        {
            get => _mDailyTotalDay;
            set
            {
                if (_mDailyTotalDay != value) UpdateDailyTotalDateTime();
                this.RaiseAndSetIfChanged(ref _mDailyTotalDay, value);
            }
        }
        public int mDailyTotalMonth
        {
            get => _mDailyTotalMonth;
            set
            {
                if (_mDailyTotalMonth != value) UpdateDailyTotalDateTime();
                this.RaiseAndSetIfChanged(ref _mDailyTotalMonth, value);
            }
        }
        public int mDailyTotalYear
        {
            get => _mDailyTotalYear;
            set
            {
                if (_mDailyTotalYear != value) UpdateDailyTotalDateTime();
                this.RaiseAndSetIfChanged(ref _mDailyTotalYear, value);
            }
        }
        public DateTime mDailyTotalDateTime { get => _mDailyTotalDateTime; set => this.RaiseAndSetIfChanged(ref _mDailyTotalDateTime, value); }

        public ushort mHWWord1
        {
            get => _mHWWord1;
            set
            {
                if (_mHWWord1 != value) UpdateHWWord1();
                this.RaiseAndSetIfChanged(ref _mHWWord1, value);
            }
        }

        public bool mHWDBSizeTooBig { get => _mHWDBSizeTooBig; set => this.RaiseAndSetIfChanged(ref _mHWDBSizeTooBig, value); }
        public bool mHWLowDiskSpace { get => _mHWLowDiskSpace; set => this.RaiseAndSetIfChanged(ref _mHWLowDiskSpace, value); }
        public bool mHWADBoardFault { get => _mHWADBoardFault; set => this.RaiseAndSetIfChanged(ref _mHWADBoardFault, value); }
        public bool mHWIOBoardFault { get => _mHWIOBoardFault; set => this.RaiseAndSetIfChanged(ref _mHWIOBoardFault, value); }
        public bool mHWCWCBoardFault { get => _mHWCWCBoardFault; set => this.RaiseAndSetIfChanged(ref _mHWCWCBoardFault, value); }
        public bool mHWLCDFault { get => _mHWLCDFault; set => this.RaiseAndSetIfChanged(ref _mHWLCDFault, value); }
        public bool mHWNVRAMFault { get => _mHWNVRAMFault; set => this.RaiseAndSetIfChanged(ref _mHWNVRAMFault, value); }

        public ushort mSelShippWord
        {
            get => _mSelShippWord;
            set
            {
                if (_mSelShippWord != 0) UpdateSelShipp();
                this.RaiseAndSetIfChanged(ref _mSelShippWord, value);
            }
        }

        public int mSelShippBinAB { get => _mSelShippBinAB; set => this.RaiseAndSetIfChanged(ref _mSelShippBinAB, value); }
        public int mSelShippBinAutoSwitch { get => _mSelShippBinAutoSwitch; set => this.RaiseAndSetIfChanged(ref _mSelShippBinAutoSwitch, value); }


    }
}
