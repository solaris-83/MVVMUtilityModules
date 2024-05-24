using MVVMNavigationModule.Core;
using System.Windows.Controls;

namespace MVVMNavigationModule
{
    public class NavigationManager : NavigationManagerBase
    {
        public NavigationManager(ContentControl frameControl)
            : base(frameControl, new ViewInteractionStrategy())
        {
        }

        public NavigationManager( ContentControl frameControl, IDataStorage dataStorage)
            : base(frameControl, new ViewInteractionStrategy(), dataStorage)
        {
        }
    }
}
