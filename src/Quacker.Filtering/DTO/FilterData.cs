using Quacker.Common;
using Quacker.Filtering.Enums;
using Quacker.Filtering.Interfaces;
using System;
using System.Collections.Generic;

namespace Quacker.Filtering.DTO
{
    internal abstract class FilterData { }

    internal class FilterData<TEntity> : IFilterData, IFilterData<TEntity>
    {
        public Func<TEntity, object[]> FilterFields { get; set; } = o => new object[] { };
        public NormalizedDictionary<ParameterAction<TEntity>> ParameterActions { get; set; } = new NormalizedDictionary<ParameterAction<TEntity>>();
        public FilterMode FilterMode { get; set; }

        public IEnumerable<TEntity> ExecuteParameters(IEnumerable<TEntity> list, NormalizedDictionary<object> parameters)
        {
            var tempList = list;
            foreach (var param in ParameterActions)
                if (parameters.ContainsKey(param.Key))
                    tempList = param.Value.ExecuteParam(tempList, parameters[param.Key]);
                else if (param.Value.ExecuteNoParam != null)
                    tempList = param.Value.ExecuteNoParam(tempList);
            return tempList;
        }

        public object[] GetFilterFields(TEntity entity)
            => FilterFields(entity);
    }
}