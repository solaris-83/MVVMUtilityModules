using Microsoft.Extensions.DependencyInjection;
using MVVMDialogsModule.Interfaces;
using MVVMDialogsModule.Samples.ViewModels;
using MVVMDialogsModule.Views.Interfaces;
using MVVMDialogsModule.Views.Services;
using MVVMNavigationModule;
using MVVMNavigationModule.Abstractions;
using MVVMNavigationModule.Core;
using System.Runtime.Versioning;
using System.Windows;

[assembly: SupportedOSPlatform("windows")]
namespace MVVMDialogsModule.Samples
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = new MainWindow();
            ServiceProvider = ConfigureServices(mainWindow);

            var navManager = ServiceProvider.GetService<INavigationManager>();
            if (navManager == null) throw new ArgumentNullException($"Couldn't get an instance of {nameof(navManager)}");
            (navManager as NavigationManagerBase)?.Register<WelcomeView>(NavigationKeys.Welcome, ServiceProvider.GetService<WelcomeViewModel>() ?? throw new ArgumentNullException($"Couldn't get an instance of {nameof(WelcomeViewModel)}"));
            (navManager as NavigationManagerBase)?.Register<DialogsView>(NavigationKeys.Dialogs, ServiceProvider.GetService<DialogsViewModel>() ?? throw new ArgumentNullException($"Couldn't get an instance of {nameof(DialogsViewModel)}"));
            //((NavigationManagerBase)navManager).Register<MessageNotification>(NavigationKeys.Dialogs, ServiceProvider.GetService<MessageNotificationViewModel>);
            mainWindow.Show();
            navManager.Navigate(NavigationKeys.Welcome);
        }

        private static IServiceProvider ConfigureServices(MainWindow mainWindow)
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<INavigationManager>(_ => new NavigationManager(mainWindow));
            services.AddTransient<IDialogService, DialogService>();
            services.AddSingleton<IWindowSupport, WindowSupport>();
            services.AddSingleton<WelcomeViewModel>();
            services.AddSingleton<DialogsViewModel>();
            services.AddTransient<MessageNotificationViewModel>();
            DialogService.AutoRegisterDialogs<App>();
            return services.BuildServiceProvider();
        }
    }

    public static class NavigationKeys
    {
        public const string Welcome = nameof(Welcome);
        public const string Dialogs = nameof(Dialogs);
    }
}
