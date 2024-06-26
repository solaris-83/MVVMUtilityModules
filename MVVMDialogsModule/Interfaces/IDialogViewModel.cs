﻿
using MVVMDialogsModule.Views.Models;
using System.Windows.Input;

namespace MVVMDialogsModule.Views.Interfaces
{
    public interface IDialogViewModel<T> where T : ICommand
    {
        /// <summary>
        /// Called when the Dialog has been closing.
        /// </summary>
        void OnDialogClosing();

        /// <summary>
        /// Method called when OK button is pressed
        /// </summary>
        T OkCommand { get; }

        /// <summary>
        /// Method called when Cancel button is pressed
        /// </summary>
        T CancelCommand { get; }

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
