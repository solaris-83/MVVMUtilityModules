using Microsoft.Extensions.DependencyInjection;
using MVVMDialogsModule.Samples.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MVVMDialogsModule.Samples
{
    /// <summary>
    /// Interaction logic for DialogsView.xaml
    /// </summary>
    public partial class DialogsView : UserControl
    {
        public DialogsView()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetRequiredService<DialogsViewModel>();
        }
    }
}
