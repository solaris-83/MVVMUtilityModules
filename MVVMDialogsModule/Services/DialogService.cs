﻿
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using MVVMDialogsModule.Interfaces;
using MVVMDialogsModule.Views.Interfaces;
using MVVMDialogsModule.Views.Models;
using MVVMDialogsModule.Views.Windows;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace MVVMDialogsModule.Views.Services
{
    /// <inheritdoc />
    public class DialogService : IDialogService
    {
        #region Private members
        private readonly IServiceProvider _serviceProvider;
        private readonly IWindowSupport _window;
        static Dictionary<Type, Type> _mappings = new Dictionary<Type, Type>();
        static Dictionary<Type, DialogWindowShell> _windowMappings = new Dictionary<Type, DialogWindowShell>();

        #endregion

        #region Public properties
        
        /// <inheritdoc />
        public DefaultDialogSettings Settings { get; private set; } = new DefaultDialogSettings();

        /// <inheritdoc />
        public object? ReturnParameters { get; private set; }

        #endregion

        #region CTOR
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceProvider">The provider needed for Dependency Injection</param>
        public DialogService(IServiceProvider serviceProvider, IWindowSupport window)
        {
            _window = window;
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Register a view via "DialogModule" attribute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AutoRegisterDialogs<T>()
        {
            var type = typeof(T);
            var viewModelNamespace = $"{ Assembly.GetCallingAssembly().GetName().Name}.ViewModels";
            foreach (var exportedType in type.GetTypeInfo().Assembly.DefinedTypes.Where(t => t.GetCustomAttribute<DialogModuleAttribute>() != null))
            {
                Type vm = Type.GetType($"{viewModelNamespace}.{exportedType.Name}ViewModel, {exportedType.Assembly.FullName}") ?? throw new ArgumentNullException($"ViewModel not found for {exportedType.Name} at {viewModelNamespace}. Make sure to place view model classes in the \"ViewModels\" folder.");
                _mappings.TryAdd(vm, exportedType);
            }
        }

        public void OpenFileDialog(DialogParameters dialogParameters)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (dialogParameters.ContainsKey("Filter") && dialogParameters["Filter"] != null)
            {
                openFileDialog.Filter = dialogParameters["Filter"].ToString();
            }
            if (dialogParameters.ContainsKey("InitialDirectory") && dialogParameters["InitialDirectory"] != null)
            {
                openFileDialog.InitialDirectory = dialogParameters["InitialDirectory"].ToString();
            }
            if (openFileDialog.ShowDialog() == true)
            {
                dialogParameters.Add("FileName", openFileDialog.FileName);
            }
        }

        /// <inheritdoc />
        public void ShowDialog<TViewModel>()
        {
            var type = _mappings[typeof(TViewModel)];
            ShowDialogInternal(type, null);
        }

        /// <inheritdoc />
        public void ShowDialog<TViewModel>(DialogParameters dialogParameters)
        {
            var type = _mappings[typeof(TViewModel)];
            ShowDialogInternal(type, dialogParameters);
        }

        /// <inheritdoc />
        public TReturn ShowDialog<TViewModel, TReturn>()
        {
            var type = _mappings[typeof(TViewModel)];
            return ShowDialogReturnInternal<TReturn>(type, null);
        }

        /// <inheritdoc />
        public TReturn ShowDialog<TViewModel, TReturn>(DialogParameters dialogParameters)
        {
            var type = _mappings[typeof(TViewModel)];
            return ShowDialogReturnInternal<TReturn>(type, dialogParameters);
        }

        /// <inheritdoc />
        public void CloseDialog<TViewModel>()
        {
            var type = _mappings[typeof(TViewModel)];

            if (!_windowMappings.ContainsKey(type))
                return;

            var dialogToClose = _windowMappings[type];
            if (dialogToClose == null)
                return;

            dialogToClose.Close();
            _windowMappings.Remove(type);
        }

        /// <inheritdoc />
        //public TReturn GetReturnParameters<TReturn>()
        //{
        //    try { return (TReturn)ReturnParameters; }
        //    catch { return default; }
        //}

        /// <inheritdoc />
        public void SetReturnParameters(object returnParameters)
        {
            ReturnParameters = returnParameters;
        }

        /// <inheritdoc />
        public void SetDefaultDialogSettings(DefaultDialogSettings dialogSettings)
        {
            Settings = dialogSettings;
        }

        /// <inheritdoc />
        public DefaultDialogSettings GetDefaultDialogSettings()
        {
            return Settings;
        }

        public bool DialogIsOpen<TViewModel>()
        {
            bool result = false;
            if (_windowMappings == null)
                return result;
            Type type = _mappings[typeof(TViewModel)];
            if (_windowMappings.ContainsKey(type))
                result = true;
            return result;
        }
        #endregion

        #region Private methods

        private void ShowDialogInternal(Type type, DialogParameters? dialogParameters)
        {

            ReturnParameters = default;

            var dialog = CreateDialogInternal(type);

            FrameworkElement? frameworkElement = dialog.Content as FrameworkElement;

            SetupEventHandlersInternal(dialog, frameworkElement, dialogParameters);
            SetupViewModelBindingsInternal(type, frameworkElement);

            if (frameworkElement != null)
            {
                dialog.Height = frameworkElement.Height;
                dialog.Width = frameworkElement.Width;
            }

            _windowMappings.Add(type, dialog);

            dialog.ShowDialog();

            _windowMappings.Remove(type);
        }

        private TReturn? ShowDialogReturnInternal<TReturn>(Type type, DialogParameters? dialogParameters)
        {
            ShowDialogInternal(type, dialogParameters);

            try { return (TReturn)ReturnParameters; }
            catch { return default; }
        }

        private DialogWindowShell CreateDialogInternal(Type type)
        {
            var dialog = new DialogWindowShell(_window);
            var content = ActivatorUtilities.CreateInstance(_serviceProvider, type);

            if (content is FrameworkElement viewForName
                && DialogSettings.GetDialogName(viewForName) != null)
            {
                dialog.Title = DialogSettings.GetDialogName(viewForName);
            }

            if (content is FrameworkElement viewForStyle
                && DialogSettings.GetWindowStyle(viewForStyle) != WindowStyle.SingleBorderWindow)
            {
                dialog.WindowStyle = DialogSettings.GetWindowStyle(viewForStyle);
            }

            dialog.Content = content;
            dialog.WindowStyle = dialog.WindowStyle == WindowStyle.SingleBorderWindow ? Settings.DialogWindowDefaultStyle : dialog.WindowStyle;
            dialog.Title = dialog.Title ?? Settings.DialogWindowDefaultTitle;

            return dialog;
        }

        private void SetupEventHandlersInternal(DialogWindowShell dialog, FrameworkElement? frameworkElement, DialogParameters? dialogParameters)
        {
            EventHandler? closeEventHandler = default;
            RoutedEventHandler? shownEventHandler = default;

            closeEventHandler = (s, e) =>
            {
                Type type = frameworkElement?.DataContext.GetType();
                MethodInfo method = type.GetMethod("OnDialogClosed");
                method?.Invoke(frameworkElement?.DataContext, [dialogParameters]);
                dialog.Closed -= closeEventHandler;
            };

            dialog.Closed += closeEventHandler;

            shownEventHandler = (s, e) =>
            {
                Type type = frameworkElement?.DataContext.GetType();
                MethodInfo method = type.GetMethod("OnDialogShown");
                method?.Invoke(frameworkElement?.DataContext, [dialogParameters]);
                dialog.Loaded -= shownEventHandler;
            };

            dialog.Loaded += shownEventHandler;
        }

        private void SetupViewModelBindingsInternal(Type type, FrameworkElement frameworkElement)
        {
            if (frameworkElement == null) return;

            if (frameworkElement.DataContext == null)
            {
                var vmType = _mappings.FirstOrDefault(x => x.Value == type).Key;
                frameworkElement.DataContext = ActivatorUtilities.CreateInstance(_serviceProvider, vmType);
            }
        }

        #endregion
    }
}