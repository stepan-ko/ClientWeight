using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ClientCW.ViewModels;
using ClientCW.Views;
using Weight;


namespace Desktop
{
    public partial class App : Application
    {
        public ModbusWeightService MbService { get; private set; }


        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            MbService = new ModbusWeightService("10.6.173.231", 1);

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {                

                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };

            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}