using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reactive;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Avalonia.Media;
using ReactiveUI;

namespace Weight
{
    public class OrderData : ReactiveObject
    {
       
        private int _DraftGrossWeightYear;
        private int _DraftGrossWeightMonth;
        private int _DraftGrossWeightDay;
        private int _DraftGrossWeightHour;
        private int _DraftGrossWeightMin;
        private int _DraftGrossWeightSec;
        private DateTime _DraftGrossWeightDateTime;

        private int _DraftTareWeightYear;
        private int _DraftTareWeightMonth;
        private int _DraftTareWeightDay;
        private int _DraftTareWeightHour;
        private int _DraftTareWeightMin;
        private int _DraftTareWeightSec;
        private DateTime _DraftTareWeightDateTime;

        private int _OrderStartYear;
        private int _OrderStartMonth;
        private int _OrderStartDay;
        private int _OrderStartHour;
        private int _OrderStartMin;
        private int _OrderStartSec;
        private DateTime _OrderStartDateTime;

        private int _OrderFinishYear;
        private int _OrderFinishMonth;
        private int _OrderFinishDay;
        private int _OrderFinishHour;
        private int _OrderFinishMin;
        private int _OrderFinishSec;
        private DateTime _OrderFinishDateTime;

        // DCR формат
        private int _DraftTareWeight;        
        private int _DraftGrossWeight;        
        private int _DraftNetWeight;
        private int _FlowRate;
        private int _OrderBalance;
        private int _TotalWeight;               
        private int _OrderDraftSize;
        private int _SubtotalWeight;
        private int _DraftTargetWeight;

        private uint _CurrentDruftCount;
        private uint _LastDruftCount;
        private uint _PlannedNumberDruftCount;                
        private uint _ExtraWeight;
        private uint _Performance; // вычисляемая
       

        /// <summary>
        /// Обновить Дату/Время Брутто отвеса
        /// </summary>
        private void UpdateDraftGrossWeightDateTime()
        {
            DraftGrossWeightDateTime = Lib.SetDateTime(DraftGrossWeightYear, DraftGrossWeightMonth, DraftGrossWeightDay, DraftGrossWeightHour, DraftGrossWeightMin, DraftGrossWeightSec);
        }

        /// <summary>
        /// Обновить Дату/Время ТАРЫ отвеса
        /// </summary>
        private void UpdateDraftTareWeightDateTime()
        {
            DraftTareWeightDateTime = Lib.SetDateTime(DraftTareWeightYear, DraftTareWeightMonth, DraftTareWeightDay, DraftTareWeightHour, DraftTareWeightMin, DraftTareWeightSec);
        }

        /// <summary>
        /// Обновить Дату/Время Начала заказа
        /// </summary>
        private void UpdateOrderStartDateTime()
        {
            OrderStartDateTime = Lib.SetDateTime(OrderStartYear, OrderStartMonth, OrderStartDay, OrderStartHour, OrderStartMin, OrderStartSec);
        }

        /// <summary>
        /// Обновить Дату/Время Окончания заказа
        /// </summary>
        private void UpdateOrderFinishDateTime()
        {
            OrderFinishDateTime = Lib.SetDateTime(OrderFinishYear, OrderFinishMonth, OrderFinishDay, OrderFinishHour, OrderFinishMin, OrderFinishSec);
        }

       
        public int DraftGrossWeightYear 
        { 
            get => _DraftGrossWeightYear;
            set
            {
                if (_DraftGrossWeightYear != value) UpdateDraftGrossWeightDateTime();
                this.RaiseAndSetIfChanged(ref _DraftGrossWeightYear, value);
            }
        }
        public int DraftGrossWeightMonth
        {
            get => _DraftGrossWeightMonth;
            set
            {
                if (_DraftGrossWeightMonth != value) UpdateDraftGrossWeightDateTime();
                this.RaiseAndSetIfChanged(ref _DraftGrossWeightMonth, value);
            }
        }
        public int DraftGrossWeightDay
        {
            get => _DraftGrossWeightDay;
            set
            {
                if (_DraftGrossWeightDay != value) UpdateDraftGrossWeightDateTime();
                this.RaiseAndSetIfChanged(ref _DraftGrossWeightDay, value);
            }
        }
        public int DraftGrossWeightHour
        {
            get => _DraftGrossWeightHour;
            set
            {
                if (_DraftGrossWeightHour != value) UpdateDraftGrossWeightDateTime();
                this.RaiseAndSetIfChanged(ref _DraftGrossWeightHour, value);
            }
        }
        public int DraftGrossWeightMin
        {
            get => _DraftGrossWeightMin;
            set
            {
                if (_DraftGrossWeightMin != value) UpdateDraftGrossWeightDateTime();
                this.RaiseAndSetIfChanged(ref _DraftGrossWeightMin, value);
            }
        }
        public int DraftGrossWeightSec
        {
            get => _DraftGrossWeightSec;
            set
            {
                if (_DraftGrossWeightSec != value) UpdateDraftGrossWeightDateTime();
                this.RaiseAndSetIfChanged(ref _DraftGrossWeightSec, value);
            }
        }
        public DateTime DraftGrossWeightDateTime { get => _DraftGrossWeightDateTime; set => this.RaiseAndSetIfChanged(ref _DraftGrossWeightDateTime, value); }

        public int DraftTareWeightYear
        {
            get => _DraftTareWeightYear;
            set
            {
                if (_DraftTareWeightYear != value) UpdateDraftTareWeightDateTime();
                this.RaiseAndSetIfChanged(ref _DraftTareWeightYear, value);
            }
        }
        public int DraftTareWeightMonth
        {
            get => _DraftTareWeightMonth;
            set
            {
                if (_DraftTareWeightMonth != value) UpdateDraftTareWeightDateTime();
                this.RaiseAndSetIfChanged(ref _DraftTareWeightMonth, value);
            }
        }
        public int DraftTareWeightDay
        {
            get => _DraftTareWeightDay;
            set
            {
                if (_DraftTareWeightDay != value) UpdateDraftTareWeightDateTime();
                this.RaiseAndSetIfChanged(ref _DraftTareWeightDay, value);
            }
        }
        public int DraftTareWeightHour
        {
            get => _DraftTareWeightHour;
            set
            {
                if (_DraftTareWeightHour != value) UpdateDraftTareWeightDateTime();
                this.RaiseAndSetIfChanged(ref _DraftTareWeightHour, value);
            }
        }
        public int DraftTareWeightMin
        {
            get => _DraftTareWeightMin;
            set
            {
                if (_DraftTareWeightMin != value) UpdateDraftTareWeightDateTime();
                this.RaiseAndSetIfChanged(ref _DraftTareWeightMin, value);
            }
        }
        public int DraftTareWeightSec
        {
            get => _DraftTareWeightSec;
            set
            {
                if (_DraftTareWeightSec != value) UpdateDraftTareWeightDateTime();
                this.RaiseAndSetIfChanged(ref _DraftTareWeightSec, value);
            }
        }
        public DateTime DraftTareWeightDateTime { get => _DraftTareWeightDateTime; set => this.RaiseAndSetIfChanged(ref _DraftTareWeightDateTime, value); }

        public int OrderStartYear
        {
            get => _OrderStartYear;
            set
            {
                if (_OrderStartYear != value) UpdateOrderStartDateTime();
                this.RaiseAndSetIfChanged(ref _OrderStartYear, value);
            }
        }
        public int OrderStartMonth
        {
            get => _OrderStartMonth;
            set
            {
                if (_OrderStartMonth != value) UpdateOrderStartDateTime();
                this.RaiseAndSetIfChanged(ref _OrderStartMonth, value);
            }
        }
        public int OrderStartDay
        {
            get => _OrderStartDay;
            set
            {
                if (_OrderStartDay != value) UpdateOrderStartDateTime();
                this.RaiseAndSetIfChanged(ref _OrderStartDay, value);
            }
        }
        public int OrderStartHour
        {
            get => _OrderStartHour;
            set
            {
                if (_OrderStartHour != value) UpdateOrderStartDateTime();
                this.RaiseAndSetIfChanged(ref _OrderStartHour, value);
            }
        }
        public int OrderStartMin
        {
            get => _OrderStartMin;
            set
            {
                if (_OrderStartMin != value) UpdateOrderStartDateTime();
                this.RaiseAndSetIfChanged(ref _OrderStartMin, value);
            }
        }
        public int OrderStartSec
        {
            get => _OrderStartSec;
            set
            {
                if (_OrderStartSec != value) UpdateOrderStartDateTime();
                this.RaiseAndSetIfChanged(ref _OrderStartSec, value);
            }
        }
        public DateTime OrderStartDateTime { get => _OrderStartDateTime; set => this.RaiseAndSetIfChanged(ref _OrderStartDateTime, value); }

        public int OrderFinishYear
        {
            get => _OrderFinishYear;
            set
            {
                if (_OrderFinishYear != value) UpdateOrderFinishDateTime();
                this.RaiseAndSetIfChanged(ref _OrderFinishYear, value);
            }
        }
        public int OrderFinishMonth
        {
            get => _OrderFinishMonth;
            set
            {
                if (_OrderFinishMonth != value) UpdateOrderFinishDateTime();
                this.RaiseAndSetIfChanged(ref _OrderFinishMonth, value);
            }
        }
        public int OrderFinishDay
        {
            get => _OrderFinishDay;
            set
            {
                if (_OrderFinishDay != value) UpdateOrderFinishDateTime();
                this.RaiseAndSetIfChanged(ref _OrderFinishDay, value);
            }
        }
        public int OrderFinishHour
        {
            get => _OrderFinishHour;
            set
            {
                if (_OrderFinishHour != value) UpdateOrderFinishDateTime();
                this.RaiseAndSetIfChanged(ref _OrderFinishHour, value);
            }
        }
        public int OrderFinishMin
        {
            get => _OrderFinishMin;
            set
            {
                if (_OrderFinishMin != value) UpdateOrderFinishDateTime();
                this.RaiseAndSetIfChanged(ref _OrderFinishMin, value);
            }
        }
        public int OrderFinishSec
        {
            get => _OrderFinishSec;
            set
            {
                if (_OrderFinishSec != value) UpdateOrderFinishDateTime();
                this.RaiseAndSetIfChanged(ref _OrderFinishSec, value);
            }
        }
        public DateTime OrderFinishDateTime { get => _OrderFinishDateTime; set => this.RaiseAndSetIfChanged(ref _OrderFinishDateTime, value); }

        public int DraftTareWeight { get => _DraftTareWeight; set => this.RaiseAndSetIfChanged(ref _DraftTareWeight, value); }
        public int DraftGrossWeight { get => _DraftGrossWeight; set => this.RaiseAndSetIfChanged(ref _DraftGrossWeight, value); }
        public int DraftNetWeight { get => _DraftNetWeight; set => this.RaiseAndSetIfChanged(ref _DraftNetWeight, value); }
        public int FlowRate { get => _FlowRate; set => this.RaiseAndSetIfChanged(ref _FlowRate, value); }
        public int OrderBalance { get => _OrderBalance; set => this.RaiseAndSetIfChanged(ref _OrderBalance, value); }
        public int TotalWeight { get => _TotalWeight; set => this.RaiseAndSetIfChanged(ref _TotalWeight, value); }
        public int OrderDraftSize { get => _OrderDraftSize; set => this.RaiseAndSetIfChanged(ref _OrderDraftSize, value); }
        public int SubtotalWeight { get => _SubtotalWeight; set => this.RaiseAndSetIfChanged(ref _SubtotalWeight, value); }
        public int DraftTargetWeight { get => _DraftTargetWeight; set => this.RaiseAndSetIfChanged(ref _DraftTargetWeight, value); }
        public uint CurrentDruftCount { get => _CurrentDruftCount; set => this.RaiseAndSetIfChanged(ref _CurrentDruftCount, value); }
        public uint LastDruftCount { get => _LastDruftCount; set => this.RaiseAndSetIfChanged(ref _LastDruftCount, value); }
        public uint PlannedNumberDruftCount { get => _PlannedNumberDruftCount; set => this.RaiseAndSetIfChanged(ref _PlannedNumberDruftCount, value); }
        public uint ExtraWeight { get => _ExtraWeight; set => this.RaiseAndSetIfChanged(ref _ExtraWeight, value); }
        public uint Performance { get => _Performance; set => this.RaiseAndSetIfChanged(ref _Performance, value); }
        
        
        

    }
}
