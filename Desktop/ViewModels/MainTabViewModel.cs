
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Weight;



namespace ClientCW.ViewModels
{
    public class MainTabViewModel : ObservableObject
    {

        private readonly ModbusWeightService _mbService;
        public WeightViewModel WeightVm { get; }
        public OrderViewModel OrderVm { get; }

        public MainTabViewModel()
        {
            _mbService = new ModbusWeightService("10.6.173.231", 1);
            WeightVm = new WeightViewModel(_mbService);
            OrderVm = new OrderViewModel(_mbService);
        }


    }
}