using Quacker.Common;
using Quacker.Filtering.DTO;
using Quacker.Filtering.Enums;
using Quacker.Filtering.Helpers;
using Quacker.Filtering.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Quacker.Filtering
{
    public abstract class Filter : IFilter
    {
        public void CreateFilter<TEntity>(Func<TEntity, object[]> filterFields = null,
                                          Dictionary<string, ParameterAction<TEntity>> paramActions = null,
                                          FilterMode filterMode = FilterMode.SimpleSearch)
            where TEntity : class
            => CreateFilter(null, filterFields, paramActions, filterMode);

        public void CreateFilter<TEntity>(string filterName,
                                          Dictionary<string, ParameterAction<TEntity>> paramActions,
                                          FilterMode filterMode = FilterMode.SimpleSearch)
            where TEntity : class
            => CreateFilter(filterName, null, paramActions, filterMode);

        public void CreateFilter<TEntity>(Dictionary<string, ParameterAction<TEntity>> paramActions,
                                          FilterMode filterMode = FilterMode.SimpleSearch)
            where TEntity : class
            => CreateFilter(null, null, paramActions, filterMode);

        public void CreateFilter<TEntity>(FilterMode filterMode) where TEntity : class
            => CreateFilter<TEntity>(null, null, null, filterMode);

        public void CreateFilter<TEntity>(Func<TEntity, object[]> filterFields,
                                          FilterMode filterMode)
            where TEntity : class
            => CreateFilter(null, filterFields, null, filterMode);

        public void CreateFilter<TEntity>(string filterName,
                                          Func<TEntity, object[]> filterFields,
                                          FilterMode filterMode)
            where TEntity : class
            => CreateFilter(filterName, filterFields, null, filterMode);

        public void CreateFilter<TEntity>(string filterName, FilterMode filterMode) where TEntity : class
            => CreateFilter<TEntity>(filterName, null, null, filterMode);

        public void CreateFilter<TEntity>(string filterName = null,
                                          Func<TEntity, object[]> filterFields = null,
                                          Dictionary<string, ParameterAction<TEntity>> paramActions = null,
                                          FilterMode filterMode = FilterMode.SimpleSearch)
            where TEntity : class
        {
            if (string.IsNullOrEmpty(filterName))
                filterName = "DefaultFilter";
            if (filterFields == null)
            {
                var propFuncs = typeof(TEntity).GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(prop =>
                {
                    Func<TEntity, object> propFunc = e => prop.GetValue(e);
                    return propFunc;
                });
                filterFields = e => propFuncs.Select(func => func(e)).ToArray();
            }
            if (paramActions == null)
                paramActions = new Dictionary<string, ParameterAction<TEntity>>();
            FilteringStorage.RegisterFilter(filterName, new FilterData<TEntity>
            {
                FilterFields = filterFields,
                ParameterActions = new NormalizedDictionary<ParameterAction<TEntity>>(paramActions),
                FilterMode = filterMode
            });
        }

        public abstract void Configure();
    }
}