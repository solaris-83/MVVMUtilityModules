using Microsoft.Extensions.DependencyInjection;
using MVVMDialogsModule.Samples.ViewModels;
using System.Windows;

namespace MVVMDialogsModule.Samples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetRequiredService<MainViewModel>();
        }
    }
}