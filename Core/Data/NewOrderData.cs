using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace Weight
{
    public class NewOrderData : ReactiveObject
    {

        private int _TransactionType;
        private int _SmartLoadType;
        private int _StartUpMode;

        private uint _OrderWeight;
        private int _DefaultDraftSize;
        private string _OrderID = "";
        private uint _TicketNumber;
        private string _VesselID = "";
        private string _BinID = "";
        private string _ProductID = "";
        private string _ProductName = "";
        private int _ProductDensity;
        private string _CustomerName = "";
        private string _UnitTrainNumber = "";
        private string _Notes = "";
        private int _OrderIDEXP;
        private int _LotIDEXP;
        private string _BargeIDEXP = "";
        private int _LotSize;

        /// <summary>
        /// Получить массив регистров для записи нового Заказа
        /// </summary>
        /// <returns>Массив ushort - 121 регистр</returns>
        public ushort[] GetRegisters()
        {
            ushort[] reg = new ushort[120];
            reg[0] = (ushort)((StartUpMode << 6) | (SmartLoadType << 4) |TransactionType);            
            Lib.UIntToDcrReg(OrderWeight).CopyTo(reg, 1);
            Lib.UIntToDcrReg((uint)DefaultDraftSize).CopyTo(reg, 3);
            Lib.StringToReg(OrderID, 10).CopyTo(reg,5);
            Lib.UIntToDcrReg(TicketNumber).CopyTo(reg, 10);
            Lib.StringToReg(VesselID, 12).CopyTo(reg, 12);
            Lib.StringToReg(BinID, 10).CopyTo(reg, 18);
            Lib.StringToReg(ProductID, 8).CopyTo(reg, 23);
            Lib.StringToReg(ProductName, 26).CopyTo(reg, 27);
            reg[40] = (ushort)ProductDensity;
            Lib.StringToReg(CustomerName, 30).CopyTo(reg, 41);
            Lib.StringToReg(UnitTrainNumber, 20).CopyTo(reg, 56);
            Lib.StringToReg(Notes, 55).CopyTo(reg, 66);
            return reg;
        }

        public int TransactionType { get => _TransactionType; set => this.RaiseAndSetIfChanged(ref _TransactionType, value); }
        public int SmartLoadType { get => _SmartLoadType; set => this.RaiseAndSetIfChanged(ref _SmartLoadType, value); }
        public int StartUpMode { get => _StartUpMode; set => this.RaiseAndSetIfChanged(ref _StartUpMode, value); }
        public uint OrderWeight { get => _OrderWeight; set => this.RaiseAndSetIfChanged(ref _OrderWeight, value); }
        public int DefaultDraftSize { get => _DefaultDraftSize; set => this.RaiseAndSetIfChanged(ref _DefaultDraftSize, value); }
        public string OrderID { get => _OrderID; set => this.RaiseAndSetIfChanged(ref _OrderID, value); }
        public uint TicketNumber { get => _TicketNumber; set => this.RaiseAndSetIfChanged(ref _TicketNumber, value); }
        public string VesselID { get => _VesselID; set => this.RaiseAndSetIfChanged(ref _VesselID, value); }
        public string BinID { get => _BinID; set => this.RaiseAndSetIfChanged(ref _BinID, value); }
        public string ProductID { get => _ProductID; set => this.RaiseAndSetIfChanged(ref _ProductID, value); }
        public string ProductName { get => _ProductName; set => this.RaiseAndSetIfChanged(ref _ProductName, value); }
        public int ProductDensity { get => _ProductDensity; set => this.RaiseAndSetIfChanged(ref _ProductDensity, value); }
        public string CustomerName { get => _CustomerName; set => this.RaiseAndSetIfChanged(ref _CustomerName, value); }
        public string UnitTrainNumber { get => _UnitTrainNumber; set => this.RaiseAndSetIfChanged(ref _UnitTrainNumber, value); }
        public string Notes { get => _Notes; set => this.RaiseAndSetIfChanged(ref _Notes, value); }
        public int OrderIDEXP { get => _OrderIDEXP; set => this.RaiseAndSetIfChanged(ref _OrderIDEXP, value); }
        public int LotIDEXP { get => _LotIDEXP; set => this.RaiseAndSetIfChanged(ref _LotIDEXP, value); }
        public string BargeIDEXP { get => _BargeIDEXP; set => this.RaiseAndSetIfChanged(ref _BargeIDEXP, value); }
        public int LotSize { get => _LotSize; set => this.RaiseAndSetIfChanged(ref _LotSize, value); }
        
    }
}
