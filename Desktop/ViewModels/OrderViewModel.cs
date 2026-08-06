using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Avalonia.Threading;
using ClientCW.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Weight;
using Weight.Data;

namespace ClientCW.ViewModels
{
    public class OrderViewModel : ObservableObject
    {
        private readonly ModbusWeightService _mbService;
        public OrderViewModel(ModbusWeightService mbService)
        {
            _mbService = mbService;
            newOrderData = new NewOrderData();
            ClickStartOrder = new AsyncRelayCommand(StartOrderAsync);
        }

        private NewOrderData _newOrderData;
        public NewOrderData newOrderData { get => _newOrderData; set => this.SetProperty(ref _newOrderData, value); }


        public AsyncRelayCommand ClickStartOrder { get; }
        private async Task StartOrderAsync()
        {
            Debug.WriteLine("StartOrderClicked() - старт нового ордера");

            if (_mbService == null)
            {
                Debug.WriteLine("Modbus-сервис не инициализирован");
                return;
            }
            try
            {
                await _mbService.StartNewOrderAsync(newOrderData);
                Debug.WriteLine("Новый ордер запущен успешно");
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Запуск ордера отменён");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "Ошибка при запуске нового ордера");
                // Тут можно показать пользователю сообщение об ошибке (через Avalonia dialog или label)
            }
        }
    }
}