using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Shintio.Json.Attributes;

namespace Shintio.Essentials.Common
{
	public class ValueObjectCollection<TValue> : ValueObject, IEnumerable<TValue>, IReadOnlyCollection<TValue>
	{
		private readonly ReadOnlyCollection<TValue> _values;

		[JsonConstructor]
		public ValueObjectCollection(IEnumerable<TValue> values)
		{
			_values = values.ToList().AsReadOnly();
		}

		public ValueObjectCollection(params TValue[] values)
		{
			_values = values.ToList().AsReadOnly();
		}

		public ValueObjectCollection()
		{
			_values = new List<TValue>().AsReadOnly();
		}

		protected override IEnumerable<object?> GetEqualityComponents()
		{
			yield return _values.Count;

			foreach (var value in _values)
			{
				yield return value;
			}
		}

		public IEnumerator<TValue> GetEnumerator()
		{
			return _values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		
		public TValue this[int index] => _values[index];
		
		public int Count => _values.Count;
	}

	public static class EnumerableExtensions
	{
		public static ValueObjectCollection<TValue> AsValueObjectCollection<TValue>(this IEnumerable<TValue> collection)
		{
			return new ValueObjectCollection<TValue>(collection);
		}

		public static ValueObjectDictionary<TKey, TValue> AsValueObjectDictionary<TKey, TValue>(
			this IEnumerable<(TKey, TValue)> collection
		)
		{
			return new ValueObjectDictionary<TKey, TValue>(collection);
		}

		public static ValueObjectDictionary<TKey, TValue> AsValueObjectDictionary<TKey, TValue>(
			this IEnumerable<KeyValuePair<TKey, TValue>> collection
		)
		{
			return new ValueObjectDictionary<TKey, TValue>(collection);
		}
	}
}