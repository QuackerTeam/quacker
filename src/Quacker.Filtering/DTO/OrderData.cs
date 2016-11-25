using Quacker.Filtering.Interfaces;
using System;

namespace Quacker.Ordering.DTO
{
    internal abstract class OrderData { }

    internal class OrderData<TEntity> : IOrderData, IOrderData<TEntity>
    {
        public Func<IFilteredEnumerableItem<TEntity>, object> OrderFn
        {
            get; private set;
        }

        public OrderData(Func<IFilteredEnumerableItem<TEntity>, object> orderFn)
        {
            OrderFn = orderFn;
        }
    }
}