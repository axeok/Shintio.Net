using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Shintio.Database.Comparers;

public class ArrayComparer<T> : ValueComparer<T[]>
{
	public ArrayComparer() : base(
		(a, b) => a != null && b != null && a.SequenceEqual(b),
		c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
		c => c)
	{
	}
}

public class ListComparer<T> : ValueComparer<List<T>>
{
	public ListComparer() : base(
		(a, b) => a != null && b != null && a.SequenceEqual(b),
		c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
		c => c.ToList())
	{
	}
}

public class ReadonlyCollectionComparer<T> : ValueComparer<ReadOnlyCollection<T>>
{
	public ReadonlyCollectionComparer() : base(
		(a, b) => a != null && b != null && a.SequenceEqual(b),
		c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
		c => c.ToList().AsReadOnly())
	{
	}
}

public class DictionaryComparer<TKey, TValue> : ValueComparer<Dictionary<TKey, TValue>> where TKey : notnull
{
	public DictionaryComparer() : base(
		(a, b) => a != null && b != null && a.SequenceEqual(b),
		c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
		c => c.ToDictionary(x => x.Key, x => x.Value))
	{
	}
}