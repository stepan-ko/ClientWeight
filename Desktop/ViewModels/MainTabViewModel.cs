using System;
using System.Diagnostics;
using System.Reactive;
using System.Runtime.Serialization;
using Avalonia.Threading;
using ClientCW.Views;
using ReactiveUI;
using Weight;
using Weight.Data;

namespace ClientCW.ViewModels
{
    public class MainTabViewModel : ReactiveObject
    {

        public MainTabViewModel()
            {

            orderData = new OrderData();
            staticOrderData = new StaticOrderData();
            statusData = new StatusData();
            miscStatusData = new MiscStatusData();
            configData = new ConfigData();
            newOrderData = new NewOrderData();

            ClickButton1 = ReactiveCommand.Create(DebudPrintData);
                        
        }

        private OrderData _orderData;
        private StaticOrderData _staticOrderData;
        private StatusData _statusData;
        private MiscStatusData _miscStatusData;
        private ConfigData _configData;
        private NewOrderData _newOrderData;


        public OrderData orderData { get => _orderData; set => this.RaiseAndSetIfChanged(ref _orderData, value); }
        public StaticOrderData staticOrderData { get => _staticOrderData; set => this.RaiseAndSetIfChanged(ref _staticOrderData, value); }
        public StatusData statusData { get => _statusData; set => this.RaiseAndSetIfChanged(ref _statusData, value); }
        public MiscStatusData miscStatusData { get => _miscStatusData; set => this.RaiseAndSetIfChanged(ref _miscStatusData, value); }
        public ConfigData configData { get => _configData; set => this.RaiseAndSetIfChanged(ref _configData, value); }
        public NewOrderData newOrderData { get => _newOrderData; set => this.RaiseAndSetIfChanged(ref _newOrderData, value); }


       
        private void DebudPrintData()
        {

            statusData.InputUGFullSensor = !statusData.InputUGFullSensor;
            Debug.WriteLine("InputUGFullSensor = " + statusData.InputUGFullSensor);
            
            // Тестовые действи для Кнопки
            //Debug.WriteLine("SetOrderType = " + Order.SetOrderIndexType);
            //Debug.WriteLine("Id = " + Order.Id);
            //Debug.WriteLine("Weight = " + Order.Weight);
            //Debug.WriteLine("Druft = " + Order.Druft);
            //Debug.WriteLine("Customer = " + Order.Customer);
            //Debug.WriteLine("SetOrderIndexProduct = " + Order.SetOrderIndexProduct);
            //Debug.WriteLine("Comment = " + Order.Comment);



        }

        public ReactiveCommand<Unit, Unit> ClickButton1 { get; }

    }
}