using MVVMNavigationModule.Abstractions;

namespace MVVMNavigationModule.Core
{
    public static class NavigationManagerExtensions
    {
        public static void Navigate(this INavigationManager navigationManager, string navigationKey)
        {
            navigationManager.Navigate(navigationKey, null);
        }
    }
}
