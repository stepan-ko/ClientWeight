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

        private CancellationTokenSource? _cts;
        private Task? _loopTask;
        private ModbusWeightService mbService;
        private Logger loggerModbus;

        public WeightViewModel()
        {
            orderData = new OrderData();
            staticOrderData = new StaticOrderData();
            statusData = new StatusData();
            miscStatusData = new MiscStatusData();
            configData = new ConfigData();
            newOrderData = new NewOrderData();
            ClickCommand = new RelayCommand(OnButtonClicked);
            LoopAsync();
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


        private async void LoopAsync()
        {
            // Отменяем предыдущий цикл, если был
            _cts?.Cancel();
            _loopTask?.Wait(0); // попытка быстро завершить (не блокирует)

            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            _loopTask = Task.Run(async () =>
            {
                try
                {
                    mbService = new ModbusWeightService("10.6.173.231", 1);
                    _ = mbService.ConnectAsync();
                    while (!token.IsCancellationRequested)
                    {
                        // Чтение информации                    
                        statusData = await mbService.ReadStatusDataAsync();
                        orderData = await mbService.ReadOrderDataAsync();


                        // Небольшая пауза, чтобы не забивать поток на 100% и дать шанс отмене
                        Thread.Sleep(10);
                    }
                }
                catch (Exception e) 
                { 
                    Debug.WriteLine("Ошибка LoopAsync() " + e);
                }
                



            }, token);
        }

    }
}