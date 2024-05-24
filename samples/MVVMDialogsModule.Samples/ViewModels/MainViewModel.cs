using MVVMDialogsModule.Samples.Helpers;
using MVVMDialogsModule.Views.Interfaces;
using MVVMDialogsModule.Views.Models;
using System.ComponentModel;
using System.Windows.Input;

namespace MVVMDialogsModule.Samples.ViewModels
{
    public class MainViewModel: INotifyPropertyChanged
    {
        private readonly IDialogService _dialog;

        public event PropertyChangedEventHandler? PropertyChanged;

        private string _returnValue;
        public string ReturnValue { get => _returnValue; set { _returnValue = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ReturnValue))); } }

        public MainViewModel(IDialogService dialog)
        {
            _dialog = dialog;
            ShowDialogCommand = new MyCommand(Ok, (t) => true);
            ShowDialogCommandWithParameters = new MyCommand((s) => Ok("This is my message"), (t) => true);
        }

        private void Ok(object obj)
        {
            ReturnValue = default;
            if (obj == null)
                _dialog.ShowDialog<MessageNotificationViewModel>();
            else
            {
                var dp = new DialogParameters
                {
                    { "Message", obj.ToString() }
                };

                _dialog.ShowDialog<MessageNotificationViewModel>(dp);
                if (dp.TryGetValue("ReturnValue", out var message))
                    ReturnValue = message?.ToString();
            }
        }

        public ICommand ShowDialogCommand { get; private set; }
        public ICommand ShowDialogCommandWithParameters { get; private set; }
    }
}
