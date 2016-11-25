using System;

namespace Quacker.Filtering.Interfaces
{
    internal interface IOrderData { }

    internal interface IOrderData<TEntity>
    {
        Func<IFilteredEnumerableItem<TEntity>, object> OrderFn { get; }
    }
}