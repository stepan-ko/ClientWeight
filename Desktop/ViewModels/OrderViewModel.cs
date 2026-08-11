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
            ClickTareZero = new AsyncRelayCommand(TareZeroAsync);
            ClickResetRunningTotal = new AsyncRelayCommand(ResetRunningTotalAsync);
        }

        private NewOrderData _newOrderData;
        public NewOrderData newOrderData { get => _newOrderData; set => this.SetProperty(ref _newOrderData, value); }



        // ------------------ Список режима заказа ----------------        
        


        // --------------- Действия кнопок ------------------------------

        public AsyncRelayCommand ClickStartOrder { get; }
        public AsyncRelayCommand ClickTareZero { get; }
        public AsyncRelayCommand ClickResetRunningTotal { get; }
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

        private async Task TareZeroAsync()
        {        
            if (_mbService == null)
            {
                Debug.WriteLine("Modbus-сервис не инициализирован");
                return;
            }
            try
            {
                await _mbService.StartCommandAsync(15);
                Debug.WriteLine("Тара обнулена");
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Обнуление тары - отмена!");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "Ошибка при Обнулении тары");               
            }
        }

        private async Task ResetRunningTotalAsync()
        {
            if (_mbService == null)
            {
                Debug.WriteLine("Modbus-сервис не инициализирован");
                return;
            }
            try
            {                
                await _mbService.StartCommandAsync(36);
                Debug.WriteLine("Текущий итог сброшен");
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Обнуление Текущий итог - отмена!");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, "Ошибка при Обнуление Текущий итог");
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