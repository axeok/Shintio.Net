using System;
using System.Collections.Generic;
using System.Linq;

namespace Shintio.Essentials.Utils.Random
{
	public partial class Random
	{
		public static char Item(string text)
		{
			return text[Random.Int(text.Length, false)];
		}

		public static int Item(int number)
		{
			return Item(number.ToString());
		}

		// ReSharper disable once MethodOverloadWithOptionalParameter
		public static T Item<T>(System.Random? random = null) where T : Enum
		{
			var values = (T[])Enum.GetValues(typeof(T));

			return (T)values.GetValue(Random.Int(values.Length, false, random))!;
		}

		public static T Item<T>(List<T> list)
		{
			return list[Random.Int(list.Count, false)];
		}

		public static T Item<T>(IEnumerable<T> list)
		{
			return Item(list.ToArray());
		}

		public static T Item<T>(T[] array, System.Random? random = null)
		{
			return array[Random.Int(array.Length, false, random)];
		}

		public static TValue Item<TKey, TValue>(Dictionary<TKey, TValue> dictionary) where TKey : notnull
		{
			return dictionary[Item(dictionary.Keys.ToArray())];
		}

		public static KeyValuePair<TKey, TValue> ItemWithKey<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
			where TKey : notnull
		{
			return dictionary.ElementAt(Random.Int(dictionary.Count, false));
		}

		public static T[] Items<T>(T[] pool, int count, bool canRepeat = true, System.Random? random = null)
		{
			if (!canRepeat)
			{
				if (pool.Length == 0)
				{
					return new T[] { };
				}

				if (pool.Length < count)
				{
					count = pool.Length;
				}

				if (pool.Length == count)
				{
					return pool;
				}
			}

			var items = new T[count];

			for (var i = 0; i < count; i++)
			{
				var item = Item(pool, random);

				if (!canRepeat)
				{
					while (items.Contains(item))
					{
						item = Item(pool, random);
					}
				}

				items[i] = item;
			}

			return items;
		}

		public static T[] Items<T>(int count, bool canRepeat = true, System.Random? random = null) where T : Enum
		{
			if (!canRepeat)
			{
				var enumValues = Enum.GetValues(typeof(T));
				if (enumValues.Length < count)
				{
					return new T[] { };
				}

				if (enumValues.Length == count)
				{
					return enumValues.Cast<T>().ToArray();
				}
			}

			var items = new T[count];

			for (var i = 0; i < count; i++)
			{
				var item = Item<T>(random);

				if (!canRepeat)
				{
					while (items.Contains(item))
					{
						item = Item<T>(random);
					}
				}

				items[i] = item;
			}

			return items;
		}
	}
}