using System.Collections.Generic;

namespace Quacker.Filtering.Interfaces
{
    public interface IFilteredList
    {
        long Count { get; }

        List<object> Source { get; }
    }

    public interface IFilteredList<TEntity> : IEnumerable<IFilteredEnumerableItem<TEntity>>
    {
        List<TEntity> GetSourceList();
    }
}