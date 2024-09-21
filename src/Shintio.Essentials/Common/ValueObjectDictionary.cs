using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Shintio.Essentials.Common
{
	// TODO: axe json - add converter
	public class ValueObjectDictionary<TKey, TValue> : ValueObject, 
		IReadOnlyDictionary<TKey, TValue>,
		IDictionary<TKey, TValue> // TODO: axe
		// IDictionary - временный фикс, потому что раскрывает методы изменения.
		// А нужен он сейчас, чтобы оно сериализовалось без выкидывания исключения.
	{
		private readonly Dictionary<TKey, TValue> _dictionary;
		private ICollection<TKey> _keys;
		private ICollection<TValue> _values;
		
		public ValueObjectDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
		{
			_dictionary = new Dictionary<TKey, TValue>(collection);
		}

		public ValueObjectDictionary(params KeyValuePair<TKey, TValue>[] collection)
		{
			_dictionary = new Dictionary<TKey, TValue>(collection);
		}

		public ValueObjectDictionary(IEnumerable<(TKey, TValue)> collection)
		{
			_dictionary = collection.ToDictionary(x => x.Item1, x => x.Item2);
		}

		public ValueObjectDictionary(params (TKey, TValue)[] collection)
		{
			_dictionary = collection.ToDictionary(x => x.Item1, x => x.Item2);
		}

		public ValueObjectDictionary()
		{
			_dictionary = new Dictionary<TKey, TValue>();
		}

		protected override IEnumerable<object?> GetEqualityComponents()
		{
			yield return _dictionary.Count;
			foreach (var (key, value) in _dictionary)
			{
				yield return key;
				yield return value;
			}
		}

		#region IReadOnlyDictionary
		
		public void Add(KeyValuePair<TKey, TValue> item)
		{
		}
		
		public void Clear()
		{
		}
		
		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return false;
		}
		
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
		}
		
		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			return false;
		}
		
		public int Count => _dictionary.Count;
		public bool IsReadOnly { get; }
		
		public TValue this[TKey key]
		{
			get => _dictionary[key];
			set { }
		}
		
		public IEnumerable<TKey> Keys => _dictionary.Keys;
		
		ICollection<TValue> IDictionary<TKey, TValue>.Values => _values;
		
		ICollection<TKey> IDictionary<TKey, TValue>.Keys => _keys;
		
		public IEnumerable<TValue> Values => _dictionary.Values;

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return _dictionary.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		
		public void Add(TKey key, TValue value)
		{
		}
		
		public bool ContainsKey(TKey key)
		{
			return _dictionary.ContainsKey(key);
		}
		
		public bool Remove(TKey key)
		{
			return false;
		}
		
		public bool TryGetValue(TKey key, out TValue value)
		{
			return _dictionary.TryGetValue(key, out value);
		}

		#endregion
	}
}