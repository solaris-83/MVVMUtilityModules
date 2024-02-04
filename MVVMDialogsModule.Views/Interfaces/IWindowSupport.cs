using System.Windows;

namespace MVVMDialogsModule.Interfaces
{
    public interface IWindowSupport
    {
        public Window Owner { get;  }
        public WindowStyle Style { get; }
        public ResizeMode ResizeMode { get;  }
        public WindowStartupLocation StartLocation { get;  }
        public SizeToContent SizeToContent { get; }
    }
}
