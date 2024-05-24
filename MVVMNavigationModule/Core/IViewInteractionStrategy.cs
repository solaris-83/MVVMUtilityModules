
namespace MVVMNavigationModule.Core
{
    public interface IViewInteractionStrategy
    {
        object GetContent(object control);

        void SetContent(object control, object content);

        object GetDataContext(object control);

        void SetDataContext(object control, object dataContext);

        void InvokeInUIThread(object control, Action action);
    }
}
