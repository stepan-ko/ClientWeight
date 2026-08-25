using System;
using System.Collections.Generic;
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
    public partial class OrderViewModel : ObservableObject
    {
        private readonly ModbusWeightService _mbService;

        public OrderViewModel(ModbusWeightService mbService)
        {
            _mbService = mbService;
            newOrderData = new NewOrderData();
            ClickTest = new AsyncRelayCommand(TestAsync);

            ClickStartOrder = new AsyncRelayCommand(StartOrderAsync);            
            ClickTareZero = new AsyncRelayCommand(() => SendCommandNumberAsync(15));
            ClickResetRunningTotal = new AsyncRelayCommand(() => SendCommandNumberAsync(36));
            ClickResetError = new AsyncRelayCommand(() => SendCommandNumberAsync(2));
        }

        private NewOrderData _newOrderData;
        public NewOrderData newOrderData { get => _newOrderData; set => this.SetProperty(ref _newOrderData, value); }



        // ------------------ Список режима заказа ----------------        
        


        // --------------- Действия кнопок ------------------------------

        public AsyncRelayCommand ClickStartOrder { get; }
        public AsyncRelayCommand ClickTareZero { get; }
        public AsyncRelayCommand ClickResetRunningTotal { get; }
        public AsyncRelayCommand ClickResetError { get; }
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


        private async Task SendCommandNumberAsync(int commandNumber)
        {
             
            if (_mbService == null)
            {
                Debug.WriteLine("Modbus-сервис не инициализирован");
                return;
            }
            if (!_mbService.IsConnected)
            {
                Debug.WriteLine("Весы не подключены!");
                return;
            }
            try
            {                
                await _mbService.StartCommandAsync(commandNumber);
                Debug.WriteLine($"Команда {commandNumber} выполнена");
                
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine($"Команда {commandNumber} - отмена!");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, $"Ошибка при команде {commandNumber}");
            }
        }
        

        // ---------------------- тестовое событие -----------------------
        public AsyncRelayCommand ClickTest { get; }
        private async Task TestAsync()
        {
            Debug.WriteLine($"newOrderData.StartUpMode = {newOrderData.StartUpMode}");
            Debug.WriteLine($"newOrderData.ProductName = {newOrderData.ProductName}");
            
        }
        // -----------------------------------------------------------------
    }
}