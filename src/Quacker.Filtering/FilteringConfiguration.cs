using Quacker.Filtering.Interfaces;
using System;
using System.Linq;
using System.Reflection;

namespace Quacker.Filtering
{
    internal class FilteringConfiguration : IFilteringConfiguration
    {
        public void RegisterAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Filter)) && !t.IsAbstract))
                RegisterFilter((Filter)Activator.CreateInstance(type));
        }

        public void RegisterFilter(Filter filter)
            => filter?.Configure();
    }
}