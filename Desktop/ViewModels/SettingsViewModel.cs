using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ClientCW.ViewModels;
using ClientCW.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Desktop.Settings;

namespace ClientCW.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {

        [ObservableProperty]
        private SettingsService _settings;

        public SettingsViewModel(SettingsService settingsService) 
       {
            Settings = settingsService;

            Debug.WriteLine($"Settings.Current.ModbusHost = {Settings.Current.ModbusHost}");


       }

    }
}