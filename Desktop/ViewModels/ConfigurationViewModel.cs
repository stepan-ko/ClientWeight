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
using static ClientCW.ViewModels.ConfigurationViewModel;

namespace ClientCW.ViewModels
{
    public partial class ConfigurationViewModel : ObservableObject
    {

        [ObservableProperty]
        private SettingsService _settings;
        private readonly ModbusWeightService _mbService;
        

        private ConfigData NewConfigData = new ConfigData();

        [ObservableProperty]
        private ObservableCollection<ConfigParameter> _configParameters = new();

        private readonly ScaleData _scale;
        
        [ObservableProperty]
        public ConfigData _currentConfig;
        
        [ObservableProperty]
        public ConfigData _newConfig = new();

        public ConfigurationViewModel(SettingsService settingsService, ModbusWeightService mbService, ScaleData scale)
        {
            _mbService = mbService;
            _scale = scale;
            Settings = settingsService;

            //CopyConfigData();
            CurrentConfig = _scale.ConfigData;

            ConfigParameters.Add(new ConfigParameter { Name = "Название весов", Value = _scale.ConfigData.ScaleName});
            ConfigParameters.Add(new ConfigParameter { Name = "Вес тары", Value = _scale.ConfigData.TareResetRange});

        }

        [RelayCommand]
        private void SaveConfig()
        {            
            Debug.WriteLine("Нажали сохранить конфигурацию в Весах");                        
        }

        [RelayCommand]
        private void ResetConfig()
        {
            Debug.WriteLine("Отменить изменения");
            ResetNewValue();
        }

        private void ResetNewValue()
        {
            ConfigParameters[0].Value = ConfigParameters[0].NewValue = _scale.ConfigData.ScaleName;
            ConfigParameters[1].Value = ConfigParameters[1].NewValue = _scale.ConfigData.TareResetRange;
        }


        private void CopyConfigData()
        {   
            NewConfigData.ScaleName = _scale.ConfigData.ScaleName;
        }

        public partial class ConfigParameter : ObservableObject
        {
            public string Name { get; set; } = "";
            
            [ObservableProperty]
            public object? _newValue;

            private object? _value;

            public object? Value { get => _value;
                set
                {
                    if (SetProperty(ref _value, value))
                    {
                        NewValue = value;
                    }
                }
            }

            
            
        }


    }
}