using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using ClientCW.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NLog;
using Weight;
using Weight.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClientCW.ViewModels
{
    public class WeightViewModel : ObservableObject
    {

        private readonly ModbusWeightService _mbService;
       
        public WeightViewModel(ModbusWeightService mbService)
        {
            _mbService = mbService;
            orderData = new OrderData();
            staticOrderData = new StaticOrderData();
            statusData = new StatusData();
            miscStatusData = new MiscStatusData();
            configData = new ConfigData();            
            ClickCommand = new RelayCommand(OnButtonClicked);
            
            StartLoop();
        }
        

        public RelayCommand ClickCommand { get; }
        private void OnButtonClicked()
        {
            Debug.WriteLine("Кнопка 1 - НАЖАТА");
            statusData.ScaleWeight += 1;
        }
                

        private OrderData _orderData;
        private StaticOrderData _staticOrderData;
        private StatusData _statusData;
        private MiscStatusData _miscStatusData;
        private ConfigData _configData;


        public OrderData orderData { get => _orderData; set => this.SetProperty(ref _orderData, value); }
        public StaticOrderData staticOrderData { get => _staticOrderData; set => this.SetProperty(ref _staticOrderData, value); }
        public StatusData statusData { get => _statusData; set => this.SetProperty(ref _statusData, value); }
        public MiscStatusData miscStatusData { get => _miscStatusData; set => this.SetProperty(ref _miscStatusData, value); }
        public ConfigData configData { get => _configData; set => this.SetProperty(ref _configData, value); }
        

        private CancellationTokenSource? _cts;
        private Task? _loopTask;
        
        private bool _isConnected;

        // Флаг, чтобы читать staticOrderData/configData только при изменении нужного поля в statusData
        private int? _lastKnownTriggerValue; // сюда положи значение поля, по которому решаем, когда читать

        public void StartLoop()
        {
            StopLoop();

            _cts = new CancellationTokenSource();
            var token = _cts.Token;
            _loopTask = RunLoopAsync(token);
        }

        public void StopLoop()
        {
            _cts?.Cancel();

            _ = _loopTask?.ContinueWith(t =>
            {
                //_mbService?.Dispose();
                //_mbService = null;
                _isConnected = false;
                Debug.WriteLine("Цикл остановлен");
            }, TaskScheduler.Default);

            _cts?.Dispose();
            _cts = null;
            _loopTask = null;
        }

        private async Task RunLoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (await TryConnectWithRetryAsync(token))
                {
                    _isConnected = true;
                    await ProcessDataLoopAsync(token);
                    _isConnected = false;
                }
                else
                {
                    _isConnected = false;
                }

                if (!token.IsCancellationRequested)
                {
                    try
                    {
                        await Task.Delay(3000, token);
                    }
                    catch (OperationCanceledException) { /* нормально */ }
                }
            }
        }

        private async Task<bool> TryConnectWithRetryAsync(CancellationToken token)
        {
            //_mbService?.Dispose();
            //_mbService = new ModbusWeightService("10.6.173.231", 1);

            try
            {
                var connected = await _mbService.ConnectAsync();
                Debug.WriteLine($"Подключение установлено: {connected}");
                return connected;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ошибка подключения: " + ex);
                return false;
            }
        }

        private async Task ProcessDataLoopAsync(CancellationToken token)
        {
            // Сбрасываем триггер при новом соединении (чтобы гарантированно прочитать при старте)
            _lastKnownTriggerValue = null;

            var fastLoopTask = ReadFastLoopAsync(token);      // statusData + miscStatusData (100 мс)
            var orderLoopTask = ReadOrderLoopAsync(token);    // orderData (1 с)

            await Task.WhenAny(fastLoopTask, orderLoopTask);

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
            cts.Cancel();

            try { await Task.WhenAll(fastLoopTask, orderLoopTask); }
            catch (OperationCanceledException) { /* нормально */ }
            catch (Exception ex)
            {
                Debug.WriteLine("Один из циклов чтения завершился с ошибкой: {Msg}", ex.Message);
            }
        }

        private async Task ReadFastLoopAsync(CancellationToken token)
        {
            int cycleCount = 0;
            var lastReportTime = DateTimeOffset.UtcNow;

            // Инициализируем старое значение null, чтобы при первом проходе гарантированно прочитать всё
            uint? EventIdOld = null;

            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (_mbService == null) break;

                    // 1. Читаем данные
                    statusData = await _mbService.ReadStatusDataAsync();
                    miscStatusData = await _mbService.ReadMiscStatusDataAsync();
                    
                    uint EventIdCurrent = statusData.EventIdMask;

                    // 2. Вызываем отдельный метод для логики флагов, чтения и обновления UI
                    await EvaluateAndReadConditionalData(
                        token,
                        EventIdCurrent,
                        EventIdOld);

                    EventIdOld = EventIdCurrent;
                    
                    
                    cycleCount++;
                    // Отчёт по частоте цикла (раз в секунду)
                    var now = DateTimeOffset.UtcNow;
                    if ((now - lastReportTime).TotalSeconds >= 1.0)
                    {
                        //Debug.WriteLine($"Быстрых циклов за секунду: {cycleCount}");
                        cycleCount = 0;
                        lastReportTime = now;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Ошибка в быстром цикле: {Msg}", ex.Message);
                    break;
                }

                await Task.Delay(100, token);
            }
        }

        /// <summary>
        /// Инкапсулирует логику:
        /// 1. Сравнение битовых масок (XOR) для поиска изменений.
        /// 2. Определение необходимости чтения или обработки события.
        /// 3. Обновление данных
        /// </summary>
        private async Task EvaluateAndReadConditionalData(CancellationToken token, uint eventIdCurrent, uint? eventIdOld)
        {
            const uint FlagStaticOrderChanged = 0x8000;         // Static Order Data Changed
            const uint FlagConfigChanged = 0x20000;             // Configuration Data Changed
            const uint FlagMiscStatusDataChanged = 0x10000;     // Misc.Status Data Changed
            const uint FlagOrderStarted = 0x2;          // Order Started
            const uint FlagOrderFinished = 0x4;         // Order Finished
            const uint FlagDraftStarted = 0x8;          // Draft Started
            const uint FlagDraftFinished = 0x40;         // Draft Finished                       

            bool needReadStaticOrder = false;
            bool needReadConfig = false;
            bool needReadMiscStatusData = false;
            bool eventOrderStarted = false;
            bool eventOrderFinished = false;
            bool eventDraftStarted = false;
            bool eventDraftFinished = false;

            // Логика определения, что нужно прочитать
            if (eventIdOld.HasValue)
            {
                uint changedBits = eventIdCurrent ^ eventIdOld.Value; // XOR: 1 только там, где значение изменилось

                // Проверяем изменение флага StaticOrder (появился: 0-> 1)
                if ((changedBits & FlagStaticOrderChanged) != 0 && (eventIdCurrent & FlagStaticOrderChanged) != 0)                   
                    needReadStaticOrder = true;

                // Проверяем изменение флага Config (появился: 0-> 1)
                if ((changedBits & FlagConfigChanged) != 0 && (eventIdCurrent & FlagConfigChanged) != 0)
                    needReadConfig = true;

                // Проверяем изменение флага Misc.Status Data (появился: 0-> 1)
                if ((changedBits & FlagMiscStatusDataChanged) != 0 && (eventIdCurrent & FlagMiscStatusDataChanged) != 0)
                    needReadMiscStatusData = true;

                // Проверяем Order Started
                if ((changedBits & FlagOrderStarted) != 0 && (eventIdCurrent & FlagOrderStarted) != 0)
                    eventOrderStarted = true;

                // Проверяем Order Finished
                if ((changedBits & FlagOrderFinished) != 0 && (eventIdCurrent & FlagOrderFinished) != 0)
                    eventOrderFinished = true;

                // Проверяем Draft Started
                if ((changedBits & FlagDraftStarted) != 0 && (eventIdCurrent & FlagDraftStarted) != 0)
                    eventDraftStarted = true;

                // Проверяем Draft Finished
                if ((changedBits & FlagDraftFinished) != 0 && (eventIdCurrent & FlagDraftFinished) != 0)
                    eventDraftFinished = true;
            }
            else
            {
                // Первый проход: считаем, что всё изменилось, чтобы прочитать актуальные данные,
                // если флаги уже установлены
                if ((eventIdCurrent & FlagStaticOrderChanged) != 0) needReadStaticOrder = true;
                if ((eventIdCurrent & FlagConfigChanged) != 0) needReadConfig = true;
                if ((eventIdCurrent & FlagMiscStatusDataChanged) != 0) needReadMiscStatusData = true;
                if ((eventIdCurrent & FlagOrderStarted) != 0) eventOrderStarted = true;
                if ((eventIdCurrent & FlagOrderFinished) != 0) eventOrderFinished = true;
                if ((eventIdCurrent & FlagDraftStarted) != 0) eventDraftStarted = true;
                if ((eventIdCurrent & FlagDraftFinished) != 0) eventDraftFinished = true;

                needReadConfig = true; //в любом случае первый раз нужно читать конфигурцию
            }

            
            // Если сработали условия — читаем дополнительные данные
            if (_mbService != null)
            {
                if (needReadStaticOrder)
                {
                    Debug.WriteLine("Сработал флаг Static Order Changed. Чтение StaticOrderData...");
                    staticOrderData = await _mbService.ReadStaticOrderDataAsync();
                }

                if (needReadConfig)
                {
                    Debug.WriteLine("Сработал флаг Config Changed. Чтение ConfigData...");
                    configData = await _mbService.ReadConfigDataAsync();
                }

                if (needReadMiscStatusData)
                {
                    Debug.WriteLine("Сработал флаг needReadConfig....");
                    miscStatusData = await _mbService.ReadMiscStatusDataAsync();
                }

                if (eventOrderStarted)
                {
                    Debug.WriteLine("Нужно прочитать статические данные Ордера : OrderStarted");
                    staticOrderData = await _mbService.ReadStaticOrderDataAsync();
                }
            }
            

            // Обработка по событиям
            if (eventOrderStarted)
            {
                Debug.WriteLine("Ордер начат : OrderStarted");
            }
            if (eventOrderFinished)
            {
                Debug.WriteLine("Ордер начат : OrderFinished");
            }
            if (eventDraftStarted)
            {
                Debug.WriteLine("Отвес начат : DraftStarted");
            }
            if (eventDraftFinished)
            {
                Debug.WriteLine("Отвес завершен : DraftFinished");
            }


        }






        // Медленный цикл: раз в 1 секунду
        private async Task ReadOrderLoopAsync(CancellationToken token)
        {
            int cycleCount = 0;
            var lastReportTime = DateTimeOffset.UtcNow;

            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (_mbService == null) break;
                    orderData = await _mbService.ReadOrderDataAsync();
                    miscStatusData = await _mbService.ReadMiscStatusDataAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Ошибка чтения orderData: {Msg}", ex.Message);
                    break;
                }

                cycleCount++;
                // Отчёт по частоте цикла (раз в секунду)
                var now = DateTimeOffset.UtcNow;
                if ((now - lastReportTime).TotalSeconds >= 1.0)
                {
                    //Debug.WriteLine($"Медленных циклов за секунду: {cycleCount}");
                    cycleCount = 0;
                    lastReportTime = now;
                }


                await Task.Delay(1000, token);
            }
        }


    }
}