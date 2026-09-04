using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using ClientCW.ViewModels;
using ClientCW.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Desktop.Settings;
using Weight;

namespace ClientCW.ViewModels
{
    public partial class ConfigurationViewModel : ObservableObject
    {

        [ObservableProperty]
        private SettingsService _settings;
        private readonly ModbusWeightService _mbService;
        private readonly ScaleData _scale;

        private ConfigData NewConfigData = new ConfigData();

        [ObservableProperty]
        private ObservableCollection<ConfigParameter> _configParameters = new();


        public ConfigurationViewModel(SettingsService settingsService, ModbusWeightService mbService, ScaleData scale)
        {
            _mbService = mbService;
            _scale = scale;
            Settings = settingsService;


            CopyConfigData();

            ConfigParameters.Add(
                new ConfigParameter {Name = "Название весов", CurrentValue = _scale.ConfigData.ScaleName, NewValue = NewConfigData.ScaleName }
                    
                );
        }

        [RelayCommand]
        private void SaveConfig()
        {            
            Debug.WriteLine("Нажали сохранить конфигурацию в Весах");
            Debug.WriteLine($"_scale.ConfigData.ScaleName = {_scale.ConfigData.ScaleName} ");
           
        }


        private void CopyConfigData()
        {
            NewConfigData.ScaleName = _scale.ConfigData.ScaleName;
        }
       

        public partial class ConfigParameter : ObservableObject
        {         
            public string Name { get; set; } = "";                       
            public object? CurrentValue { get; set; }
            public object? NewValue { get; set; }
        }


    }
}