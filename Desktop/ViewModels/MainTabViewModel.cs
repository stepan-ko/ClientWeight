
using System.Diagnostics;
using System.Reactive;
using ReactiveUI;


namespace ClientCW.ViewModels
{
    public class MainTabViewModel : ReactiveObject
    {

        public MainTabViewModel()
        {
            //Weight = new WeightViewModel();
            //Order = new OrderViewModel();

           
                ClickMainTab = ReactiveCommand.Create(OnButtonClicked);
           
            
           
        }

    

        private void OnButtonClicked()
        {
         
                Debug.WriteLine("Команда выполнена!  MainTabViewModel()");
           
            
        }
        public ReactiveCommand<Unit, Unit> ClickMainTab { get; }

      
    }
}