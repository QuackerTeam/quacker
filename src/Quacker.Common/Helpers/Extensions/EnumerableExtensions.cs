using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quacker.Common.Helpers.Extensions
{
    public static class EnumerableExtensions
    {
        public static async Task<List<T>> ToListAsync<T>(this IEnumerable<T> source)
        {
            var list = new List<T>();
            await source.ForEachAsync(list.Add);
            return list;
        }

        public static Task ForEachAsync<T>(this IEnumerable<T> source, Action<T> body, int maxThreads = 1)
            => Task.WhenAll(
                from partition in Partitioner.Create(source).GetPartitions(maxThreads)
                select Task.Run(() =>
                {
                    using (partition)
                        while (partition.MoveNext())
                            body(partition.Current);
                }));
    }
}