using Quacker.Common;
using Quacker.Filtering.Enums;
using System.Collections.Generic;

namespace Quacker.Filtering.Interfaces
{
    internal interface IFilterData { }

    internal interface IFilterData<TEntity>
    {
        object[] GetFilterFields(TEntity entity);

        IEnumerable<TEntity> ExecuteParameters(IEnumerable<TEntity> list, NormalizedDictionary<object> parameters);

        FilterMode FilterMode { get; set; }
    }
}