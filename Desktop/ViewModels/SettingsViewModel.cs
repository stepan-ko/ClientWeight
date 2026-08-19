using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using ClientCW.ViewModels;
using ClientCW.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Desktop.Settings;

namespace ClientCW.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {

        [ObservableProperty]
        private SettingsService _settings;

        [ObservableProperty]
        private bool _canSave = false;

        

        public SettingsViewModel(SettingsService settingsService)
        {
            Settings = settingsService;
            UpdateCanSave();
            Settings.Current.PropertyChanged += (_, _) => UpdateCanSave();
        }

        private void UpdateCanSave()
        {
            CanSave = Settings.HasChanges();
            //Debug.WriteLine($"Сработал UpdateCanSave() = {CanSave}");
        }

        [RelayCommand]
        private void Save()
        {
            Settings.Save();
            UpdateCanSave();
            // после Save() HasChanges() вернёт false, CanSave станет false
        }

        [RelayCommand]
        private void Reset()
        {
            Settings.ResetToOriginal();
            UpdateCanSave();
            // Current обновится, сработает PropertyChanged, CanSave станет false
        }
    }
}