using System.Windows;

namespace MVVMDialogsModule.Interfaces
{
    /// <summary>
    /// General settings for the DialogWindowShell
    /// </summary>
    public interface IWindowSupport
    {
        public Window Owner { get;  }
        public WindowStyle Style { get; }
        public ResizeMode ResizeMode { get;  }
        public WindowStartupLocation StartLocation { get;  }
        public SizeToContent SizeToContent { get; }
    }
}
