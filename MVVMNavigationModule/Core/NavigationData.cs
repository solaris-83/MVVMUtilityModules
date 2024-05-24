
namespace MVVMNavigationModule.Core
{
    public sealed class NavigationData
    {
        public NavigationData(Func<object> viewModelFunc, Func<object> viewFunc)
        {
            ViewModelFunc = viewModelFunc ?? throw new ArgumentNullException(nameof(viewModelFunc));
            ViewFunc = viewFunc ?? throw new ArgumentNullException(nameof(viewFunc));
        }

        public Func<object> ViewModelFunc { get; }

        public Func<object> ViewFunc { get; }
    }
}
