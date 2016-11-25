using Quacker.Common.Helpers.Extensions;
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
            foreach (var type in assembly.GetTypes().Where(t => t.IsSubclassOfRawGeneric(typeof(Filter<>)) && !t.IsAbstract))
                RegisterFilter((IFilter)Activator.CreateInstance(type));
        }

        public void RegisterFilter(IFilter filter)
            => filter?.Configure();
    }
}