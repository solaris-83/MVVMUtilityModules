
using MVVMDialogsModule.Views.Models;
using System.Windows.Input;

namespace MVVMDialogsModule.Views.Interfaces
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
        ICommand OkCommand { get; }

        /// <summary>
        /// Method called when Cancel button is pressed
        /// </summary>
        ICommand CancelCommand { get; }

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
