
using System.Diagnostics;
using ClientCW.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Weight;



namespace ClientCW.ViewModels
{
    public partial class MainTabViewModel : ObservableObject
    {
        [ObservableProperty]
        private WeightViewModel _weightVM;

        [ObservableProperty]
        private OrderViewModel _orderVM;

        

        public MainTabViewModel(WeightViewModel weight, OrderViewModel order)
        {
            WeightVM = weight;
            OrderVM = order;
            
        }
    }

    
}