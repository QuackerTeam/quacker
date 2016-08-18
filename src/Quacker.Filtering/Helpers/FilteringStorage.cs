using Quacker.Filtering.Interfaces;
using System;
using System.Collections.Generic;

namespace Quacker.Filtering.Helpers
{
    internal static class FilteringStorage
    {
        private static readonly Dictionary<string, Dictionary<string, IFilterData>> _filterData = new Dictionary<string, Dictionary<string, IFilterData>>();

        public static IFilterData<TEntity> GetFilter<TEntity>(string filterName)
        {
            var typeFullName = typeof(TEntity).FullName;
            if (string.IsNullOrEmpty(filterName))
                throw new ArgumentNullException(nameof(filterName));
            if (!_filterData.ContainsKey(typeFullName) || !_filterData[typeFullName].ContainsKey(filterName))
                throw new InvalidOperationException($"Filter not found: {filterName} in entity {typeFullName}");
            return (IFilterData<TEntity>)_filterData[typeFullName][filterName];
        }

        public static void RegisterFilter<TEntity>(string filterName, IFilterData<TEntity> filterData)
        {
            var typeFullName = typeof(TEntity).FullName;
            if (_filterData.ContainsKey(typeFullName) && _filterData[typeFullName].ContainsKey(filterName))
            {
                var msg = filterName == "DefaultFilter"
                        ? $"There is already a default filter created for the instance {typeFullName}. Please name this filter to distinguish it from the others."
                        : $"There is already a filter named {filterName} for the instance {typeFullName}. Please choose another name.";
                throw new InvalidOperationException(msg);
            }
            if (!_filterData.ContainsKey(typeFullName))
                _filterData.Add(typeFullName, new Dictionary<string, IFilterData>());
            _filterData[typeFullName].Add(filterName, (IFilterData)filterData);
        }
    }
}