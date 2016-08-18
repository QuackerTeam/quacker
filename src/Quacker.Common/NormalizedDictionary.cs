using System;
using System.Collections;
using System.Collections.Generic;

namespace Quacker.Common
{
    public class NormalizedDictionary<TValue> : IDictionary<string, TValue>
    {
        private readonly Func<string, string> _prepareKeyFunc = s => s.Trim().ToLower();
        private readonly Dictionary<string, TValue> _data = new Dictionary<string, TValue>();

        public NormalizedDictionary(Dictionary<string, TValue> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            foreach (var kv in source) Add(kv);
        }

        public NormalizedDictionary()
        {
        }

        public TValue this[string key]
        {
            get
            {
                return _data[_prepareKeyFunc(key)];
            }

            set
            {
                _data[_prepareKeyFunc(key)] = value;
            }
        }

        public int Count
            => _data.Count;

        public bool IsReadOnly
            => ((IDictionary<string, TValue>)_data).IsReadOnly;

        public ICollection<string> Keys
            => _data.Keys;

        public ICollection<TValue> Values
            => _data.Values;

        public void Add(KeyValuePair<string, TValue> item)
            => Add(item.Key, item.Value);

        public void Add(string key, TValue value)
            => _data[_prepareKeyFunc(key)] = value;

        public void Clear()
            => _data.Clear();

        public bool Contains(KeyValuePair<string, TValue> item)
            => ContainsKey(item.Key);

        public bool ContainsKey(string key)
            => _data.ContainsKey(_prepareKeyFunc(key));

        public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
            => ((IDictionary<string, TValue>)_data).CopyTo(array, arrayIndex);

        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
            => _data.GetEnumerator();

        public bool Remove(KeyValuePair<string, TValue> item)
            => Remove(item.Key);

        public bool Remove(string key)
            => _data.Remove(_prepareKeyFunc(key));

        public bool TryGetValue(string key, out TValue value)
            => _data.TryGetValue(_prepareKeyFunc(key), out value);

        IEnumerator IEnumerable.GetEnumerator()
            => _data.GetEnumerator();
    }
}