using Quacker.Common;
using Quacker.Filtering.Interfaces;
using Quacker.Ordering.DTO;
using System;
using System.Collections.Generic;

namespace Quacker.Filtering.Helpers
{
    internal static class FilteringStorage
    {
        private static readonly Dictionary<string, Dictionary<string, IFilterData>> _filterData = new Dictionary<string, Dictionary<string, IFilterData>>();
        private static readonly Dictionary<string, Dictionary<string, IOrderData>> _orderData = new Dictionary<string, Dictionary<string, IOrderData>>();

        public static IFilterData<TEntity> GetFilter<TEntity>(string filterName)
        {
            var typeFullName = typeof(TEntity).FullName;
            if (string.IsNullOrEmpty(filterName))
                throw new ArgumentNullException(nameof(filterName));
            if (!_filterData.ContainsKey(typeFullName) || !_filterData[typeFullName].ContainsKey(filterName))
                throw new InvalidOperationException($"Filter not found: {filterName} in entity {typeFullName}");
            return (IFilterData<TEntity>)_filterData[typeFullName][filterName];
        }

        public static IOrderData<TEntity> GetOrder<TEntity>(string orderName)
        {
            var typeFullName = typeof(TEntity).FullName;
            if (string.IsNullOrEmpty(orderName))
                throw new ArgumentNullException(nameof(orderName));
            if (!_orderData.ContainsKey(typeFullName) || !_orderData[typeFullName].ContainsKey(orderName))
                throw new InvalidOperationException($"Order not found: {orderName} in entity {typeFullName}");
            return (IOrderData<TEntity>)_orderData[typeFullName][orderName];
        }

        public static void RegisterFilter<TEntity>(string filterName, IFilterData<TEntity> filterData)
        {
            var typeFullName = typeof(TEntity).FullName;
            if (_filterData.ContainsKey(typeFullName) && _filterData[typeFullName].ContainsKey(filterName))
            {
                var msg = filterName == "DefaultFilter"
                        ? $"There is already a default filter created for the instance {typeFullName}. Please name this filter to distinguish it from the others."
                        : $"There is already a filter named {filterName} for the instance {typeFullName}. Please choose another name.";
                throw new InvalidOperationException(msg);
            }
            if (!_filterData.ContainsKey(typeFullName))
                _filterData.Add(typeFullName, new Dictionary<string, IFilterData>());
            _filterData[typeFullName].Add(filterName, (IFilterData)filterData);
        }

        internal static void RegisterOrder<TEntity>(string orderName, Func<IFilteredEnumerableItem<TEntity>, object> orderFn) where TEntity : class
        {
            var typeFullName = typeof(TEntity).FullName;
            if (_orderData.ContainsKey(typeFullName) && _orderData[typeFullName].ContainsKey(orderName))
            {
                var msg = $"There is already a order named {orderName} for the instance {typeFullName}. Please choose another name.";
                throw new InvalidOperationException("");
            }
            if (!_orderData.ContainsKey(typeFullName))
                _orderData.Add(typeFullName, new Dictionary<string, IOrderData>());
            _orderData[typeFullName].Add(orderName, new OrderData<TEntity>(orderFn));
        }
    }
}