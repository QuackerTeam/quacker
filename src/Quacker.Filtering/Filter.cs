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
    public abstract class Filter<TEntity> : IFilter<TEntity>, IFilter
            where TEntity : class
    {
        public void CreateFilter(Func<TEntity, object[]> filterFields = null,
                                          Dictionary<string, ParameterAction<TEntity>> paramActions = null,
                                          FilterMode filterMode = FilterMode.SimpleSearch)
            => CreateFilter(null, filterFields, paramActions, filterMode);

        public void CreateFilter(string filterName,
                                          Dictionary<string, ParameterAction<TEntity>> paramActions,
                                          FilterMode filterMode = FilterMode.SimpleSearch)
            => CreateFilter(filterName, null, paramActions, filterMode);

        public void CreateFilter(Dictionary<string, ParameterAction<TEntity>> paramActions,
                                          FilterMode filterMode = FilterMode.SimpleSearch)
            => CreateFilter(null, null, paramActions, filterMode);

        public void CreateFilter(FilterMode filterMode)
            => CreateFilter(null, null, null, filterMode);

        public void CreateFilter(Func<TEntity, object[]> filterFields,
                                          FilterMode filterMode)
            => CreateFilter(null, filterFields, null, filterMode);

        public void CreateFilter(string filterName,
                                          Func<TEntity, object[]> filterFields,
                                          FilterMode filterMode)
            => CreateFilter(filterName, filterFields, null, filterMode);

        public void CreateFilter(string filterName, FilterMode filterMode)
            => CreateFilter(filterName, null, null, filterMode);

        public void CreateFilter(string filterName = null,
                                          Func<TEntity, object[]> filterFields = null,
                                          Dictionary<string, ParameterAction<TEntity>> paramActions = null,
                                          FilterMode filterMode = FilterMode.SimpleSearch)
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

        public void CreateOrder(string orderName,
                                         Func<IFilteredEnumerableItem<TEntity>, object> orderFn)
        {
            if (string.IsNullOrEmpty(orderName))
                throw new ArgumentNullException(nameof(orderName));
            if (orderFn == null)
                throw new ArgumentNullException(nameof(orderFn));

            FilteringStorage.RegisterOrder(orderName, orderFn);
        }

        public abstract void Configure();
    }
}