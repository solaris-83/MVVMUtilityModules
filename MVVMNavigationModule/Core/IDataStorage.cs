
namespace MVVMNavigationModule.Core
{
    public interface IDataStorage
    {
        void Add(string key, NavigationData navigationData);

        bool IsExist(string key);

        NavigationData Get(string key);
    }
}
