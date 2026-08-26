using System.Diagnostics.Metrics;
using System.Dynamic;
using System.Net;
using System.Net.Sockets;
using NModbus;
using NLog;
using Weight.Data;
using System.Diagnostics;

namespace Weight
{
    public class ModbusWeightService : IWeightConnectionService, IWeightDataReader, IWeightCommandService
    {
        private TcpClient _tcpClient;
        private IModbusMaster _master;
        private bool _isConnected;
        //private Logger _logger;
        private static readonly ILogger _logger = LogManager.GetLogger("Modbus");
        private byte _slaveId;

        private string host;
        private IPAddress localIP;
        private int port;        
        public ModbusWeightService(string Host, string LocalHost, int slaveId, int Port = 502)
        {
            //_logger = logger;
            _slaveId = (byte)slaveId;
            host = Host;            
            localIP = IPAddress.Parse(LocalHost);
            port = Port;            
        }

        // ---------------- IWeightConnectionService ---------------
        public async Task<bool>  ConnectAsync()
        {
            try
            {     
                _tcpClient = new TcpClient(new IPEndPoint(localIP, 0));
                await _tcpClient.ConnectAsync(host, port);
                var factory = new ModbusFactory();
                _master = factory.CreateMaster(_tcpClient);
                _master.Transport.ReadTimeout = 1000;
                _master.Transport.WriteTimeout = 1000;
                _isConnected = true;
                //_logger.Info("Подключение к весам установлено");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка подключения: {ex}");                
                Debug.WriteLine($"Ошибка подключения: {ex}");
                _isConnected = false;
                return false;
            }
        }

        public Task DisconnectAsync()
        {
            _tcpClient?.Close();
            _isConnected = false;
            return Task.CompletedTask;
        }

        public bool IsConnected => _isConnected;

        // ------------------ IWeightDataReader --------------------
        public async Task ReadStatusDataAsync(StatusData target)
        {
            if (!_isConnected) throw new InvalidOperationException("Не подключено к весам");

            var registers = await Task.Run(() => _master.ReadHoldingRegistersAsync(_slaveId, 49, 31));           
            
            target.ScaleMode = registers[0];
            target.StatusWord1 = registers[1];
            target.StatusWord2 = registers[2];
            target.ScaleAlarmWord1 = registers[3];
            target.ScaleAlarmWord2 = registers[4];
            target.InputWord1 = registers[5];
            target.InputWord2 = registers[6];
            target.OutputWord1 = registers[7];
            target.OutputWord2 = registers[8];
            target.SmartAlarmWord1 = registers[9];
            target.SmartAlarmWord2 = registers[10];
            target.DWIStatus = registers[13];
            target.ScaleWeight = Lib.DcrToInt(registers[14], registers[15]);
            target.ScaleFlowRate = Lib.DcrToInt(registers[16], registers[17]);
            target.ScaleGatePosition = registers[18];
            target.RejectCode = registers[19];
            target.AcceptedCode = registers[20];
            target.CommunicationResult = registers[21];
            target.EventIdMask = (uint)registers[22] << 16 | registers[23];
            target.MastersCount = Lib.GetLowByte(registers[24]);
            target.ObservCount = Lib.GetHiByte(registers[24]);
            target.CounterLive = registers[25];
            target.TimeOur = registers[26];
            target.TimeMin = registers[27];
            target.TimeSec = registers[28];
            target.TimeMSec = registers[29];
            target.ClientInfo = registers[30]; 
        }

        public async Task ReadMiscStatusDataAsync(MiscStatusData target)
        {
            if (!_isConnected) throw new InvalidOperationException("Не подключено к весам");
            var registers = await Task.Run(() => _master.ReadHoldingRegistersAsync(_slaveId, 399, 30));           

            target.mBuildupTestWeight = Lib.DcrToInt(registers[0], registers[1]);
            target.mRunningTotalWeight = Lib.DcrToInt(registers[2], registers[3]);
            target.mTotalDraftCounter = registers[4];
            target.mGateSetpoint = registers[5];
            target.mSelShippWord = registers[6];
            target.mHWWord1 = registers[8]; // уточнить какой регистр
            target.mStatisticsLoggingStatus = registers[9];
            target.mOrderPreCutoff = (uint)registers[10] << 16 | registers[11];
            target.mLGGateState = registers[12];
            target.mHourlyTotal = Lib.DcrToInt(registers[13], registers[14]);
            target.mDailyTotal = Lib.DcrToInt(registers[15], registers[16]);
            target.mHourlyTotalHour = registers[17];
            target.mHourlyTotalMin = registers[18];
            target.mHourlyTotalSec = registers[19];
            target.mHourlyTotalDay = registers[20];
            target.mHourlyTotalMonth = registers[21];
            target.mHourlyTotalYear = registers[22];
            target.mDailyTotalHour = registers[23];
            target.mDailyTotalMin = registers[24];
            target.mDailyTotalSec = registers[25];
            target.mDailyTotalDay = registers[26];
            target.mDailyTotalMonth = registers[27];
            target.mDailyTotalYear = registers[28];

        }


        public async Task ReadOrderDataAsync(OrderData target)
        {
            if (!_isConnected) throw new InvalidOperationException("Не подключено к весам");
            var registers = await Task.Run(() => _master.ReadHoldingRegistersAsync(_slaveId, 99, 72));
            
            target.DraftGrossWeight = Lib.DcrToInt(registers[0], registers[1]);
            target.DraftGrossWeightHour = registers[2];
            target.DraftGrossWeightMin = registers[3];
            target.DraftGrossWeightSec = registers[4];
            target.DraftGrossWeightDay = registers[5];
            target.DraftGrossWeightMonth = registers[6];
            target.DraftGrossWeightYear = registers[7];
            target.DraftTareWeight = Lib.DcrToInt(registers[8], registers[9]);
            target.DraftTareWeightHour = registers[10];
            target.DraftTareWeightMin = registers[11];
            target.DraftTareWeightSec = registers[12];
            target.DraftTareWeightDay = registers[13];
            target.DraftTareWeightMonth = registers[14];
            target.DraftTareWeightYear = registers[15];
            target.DraftNetWeight = Lib.DcrToInt(registers[16], registers[17]);
            target.FlowRate = Lib.DcrToInt(registers[18], registers[19]);
            target.OrderBalance = Lib.DcrToInt(registers[22], registers[23]);
            target.TotalWeight = Lib.DcrToInt(registers[24], registers[25]);
            target.OrderDraftSize = Lib.DcrToInt(registers[26], registers[27]);
            target.SubtotalWeight = Lib.DcrToInt(registers[28], registers[29]);
            target.CurrentDruftCount = registers[30];
            target.PlannedNumberDruftCount = registers[31];
            target.DraftTargetWeight = Lib.DcrToInt(registers[32], registers[33]);
            target.OrderStartHour = registers[34];
            target.OrderStartMin = registers[35];
            target.OrderStartSec = registers[36];
            target.OrderStartDay = registers[37];
            target.OrderStartMonth = registers[38];
            target.OrderStartYear = registers[39];
            target.OrderFinishHour = registers[40];
            target.OrderFinishMin = registers[41];
            target.OrderFinishSec = registers[42];
            target.OrderFinishDay = registers[43];
            target.OrderFinishMonth = registers[44];
            target.OrderFinishYear = registers[45];
            target.ExtraWeight = (uint)registers[70] << 16 | registers[71];
        }

        public async Task ReadStaticOrderDataAsync(StaticOrderData target)
        {
            if (!_isConnected) throw new InvalidOperationException("Не подключено к весам");
            var registers = await Task.Run(() => _master.ReadHoldingRegistersAsync(_slaveId, 199, 66));
            
            target.sOrderTypeInformation = registers[0];
            target.sOrderWeight = Lib.DcrToInt(registers[1], registers[2]);
            target.sDefaultDraftSize = Lib.DcrToInt(registers[3], registers[4]);
            target.sOrderID = Lib.RegToString(registers, 5, 10);
            target.sTicketNumber = (uint)registers[10] << 16 | registers[11];
            target.sVesselID = Lib.RegToString(registers, 12, 12);
            target.sBinID = Lib.RegToString(registers, 18, 10);
            target.sProductID = Lib.RegToString(registers, 23, 8);
            target.sProductName = Lib.RegToString(registers, 27, 26);
            target.sProductDensity = registers[40];
            target.sCustomerName = Lib.RegToString(registers, 41, 30);
            target.sUnitTrainNumber = Lib.RegToString(registers, 56, 20);

            ushort[] reg = await Task.Run(() => _master.ReadHoldingRegistersAsync(_slaveId, 265, 74));
            target.sNotes = Lib.RegToString(reg, 0, 120);
            target.sLotSize = Lib.DcrToInt(reg[70], reg[71]);
            target.sGlobalTicketCounter = (uint)reg[72] << 16 | reg[73];

        }

        public async Task ReadConfigDataAsync(ConfigData target)
        {
            if (!_isConnected) throw new InvalidOperationException("Не подключено к весам");
            var registers = await Task.Run(() => _master.ReadHoldingRegistersAsync(_slaveId, 499, 110));
            
            target.ScaleName = Lib.RegToString(registers, 0, 20);
            target.ScaleCapacityInDecimal = Lib.DcrToInt(registers[10], registers[11]);
            target.DivisionSizeInDecimal = registers[12];
            target.Decimals = registers[13];
            target.WeightUnits = registers[14];
            target.DefaultDraftSize = Lib.DcrToInt(registers[15], registers[16]);
            target.MaxAddtoDraftSize = Lib.DcrToInt(registers[17], registers[18]);
            target.TareResetRange = Lib.DcrToInt(registers[19], registers[20]);
            target.WHEmptyOffset = Lib.DcrToInt(registers[21], registers[21]);
            target.GateControlType = registers[23];
            target.GatePulseTime = registers[24];
            target.GateDeadBand = registers[25];
            target.GateJoggingDeadBand = registers[26];
            target.LGCleanoutTime = registers[27];
            target.LGGateType = registers[28];
            target.MiscSingleBitData1 = registers[29];
            target.MiscSingleBitData2 = registers[30];
            target.DraftCountRollover = registers[31];
            target.DraftsPerSubtotal = registers[32];
            target.FillMotionDelayTime = registers[33];
            target.DumpMotionDelayTime = registers[34];
            target.AllowedTransactionTypes = registers[35];
            target.AuditPrinterPaperSize = registers[36];
            target.RemotePrinterPaperSize = registers[37];
            target.ReportPrinterPaperSize = registers[38];
            target.HeaderLine1 = Lib.RegToString(registers, 39, 40);
            target.HeaderLine2 = Lib.RegToString(registers, 59, 40);
            target.SignatureLine1 = Lib.RegToString(registers, 79, 30);
            target.SignatureLine2 = Lib.RegToString(registers, 94, 30);            
        }



        // -------------------- IWeightCommandService --------------------------
        public async Task StartCommandAsync(int numCommand)
        {
            if (!_isConnected) throw new InvalidOperationException("Не подключено к весам");

            // Логика отправки команд
            ushort[] CommandCode = { 0x0000, 0x0001, 0x0002, 0x0003, 0x1040, 0x1041, 0x1042, 0x1043, 0x1044, 0x1045, 0x1046, 0x1047, 0x104A, 0x104B, 0x104C, 0x104D, 0x104E, 0x104F, 0x1051, 0x1052, 0x1053, 0x1054, 0x1055, 0x1056, 0x1057, 0x1059, 0x105D, 0x105F, 0x1060, 0x1061, 0x1062, 0x1063, 0x1067, 0x1069, 0x106E, 0x1070, 0x1071, 0x1072, 0x1073, 0x1074, 0x1075, 0x1076, 0x3401, 0x3402, 0x340A, 0x4000 };
            ushort[] wr = new ushort[3];
            wr[0] = CommandCode[numCommand];

            // Все команды по умолчанию
            Debug.WriteLine($"numCommand = {numCommand}, wr[0] = {wr[0]}");

            await _master.WriteMultipleRegistersAsync(_slaveId, 1000, wr);            
          
        }




        public async Task StartNewOrderAsync(NewOrderData newOrder)
        {
            ushort[] wr = newOrder.GetRegisters();           
            await _master.WriteMultipleRegistersAsync(_slaveId, 1004, wr);
            await StartCommandAsync(16);            
        }

        //public async Task<bool> ReviseOrderAsync(uint newWeight, uint newDraft)
        //{
        //    // Логика отправки команд
        //    return true;
        //}

        //public async Task<bool> SetGatePositionAsync(int position)
        //{
        //    // Логика отправки команд
        //    return true;
        //}

    }



}
