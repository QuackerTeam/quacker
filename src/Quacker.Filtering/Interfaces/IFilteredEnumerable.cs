using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quacker.Filtering.Interfaces
{
    public interface IFilteredEnumerable<TEntity> : IEnumerable<IFilteredEnumerableItem<TEntity>>
    {
        long Count { get; }

        IEnumerable<TEntity> GetSource();
    }
}