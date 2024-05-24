using MVVMDialogsModule.Samples.Helpers;
using MVVMNavigationModule.Abstractions;
using System.Windows.Input;

namespace MVVMDialogsModule.Samples.ViewModels
{
    public class WelcomeViewModel
    {
        private readonly INavigationManager _navigationManager;
        public WelcomeViewModel(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            WorkWithDialogCommand = new MyCommand(ShowDialog, (t) => true);
        }

        private void ShowDialog(object obj)
        {
           _navigationManager.Navigate(NavigationKeys.Dialogs, obj);
        }

        public ICommand WorkWithDialogCommand { get; private set; }
    }
}
