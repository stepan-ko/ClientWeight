using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ClientCW.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();           
        }

        public void CloseProgram(object sender, RoutedEventArgs args)
        {
            this.Close();
        }

    }
}