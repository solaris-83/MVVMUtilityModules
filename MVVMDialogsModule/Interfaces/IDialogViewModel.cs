

using CommunityToolkit.Mvvm.Input;
using MVVMDialogsModule.Models;

namespace MVVMDialogsModule.Interfaces
{
    public interface IDialogViewModel
    {
        /// <summary>
        /// Called when the Dialog has been closing.
        /// </summary>
        void DialogClosing();

        /// <summary>
        /// Method called when OK button is pressed
        /// </summary>
        IRelayCommand OkCommand { get; }

        /// <summary>
        /// Method called when Cancel button is pressed
        /// </summary>
        IRelayCommand CancelCommand { get; }

        /// <summary>
        /// Called when the Dialog has been shown.
        /// </summary>
        void OnDialogShown(DialogParameters dialogParameters);

        /// <summary>
        /// Called when the Dialog is closed
        /// </summary>
        void OnDialogClosed(DialogParameters dialogParameters);
    }
}
