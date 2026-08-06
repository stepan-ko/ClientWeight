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
        private int port;
        public ModbusWeightService(string Host, int slaveId, int Port = 502)
        {
            //_logger = logger;
            _slaveId = (byte)slaveId;
            host = Host;
            port = Port;
        }

        // ---------------- IWeightConnectionService ---------------
        public async Task<bool>  ConnectAsync()
        {
            try
            {
                Debug.WriteLine("ConnectAsync() host = " + host);
                
                _tcpClient = new TcpClient();
                await _tcpClient.ConnectAsync(host, port);
                var factory = new ModbusFactory();
                _master = factory.CreateMaster(_tcpClient);
                _master.Transport.ReadTimeout = 1000;
                _master.Transport.WriteTimeout = 1000;
                _isConnected = true;
                _logger.Info("Подключение к весам установлено");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка подключения: {ex}");
                _isConnected = false;
                //await ReConnectAsync();
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
        public async Task<StatusData> ReadStatusDataAsync()
        {
            if (!_isConnected) throw new InvalidOperationException("Не подключено к весам");

            var registers = await Task.Run(() => _master.ReadHoldingRegistersAsync(_slaveId, 49, 31));
            var statusData = new StatusData();
            
            statusData.ScaleMode = registers[0];
            statusData.StatusWord1 = registers[1];
            statusData.StatusWord2 = registers[2];
            statusData.ScaleAlarmWord1 = registers[3];
            statusData.ScaleAlarmWord2 = registers[4];
            statusData.InputWord1 = registers[5];
            statusData.InputWord2 = registers[6];
            statusData.OutputWord1 = registers[7];
            statusData.OutputWord2 = registers[8];
            statusData.SmartAlarmWord1 = registers[9];
            statusData.SmartAlarmWord2 = registers[10];
            statusData.DWIStatus = registers[13];
            statusData.ScaleWeight = Lib.DcrToInt(registers[14], registers[15]);
            statusData.ScaleFlowRate = Lib.DcrToInt(registers[16], registers[17]);
            statusData.ScaleGatePosition = registers[18];
            statusData.RejectCode = registers[19];
            statusData.AcceptedCode = registers[20];
            statusData.CommunicationResult = registers[21];
            statusData.EventIdMask = (uint)registers[22] << 16 | registers[23];
            statusData.MastersCount = Lib.GetLowByte(registers[24]);
            statusData.ObservCount = Lib.GetHiByte(registers[24]);
            statusData.CounterLive = registers[25];
            statusData.TimeOur = registers[26];
            statusData.TimeMin = registers[27];
            statusData.TimeSec = registers[28];
            statusData.TimeMSec = registers[29];
            statusData.ClientInfo = registers[30];
            //Debug.WriteLine("statusData.ScaleWeight = " + statusData.ScaleWeight);
            return statusData;
        }

        public async Task<MiscStatusData> ReadMiscStatusDataAsync()
        {
            if (!_isConnected) throw new InvalidOperationException("Не подключено к весам");
            var registers = await Task.Run(() => _master.ReadHoldingRegistersAsync(_slaveId, 399, 30));
            MiscStatusData miscData = new MiscStatusData();

            miscData.mBuildupTestWeight = Lib.DcrToInt(registers[0], registers[1]);
            miscData.mRunningTotalWeight = Lib.DcrToInt(registers[2], registers[3]);
            miscData.mTotalDraftCounter = registers[4];
            miscData.mGateSetpoint = registers[5];
            miscData.mSelShippWord = registers[6];
            miscData.mHWWord1 = registers[8]; // уточнить какой регистр
            miscData.mStatisticsLoggingStatus = registers[9];
            miscData.mOrderPreCutoff = (uint)registers[10] << 16 | registers[11];
            miscData.mLGGateState = registers[12];
            miscData.mHourlyTotal = Lib.DcrToInt(registers[13], registers[14]);
            miscData.mDailyTotal = Lib.DcrToInt(registers[15], registers[16]);
            miscData.mHourlyTotalHour = registers[17];
            miscData.mHourlyTotalMin = registers[18];
            miscData.mHourlyTotalSec = registers[19];
            miscData.mHourlyTotalDay = registers[20];
            miscData.mHourlyTotalMonth = registers[21];
            miscData.mHourlyTotalYear = registers[22];
            miscData.mDailyTotalHour = registers[23];
            miscData.mDailyTotalMin = registers[24];
            miscData.mDailyTotalSec = registers[25];
            miscData.mDailyTotalDay = registers[26];
            miscData.mDailyTotalMonth = registers[27];
            miscData.mDailyTotalYear = registers[28];

            return miscData;
        }


        public async Task<OrderData> ReadOrderDataAsync()
        {
            if (!_isConnected) throw new InvalidOperationException("Не подключено к весам");
            var registers = await Task.Run(() => _master.ReadHoldingRegistersAsync(_slaveId, 99, 72));
            OrderData orderData = new OrderData();
            orderData.DraftGrossWeight = Lib.DcrToInt(registers[0], registers[1]);
            orderData.DraftGrossWeightHour = registers[2];
            orderData.DraftGrossWeightMin = registers[3];
            orderData.DraftGrossWeightSec = registers[4];
            orderData.DraftGrossWeightDay = registers[5];
            orderData.DraftGrossWeightMonth = registers[6];
            orderData.DraftGrossWeightYear = registers[7];
            orderData.DraftTareWeight = Lib.DcrToInt(registers[8], registers[9]);
            orderData.DraftTareWeightHour = registers[10];
            orderData.DraftTareWeightMin = registers[11];
            orderData.DraftTareWeightSec = registers[12];
            orderData.DraftTareWeightDay = registers[13];
            orderData.DraftTareWeightMonth = registers[14];
            orderData.DraftTareWeightYear = registers[15];
            orderData.DraftNetWeight = Lib.DcrToInt(registers[16], registers[17]);
            orderData.FlowRate = Lib.DcrToInt(registers[18], registers[19]);
            orderData.OrderBalance = Lib.DcrToInt(registers[22], registers[23]);
            orderData.TotalWeight = Lib.DcrToInt(registers[24], registers[25]);
            orderData.OrderDraftSize = Lib.DcrToInt(registers[26], registers[27]);
            orderData.SubtotalWeight = Lib.DcrToInt(registers[28], registers[29]);
            orderData.CurrentDruftCount = registers[30];
            orderData.PlannedNumberDruftCount = registers[31];
            orderData.DraftTargetWeight = Lib.DcrToInt(registers[32], registers[33]);
            orderData.OrderStartHour = registers[34];
            orderData.OrderStartMin = registers[35];
            orderData.OrderStartSec = registers[36];
            orderData.OrderStartDay = registers[37];
            orderData.OrderStartMonth = registers[38];
            orderData.OrderStartYear = registers[39];
            orderData.OrderFinishHour = registers[40];
            orderData.OrderFinishMin = registers[41];
            orderData.OrderFinishSec = registers[42];
            orderData.OrderFinishDay = registers[43];
            orderData.OrderFinishMonth = registers[44];
            orderData.OrderFinishYear = registers[45];
            orderData.ExtraWeight = (uint)registers[70] << 16 | registers[71];

            return orderData;
        }

        public async Task<StaticOrderData> ReadStaticOrderDataAsync()
        {
            if (!_isConnected) throw new InvalidOperationException("Не подключено к весам");
            var registers = await Task.Run(() => _master.ReadHoldingRegistersAsync(_slaveId, 199, 66));
            StaticOrderData orderDataStatic = new StaticOrderData();
            
            orderDataStatic.sOrderTypeInformation = registers[0];
            orderDataStatic.sOrderWeight = Lib.DcrToInt(registers[1], registers[2]);
            orderDataStatic.sDefaultDraftSize = Lib.DcrToInt(registers[3], registers[4]);
            orderDataStatic.sOrderID = Lib.RegToString(registers, 5, 10);
            orderDataStatic.sTicketNumber = (uint)registers[10] << 16 | registers[11];
            orderDataStatic.sVesselID = Lib.RegToString(registers, 12, 12);
            orderDataStatic.sBinID = Lib.RegToString(registers, 18, 10);
            orderDataStatic.sProductID = Lib.RegToString(registers, 23, 8);
            orderDataStatic.sProductName = Lib.RegToString(registers, 27, 26);
            orderDataStatic.sProductDensity = registers[40];
            orderDataStatic.sCustomerName = Lib.RegToString(registers, 41, 30);
            orderDataStatic.sUnitTrainNumber = Lib.RegToString(registers, 56, 20);

            ushort[] reg = await Task.Run(() => _master.ReadHoldingRegistersAsync(_slaveId, 265, 74));
            orderDataStatic.sNotes = Lib.RegToString(reg, 0, 120);
            orderDataStatic.sLotSize = Lib.DcrToInt(reg[70], reg[71]);
            orderDataStatic.sGlobalTicketCounter = (uint)reg[72] << 16 | reg[73];

            return orderDataStatic;
        }

        public async Task<ConfigData> ReadConfigDataAsync()
        {
            if (!_isConnected) throw new InvalidOperationException("Не подключено к весам");

            var registers = await Task.Run(() => _master.ReadHoldingRegistersAsync(_slaveId, 499, 110));
            ConfigData configData = new ConfigData();
           
            // if ((statusData.EventIdMask & 0x20000) == 0x20000 || firstConn)
            configData.ScaleName = Lib.RegToString(registers, 0, 20);
            configData.ScaleCapacityInDecimal = Lib.DcrToInt(registers[10], registers[11]);
            configData.DivisionSizeInDecimal = registers[12];
            configData.Decimals = registers[13];
            configData.WeightUnits = registers[14];
            configData.DefaultDraftSize = Lib.DcrToInt(registers[15], registers[16]);
            configData.MaxAddtoDraftSize = Lib.DcrToInt(registers[17], registers[18]);
            configData.TareResetRange = Lib.DcrToInt(registers[19], registers[20]);
            configData.WHEmptyOffset = Lib.DcrToInt(registers[21], registers[21]);
            configData.GateControlType = registers[23];
            configData.GatePulseTime = registers[24];
            configData.GateDeadBand = registers[25];
            configData.GateJoggingDeadBand = registers[26];
            configData.LGCleanoutTime = registers[27];
            configData.LGGateType = registers[28];
            configData.MiscSingleBitData1 = registers[29];
            configData.MiscSingleBitData2 = registers[30];
            configData.DraftCountRollover = registers[31];
            configData.DraftsPerSubtotal = registers[32];
            configData.FillMotionDelayTime = registers[33];
            configData.DumpMotionDelayTime = registers[34];
            configData.AllowedTransactionTypes = registers[35];
            configData.AuditPrinterPaperSize = registers[36];
            configData.RemotePrinterPaperSize = registers[37];
            configData.ReportPrinterPaperSize = registers[38];
            configData.HeaderLine1 = Lib.RegToString(registers, 39, 40);
            configData.HeaderLine2 = Lib.RegToString(registers, 59, 40);
            configData.SignatureLine1 = Lib.RegToString(registers, 79, 30);
            configData.SignatureLine2 = Lib.RegToString(registers, 94, 30);
            return configData;
        }



        // -------------------- IWeightCommandService --------------------------
        public async Task<bool> StartCommandAsync(int numCommand)
        {
            if (!_isConnected) throw new InvalidOperationException("Не подключено к весам");

            // Логика отправки команд
            ushort[] CommandCode = { 0x0000, 0x0001, 0x0002, 0x0003, 0x1040, 0x1041, 0x1042, 0x1043, 0x1044, 0x1045, 0x1046, 0x1047, 0x104A, 0x104B, 0x104C, 0x104D, 0x104E, 0x104F, 0x1051, 0x1052, 0x1053, 0x1054, 0x1055, 0x1056, 0x1057, 0x1059, 0x105D, 0x105F, 0x1060, 0x1061, 0x1062, 0x1063, 0x1067, 0x1069, 0x106E, 0x1070, 0x1071, 0x1072, 0x1073, 0x1074, 0x1075, 0x1076, 0x3401, 0x3402, 0x340A, 0x4000 };
            ushort[] wr = new ushort[3];
            wr[0] = CommandCode[numCommand];

            // Все команды по умолчанию
            await Task.Run(() => _master.WriteMultipleRegistersAsync(_slaveId, 1000, wr));
            
            return true;
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
