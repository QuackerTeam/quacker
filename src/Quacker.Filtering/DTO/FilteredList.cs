using Quacker.Filtering.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Quacker.Filtering.DTO
{
    public class FilteredList<TEntity> : IFilteredList<TEntity>, IFilteredList
    {
        private readonly IList<IFilteredEnumerableItem<TEntity>> _source;

        public long Count { get; private set; }

        public List<object> Source
            => _source.Select(i => (object)i.Item).ToList();

        internal FilteredList(IList<IFilteredEnumerableItem<TEntity>> source, long count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            _source = source;
            Count = count;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => _source.GetEnumerator();

        public IEnumerator<IFilteredEnumerableItem<TEntity>> GetEnumerator()
            => _source.GetEnumerator();

        public List<TEntity> GetSourceList()
            => _source.Select(i => i.Item).ToList();

        public IFilteredList<TDestination> Map<TDestination>(Func<TEntity, TDestination> mapFunc)
            => new FilteredList<TDestination>(_source.Select(i => (IFilteredEnumerableItem<TDestination>)new FilteredEnumerableItem<TDestination>(mapFunc(i.Item), i.Relevance)).ToList(), Count);
    }
}