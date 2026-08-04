using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using static Weight.Lib;

namespace Weight
{
    public class StatusData : ObservableObject
    {
        private int _ScaleMode;
        private int _ScaleWeight;
        private int _ScaleFlowRate;
        private int _ScaleGatePosition;
        private int _NewGatePosition;
        private int _DWIStatus;
        private int _CounterLive;
        private int _TimeOur;
        private int _TimeMin;
        private int _TimeSec;
        private int _TimeMSec;
        private ushort _AcceptedCode;
        private ushort _RejectCode;

        private int _CommunicationResult;
        private uint _EventIdMask;
        private int _MastersCount;
        private int _ObservCount;

        private ushort _ClientInfo;
        private string _TimeString = "";

        // -------------------------------  Статусы --------------------------
        private ushort _StatusWord1;
        private ushort _StatusWord2;

        private bool _StatusSmartLoadStop;
        private bool _StatusOrderReplaceOK;
        private bool _StatusWaitingUGmidlevel;
        private bool _StatusCleanout;
        private bool _StatusMotion;
        private bool _StatusOrderInMemory;
        private bool _StatusSlowDelivery;
        private bool _StatusKeysEnabled;
        private bool _StatusFillDischargeCycle;

        private bool _StatusWaitingForPurge;
        private bool _StatusAuthorizingDISABLED;
        private bool _StatusWaitingForAcceptfromAWMS;
        private bool _StatusSmartLoadActive;
        private bool _StatusOrderInProgress;
        private bool _StatusFinishInProgress;

        private int _StatusMode;
        private int _StatusCurrentKeySet;

        // -------------------------------  Аварии --------------------------
        private ushort _ScaleAlarmWord1_last;
        private ushort _ScaleAlarmWord2_last;

        private ushort _ScaleAlarmWord1;
        private ushort _ScaleAlarmWord2;

        private bool _AlarmReportsPrinterErr;
        private bool _AlarmGateNotInPosition;
        private bool _AlarmSmartTechAlarm;
        private bool _AlarmDWIOffline;
        private bool _AlarmGateNotCalibrated;
        private bool _AlarmRemoteLinkShutdown;
        private bool _AlarmCommunicationError;
        private bool _AlarmRemoteLinkOffline;
        private bool _AlarmDataErrBadConfigCRC;
        private bool _AlarmAuditPrinterErr;
        private bool _AlarmUnderzero;
        private bool _AlarmOvercapacity;
        private bool _AlarmInterlockAlarm;
        private bool _AlarmLGFullAlarm;
        private bool _AlarmUGFullAlarm;
        private bool _AlarmWHFullAlarm;
        private bool _AlarmLGNotEmpty;
        private bool _AlarmDWIJumperErr;
        private bool _AlarmDWIDataErr;
        private bool _AlarmRemotePrinterErr;

        /// <summary>
        /// Текстовый список аварий
        /// </summary>
        private string _AlarmList = "";

        // -------------------------------  Smart Аварии --------------------------   

        private ushort _SmartAlarmWord1;
        private ushort _SmartAlarmWord2;

        /// <summary>
        /// Текстовый список аварий Smart
        /// </summary>
        private string _SmartAlarmList = "";


        // -------------------------------  ЦВЕТА для привязки --------------------------   

        private MyColor _ColorUGFullSensor = 0;
        private MyColor _ColorWHFullSensor = 0;
        private MyColor _ColorLGFullSensor = 0;

        private MyColor _ColorWGate = 0;
        private MyColor _ColorUGate = 0;
        private MyColor _ColorLGate = 0;

        public MyColor ColorUGFullSensor { get => _ColorUGFullSensor; set => this.SetProperty(ref _ColorUGFullSensor, value); }
        public MyColor ColorWHFullSensor { get => _ColorWHFullSensor; set => this.SetProperty(ref _ColorWHFullSensor, value); }
        public MyColor ColorLGFullSensor { get => _ColorLGFullSensor; set => this.SetProperty(ref _ColorLGFullSensor, value); }

        public MyColor ColorUGate { get => _ColorUGate; set => this.SetProperty(ref _ColorUGate, value); }
        public MyColor ColorWGate { get => _ColorWGate; set => this.SetProperty(ref _ColorWGate, value); }
        public MyColor ColorLGate { get => _ColorLGate; set => this.SetProperty(ref _ColorLGate, value); }

        // -------------------------------  ВХОДА/ВЫХОДА --------------------------

        private ushort _InputWord1;
        private ushort _InputWord2;

        private bool _InputLGClosedLS;
        private bool _InputFinishOrder;
        private bool _InputUGMidLevelSensor;
        private bool _InputRemoteStop;
        private bool _InputLGFullSensor;
        private bool _InputUGTrimLS;
        private bool _InputWHFullSensor;
        private bool _InputUGFullSensor;
        private bool _InputUGClosedLS;
        private bool _InputWHClosedLS;

        private bool _InputFinishInhibit;
        private bool _InputHydraulicUnitStatus;
        private bool _InputLGLowLevelSensor;
        private bool _InputControlPowerON;
        private bool _InputFieldPowerON;
        private bool _InputUGClosePowerON;
        private bool _InputUGOpenPowerON;
        private bool _InputWHClosePowerON;
        private bool _InputWHOpenPowerON;

        private ushort _OutputWord1;
        private ushort _OutputWord2;

        private bool _OutputOrderCompleted;
        private bool _OutputLGGateClose;
        private bool _OutputLGGateOpen;
        private bool _OutputOrderInProgress;
        private bool _OutputExternalAlarmIndicator;
        private bool _OutputTestWtsDown;
        private bool _OutputTestWtsUp;
        private bool _OutputOrderPreCutoff;
        private bool _OutputUGGateClose;
        private bool _OutputUGGateOpen;
        private bool _OutputWHGateClose;
        private bool _OutputWHGateOpen;

        private bool _OutputFGISConnected;
        private bool _OutputRemoteControlMode;
        private bool _OutputBinComplete;
        private bool _OutputTestMode;
        private bool _OutputShippingMode;
        private bool _OutputOverCapacity;

       


        /// <summary>
        /// Обновляет TimeString - Время в текстовом формате
        /// </summary>        
        private void UpdateTimeString()
        {
            string strTime = "";

            string GetStrFromInt(int val, int maxVal)
            {
                string str = "";
                if (val >= 0 && val <= 9)
                {
                    str += "0" + val.ToString();
                }
                else if (val >= 10 && val <= maxVal)
                {
                    str += val.ToString();
                }
                else
                {
                    str += "##";
                }
                return str;
            }

            strTime += GetStrFromInt(TimeOur, 23) + ":";
            strTime += GetStrFromInt(TimeMin, 59) + ":";
            strTime += GetStrFromInt(TimeSec, 59);

            this.TimeString = strTime;
        }


        /// <summary>
        /// Обновляет SmartAlarmList - текстовый список аварий Smart
        /// </summary>        
        private void UpdateSmartAlarmList()
        {
            string str = "";
            string[] smartTechText = {"", "Сбой питания Оптобокса",
                                            "Сбой питания DC 5v Оптобокса",
                                            "Сбой питания управления",
                                            "Неисправность датчика переполнения весового бункера",
                                            "Ошибка #1. Концевой выключателя регулирования. Задвижка верхнего бункера",
                                            "Неисправность датчика переполнения верхнего бункера",
                                            "Неисправность датчика наполнения разгрузочного бункера",
                                            "Неисправность гидравлики",
                                            "Ошибка #1. Задвижка верхнего бункера",
                                            "Ошибка #2. Задвижка верхнего бункера",
                                            "Ошибка #1 концевого выключателя. Задвижка верхнего бункера",
                                            "Ошибка #3. Задвижка верхнего бункера",
                                            "Ошибка Оптобокса. Закртытие задвижки верхнего бункера",
                                            "Ошибка #1. Задвижка весового бункера",
                                            "Ошибка #2. Задвижка весового бункера",
                                            "Ошибка #3. Задвижка весового бункера",
                                            "Ошибка #1 концевого выключателя. Задвижка весового бункера",
                                            "Ошибка Оптобокса. Закртытие задвижки весового бункера",
                                            "Ошибка #2 концевого выключателя. Задвижка верхнего бункера",
                                            "Ошибка #4. Задвижка верхнего бункера",
                                            "Ошибка Оптобокса. Открытие задвижки верхнего бункера",
                                            "Ошибка #5. Задвижка верхнего бункера",
                                            "Ошибка #2. Концевой выключателя регулирования. Задвижка верхнего бункера",
                                            "Ошибка #6. Задвижка верхнего бункера",
                                            "Ошибка #4. Задвижка весового бункера",
                                            "Ошибка #2 концевого выключателя. Задвижка весового бункера",
                                            "Ошибка #5. Задвижка весового бункера",
                                            "Ошибка #6. Задвижка весового бункера",
                                            "Ошибка Оптобокса. Открытие задвижки весового бункера",
                                            "Неисправность датчика среднего уровня верхнего бункера",
                                            "Ошибка #7. Задвижка верхнего бункера",
                                            "Ошибка #3 концевого выключателя. Задвижка верхнего бункера",
                                            "Ошибка #3. Концевой выключателя регулирования. Задвижка верхнего бункера",
                                            "Ошибка #4. Концевой выключателя регулирования. Задвижка верхнего бункера",
                                            "Мало свободного места на диске CD-4000",
                                            "Весовой индикатор. Настроен не на граммы",
                                            "Весовой индикатор. Ошибка в данных",
                                            "Ошибка #7. Задвижка весового бункера"};

            Debug.WriteLine("SmartAlarmWord1 = " + SmartAlarmWord1);

            str += ((byte)SmartAlarmWord1 != 0) ? smartTechText[(byte)SmartAlarmWord1] : "";
            str += (SmartAlarmWord1 >> 8 != 0) ? smartTechText[SmartAlarmWord1 >> 8] + "\n\r" : "";
            str += ((byte)SmartAlarmWord2 != 0) ? smartTechText[(byte)SmartAlarmWord2] + "\n\r" : "";
            str += (SmartAlarmWord2 >> 8 != 0) ? smartTechText[SmartAlarmWord2 >> 8] + "\n\r" : "";

            this.SmartAlarmList = str;
        }

        /// <summary>
        /// Обновляет биты аварий для первого слова
        /// </summary>  
        private void UpdateAlarms1()
        {
            Debug.WriteLine("ScaleAlarmWord1 = " + ScaleAlarmWord1);

            AlarmReportsPrinterErr = Lib.GetBitWord(ScaleAlarmWord1, 15);
            AlarmGateNotInPosition = Lib.GetBitWord(ScaleAlarmWord1, 14);
            AlarmSmartTechAlarm = Lib.GetBitWord(ScaleAlarmWord1, 13);
            AlarmDWIOffline = Lib.GetBitWord(ScaleAlarmWord1, 12);
            AlarmGateNotCalibrated = Lib.GetBitWord(ScaleAlarmWord1, 11);
            AlarmRemoteLinkShutdown = Lib.GetBitWord(ScaleAlarmWord1, 10);
            AlarmCommunicationError = Lib.GetBitWord(ScaleAlarmWord1, 9);
            AlarmRemoteLinkOffline = Lib.GetBitWord(ScaleAlarmWord1, 8);
            AlarmDataErrBadConfigCRC = Lib.GetBitWord(ScaleAlarmWord1, 7);
            AlarmAuditPrinterErr = Lib.GetBitWord(ScaleAlarmWord1, 6);
            AlarmUnderzero = Lib.GetBitWord(ScaleAlarmWord1, 5);
            AlarmOvercapacity = Lib.GetBitWord(ScaleAlarmWord1, 4);
            AlarmInterlockAlarm = Lib.GetBitWord(ScaleAlarmWord1, 3);
            AlarmLGFullAlarm = Lib.GetBitWord(ScaleAlarmWord1, 2);
            AlarmUGFullAlarm = Lib.GetBitWord(ScaleAlarmWord1, 1);
            AlarmWHFullAlarm = Lib.GetBitWord(ScaleAlarmWord1, 0);

            AlarmLGNotEmpty = Lib.GetBitWord(ScaleAlarmWord2, 3);
            AlarmDWIJumperErr = Lib.GetBitWord(ScaleAlarmWord2, 2);
            AlarmDWIDataErr = Lib.GetBitWord(ScaleAlarmWord2, 1);
            AlarmRemotePrinterErr = Lib.GetBitWord(ScaleAlarmWord2, 0);

            UpdateAlarmList();
        }

        /// <summary>
        /// Обновляет биты аварий для второго слова
        /// </summary>  
        private void UpdateAlarms2()
        {
            AlarmLGNotEmpty = Lib.GetBitWord(ScaleAlarmWord2, 3);
            AlarmDWIJumperErr = Lib.GetBitWord(ScaleAlarmWord2, 2);
            AlarmDWIDataErr = Lib.GetBitWord(ScaleAlarmWord2, 1);
            AlarmRemotePrinterErr = Lib.GetBitWord(ScaleAlarmWord2, 0);

            UpdateAlarmList();
        }

        /// <summary>
        /// Обновляет текстовый список аварий - AlarmList
        /// </summary>  
        private void UpdateAlarmList()
        {
            int num = 1;
            string strAlarm = "";
            if (AlarmSmartTechAlarm) strAlarm += $"{num++}. Ошибка Smart\r\n";
            if (AlarmReportsPrinterErr) strAlarm += $"{num++}. Ошибка принтера отчётов\r\n";
            if (AlarmGateNotInPosition) strAlarm += $"{num++}. Задвижка не в позиции\r\n";
            if (AlarmDWIOffline) strAlarm += $"{num++}. Нет связи с цифровым индикатором\r\n";
            if (AlarmGateNotCalibrated) strAlarm += $"{num++}. Задвижка не откалибрована\r\n";
            if (AlarmRemoteLinkShutdown) strAlarm += $"{num++}. \r\n";
            if (AlarmCommunicationError) strAlarm += $"{num++}. Ошибка связи\r\n";
            if (AlarmRemoteLinkOffline) strAlarm += $"{num++}. \r\n";
            if (AlarmDataErrBadConfigCRC) strAlarm += $"{num++}. Ошибка данных CRC\r\n";
            if (AlarmAuditPrinterErr) strAlarm += $"{num++}. Ошибка основного принтера\r\n";
            if (AlarmUnderzero) strAlarm += $"{num++}. Весовые датчики отключены или низкий сигнал\r\n";
            if (AlarmOvercapacity) strAlarm += $"{num++}. Весовые датчики - Запредельное значение\r\n";
            if (AlarmInterlockAlarm) strAlarm += $"{num++}. Внутренная блокировка\r\n";
            if (AlarmLGFullAlarm) strAlarm += $"{num++}. Нет места в нижнем бункере\r\n";
            if (AlarmUGFullAlarm) strAlarm += $"{num++}. Нет места в верхнем бункере\r\n";
            if (AlarmWHFullAlarm) strAlarm += $"{num++}. Весовой бункер переполнен\r\n";
            if (AlarmLGNotEmpty) strAlarm += $"{num++}. Нижний бунке не пустой\r\n";
            if (AlarmDWIJumperErr) strAlarm += $"{num++}. Отсутствует перемычка в цифровом индикаторе\r\n";
            if (AlarmDWIDataErr) strAlarm += $"{num++}. Ошибка цифрового индикатора\r\n";
            if (AlarmRemotePrinterErr) strAlarm += $"{num++}. Ошибка удаленного принтера\r\n";


            Debug.WriteLine(" AlarmSmartTechAlarm = " + AlarmSmartTechAlarm);
            Debug.WriteLine(" AlarmReportsPrinterErr = " + AlarmReportsPrinterErr);
            Debug.WriteLine(" AlarmGateNotInPosition = " + AlarmGateNotInPosition);
            Debug.WriteLine(" AlarmDWIOffline = " + AlarmDWIOffline);
            Debug.WriteLine(" AlarmGateNotCalibrated = " + AlarmGateNotCalibrated);
            Debug.WriteLine(" AlarmRemoteLinkShutdown = " + AlarmRemoteLinkShutdown);
            Debug.WriteLine(" AlarmCommunicationError = " + AlarmCommunicationError);
            Debug.WriteLine(" AlarmRemoteLinkOffline = " + AlarmRemoteLinkOffline);
            Debug.WriteLine(" AlarmDataErrBadConfigCRC = " + AlarmDataErrBadConfigCRC);
            Debug.WriteLine(" AlarmAuditPrinterErr = " + AlarmAuditPrinterErr);
            Debug.WriteLine(" AlarmUnderzero = " + AlarmUnderzero);
            Debug.WriteLine(" AlarmOvercapacity = " + AlarmOvercapacity);
            Debug.WriteLine(" AlarmInterlockAlarm = " + AlarmInterlockAlarm);
            Debug.WriteLine(" AlarmLGFullAlarm = " + AlarmLGFullAlarm);
            Debug.WriteLine(" AlarmUGFullAlarm = " + AlarmUGFullAlarm);
            Debug.WriteLine(" AlarmWHFullAlarm = " + AlarmWHFullAlarm);
            Debug.WriteLine(" AlarmLGNotEmpty = " + AlarmLGNotEmpty);
            Debug.WriteLine(" AlarmDWIJumperErr = " + AlarmDWIJumperErr);
            Debug.WriteLine(" AlarmDWIDataErr = " + AlarmDWIDataErr);
            Debug.WriteLine(" AlarmRemotePrinterErr = " + AlarmRemotePrinterErr);
            Debug.WriteLine("");

            this.AlarmList = strAlarm;

            Debug.WriteLine("AlarmList=" + AlarmList);
        }

        /// <summary>
        /// Обновить значения битов Статусов Весов слова 1, StatusMode, StatusCurrentKeySet
        /// </summary>
        private void UpdateStatus1()
        {
            StatusSmartLoadStop = Lib.GetBitWord(StatusWord1, 15);
            StatusOrderReplaceOK = Lib.GetBitWord(StatusWord1, 14);
            StatusWaitingUGmidlevel = Lib.GetBitWord(StatusWord1, 13);
            StatusCleanout = Lib.GetBitWord(StatusWord1, 12);
            StatusMotion = Lib.GetBitWord(StatusWord1, 11);
            StatusOrderInMemory = Lib.GetBitWord(StatusWord1, 10);
            StatusSlowDelivery = Lib.GetBitWord(StatusWord1, 9);
            StatusKeysEnabled = Lib.GetBitWord(StatusWord1, 8);
            StatusFillDischargeCycle = Lib.GetBitWord(StatusWord1, 4);

            StatusCurrentKeySet = StatusWord1 & 0x7;
            StatusMode = (StatusWord1 >> 5) & 0x3;
        }

        /// <summary>
        /// Обновить значения битов Статусов Весов слова 2
        /// </summary>
        private void UpdateStatus2()
        {
            StatusWaitingForPurge = Lib.GetBitWord(StatusWord2, 5);
            StatusAuthorizingDISABLED = Lib.GetBitWord(StatusWord2, 4);
            StatusWaitingForAcceptfromAWMS = Lib.GetBitWord(StatusWord2, 3);
            StatusSmartLoadActive = Lib.GetBitWord(StatusWord2, 2);
            StatusOrderInProgress = Lib.GetBitWord(StatusWord2, 1);
            StatusFinishInProgress = Lib.GetBitWord(StatusWord2, 0);
        }

        /// <summary>
        /// Обновить значения битов входов слова 1
        /// </summary>
        private void UpdateInput1()
        {
            InputLGClosedLS = Lib.GetBitWord(InputWord1, 11);
            InputFinishOrder = Lib.GetBitWord(InputWord1, 8);
            InputUGMidLevelSensor = Lib.GetBitWord(InputWord1, 7);
            InputRemoteStop = Lib.GetBitWord(InputWord1, 6);
            InputLGFullSensor = Lib.GetBitWord(InputWord1, 5);
            InputUGTrimLS = Lib.GetBitWord(InputWord1, 4);
            InputUGFullSensor = Lib.GetBitWord(InputWord1, 3);
            InputUGClosedLS = Lib.GetBitWord(InputWord1, 2);
            InputWHFullSensor = Lib.GetBitWord(InputWord1, 1);
            InputWHClosedLS = Lib.GetBitWord(InputWord1, 0);
        }

        /// <summary>
        /// Обновить значения битов входов слова 2
        /// </summary>
        private void UpdateInput2()
        {
            InputFinishInhibit = Lib.GetBitWord(InputWord2, 8);
            InputHydraulicUnitStatus = Lib.GetBitWord(InputWord2, 7);
            InputLGLowLevelSensor = Lib.GetBitWord(InputWord2, 6);
            InputControlPowerON = Lib.GetBitWord(InputWord2, 5);
            InputFieldPowerON = Lib.GetBitWord(InputWord2, 4);
            InputUGClosePowerON = Lib.GetBitWord(InputWord2, 3);
            InputUGOpenPowerON = Lib.GetBitWord(InputWord2, 2);
            InputWHClosePowerON = Lib.GetBitWord(InputWord2, 1);
            InputWHOpenPowerON = Lib.GetBitWord(InputWord2, 0);
        }

        /// <summary>
        /// Обновить значения битов выходов слова 1
        /// </summary>
        private void UpdateOutput1()
        {
            OutputOrderCompleted = Lib.GetBitWord(OutputWord1, 15);
            OutputLGGateClose = Lib.GetBitWord(OutputWord1, 14);
            OutputLGGateOpen = Lib.GetBitWord(OutputWord1, 13);
            OutputOrderInProgress = Lib.GetBitWord(OutputWord1, 12);
            OutputExternalAlarmIndicator = Lib.GetBitWord(OutputWord1, 7);
            OutputTestWtsDown = Lib.GetBitWord(OutputWord1, 6);
            OutputTestWtsUp = Lib.GetBitWord(OutputWord1, 5);
            OutputOrderPreCutoff = Lib.GetBitWord(OutputWord1, 4);
            OutputUGGateClose = Lib.GetBitWord(OutputWord1, 3);
            OutputUGGateOpen = Lib.GetBitWord(OutputWord1, 2);
            OutputWHGateClose = Lib.GetBitWord(OutputWord1, 1);
            OutputWHGateOpen = Lib.GetBitWord(OutputWord1, 0);
        }

        /// <summary>
        /// Обновить значения битов выходов слова 2
        /// </summary>
        private void UpdateOutput2()
        {
            OutputFGISConnected = Lib.GetBitWord(OutputWord2, 5);
            OutputRemoteControlMode = Lib.GetBitWord(OutputWord2, 4);
            OutputBinComplete = Lib.GetBitWord(OutputWord2, 3);
            OutputTestMode = Lib.GetBitWord(OutputWord2, 2);
            OutputShippingMode = Lib.GetBitWord(OutputWord2, 1);
            OutputOverCapacity = Lib.GetBitWord(OutputWord2, 0);
        }


        
        


        public int ScaleMode { get => _ScaleMode; set => this.SetProperty(ref _ScaleMode, value); }
        public int ScaleWeight { get => _ScaleWeight; set => this.SetProperty(ref _ScaleWeight, value); }
        public int ScaleFlowRate { get => _ScaleFlowRate; set => this.SetProperty(ref _ScaleFlowRate, value); }
        public int ScaleGatePosition { get => _ScaleGatePosition; set => this.SetProperty(ref _ScaleGatePosition, value); }
        public int NewGatePosition { get => _NewGatePosition; set => this.SetProperty(ref _NewGatePosition, value); }
        public int DWIStatus { get => _DWIStatus; set => this.SetProperty(ref _DWIStatus, value); }
        public int CounterLive { get => _CounterLive; set => this.SetProperty(ref _CounterLive, value); }
        public int TimeOur { get => _TimeOur; set => this.SetProperty(ref _TimeOur, value); }
        public int TimeMin { get => _TimeMin; set => this.SetProperty(ref _TimeMin, value); }
        public int TimeSec
        {
            get => _TimeSec;
            set
            {
                if (_TimeSec != value) UpdateTimeString();
                this.SetProperty(ref _TimeSec, value);
            }
        }
        public int TimeMSec { get => _TimeMSec; set => this.SetProperty(ref _TimeMSec, value); }
        public ushort AcceptedCode { get => _AcceptedCode; set => this.SetProperty(ref _AcceptedCode, value); }
        public ushort RejectCode { get => _RejectCode; set => this.SetProperty(ref _RejectCode, value); }
        public int CommunicationResult { get => _CommunicationResult; set => this.SetProperty(ref _CommunicationResult, value); }
        public uint EventIdMask { get => _EventIdMask; set => this.SetProperty(ref _EventIdMask, value); }
        public int MastersCount { get => _MastersCount; set => this.SetProperty(ref _MastersCount, value); }
        public int ObservCount { get => _ObservCount; set => this.SetProperty(ref _ObservCount, value); }
        public ushort ClientInfo { get => _ClientInfo; set => this.SetProperty(ref _ClientInfo, value); }
        public string TimeString { get => _TimeString; set => this.SetProperty(ref _TimeString, value); }

        public ushort StatusWord1
        {
            get => _StatusWord1;
            set
            {
                if (_StatusWord1 != value) UpdateStatus1();
                this.SetProperty(ref _StatusWord1, value);
            }
        }
        public ushort StatusWord2
        {
            get => _StatusWord2;
            set
            {
                if (_StatusWord2 != value) UpdateStatus2();
                this.SetProperty(ref _StatusWord2, value);
            }
        }

        public bool StatusSmartLoadStop { get => _StatusSmartLoadStop; set => this.SetProperty(ref _StatusSmartLoadStop, value); }
        public bool StatusOrderReplaceOK { get => _StatusOrderReplaceOK; set => this.SetProperty(ref _StatusOrderReplaceOK, value); }
        public bool StatusWaitingUGmidlevel { get => _StatusWaitingUGmidlevel; set => this.SetProperty(ref _StatusWaitingUGmidlevel, value); }
        public bool StatusCleanout { get => _StatusCleanout; set => this.SetProperty(ref _StatusCleanout, value); }
        public bool StatusMotion { get => _StatusMotion; set => this.SetProperty(ref _StatusMotion, value); }
        public bool StatusOrderInMemory { get => _StatusOrderInMemory; set => this.SetProperty(ref _StatusOrderInMemory, value); }
        public bool StatusSlowDelivery { get => _StatusSlowDelivery; set => this.SetProperty(ref _StatusSlowDelivery, value); }
        public bool StatusKeysEnabled { get => _StatusKeysEnabled; set => this.SetProperty(ref _StatusKeysEnabled, value); }
        public bool StatusFillDischargeCycle { get => _StatusFillDischargeCycle; set => this.SetProperty(ref _StatusFillDischargeCycle, value); }
        public bool StatusWaitingForPurge { get => _StatusWaitingForPurge; set => this.SetProperty(ref _StatusWaitingForPurge, value); }
        public bool StatusAuthorizingDISABLED { get => _StatusAuthorizingDISABLED; set => this.SetProperty(ref _StatusAuthorizingDISABLED, value); }
        public bool StatusWaitingForAcceptfromAWMS { get => _StatusWaitingForAcceptfromAWMS; set => this.SetProperty(ref _StatusWaitingForAcceptfromAWMS, value); }
        public bool StatusSmartLoadActive { get => _StatusSmartLoadActive; set => this.SetProperty(ref _StatusSmartLoadActive, value); }
        public bool StatusOrderInProgress { get => _StatusOrderInProgress; set => this.SetProperty(ref _StatusOrderInProgress, value); }
        public bool StatusFinishInProgress { get => _StatusFinishInProgress; set => this.SetProperty(ref _StatusFinishInProgress, value); }
        public int StatusMode { get => _StatusMode; set => this.SetProperty(ref _StatusMode, value); }
        public int StatusCurrentKeySet { get => _StatusCurrentKeySet; set => this.SetProperty(ref _StatusCurrentKeySet, value); }
        public bool AlarmReportsPrinterErr { get => _AlarmReportsPrinterErr; set => this.SetProperty(ref _AlarmReportsPrinterErr, value); }
        public bool AlarmGateNotInPosition { get => _AlarmGateNotInPosition; set => this.SetProperty(ref _AlarmGateNotInPosition, value); }
        public bool AlarmSmartTechAlarm { get => _AlarmSmartTechAlarm; set => this.SetProperty(ref _AlarmSmartTechAlarm, value); }
        public bool AlarmDWIOffline { get => _AlarmDWIOffline; set => this.SetProperty(ref _AlarmDWIOffline, value); }
        public bool AlarmGateNotCalibrated { get => _AlarmGateNotCalibrated; set => this.SetProperty(ref _AlarmGateNotCalibrated, value); }
        public bool AlarmRemoteLinkShutdown { get => _AlarmRemoteLinkShutdown; set => this.SetProperty(ref _AlarmRemoteLinkShutdown, value); }
        public bool AlarmCommunicationError { get => _AlarmCommunicationError; set => this.SetProperty(ref _AlarmCommunicationError, value); }
        public bool AlarmRemoteLinkOffline { get => _AlarmRemoteLinkOffline; set => this.SetProperty(ref _AlarmRemoteLinkOffline, value); }
        public bool AlarmDataErrBadConfigCRC { get => _AlarmDataErrBadConfigCRC; set => this.SetProperty(ref _AlarmDataErrBadConfigCRC, value); }
        public bool AlarmAuditPrinterErr { get => _AlarmAuditPrinterErr; set => this.SetProperty(ref _AlarmAuditPrinterErr, value); }
        public bool AlarmUnderzero { get => _AlarmUnderzero; set => this.SetProperty(ref _AlarmUnderzero, value); }
        public bool AlarmOvercapacity { get => _AlarmOvercapacity; set => this.SetProperty(ref _AlarmOvercapacity, value); }
        public bool AlarmInterlockAlarm { get => _AlarmInterlockAlarm; set => this.SetProperty(ref _AlarmInterlockAlarm, value); }
        public bool AlarmLGFullAlarm { get => _AlarmLGFullAlarm; set => this.SetProperty(ref _AlarmLGFullAlarm, value); }
        public bool AlarmUGFullAlarm { get => _AlarmUGFullAlarm; set => this.SetProperty(ref _AlarmUGFullAlarm, value); }
        public bool AlarmWHFullAlarm { get => _AlarmWHFullAlarm; set => this.SetProperty(ref _AlarmWHFullAlarm, value); }
        public bool AlarmLGNotEmpty { get => _AlarmLGNotEmpty; set => this.SetProperty(ref _AlarmLGNotEmpty, value); }
        public bool AlarmDWIJumperErr { get => _AlarmDWIJumperErr; set => this.SetProperty(ref _AlarmDWIJumperErr, value); }
        public bool AlarmDWIDataErr { get => _AlarmDWIDataErr; set => this.SetProperty(ref _AlarmDWIDataErr, value); }
        public bool AlarmRemotePrinterErr { get => _AlarmRemotePrinterErr; set => this.SetProperty(ref _AlarmRemotePrinterErr, value); }
        public string AlarmList { get => _AlarmList; set => this.SetProperty(ref _AlarmList, value); }
        public ushort SmartAlarmWord1
        {
            get => _SmartAlarmWord1;
            set
            {
                this.SetProperty(ref _SmartAlarmWord1, value);

                if (_ScaleAlarmWord1_last != value)
                {
                    UpdateAlarmList();
                    _ScaleAlarmWord1_last = value;
                }
            }
        }
        public ushort SmartAlarmWord2
        {
            get => _SmartAlarmWord2;
            set
            {
                this.SetProperty(ref _SmartAlarmWord2, value);

                if (_ScaleAlarmWord2_last != value)
                {
                    UpdateAlarmList();
                    _ScaleAlarmWord2_last = value;
                }
            }
        }


        public ushort ScaleAlarmWord1
        {
            get => _ScaleAlarmWord1;
            set
            {
                if (_ScaleAlarmWord1 != value)
                {
                    this.SetProperty(ref _ScaleAlarmWord1, value);
                    UpdateAlarms1();
                }
            }
        }
        public ushort ScaleAlarmWord2
        {
            get => _ScaleAlarmWord2;
            set
            {
                if (_ScaleAlarmWord2 != value)
                {
                    this.SetProperty(ref _ScaleAlarmWord2, value);
                    UpdateAlarms2();
                }
            }
        }


        public string SmartAlarmList { get => _SmartAlarmList; set => this.SetProperty(ref _SmartAlarmList, value); }

        public ushort InputWord1
        {
            get => _InputWord1;
            set
            {
                if (_InputWord1 != value) UpdateInput1();
                this.SetProperty(ref _InputWord1, value);
            }
        }
        public ushort InputWord2
        {
            get => _InputWord2;
            set
            {
                if (_InputWord2 != value) UpdateInput2();
                this.SetProperty(ref _InputWord2, value);
            }
        }
        public ushort OutputWord1
        {
            get => _OutputWord1;
            set
            {
                if (_OutputWord1 != value) UpdateOutput1();
                this.SetProperty(ref _OutputWord1, value);
            }
        }
        public ushort OutputWord2
        {
            get => _OutputWord2;
            set
            {
                if (_OutputWord2 != value) UpdateOutput2();
                this.SetProperty(ref _OutputWord2, value);
            }
        }
        public bool InputLGClosedLS { get => _InputLGClosedLS; set => this.SetProperty(ref _InputLGClosedLS, value); }
        public bool InputFinishOrder { get => _InputFinishOrder; set => this.SetProperty(ref _InputFinishOrder, value); }
        public bool InputUGMidLevelSensor { get => _InputUGMidLevelSensor; set => this.SetProperty(ref _InputUGMidLevelSensor, value); }
        public bool InputRemoteStop { get => _InputRemoteStop; set => this.SetProperty(ref _InputRemoteStop, value); }
        public bool InputLGFullSensor { get => _InputLGFullSensor; set => this.SetProperty(ref _InputLGFullSensor, value); }
        public bool InputUGTrimLS { get => _InputUGTrimLS; set => this.SetProperty(ref _InputUGTrimLS, value); }
        public bool InputWHFullSensor { get => _InputWHFullSensor; set => this.SetProperty(ref _InputWHFullSensor, value); }
        public bool InputUGFullSensor
        {
            get => _InputUGFullSensor;
            set
            {
                if (_InputUGFullSensor != value)
                {
                    ColorUGFullSensor = value ? Lib.MyColor.Green : Lib.MyColor.Red;

                }
                this.SetProperty(ref _InputUGFullSensor, value);
            }
        }
        public bool InputUGClosedLS { get => _InputUGClosedLS; set => this.SetProperty(ref _InputUGClosedLS, value); }
        public bool InputWHClosedLS { get => _InputWHClosedLS; set => this.SetProperty(ref _InputWHClosedLS, value); }
        public bool InputFinishInhibit { get => _InputFinishInhibit; set => this.SetProperty(ref _InputFinishInhibit, value); }
        public bool InputHydraulicUnitStatus { get => _InputHydraulicUnitStatus; set => this.SetProperty(ref _InputHydraulicUnitStatus, value); }
        public bool InputLGLowLevelSensor { get => _InputLGLowLevelSensor; set => this.SetProperty(ref _InputLGLowLevelSensor, value); }
        public bool InputControlPowerON { get => _InputControlPowerON; set => this.SetProperty(ref _InputControlPowerON, value); }
        public bool InputFieldPowerON { get => _InputFieldPowerON; set => this.SetProperty(ref _InputFieldPowerON, value); }
        public bool InputUGClosePowerON { get => _InputUGClosePowerON; set => this.SetProperty(ref _InputUGClosePowerON, value); }
        public bool InputUGOpenPowerON { get => _InputUGOpenPowerON; set => this.SetProperty(ref _InputUGOpenPowerON, value); }
        public bool InputWHClosePowerON { get => _InputWHClosePowerON; set => this.SetProperty(ref _InputWHClosePowerON, value); }
        public bool InputWHOpenPowerON { get => _InputWHOpenPowerON; set => this.SetProperty(ref _InputWHOpenPowerON, value); }
        public bool OutputOrderCompleted { get => _OutputOrderCompleted; set => this.SetProperty(ref _OutputOrderCompleted, value); }
        public bool OutputLGGateClose { get => _OutputLGGateClose; set => this.SetProperty(ref _OutputLGGateClose, value); }
        public bool OutputLGGateOpen { get => _OutputLGGateOpen; set => this.SetProperty(ref _OutputLGGateOpen, value); }
        public bool OutputOrderInProgress { get => _OutputOrderInProgress; set => this.SetProperty(ref _OutputOrderInProgress, value); }
        public bool OutputExternalAlarmIndicator { get => _OutputExternalAlarmIndicator; set => this.SetProperty(ref _OutputExternalAlarmIndicator, value); }
        public bool OutputTestWtsDown { get => _OutputTestWtsDown; set => this.SetProperty(ref _OutputTestWtsDown, value); }
        public bool OutputTestWtsUp { get => _OutputTestWtsUp; set => this.SetProperty(ref _OutputTestWtsUp, value); }
        public bool OutputOrderPreCutoff { get => _OutputOrderPreCutoff; set => this.SetProperty(ref _OutputOrderPreCutoff, value); }
        public bool OutputUGGateClose { get => _OutputUGGateClose; set => this.SetProperty(ref _OutputUGGateClose, value); }
        public bool OutputUGGateOpen { get => _OutputUGGateOpen; set => this.SetProperty(ref _OutputUGGateOpen, value); }
        public bool OutputWHGateClose { get => _OutputWHGateClose; set => this.SetProperty(ref _OutputWHGateClose, value); }
        public bool OutputWHGateOpen { get => _OutputWHGateOpen; set => this.SetProperty(ref _OutputWHGateOpen, value); }
        public bool OutputFGISConnected { get => _OutputFGISConnected; set => this.SetProperty(ref _OutputFGISConnected, value); }
        public bool OutputRemoteControlMode { get => _OutputRemoteControlMode; set => this.SetProperty(ref _OutputRemoteControlMode, value); }
        public bool OutputBinComplete { get => _OutputBinComplete; set => this.SetProperty(ref _OutputBinComplete, value); }
        public bool OutputTestMode { get => _OutputTestMode; set => this.SetProperty(ref _OutputTestMode, value); }
        public bool OutputShippingMode { get => _OutputShippingMode; set => this.SetProperty(ref _OutputShippingMode, value); }
        public bool OutputOverCapacity { get => _OutputOverCapacity; set => this.SetProperty(ref _OutputOverCapacity, value); }


    }
}
