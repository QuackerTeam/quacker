using System.Reflection;

namespace Quacker.Filtering.Interfaces
{
    public interface IFilteringConfiguration
    {
        void RegisterFilter(IFilter filter);

        void RegisterAssembly(Assembly assembly);
    }
}