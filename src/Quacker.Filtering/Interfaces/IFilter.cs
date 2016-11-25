using Quacker.Filtering.DTO;
using Quacker.Filtering.Enums;
using System;
using System.Collections.Generic;

namespace Quacker.Filtering.Interfaces
{
    public interface IFilter
    {
        void Configure();
    }

    internal interface IFilter<TEntity>
            where TEntity : class
    {
        void CreateFilter(string filterName = null,
                                   Func<TEntity, object[]> filterFields = null,
                                   Dictionary<string, ParameterAction<TEntity>> paramActions = null,
                                   FilterMode filterMode = FilterMode.SimpleSearch);

        void CreateFilter(Func<TEntity, object[]> filterFields = null,
                                   Dictionary<string, ParameterAction<TEntity>> paramActions = null,
                                   FilterMode filterMode = FilterMode.SimpleSearch);

        void CreateFilter(Func<TEntity, object[]> filterFields,
                                   FilterMode filterMode);

        void CreateFilter(string filterName,
                                   Func<TEntity, object[]> filterFields,
                                   FilterMode filterMode);

        void CreateFilter(string filterName,
                                   FilterMode filterMode);

        void CreateFilter(string filterName,
                                   Dictionary<string, ParameterAction<TEntity>> paramActions,
                                   FilterMode filterMode = FilterMode.SimpleSearch);

        void CreateFilter(Dictionary<string, ParameterAction<TEntity>> paramActions,
                                   FilterMode filterMode = FilterMode.SimpleSearch);

        void CreateFilter(FilterMode filterMode);

        void CreateOrder(string orderName,
                                         Func<IFilteredEnumerableItem<TEntity>, object> orderFn);

        void Configure();
    }
}