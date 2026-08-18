using System;
using Avalonia;
using ClientCW.ViewModels;
using Desktop.Settings;
using Microsoft.Extensions.DependencyInjection;
using Weight;

namespace Desktop
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.

        internal static IServiceProvider? _serviceProvider;


        [STAThread]
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();

            // Сначала настройки
            services.AddSingleton<SettingsService>();

            // Регистрируем ModbusWeightService с нужными параметрами
            services.AddSingleton<ModbusWeightService>(provider =>
            {
                var settings = provider.GetRequiredService<SettingsService>();                
                return new ModbusWeightService(settings.Current.ModbusHost, settings.Current.ModbusLocalHost, settings.Current.ModbusUnitId);
            });

            // ViewModels
            services.AddTransient<WeightViewModel>();
            services.AddTransient<OrderViewModel>();
            services.AddTransient<MainTabViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<MainWindowViewModel>();


            _serviceProvider = services.BuildServiceProvider();

            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
            

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()                
                .LogToTrace();
    }
}
