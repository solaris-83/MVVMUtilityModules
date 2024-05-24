using MVVMDialogsModule.Interfaces;
using System.Windows;

namespace MVVMDialogsModule.Samples
{
    internal class WindowSupport : IWindowSupport
    {
        public Window Owner => Application.Current.MainWindow;
        public WindowStyle Style => WindowStyle.None;
        public ResizeMode ResizeMode => ResizeMode.NoResize;
        public WindowStartupLocation StartLocation => WindowStartupLocation.CenterOwner;
        public SizeToContent SizeToContent => SizeToContent.WidthAndHeight;
    }
}
