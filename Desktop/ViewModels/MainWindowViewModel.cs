using ClientCW.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Weight;

namespace ClientCW.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        //private readonly ModbusWeightService _mbService;
        //public WeightViewModel WeightVm { get; }
        //public OrderViewModel OrderVm { get; }
        [ObservableProperty] 
        private MainTabViewModel _mainTabVM;

        [ObservableProperty]
        private SettingsViewModel _settingsVM;

        [ObservableProperty]
        private ConfigurationViewModel _configVM;

        public MainWindowViewModel(MainTabViewModel mainTab, SettingsViewModel settings, ConfigurationViewModel configVM)
        {
            MainTabVM = mainTab;
            SettingsVM = settings;
            ConfigVM = configVM;

            //_mbService = new ModbusWeightService("10.6.173.231", 1);
            //WeightVm = new WeightViewModel(_mbService);
            //OrderVm = new OrderViewModel(_mbService);
        }

    }
}
