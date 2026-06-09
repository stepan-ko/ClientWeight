using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace Weight.Data
{
    public class StaticOrderData : ReactiveObject
    {

        //---------- Статические Данные заказа (не изменяемые) -----------
        private int _sOrderTypeInformation;
        private int _sTransactionType;
        private int _sSmartLoadType;
        private int _sStartUpMode;
        // DCR формат
        private int _sOrderWeight;
        private int _sDefaultDraftSize;
        private uint _sTicketNumber;
        private int _sProductDensity;
        private uint _sGlobalTicketCounter;
        private int _sLotSize;

        private string? _sOrderID;          // max 10 символов                          
        private string? _sVesselID;         // max 12 символов
        private string? _sBinID;            // max 10 символов
        private string? _sProductID;        // max 8 символов
        private string? _sProductName;      // max 26 символов
        private string? _sCustomerName;     // max 30 символов
        private string? _sUnitTrainNumber;  // max 20 символов
        private string? _sNotes;            // max 120 символов


        /// <summary>
        /// Обновить информацию по ордеру
        /// </summary>
        private void UpdatesOrderTypeInformation()
        {
            sTransactionType = sOrderTypeInformation & 0xF;
            sSmartLoadType = sOrderTypeInformation >> 4 & 0x3;
            sStartUpMode = sOrderTypeInformation >> 6 & 0x3;
        }

        public int sOrderTypeInformation
        {
            get => _sOrderTypeInformation;
            set
            {
                if (_sOrderTypeInformation != value) UpdatesOrderTypeInformation();
                this.RaiseAndSetIfChanged(ref _sOrderTypeInformation, value);
            }
        }
        public int sTransactionType { get => _sTransactionType; set => this.RaiseAndSetIfChanged(ref _sTransactionType, value); }
        public int sSmartLoadType { get => _sSmartLoadType; set => this.RaiseAndSetIfChanged(ref _sSmartLoadType, value); }
        public int sStartUpMode { get => _sStartUpMode; set => this.RaiseAndSetIfChanged(ref _sStartUpMode, value); }
        public int sOrderWeight { get => _sOrderWeight; set => this.RaiseAndSetIfChanged(ref _sOrderWeight, value); }
        public int sDefaultDraftSize { get => _sDefaultDraftSize; set => this.RaiseAndSetIfChanged(ref _sDefaultDraftSize, value); }
        public uint sTicketNumber { get => _sTicketNumber; set => this.RaiseAndSetIfChanged(ref _sTicketNumber, value); }
        public int sProductDensity { get => _sProductDensity; set => this.RaiseAndSetIfChanged(ref _sProductDensity, value); }
        public uint sGlobalTicketCounter { get => _sGlobalTicketCounter; set => this.RaiseAndSetIfChanged(ref _sGlobalTicketCounter, value); }
        public int sLotSize { get => _sLotSize; set => this.RaiseAndSetIfChanged(ref _sLotSize, value); }
        public string? sOrderID { get => _sOrderID; set => this.RaiseAndSetIfChanged(ref _sOrderID, value); }
        public string? sVesselID { get => _sVesselID; set => this.RaiseAndSetIfChanged(ref _sVesselID, value); }
        public string? sBinID { get => _sBinID; set => this.RaiseAndSetIfChanged(ref _sBinID, value); }
        public string? sProductID { get => _sProductID; set => this.RaiseAndSetIfChanged(ref _sProductID, value); }
        public string? sProductName { get => _sProductName; set => this.RaiseAndSetIfChanged(ref _sProductName, value); }
        public string? sCustomerName { get => _sCustomerName; set => this.RaiseAndSetIfChanged(ref _sCustomerName, value); }
        public string? sUnitTrainNumber { get => _sUnitTrainNumber; set => this.RaiseAndSetIfChanged(ref _sUnitTrainNumber, value); }
        public string? sNotes { get => _sNotes; set => this.RaiseAndSetIfChanged(ref _sNotes, value); }

    }
}
