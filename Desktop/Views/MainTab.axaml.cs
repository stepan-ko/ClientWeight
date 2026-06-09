using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ClientCW.ViewModels;

namespace ClientCW.Views;

public partial class MainTab : UserControl
{
    public MainTab()
    {
        InitializeComponent();
        DataContext = new MainTabViewModel();
    }
}