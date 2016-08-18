using Quacker.Common;
using Quacker.Filtering.DTO;
using Quacker.Filtering.Enums;
using Quacker.Filtering.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Quacker.Filtering.Helpers.Extensions
{
    public static class EnumerableExtensions
    {
        private static string PrepareSearchString(string str)
            => string.Concat((Encoding.ASCII.GetString(Encoding.GetEncoding(1251).GetBytes(str?.Trim()?.ToLower())) ?? string.Empty).Where(c => (c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')));

        private static Tuple<Func<IFilteredEnumerableItem<TEntity>, object>, SortOrder> SetSortData<TEntity>(string sortData)
        {
            if (!new char?[] { '+', '-' }.Contains(sortData?[0]) || sortData?.Length < 2)
                throw new ArgumentException($"Argument sortData is invalid: '{sortData}'. It must start with '+' or '-' as ascending or descending and be followed by the field name to order by");
            return new Tuple<Func<IFilteredEnumerableItem<TEntity>, object>, SortOrder>(SetSortBy<TEntity>(sortData.Substring(1)), sortData[0] == '-'
                      ? SortOrder.Descending
                      : SortOrder.Ascending);
        }

        private static Func<IFilteredEnumerableItem<TEntity>, object> SetSortBy<TEntity>(string sortBy)
        {
            var orderByProp = TypeDescriptor.GetProperties(typeof(TEntity)).Find(sortBy ?? "", true);
            Func<IFilteredEnumerableItem<TEntity>, object> orderFn = null;
            if (sortBy.Trim().ToLower() == "relevance")
                orderFn = i => i.Relevance;
            else if (orderByProp != null)
                orderFn = e => orderByProp.GetValue(e.Item);

            if (orderFn == null)
                throw new InvalidOperationException($"Property not found: {sortBy} in entity {typeof(TEntity).FullName}");

            return orderFn;
        }

        public static IFilteredEnumerable<TEntity> Filter<TEntity>(this IEnumerable<TEntity> source,
                                                                   string filterName = null,
                                                                   string search = null,
                                                                   NormalizedDictionary<object> parameters = null,
                                                                   Func<IFilteredEnumerableItem<TEntity>, object> sortByFunc = null,
                                                                   string sortByField = null,
                                                                   SortOrder? sortOrder = null,
                                                                   string sortData = null,
                                                                   int? offset = null,
                                                                   int? limit = null)
        //where TEntity : class
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            filterName = filterName ?? "DefaultFilter";
            parameters = parameters ?? new NormalizedDictionary<object>();

            Func<IFilteredEnumerableItem<TEntity>, object> sortBy = null;
            if (sortData != null)
            {
                if ((sortByField ?? sortByFunc ?? (object)sortOrder) != null)
                    throw new ArgumentException($"The parameters '{nameof(sortByFunc)}', '{nameof(sortByField)}' and '{nameof(sortOrder)}' cannot be used along with the parameter '{nameof(sortData)}'");
                var data = SetSortData<TEntity>(sortData);
                sortBy = data.Item1;
                sortOrder = data.Item2;
            }
            else if (!string.IsNullOrEmpty(sortByField))
            {
                if ((sortByFunc ?? (object)sortData) != null)
                    throw new ArgumentException($"The parameters '{nameof(sortByFunc)}' and '{nameof(sortData)}' cannot be used along with the parameter '{nameof(sortByField)}'");
                sortBy = SetSortBy<TEntity>(sortByField);
            }
            else if (sortByFunc != null)
                sortBy = o => sortByFunc(o.Cast<TEntity>());

            if (sortBy != null && sortOrder == null)
                sortOrder = SortOrder.Ascending;

            try
            {
                var filter = FilteringStorage.GetFilter<TEntity>(filterName);

                source = filter.ExecuteParameters(source, parameters);

                IEnumerable<IFilteredEnumerableItem<TEntity>> tempList;
                if (!string.IsNullOrEmpty(search))
                {
                    var filterFn = new Dictionary<FilterMode, Func<TEntity, IFilteredEnumerableItem<TEntity>>>
                    {
                        [FilterMode.SimpleSearch] = item =>
                        {
                            var fields = filter.GetFilterFields(item);
                            return new FilteredEnumerableItem<TEntity>(item, fields.Sum(field =>
                            {
                                if (field.GetType() == typeof(string))
                                    return PrepareSearchString(field.ToString()) == PrepareSearchString(search) ? 1 : 0;
                                var strs = field as IEnumerable<string>;
                                return strs == null
                                    ? 0 : strs.Select(PrepareSearchString).Contains(PrepareSearchString(search)) ? 1 : 0;
                            }));
                        },
                        [FilterMode.ComplexSearch] = item =>
                        {
                            var fields = filter.GetFilterFields(item);
                            var arrPalavras = search.Split(' ');
                            return new FilteredEnumerableItem<TEntity>(item, fields.Sum(field =>
                            {
                                if (field.GetType() == typeof(string))
                                    return arrPalavras.Sum(p => PrepareSearchString(field.ToString()).Contains(PrepareSearchString(p)) ? 1 : 0);
                                var strs = field as IEnumerable<string>;
                                return strs == null
                                    ? 0
                                    : strs.Select(PrepareSearchString)
                                            .Sum(f => arrPalavras.Sum(p => PrepareSearchString(f).Contains(PrepareSearchString(p)) ? 1 : 0));
                            }));
                        }
                    }[filter.FilterMode];
                    tempList = source.Select(filterFn).Where(i => i.Relevance > 0);
                }
                else
                    tempList = source.Select(item => new FilteredEnumerableItem<TEntity>(item, 1));

                if (sortBy != null)
                    tempList = sortOrder == SortOrder.Ascending
                             ? tempList.OrderBy(sortBy)
                             : tempList.OrderByDescending(sortBy);

                var count = tempList.Count();

                limit = limit ?? 0;
                if (limit >= 0)
                    tempList = tempList.Skip(offset ?? 0).Take(limit.Value);

                return new FilteredEnumerable<TEntity>(tempList, count);
            }
            catch
            {
                throw;
            }
        }
    }
}