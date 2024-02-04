using MVVMDialogsModule.Interfaces;
using System.Windows;

namespace MVVMDialogsModule.Views.Windows
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DialogWindowShell : Window
    {
        public DialogWindowShell(IWindowSupport window)
        {
            InitializeComponent();
            Owner = window.Owner;
            WindowStyle = window.Style;
            ResizeMode = window.ResizeMode;
            WindowStartupLocation = window.StartLocation;
            SizeToContent = window.SizeToContent;
        }
    }
}
