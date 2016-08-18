using Quacker.Common.Helpers.Extensions;
using Quacker.Filtering.DTO;
using Quacker.Filtering.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Quacker.Filtering.Helpers.Extensions
{
    public static class FilteredEnumerableExtensions
    {
        public static IFilteredList<TEntity> ToList<TEntity>(this IFilteredEnumerable<TEntity> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            return new FilteredList<TEntity>(source.AsEnumerable().ToList(), source.Count);
        }

        public static async Task<IFilteredList<TEntity>> ToListAsync<TEntity>(this IFilteredEnumerable<TEntity> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            return new FilteredList<TEntity>(await source.AsEnumerable().ToListAsync(), source.Count);
        }
    }
}