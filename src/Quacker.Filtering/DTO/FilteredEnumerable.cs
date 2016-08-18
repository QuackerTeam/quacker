using Quacker.Filtering.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Quacker.Filtering.DTO
{
    public class FilteredEnumerable<TEntity> : IFilteredEnumerable<TEntity>
    {
        private IEnumerable<IFilteredEnumerableItem<TEntity>> _source;

        public long Count { get; private set; }

        public FilteredEnumerable(IEnumerable<IFilteredEnumerableItem<TEntity>> source, long count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            _source = source;
            Count = count;
        }

        public IEnumerable<TEntity> GetSource()
            => _source.Select(i => i.Item);

        public IEnumerator<IFilteredEnumerableItem<TEntity>> GetEnumerator()
            => _source.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _source.GetEnumerator();
    }
}