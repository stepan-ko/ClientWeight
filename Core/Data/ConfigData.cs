using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace Weight
{
    public class ConfigData : ReactiveObject
    {
        
        private string _ScaleName = "";
        private int _ScaleCapacityInDecimal;
        private int _DivisionSizeInDecimal;
        private int _Decimals;
        private int _WeightUnits;
        private int _DefaultDraftSize;
        private int _MaxAddtoDraftSize;
        private int _TareResetRange;
        private int _WHEmptyOffset;
        private int _GateControlType;
        private int _GatePulseTime;
        private int _GateDeadBand;
        private int _GateJoggingDeadBand;
        private int _LGCleanoutTime;
        private int _LGGateType;
        private ushort _MiscSingleBitData1;
        private ushort _MiscSingleBitData2;
        private int _DraftCountRollover;
        private int _DraftsPerSubtotal;
        private int _FillMotionDelayTime;
        private int _DumpMotionDelayTime;
        private ushort _AllowedTransactionTypes;
        private int _AuditPrinterPaperSize;
        private int _RemotePrinterPaperSize;
        private int _ReportPrinterPaperSize;
        private string _HeaderLine1 = "";
        private string _HeaderLine2 = "";
        private string _SignatureLine1 = "";
        private string _SignatureLine2 = "";
        private int _PrintCustomTicketInformation;
        private int _DeviationInfluencein;
        private int _UnderZeroRange;
        private int _OvercapacityRange;
        private int _MaxAllowableDraftSize;
        private int _MinAllowableDraftSize;
        private int _DirectTrimRange;
        private int _JogRangein;
        private int _UGGateJogTime;
        private int _MaxWeightThroughUG;
        private int _TrimLSPosition;
        private uint _SmartTechAlarmsMask1;
        private uint _SmartTechAlarmsMask2;
        private int _GateFullMotionTime;
        private int _GateTrimMotionTime;
        private int _GateDirectTrimMotionTime;
        private int _GateFullTrimMotionTime;
        private int _LGGateADRange;
        private int _DWIMotionRange;
        private ushort _SubtotalsConfiguration;
        private int _ModbusNodeAddress;
        private ushort _SerialCommParameters;
        private uint _IPAddress;
        private uint _SubnetMask;
        private int _TCPPort;
        private int _ModbusTimeout;
        private int _SocketTimeout;
        private int _MaxTCPConnectionsAllowed;
        private uint _DefaultGateway;
        private int _WHGateType;

        private bool _mbHydraulicUnitStatus; // 15
        private bool _mbBypassDSOptimizing; // 14
        private bool _mbLGLowLevelPresent; // 13
        private bool _mbWHCloseSolenoidPresent; // 12
        private bool _mbUGCloseSolenoidPresent; // 11
        private bool _mbReportPrinterPresent; // 10
        private bool _mbRemotePrinterFormFeed; // 9
        private bool _mbWideRemotePrinter; // 8
        private bool _mbRemotePrinterPresent; // 7
        private bool _mbAuditPrinterFormFeed; // 6
        private bool _mbWideAuditPrinter; // 5
        private bool _mbPrintSignatureLines; // 4
        private bool _mbExtendMotionInterval; // 3
        private bool _mbOvercapacityManualPrint; // 2
        private bool _mbUGMidLevelContactInUse; // 1
        private bool _mbAutoTrimInUse; // 0

        private bool _mbWaitingforPurge; // 8
        private bool _mbBypassOvercapAssistant; // 7
        private bool _mbDisableAuthorizingEXP; // 6
        private bool _mbAskforBinCompleteEXP; // 5
        private bool _mbAutoSelfLearningOn; // 4
        private bool _mbLocalInhibitEXP; // 3
        private bool _mbClearMemOnBootEXP; // 2
        private bool _mbRecvOrderWtAllowed; // 1
        private bool _mbPrintCertificates; // 0

        private bool _typeXFER; // 10
        private bool _typeGenericInbound; // 9
        private bool _typeGenericOutbound; // 8
        private bool _typeShipInbound; // 7
        private bool _typeShipOutbound; // 6
        private bool _typeBargeInbound; // 5
        private bool _typeBargeOutbound; // 4
        private bool _typeTruckInbound; // 3
        private bool _typeTruckOutbound; // 2
        private bool _typeRailInbound; // 1
        private bool _typeRailOutbound; // 0

        private bool _subtotalPrintHourlyTotal; // 1
        private bool _subtotalPrintOrderSubtotal; // 0
        private int _subtotalResetHourlyTotal;
        private bool _subtotalResetDailyTotalOnPrint; // 5
        private bool _subtotalPrintDailyTotal; // 4
        private int _subtotalHourOfDayWhenPrintDailyTotal;

        private int _commBaudRate;
        private int _commDataWord;
        private int _commStopBits;
        private int _commParity;
        private int _commHandshaking;

        private string _IPAddressString = "";
        private string _SubnetMaskString = "";
        private string _DefaultGatewayString = "";
                
        private void UpdateMiscWord1()
        {
            mbHydraulicUnitStatus = Lib.GetBitWord(MiscSingleBitData1, 15);           
            mbBypassDSOptimizing = Lib.GetBitWord(MiscSingleBitData1, 14);
            mbLGLowLevelPresent = Lib.GetBitWord(MiscSingleBitData1, 13);
            mbWHCloseSolenoidPresent = Lib.GetBitWord(MiscSingleBitData1, 12);
            mbUGCloseSolenoidPresent = Lib.GetBitWord(MiscSingleBitData1, 11);
            mbReportPrinterPresent = Lib.GetBitWord(MiscSingleBitData1, 10);
            mbRemotePrinterFormFeed = Lib.GetBitWord(MiscSingleBitData1, 9);
            mbWideRemotePrinter = Lib.GetBitWord(MiscSingleBitData1, 8);
            mbRemotePrinterPresent = Lib.GetBitWord(MiscSingleBitData1, 7);
            mbAuditPrinterFormFeed = Lib.GetBitWord(MiscSingleBitData1, 6);
            mbWideAuditPrinter = Lib.GetBitWord(MiscSingleBitData1, 5);
            mbPrintSignatureLines = Lib.GetBitWord(MiscSingleBitData1, 4);
            mbExtendMotionInterval = Lib.GetBitWord(MiscSingleBitData1, 3);
            mbOvercapacityManualPrint = Lib.GetBitWord(MiscSingleBitData1, 2);
            mbUGMidLevelContactInUse = Lib.GetBitWord(MiscSingleBitData1, 1);
            mbAutoTrimInUse = Lib.GetBitWord(MiscSingleBitData1, 0);
        }

        /// <summary>
        /// Обновить биты дополнительного слова 2
        /// </summary>
        private void UpdateMiscWord2()
        {
            mbWaitingforPurge = Lib.GetBitWord(MiscSingleBitData2, 8);
            mbBypassOvercapAssistant = Lib.GetBitWord(MiscSingleBitData2, 7);
            mbDisableAuthorizingEXP = Lib.GetBitWord(MiscSingleBitData2, 6);
            mbAskforBinCompleteEXP = Lib.GetBitWord(MiscSingleBitData2, 5);
            mbAutoSelfLearningOn = Lib.GetBitWord(MiscSingleBitData2, 4);
            mbLocalInhibitEXP = Lib.GetBitWord(MiscSingleBitData2, 3);
            mbClearMemOnBootEXP = Lib.GetBitWord(MiscSingleBitData2, 2);
            mbRecvOrderWtAllowed = Lib.GetBitWord(MiscSingleBitData2, 1);
            mbPrintCertificates = Lib.GetBitWord(MiscSingleBitData2, 0);
        }

        /// <summary>
        /// Обновить биты Разрешенные типы работы весов
        /// </summary>
        private void UpdateTypes()
        {
            typeXFER = Lib.GetBitWord(AllowedTransactionTypes, 10);
            typeGenericInbound = Lib.GetBitWord(AllowedTransactionTypes, 9);
            typeGenericOutbound = Lib.GetBitWord(AllowedTransactionTypes, 8);
            typeShipInbound = Lib.GetBitWord(AllowedTransactionTypes, 7);
            typeShipOutbound = Lib.GetBitWord(AllowedTransactionTypes, 6);
            typeBargeInbound = Lib.GetBitWord(AllowedTransactionTypes, 5);
            typeBargeOutbound = Lib.GetBitWord(AllowedTransactionTypes, 4);
            typeTruckInbound = Lib.GetBitWord(AllowedTransactionTypes, 3);
            typeTruckOutbound = Lib.GetBitWord(AllowedTransactionTypes, 2);
            typeRailInbound = Lib.GetBitWord(AllowedTransactionTypes, 1);
            typeRailOutbound = Lib.GetBitWord(AllowedTransactionTypes, 0);
        }

        /// <summary>
        /// Обновить настройки конфигурации счетчиков
        /// </summary>
        private void UpdateSubtotalsConfig()
        {
            subtotalPrintHourlyTotal = Lib.GetBitWord(SubtotalsConfiguration, 1);
            subtotalPrintOrderSubtotal = Lib.GetBitWord(SubtotalsConfiguration, 0);
            subtotalResetDailyTotalOnPrint = Lib.GetBitWord(SubtotalsConfiguration, 5);
            subtotalPrintDailyTotal = Lib.GetBitWord(SubtotalsConfiguration, 4);
            subtotalHourOfDayWhenPrintDailyTotal = SubtotalsConfiguration >> 2 | 0x3;
            subtotalResetHourlyTotal = SubtotalsConfiguration >> 8 | 0xFF;
        }

        /// <summary>
        /// Обновить параметры настройки com-порта
        /// </summary>
        private void UpdateComm()
        {
            commBaudRate = SerialCommParameters| 0xF;
            commDataWord = SerialCommParameters >> 4 | 0x3;
            commStopBits = SerialCommParameters >> 6 | 0x3;
            commParity = SerialCommParameters >> 8 | 0x3;
            commHandshaking = SerialCommParameters >> 10 | 0x3;
        }




        public string ScaleName { get => _ScaleName; set => this.RaiseAndSetIfChanged(ref _ScaleName, value); }
        public int ScaleCapacityInDecimal { get => _ScaleCapacityInDecimal; set => this.RaiseAndSetIfChanged(ref _ScaleCapacityInDecimal, value); }
        public int DivisionSizeInDecimal { get => _DivisionSizeInDecimal; set => this.RaiseAndSetIfChanged(ref _DivisionSizeInDecimal, value); }
        public int Decimals { get => _Decimals; set => this.RaiseAndSetIfChanged(ref _Decimals, value); }
        public int WeightUnits { get => _WeightUnits; set => this.RaiseAndSetIfChanged(ref _WeightUnits, value); }
        public int DefaultDraftSize { get => _DefaultDraftSize; set => this.RaiseAndSetIfChanged(ref _DefaultDraftSize, value); }
        public int MaxAddtoDraftSize { get => _MaxAddtoDraftSize; set => this.RaiseAndSetIfChanged(ref _MaxAddtoDraftSize, value); }
        public int TareResetRange { get => _TareResetRange; set => this.RaiseAndSetIfChanged(ref _TareResetRange, value); }
        public int WHEmptyOffset { get => _WHEmptyOffset; set => this.RaiseAndSetIfChanged(ref _WHEmptyOffset, value); }
        public int GateControlType { get => _GateControlType; set => this.RaiseAndSetIfChanged(ref _GateControlType, value); }
        public int GatePulseTime { get => _GatePulseTime; set => this.RaiseAndSetIfChanged(ref _GatePulseTime, value); }
        public int GateDeadBand { get => _GateDeadBand; set => this.RaiseAndSetIfChanged(ref _GateDeadBand, value); }
        public int GateJoggingDeadBand { get => _GateJoggingDeadBand; set => this.RaiseAndSetIfChanged(ref _GateJoggingDeadBand, value); }
        public int LGCleanoutTime { get => _LGCleanoutTime; set => this.RaiseAndSetIfChanged(ref _LGCleanoutTime, value); }
        public int LGGateType { get => _LGGateType; set => this.RaiseAndSetIfChanged(ref _LGGateType, value); }
        public ushort MiscSingleBitData1 
        { 
            get => _MiscSingleBitData1;
            set
            {
                if (_MiscSingleBitData1 != value) UpdateMiscWord1();
                this.RaiseAndSetIfChanged(ref _MiscSingleBitData1, value);
            }
        }
        public ushort MiscSingleBitData2
        {
            get => _MiscSingleBitData2;
            set
            {
                if (_MiscSingleBitData2 != value) UpdateMiscWord2();
                this.RaiseAndSetIfChanged(ref _MiscSingleBitData2, value);
            }
        }
        public int DraftCountRollover { get => _DraftCountRollover; set => this.RaiseAndSetIfChanged(ref _DraftCountRollover, value); }
        public int DraftsPerSubtotal { get => _DraftsPerSubtotal; set => this.RaiseAndSetIfChanged(ref _DraftsPerSubtotal, value); }
        public int FillMotionDelayTime { get => _FillMotionDelayTime; set => this.RaiseAndSetIfChanged(ref _FillMotionDelayTime, value); }
        public int DumpMotionDelayTime { get => _DumpMotionDelayTime; set => this.RaiseAndSetIfChanged(ref _DumpMotionDelayTime, value); }
        public ushort AllowedTransactionTypes 
        { 
            get => _AllowedTransactionTypes;
            set
            {
                if (_AllowedTransactionTypes != value) UpdateTypes();
                this.RaiseAndSetIfChanged(ref _AllowedTransactionTypes, value);
            }
        }
        public int AuditPrinterPaperSize { get => _AuditPrinterPaperSize; set => this.RaiseAndSetIfChanged(ref _AuditPrinterPaperSize, value); }
        public int RemotePrinterPaperSize { get => _RemotePrinterPaperSize; set => this.RaiseAndSetIfChanged(ref _RemotePrinterPaperSize, value); }
        public int ReportPrinterPaperSize { get => _ReportPrinterPaperSize; set => this.RaiseAndSetIfChanged(ref _ReportPrinterPaperSize, value); }
        public string HeaderLine1 { get => _HeaderLine1; set => this.RaiseAndSetIfChanged(ref _HeaderLine1, value); }
        public string HeaderLine2 { get => _HeaderLine2; set => this.RaiseAndSetIfChanged(ref _HeaderLine2, value); }
        public string SignatureLine1 { get => _SignatureLine1; set => this.RaiseAndSetIfChanged(ref _SignatureLine1, value); }
        public string SignatureLine2 { get => _SignatureLine2; set => this.RaiseAndSetIfChanged(ref _SignatureLine2, value); }
        public int PrintCustomTicketInformation { get => _PrintCustomTicketInformation; set => this.RaiseAndSetIfChanged(ref _PrintCustomTicketInformation, value); }
        public int DeviationInfluencein { get => _DeviationInfluencein; set => this.RaiseAndSetIfChanged(ref _DeviationInfluencein, value); }
        public int UnderZeroRange { get => _UnderZeroRange; set => this.RaiseAndSetIfChanged(ref _UnderZeroRange, value); }
        public int OvercapacityRange { get => _OvercapacityRange; set => this.RaiseAndSetIfChanged(ref _OvercapacityRange, value); }
        public int MaxAllowableDraftSize { get => _MaxAllowableDraftSize; set => this.RaiseAndSetIfChanged(ref _MaxAllowableDraftSize, value); }
        public int MinAllowableDraftSize { get => _MinAllowableDraftSize; set => this.RaiseAndSetIfChanged(ref _MinAllowableDraftSize, value); }
        public int DirectTrimRange { get => _DirectTrimRange; set => this.RaiseAndSetIfChanged(ref _DirectTrimRange, value); }
        public int JogRangein { get => _JogRangein; set => this.RaiseAndSetIfChanged(ref _JogRangein, value); }
        public int UGGateJogTime { get => _UGGateJogTime; set => this.RaiseAndSetIfChanged(ref _UGGateJogTime, value); }
        public int MaxWeightThroughUG { get => _MaxWeightThroughUG; set => this.RaiseAndSetIfChanged(ref _MaxWeightThroughUG, value); }
        public int TrimLSPosition { get => _TrimLSPosition; set => this.RaiseAndSetIfChanged(ref _TrimLSPosition, value); }
        public uint SmartTechAlarmsMask1 { get => _SmartTechAlarmsMask1; set => this.RaiseAndSetIfChanged(ref _SmartTechAlarmsMask1, value); }
        public uint SmartTechAlarmsMask2 { get => _SmartTechAlarmsMask2; set => this.RaiseAndSetIfChanged(ref _SmartTechAlarmsMask2, value); }
        public int GateFullMotionTime { get => _GateFullMotionTime; set => this.RaiseAndSetIfChanged(ref _GateFullMotionTime, value); }
        public int GateTrimMotionTime { get => _GateTrimMotionTime; set => this.RaiseAndSetIfChanged(ref _GateTrimMotionTime, value); }
        public int GateDirectTrimMotionTime { get => _GateDirectTrimMotionTime; set => this.RaiseAndSetIfChanged(ref _GateDirectTrimMotionTime, value); }
        public int GateFullTrimMotionTime { get => _GateFullTrimMotionTime; set => this.RaiseAndSetIfChanged(ref _GateFullTrimMotionTime, value); }
        public int LGGateADRange { get => _LGGateADRange; set => this.RaiseAndSetIfChanged(ref _LGGateADRange, value); }
        public int DWIMotionRange { get => _DWIMotionRange; set => this.RaiseAndSetIfChanged(ref _DWIMotionRange, value); }
        public ushort SubtotalsConfiguration 
        { 
            get => _SubtotalsConfiguration;
            set
            {
                if(_SubtotalsConfiguration != value) UpdateSubtotalsConfig();
                this.RaiseAndSetIfChanged(ref _SubtotalsConfiguration, value);
            }
        }
        public int ModbusNodeAddress { get => _ModbusNodeAddress; set => this.RaiseAndSetIfChanged(ref _ModbusNodeAddress, value); }
        public ushort SerialCommParameters 
        { 
            get => _SerialCommParameters;
            set
            {
                if (_SerialCommParameters != value) UpdateComm();
                this.RaiseAndSetIfChanged(ref _SerialCommParameters, value);
            }
        }
        public uint IPAddress 
        { 
            get => _IPAddress; 
            set 
            {
                if (_IPAddress != value) IPAddressString = Lib.RegIpToString(value);
                this.RaiseAndSetIfChanged(ref _IPAddress, value); 
            } 
        }
        public uint SubnetMask 
        { 
            get => _SubnetMask;
            set
            {
                if (_SubnetMask != value) SubnetMaskString = Lib.RegIpToString(value);
                this.RaiseAndSetIfChanged(ref _SubnetMask, value);
            }
        }
        public int TCPPort { get => _TCPPort; set => this.RaiseAndSetIfChanged(ref _TCPPort, value); }
        public int ModbusTimeout { get => _ModbusTimeout; set => this.RaiseAndSetIfChanged(ref _ModbusTimeout, value); }
        public int SocketTimeout { get => _SocketTimeout; set => this.RaiseAndSetIfChanged(ref _SocketTimeout, value); }
        public int MaxTCPConnectionsAllowed { get => _MaxTCPConnectionsAllowed; set => this.RaiseAndSetIfChanged(ref _MaxTCPConnectionsAllowed, value); }
        public uint DefaultGateway
        {
            get => _DefaultGateway;
            set 
            { 
                if (_DefaultGateway != value) DefaultGatewayString = Lib.RegIpToString(value);
                this.RaiseAndSetIfChanged(ref _DefaultGateway, value); 
            }
        }
        public int WHGateType { get => _WHGateType; set => this.RaiseAndSetIfChanged(ref _WHGateType, value); }

        public bool mbHydraulicUnitStatus { get => _mbHydraulicUnitStatus; set => this.RaiseAndSetIfChanged(ref _mbHydraulicUnitStatus, value); }
        public bool mbBypassDSOptimizing { get => _mbBypassDSOptimizing; set => this.RaiseAndSetIfChanged(ref _mbBypassDSOptimizing, value); }
        public bool mbLGLowLevelPresent { get => _mbLGLowLevelPresent; set => this.RaiseAndSetIfChanged(ref _mbLGLowLevelPresent, value); }
        public bool mbWHCloseSolenoidPresent { get => _mbWHCloseSolenoidPresent; set => this.RaiseAndSetIfChanged(ref _mbWHCloseSolenoidPresent, value); }
        public bool mbUGCloseSolenoidPresent { get => _mbUGCloseSolenoidPresent; set => this.RaiseAndSetIfChanged(ref _mbUGCloseSolenoidPresent, value); }
        public bool mbReportPrinterPresent { get => _mbReportPrinterPresent; set => this.RaiseAndSetIfChanged(ref _mbReportPrinterPresent, value); }
        public bool mbRemotePrinterFormFeed { get => _mbRemotePrinterFormFeed; set => this.RaiseAndSetIfChanged(ref _mbRemotePrinterFormFeed, value); }
        public bool mbWideRemotePrinter { get => _mbWideRemotePrinter; set => this.RaiseAndSetIfChanged(ref _mbWideRemotePrinter, value); }
        public bool mbRemotePrinterPresent { get => _mbRemotePrinterPresent; set => this.RaiseAndSetIfChanged(ref _mbRemotePrinterPresent, value); }
        public bool mbAuditPrinterFormFeed { get => _mbAuditPrinterFormFeed; set => this.RaiseAndSetIfChanged(ref _mbAuditPrinterFormFeed, value); }
        public bool mbWideAuditPrinter { get => _mbWideAuditPrinter; set => this.RaiseAndSetIfChanged(ref _mbWideAuditPrinter, value); }
        public bool mbPrintSignatureLines { get => _mbPrintSignatureLines; set => this.RaiseAndSetIfChanged(ref _mbPrintSignatureLines, value); }
        public bool mbExtendMotionInterval { get => _mbExtendMotionInterval; set => this.RaiseAndSetIfChanged(ref _mbExtendMotionInterval, value); }
        public bool mbOvercapacityManualPrint { get => _mbOvercapacityManualPrint; set => this.RaiseAndSetIfChanged(ref _mbOvercapacityManualPrint, value); }
        public bool mbUGMidLevelContactInUse { get => _mbUGMidLevelContactInUse; set => this.RaiseAndSetIfChanged(ref _mbUGMidLevelContactInUse, value); }
        public bool mbAutoTrimInUse { get => _mbAutoTrimInUse; set => this.RaiseAndSetIfChanged(ref _mbAutoTrimInUse, value); }

        public bool mbWaitingforPurge { get => _mbWaitingforPurge; set => this.RaiseAndSetIfChanged(ref _mbWaitingforPurge, value); }
        public bool mbBypassOvercapAssistant { get => _mbBypassOvercapAssistant; set => this.RaiseAndSetIfChanged(ref _mbBypassOvercapAssistant, value); }
        public bool mbDisableAuthorizingEXP { get => _mbDisableAuthorizingEXP; set => this.RaiseAndSetIfChanged(ref _mbDisableAuthorizingEXP, value); }
        public bool mbAskforBinCompleteEXP { get => _mbAskforBinCompleteEXP; set => this.RaiseAndSetIfChanged(ref _mbAskforBinCompleteEXP, value); }
        public bool mbAutoSelfLearningOn { get => _mbAutoSelfLearningOn; set => this.RaiseAndSetIfChanged(ref _mbAutoSelfLearningOn, value); }
        public bool mbLocalInhibitEXP { get => _mbLocalInhibitEXP; set => this.RaiseAndSetIfChanged(ref _mbLocalInhibitEXP, value); }
        public bool mbClearMemOnBootEXP { get => _mbClearMemOnBootEXP; set => this.RaiseAndSetIfChanged(ref _mbClearMemOnBootEXP, value); }
        public bool mbRecvOrderWtAllowed { get => _mbRecvOrderWtAllowed; set => this.RaiseAndSetIfChanged(ref _mbRecvOrderWtAllowed, value); }
        public bool mbPrintCertificates { get => _mbPrintCertificates; set => this.RaiseAndSetIfChanged(ref _mbPrintCertificates, value); }

        public bool typeXFER { get => _typeXFER; set => this.RaiseAndSetIfChanged(ref _typeXFER, value); }
        public bool typeGenericInbound { get => _typeGenericInbound; set => this.RaiseAndSetIfChanged(ref _typeGenericInbound, value); }
        public bool typeGenericOutbound { get => _typeGenericOutbound; set => this.RaiseAndSetIfChanged(ref _typeGenericOutbound, value); }
        public bool typeShipInbound { get => _typeShipInbound; set => this.RaiseAndSetIfChanged(ref _typeShipInbound, value); }
        public bool typeShipOutbound { get => _typeShipOutbound; set => this.RaiseAndSetIfChanged(ref _typeShipOutbound, value); }
        public bool typeBargeInbound { get => _typeBargeInbound; set => this.RaiseAndSetIfChanged(ref _typeBargeInbound, value); }
        public bool typeBargeOutbound { get => _typeBargeOutbound; set => this.RaiseAndSetIfChanged(ref _typeBargeOutbound, value); }
        public bool typeTruckInbound { get => _typeTruckInbound; set => this.RaiseAndSetIfChanged(ref _typeTruckInbound, value); }
        public bool typeTruckOutbound { get => _typeTruckOutbound; set => this.RaiseAndSetIfChanged(ref _typeTruckOutbound, value); }
        public bool typeRailInbound { get => _typeRailInbound; set => this.RaiseAndSetIfChanged(ref _typeRailInbound, value); }
        public bool typeRailOutbound { get => _typeRailOutbound; set => this.RaiseAndSetIfChanged(ref _typeRailOutbound, value); }

        public bool subtotalPrintHourlyTotal { get => _subtotalPrintHourlyTotal; set => this.RaiseAndSetIfChanged(ref _subtotalPrintHourlyTotal, value); }
        public bool subtotalPrintOrderSubtotal { get => _subtotalPrintOrderSubtotal; set => this.RaiseAndSetIfChanged(ref _subtotalPrintOrderSubtotal, value); }
        public int subtotalResetHourlyTotal { get => _subtotalResetHourlyTotal; set => this.RaiseAndSetIfChanged(ref _subtotalResetHourlyTotal, value); }
        public bool subtotalResetDailyTotalOnPrint { get => _subtotalResetDailyTotalOnPrint; set => this.RaiseAndSetIfChanged(ref _subtotalResetDailyTotalOnPrint, value); }
        public bool subtotalPrintDailyTotal { get => _subtotalPrintDailyTotal; set => this.RaiseAndSetIfChanged(ref _subtotalPrintDailyTotal, value); }
        public int subtotalHourOfDayWhenPrintDailyTotal { get => _subtotalHourOfDayWhenPrintDailyTotal; set => this.RaiseAndSetIfChanged(ref _subtotalHourOfDayWhenPrintDailyTotal, value); }

        public int commBaudRate { get => _commBaudRate; set => this.RaiseAndSetIfChanged(ref _commBaudRate, value); }
        public int commDataWord { get => _commDataWord; set => this.RaiseAndSetIfChanged(ref _commDataWord, value); }
        public int commStopBits { get => _commStopBits; set => this.RaiseAndSetIfChanged(ref _commStopBits, value); }
        public int commParity { get => _commParity; set => this.RaiseAndSetIfChanged(ref _commParity, value); }
        public int commHandshaking { get => _commHandshaking; set => this.RaiseAndSetIfChanged(ref _commHandshaking, value); }

        public string IPAddressString { get => _IPAddressString; set => this.RaiseAndSetIfChanged(ref _IPAddressString, value); }
        public string SubnetMaskString { get => _SubnetMaskString; set => this.RaiseAndSetIfChanged(ref _SubnetMaskString, value); }
        public string DefaultGatewayString { get => _DefaultGatewayString; set => this.RaiseAndSetIfChanged(ref _DefaultGatewayString, value); }




    }
}
