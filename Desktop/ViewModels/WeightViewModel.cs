using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using ClientCW.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Weight;
using Weight.Data;
using NLog;

namespace ClientCW.ViewModels
{
    public class WeightViewModel : ObservableObject
    {

       

        public WeightViewModel()
        {
            orderData = new OrderData();
            staticOrderData = new StaticOrderData();
            statusData = new StatusData();
            miscStatusData = new MiscStatusData();
            configData = new ConfigData();
            newOrderData = new NewOrderData();
            ClickCommand = new RelayCommand(OnButtonClicked);
            StartLoop();
        }
        

        public RelayCommand ClickCommand { get; }
        private void OnButtonClicked()
        {
            //Debug.WriteLine("Команда выполнена! Отладочная строка из View‑Model");
            statusData.ScaleWeight += 1;
        }

        private OrderData _orderData;
        private StaticOrderData _staticOrderData;
        private StatusData _statusData;
        private MiscStatusData _miscStatusData;
        private ConfigData _configData;
        private NewOrderData _newOrderData;


        public OrderData orderData { get => _orderData; set => this.SetProperty(ref _orderData, value); }
        public StaticOrderData staticOrderData { get => _staticOrderData; set => this.SetProperty(ref _staticOrderData, value); }
        public StatusData statusData { get => _statusData; set => this.SetProperty(ref _statusData, value); }
        public MiscStatusData miscStatusData { get => _miscStatusData; set => this.SetProperty(ref _miscStatusData, value); }
        public ConfigData configData { get => _configData; set => this.SetProperty(ref _configData, value); }
        public NewOrderData newOrderData { get => _newOrderData; set => this.SetProperty(ref _newOrderData, value); }


        private CancellationTokenSource? _cts;
        private Task? _loopTask;
        private ModbusWeightService? _mbService;
        private bool _isConnected;

        public void StartLoop()
        {
            StopLoop(); // отменяем предыдущий

            _cts = new CancellationTokenSource();
            var token = _cts.Token;
            _loopTask = RunLoopAsync(token);
        }

        public void StopLoop()
        {
            _cts?.Cancel();
            // ждём завершения без блокировки основного потока
            _ = _loopTask?.ContinueWith(t =>
            {                
                _mbService = null;
                _isConnected = false;
            }, TaskScheduler.Default);

            _cts?.Dispose();
            _cts = null;
            _loopTask = null;
        }

        private async Task RunLoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // Подключаемся (с ограничением времени на попытку)
                if (await TryConnectWithRetryAsync(token))
                {
                    _isConnected = true;
                    Debug.WriteLine("Перед ProcessDataLoopAsync(token)");
                    await ProcessDataLoopAsync(token); // цикл чтения данных
                    _isConnected = false; // вышли из цикла чтения — значит, соединение потеряно
                }
                else
                {
                    _isConnected = false;
                }

                // Если не отменено — ждём 3 секунды перед следующей попыткой
                if (!token.IsCancellationRequested)
                {
                    try
                    {
                        await Task.Delay(3000, token);
                    }
                    catch (OperationCanceledException)
                    {
                        // нормально: сервис останавливается
                    }
                }
            }
        }

        // Оркестратор подключения: одна попытка, без рекурсии
        private async Task<bool> TryConnectWithRetryAsync(CancellationToken token)
        {
            //_mbService?.Dispose();
            _mbService = new ModbusWeightService("10.6.173.231", 1);

            try
            {
                var connected = await _mbService.ConnectAsync();
                Debug.WriteLine($"Подключение {connected} 10.6.173.231");
                return connected;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ошибка при попытке подключения: ", ex.Message);
                return false;
            }
        }

        // Цикл только для чтения данных, пока соединение активно
        private async Task ProcessDataLoopAsync(CancellationToken token)
        {
            int cycleCount = 0;
            var lastReportTime = DateTimeOffset.UtcNow;
            while (!token.IsCancellationRequested)
            {
                try
                {                    
                    // Проверяем, что сервис и соединение ещё валидны
                    if (_mbService == null) break;                   
                    
                    statusData = await _mbService.ReadStatusDataAsync();
                    orderData = await _mbService.ReadOrderDataAsync();

                    cycleCount++;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Ошибка чтения данных: ", ex.Message);
                    break; // выходим из цикла чтения, дальше сработает переподключение
                }

                // Асинхронная пауза (не Thread.Sleep!)
                await Task.Delay(100, token); // 100 мс достаточно, чтобы не забивать CPU

                // Проверка: прошла ли хотя бы 1 секунда с последнего отчёта
                var now = DateTimeOffset.UtcNow;
                if ((now - lastReportTime).TotalSeconds >= 1.0)
                {
                    Debug.WriteLine($"Циклов за последнюю секунду: {cycleCount}");
                    cycleCount = 0;                 // сбрасываем счётчик
                    lastReportTime = now;          // обновляем время отчёта
                }
            }
        }


    }
}