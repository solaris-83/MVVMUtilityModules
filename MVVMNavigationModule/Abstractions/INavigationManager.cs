
namespace MVVMNavigationModule.Abstractions
{
    public interface INavigationManager
    {
        bool CanNavigate(string navigationKey);

        void Navigate(string navigationKey, object arg);

        event EventHandler<NavigationEventArgs> NavigationCompleted;
    }
}
