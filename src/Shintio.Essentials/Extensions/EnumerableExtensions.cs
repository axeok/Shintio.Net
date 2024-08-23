using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shintio.Essentials.Interfaces;

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

		public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> enumerable)
		{
			return enumerable.SelectMany(i => i);
		}

		public static IEnumerable<T> GetShuffled<T>(this IEnumerable<T> enumerable)
		{
			return enumerable.OrderBy(a => Guid.NewGuid());
		}

		public static void Shuffle<T>(this List<T> list)
		{
			var shuffled = list.GetShuffled().ToList();
			list.Clear();
			list.AddRange(shuffled);
		}

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

		public static void Replace<T>(this List<T> enumerable, T oldItem, T newItem)
		{
			var index = enumerable.IndexOf(oldItem);
			enumerable.Remove(oldItem);
			enumerable.Insert(index, newItem);
		}

		public static IEnumerable<KeyValuePair<TKey, TValue>> Exclude<TKey, TValue>(
			this IEnumerable<KeyValuePair<TKey, TValue>> enumerable,
			TKey key
		)
		{
			return enumerable.Where(p => !(p.Key?.Equals(key) ?? key == null));
		}

		public static IEnumerable<KeyValuePair<TKey, TValue>> With<TKey, TValue>(
			this IEnumerable<KeyValuePair<TKey, TValue>> enumerable,
			TKey key,
			TValue value
		)
		{
			return enumerable.With(new KeyValuePair<TKey, TValue>(key, value));
		}

		public static void RemoveMultiple<T>(this List<T> list, params T[] items) where T : class
		{
			foreach (var item in items)
			{
				list.RemoveAll(i => i == item);
			}
		}

		public static T? Random<T>(this List<T> loot) where T : class, IChanceItem
		{
			var count = loot.Count;

			if (count == 0)
			{
				return null;
			}

			var chance = 0f;
			var chances = loot.OrderByDescending(i => i.Chance).Select(item => chance += item.Chance).ToArray();

			var resultChance = Utils.Random.Random.Instance.Float(chance);

			for (var i = 0; i < count; i++)
			{
				if (chances[i] >= resultChance)
				{
					return loot[i];
				}
			}

			return null;
		}

		// Я на 100% не уверен в первом варианте, потому на всякий вот.
		public static T Random2<T>(this List<T> loot) where T : class, IChanceItem
		{
			// https://stackoverflow.com/a/11930875

			var totalWeight = loot.Sum(l => l.Chance);
			var itemWeightIndex = (float)Utils.Random.Random.Instance.NextDouble() * totalWeight;

			float currentWeightIndex = 0;
			foreach (var item in loot)
			{
				currentWeightIndex += item.Chance;

				if (currentWeightIndex > itemWeightIndex)
				{
					return item;
				}
			}

			return default;
		}

		public static List<T> WeightedShuffle<T>(this IEnumerable<T> items) where T : class, IChanceItem
		{
			// https://softwareengineering.stackexchange.com/a/344274

			return items
				.OrderBy(i => Math.Pow(Utils.Random.Random.Instance.NextDouble(), i.Chance))
				.ToList();
		}
	}
}