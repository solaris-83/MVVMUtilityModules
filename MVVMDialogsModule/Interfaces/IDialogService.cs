using MVVMDialogsModule.Models;

namespace MVVMDialogsModule.Interfaces
{
    /// <summary>
    /// Represents Dialog Service.
    /// </summary>
    /// <remarks>
    /// A ViewModel that injects this interface can open, close, 
    /// and return parameters during the showing and closing of Dialogs.
    /// </remarks>
    public interface IDialogService
    {
        /// <summary>
        /// Open file dialog with parameter to pass to the service and back again to the caller
        /// </summary>
        /// <param name="dialogParameters"></param>
        void OpenFileDialog(DialogParameters dialogParameters);

        /// <summary>
        /// Shows the dialog associated with the passed ViewModel.
        /// </summary>
        /// <typeparam name="TViewModel">The associated ViewModel to the requested View</typeparam>
        void ShowDialog<TViewModel>();

        /// <summary>
        /// Shows the dialog associated with the passed ViewModel.
        /// </summary>
        /// <typeparam name="TViewModel">The associated ViewModel to the requested View</typeparam>
        /// <param name="dialogParameters">KeyValuePair of parameters used in IDialogAware ViewModels</param>
        void ShowDialog<TViewModel>(DialogParameters dialogParameters);

        /// <summary>
        ///  Shows the dialog associated with the passed ViewModel.
        /// </summary>
        /// <typeparam name="TViewModel">The associated ViewModel to the requested View</typeparam>
        /// <typeparam name="TReturn">The expected return type</typeparam>
        /// <returns>Returns the 'ReturnParameters' set in the dialog ViewModel</returns>
        TReturn ShowDialog<TViewModel, TReturn>();

        /// <summary>
        ///  Shows the dialog associated with the passed ViewModel.
        /// </summary>
        /// <typeparam name="TViewModel">The associated ViewModel to the requested View</typeparam>
        /// <typeparam name="TReturn">The expected return type</typeparam>
        /// <param name="dialogParameters">KeyValuePair of parameters used in IDialogAware ViewModels</param>
        /// <returns></returns>
        TReturn ShowDialog<TViewModel, TReturn>(DialogParameters dialogParameters);

        /// <summary>
        /// Closes the dialog associated with the passed ViewModel
        /// </summary>
        /// <typeparam name="TViewModel">The associated ViewModel to the requested View</typeparam>
        void CloseDialog<TViewModel>();

        /// <summary>
        /// Get's the current ReturnParameters
        /// </summary>
        /// <typeparam name="TReturn">The expected return type</typeparam>
        /// <returns>Returns the ReturnParameters as the requested type</returns>
        TReturn GetReturnParameters<TReturn>();

        /// <summary>
        /// Set's the ReturnParameters
        /// </summary>
        /// <param name="returnParameters">The value of the expected return parameters</param>
        void SetReturnParameters(object returnParameters);

        /// <summary>
        /// Set's the DefaultDialogSettings
        /// </summary>
        /// <param name="dialogSettings">The value of the expected default settings</param>
        void SetDefaultDialogSettings(DefaultDialogSettings dialogSettings);

        /// <summary>
        /// Get's the current Default Dialog Settings
        /// </summary>
        /// <returns>Returns the Default Dialog Settings</returns>
        DefaultDialogSettings GetDefaultDialogSettings();
        bool DialogIsOpen<TViewModel>();
    }
}