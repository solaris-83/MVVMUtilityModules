using Microsoft.Extensions.DependencyInjection;
using MVVMDialogsModule.Interfaces;
using MVVMDialogsModule.Samples.ViewModels;
using MVVMDialogsModule.Views.Interfaces;
using MVVMDialogsModule.Views.Services;
using System.Windows;

namespace MVVMDialogsModule.Samples
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddTransient<IDialogService, DialogService>();
            services.AddSingleton<IWindowSupport, WindowSupport>();
            services.AddSingleton<MainViewModel>();
            services.AddTransient<MessageNotificationViewModel>();
            DialogService.AutoRegisterDialogs<App>();
        }
    }
}
