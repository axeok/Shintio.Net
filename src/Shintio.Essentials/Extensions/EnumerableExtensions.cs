using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shintio.Essentials.Extensions
{
	public static class EnumerableExtensions
	{
#if NETCOREAPP3_0_OR_GREATER
		public static async Task WriteTo(this IAsyncEnumerable<string> source, Action<string> action)
		{
			await foreach (var item in source)
			{
				action(item);
			}
		}
#endif

		public static IEnumerable<T> Exclude<T>(this IEnumerable<T> enumerable, params T[] items)
		{
			return enumerable.Except(items);
		}

		public static IEnumerable<T> Without<T>(this IEnumerable<T> enumerable, params T[] items)
		{
			return Exclude(enumerable, items);
		}

		public static IEnumerable<T> With<T>(this IEnumerable<T> enumerable, params T[] items)
		{
			return enumerable.Concat(items);
		}

		public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> enumerable)
		{
			return enumerable.SelectMany(i => i);
		}
	}
}