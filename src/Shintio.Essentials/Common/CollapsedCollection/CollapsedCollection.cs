using System;
using System.Collections.Generic;
using System.Linq;
using Shintio.Essentials.Interfaces;
using Shintio.Json.Attributes;

namespace Shintio.Essentials.Common
{
	public class CollapsedCollection<T> : ValueObjectCollection<T>, IReversable<CollapsedCollection<T>>
		where T : ICollapsedCollectionItem
	{
		public CollapsedCollection() : base(new List<T>())
		{
		}
		
		[JsonConstructor]
		public CollapsedCollection(IEnumerable<T> values) : base(values)
		{
		}

		private Dictionary<int, int>? _indexMapping;

		public IReadOnlyDictionary<int, int> IndexMapping => _indexMapping ??= GetIndexMapping();
		[JsonIgnore] public uint TotalCount => (uint)_values.Sum(s => s.Count);
		
		public static implicit operator CollapsedCollection<T>(List<T> list) => new CollapsedCollection<T>(list);

		private Dictionary<int, int> GetIndexMapping()
		{
			var result = new Dictionary<int, int>();

			var theoreticalIndex = 0;
			for (var practicalIndex = 0; practicalIndex < _values.Count; practicalIndex++)
			{
				var record = _values[practicalIndex];
				for (var j = 0; j < record.Count; j++)
				{
					result.Add(theoreticalIndex++, practicalIndex);
				}
			}

			return result;
		}

		public new CollapsedCollection<T> GetReversed()
		{
			return new CollapsedCollection<T>(
				_values
					.Select(x => x is IReversable<T> reversableValue ? reversableValue.GetReversed() : x)
					.Reverse()
			);
		}
	}
}