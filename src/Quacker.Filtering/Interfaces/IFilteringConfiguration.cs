using System.Reflection;

namespace Quacker.Filtering.Interfaces
{
    public interface IFilteringConfiguration
    {
        void RegisterFilter(Filter filter);

        void RegisterAssembly(Assembly assembly);
    }
}