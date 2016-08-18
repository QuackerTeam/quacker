using Quacker.Filtering.DTO;
using Quacker.Filtering.Enums;
using System;
using System.Collections.Generic;

namespace Quacker.Filtering.Interfaces
{
    internal interface IFilter
    {
        void CreateFilter<TEntity>(string filterName = null,
                                   Func<TEntity, object[]> filterFields = null,
                                   Dictionary<string, ParameterAction<TEntity>> paramActions = null,
                                   FilterMode filterMode = FilterMode.SimpleSearch)
            where TEntity : class;

        void CreateFilter<TEntity>(Func<TEntity, object[]> filterFields = null,
                                   Dictionary<string, ParameterAction<TEntity>> paramActions = null,
                                   FilterMode filterMode = FilterMode.SimpleSearch)
            where TEntity : class;

        void CreateFilter<TEntity>(Func<TEntity, object[]> filterFields,
                                   FilterMode filterMode)
            where TEntity : class;

        void CreateFilter<TEntity>(string filterName,
                                   Func<TEntity, object[]> filterFields,
                                   FilterMode filterMode)
            where TEntity : class;

        void CreateFilter<TEntity>(string filterName,
                                   FilterMode filterMode)
            where TEntity : class;

        void CreateFilter<TEntity>(string filterName,
                                   Dictionary<string, ParameterAction<TEntity>> paramActions,
                                   FilterMode filterMode = FilterMode.SimpleSearch)
            where TEntity : class;

        void CreateFilter<TEntity>(Dictionary<string, ParameterAction<TEntity>> paramActions,
                                   FilterMode filterMode = FilterMode.SimpleSearch)
            where TEntity : class;

        void CreateFilter<TEntity>(FilterMode filterMode)
            where TEntity : class;

        void Configure();
    }
}