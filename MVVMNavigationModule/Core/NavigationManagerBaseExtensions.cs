
namespace MVVMNavigationModule.Core
{
    public static class NavigationManagerBaseExtensions
    {
        public static void Register<TView>(this NavigationManagerBase navigationManager,
                                           string navigationKey,
                                           Func<object> getViewModel)
            where TView : class, new()
        {
            Func<object> getView = Activator.CreateInstance<TView>;
            navigationManager.Register(navigationKey, getViewModel, getView);
        }

        public static void Register<TView>(this NavigationManagerBase navigationManager,
                                           string navigationKey,
                                           object viewModel)
            where TView : class, new()
        {
            Func<object> getView = Activator.CreateInstance<TView>;
            navigationManager.Register(navigationKey, () => viewModel, getView);
        }
    }
}
