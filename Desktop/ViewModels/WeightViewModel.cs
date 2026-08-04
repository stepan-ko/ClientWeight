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
    public class WeightViewModel : ReactiveObject
    {

        public WeightViewModel()
            {

            orderData = new OrderData();
            staticOrderData = new StaticOrderData();
            statusData = new StatusData();
            miscStatusData = new MiscStatusData();
            configData = new ConfigData();
            newOrderData = new NewOrderData();

            ClickCommand = ReactiveCommand.Create(OnButtonClicked);

            weightTitle = "Это public class WeightViewModel : ReactiveObject";
        }

        private string _weightTitle;
        public string weightTitle { get => _weightTitle; set => this.RaiseAndSetIfChanged(ref _weightTitle, value); }

        public ReactiveCommand<Unit, Unit> ClickCommand { get; }

        private void OnButtonClicked()
        {
            //Debug.WriteLine("Команда выполнена! Отладочная строка из View‑Model");

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


       
    }
}