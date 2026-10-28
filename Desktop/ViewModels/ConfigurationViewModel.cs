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
        private ObservableCollection<ConfigParameter> _configMain = new();
        
        [ObservableProperty]
        private ObservableCollection<ConfigParameter> _configNetwork = new();

        private readonly ScaleData _scale;
        
        
        [ObservableProperty]
        public ConfigData _newConfig = new();

        public ConfigurationViewModel(SettingsService settingsService, ModbusWeightService mbService, ScaleData scale)
        {
            _mbService = mbService;
            _scale = scale;
            Settings = settingsService;

            InitConfigMain();
            InitConfigNetwork();

            // Одна подписка на весь singleton — на всё время жизни приложения
            _scale.ConfigData.PropertyChanged += OnConfigDataChanged;
           
        }

        private void OnConfigDataChanged(object? sender, PropertyChangedEventArgs e)
        {
            // Простой вариант: обновляем все параметры
            foreach (var p in ConfigMain)
                p.RefreshValue();

            foreach (var p in ConfigNetwork)
                p.RefreshValue();
        }

        private static int ToInt(object? v) => int.TryParse(v?.ToString(), out var i) ? i : 0;


        private void InitConfigMain()
        {           
            ConfigMain.Add(new ConfigParameter(
                "Название весов",
                getter: () => _scale.ConfigData.ScaleName,
                setter: v => _scale.ConfigData.ScaleName = v?.ToString() ?? ""));

            ConfigMain.Add(new ConfigParameter(
                "Вес тары",
                getter: () => _scale.ConfigData.TareResetRange,
                setter: v => _scale.ConfigData.TareResetRange = ToInt(v)));
        }

        private void InitConfigNetwork()
        {
            ConfigNetwork.Add(new ConfigParameter(
                "IP-адрес",
                getter: () => _scale.ConfigData.IPAddressString,
                setter: v => _scale.ConfigData.IPAddressString = v?.ToString() ?? ""));

            ConfigNetwork.Add(new ConfigParameter(
                "Маска сети",
                getter: () => _scale.ConfigData.SubnetMaskString,
                setter: v => _scale.ConfigData.SubnetMaskString = v?.ToString() ?? ""));
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
            //ConfigParameters[0].Value = ConfigParameters[0].NewValue = _scale.ConfigData.ScaleName;
            //ConfigParameters[1].Value = ConfigParameters[1].NewValue = _scale.ConfigData.TareResetRange;

            Debug.WriteLine($"_scale.ConfigData.ScaleName = {_scale.ConfigData.ScaleName}");
        }


        public partial class ConfigParameter : ObservableObject
        {
            public string Name { get; set; } = "";

            [ObservableProperty] private string? _newValue;

            private readonly Func<object?>? _getter;
            private readonly Action<object?>? _setter;
            private object? _value;

            public ConfigParameter() { }

            public ConfigParameter(string name, Func<object?> getter, Action<object?>? setter = null)
            {
                Name = name;
                _getter = getter;
                _setter = setter;
            }

            public object? Value
            {
                get => _getter is not null ? _getter() : _value;
                set
                {
                    if (_setter is not null)
                    {
                        _setter(value);
                        OnPropertyChanged();
                    }
                    else
                    {
                        SetProperty(ref _value, value);
                    }
                }
            }

            internal void RefreshValue()
            {
                OnPropertyChanged(nameof(Value));
                NewValue = Value?.ToString();
            }
        }


    }
}