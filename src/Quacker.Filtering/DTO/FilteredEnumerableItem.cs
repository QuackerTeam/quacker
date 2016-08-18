using Quacker.Filtering.Interfaces;
using System;

namespace Quacker.Filtering.DTO
{
    public class FilteredEnumerableItem<TEntity> : IFilteredEnumerableItem<TEntity>
    {
        public TEntity Item { get; private set; }

        public int Relevance { get; private set; }

        public FilteredEnumerableItem(TEntity item, int relevance = 1)
        {
            Item = item;
            Relevance = relevance;
        }

        public IFilteredEnumerableItem<TDestination> Cast<TDestination>()
            => new FilteredEnumerableItem<TDestination>((TDestination)Convert.ChangeType(Item, typeof(TDestination)), Relevance);
    }
}