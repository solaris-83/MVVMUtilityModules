using MVVMDialogsModule.Samples.Helpers;
using MVVMDialogsModule.Views.Interfaces;
using MVVMDialogsModule.Views.Models;
using System.ComponentModel;
using System.Windows.Input;

namespace MVVMDialogsModule.Samples.ViewModels
{
    public class MessageNotificationViewModel: IDialogViewModel, INotifyPropertyChanged 
    {
        private readonly IDialogService _dialogService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public MessageNotificationViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            OkCommand = new MyCommand(Ok, (t) => true);
        }

        private string _message = "Hello";
        public string Message { get => _message; set { _message = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message))); } }

        public ICommand OkCommand { get; private set; }

        public ICommand CancelCommand => throw new NotImplementedException();

        public void OnDialogClosing()
        {
           
        }

        public void OnDialogClosed(DialogParameters dialogParameters)
        {
            if (dialogParameters != null && dialogParameters.TryGetValue("Message", out object? value))
                dialogParameters.Add("ReturnValue", "This is the return value");
        }

        public void OnDialogShown(DialogParameters dialogParameters)
        {
            if (dialogParameters != null && dialogParameters.TryGetValue("Message", out object? value))
                Message = value?.ToString();
        }

        private void Ok(object obj)
        {
            _dialogService.SetReturnParameters(true);
            _dialogService.CloseDialog<MessageNotificationViewModel>();
        }
    }
}