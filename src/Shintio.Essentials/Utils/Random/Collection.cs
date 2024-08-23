using System;
using System.Collections.Generic;
using System.Linq;

namespace Shintio.Essentials.Utils.Random
{
	public partial class Random
	{
		public char Item(string text)
		{
			return text[Int(text.Length, false)];
		}

		public int Item(int number)
		{
			return Item(number.ToString());
		}

		public T Item<T>() where T : Enum
		{
			var values = (T[])Enum.GetValues(typeof(T));

			return (T)values.GetValue(Int(values.Length, false))!;
		}

		public T Item<T>(List<T> list)
		{
			return list[Int(list.Count, false)];
		}

		public T Item<T>(IEnumerable<T> list)
		{
			return Item(list.ToArray());
		}

		public T Item<T>(T[] array)
		{
			return array[Int(array.Length, false)];
		}

		public TValue Item<TKey, TValue>(Dictionary<TKey, TValue> dictionary) where TKey : notnull
		{
			return dictionary[Item(dictionary.Keys.ToArray())];
		}

		public KeyValuePair<TKey, TValue> ItemWithKey<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
			where TKey : notnull
		{
			return dictionary.ElementAt(Int(dictionary.Count, false));
		}

		public T[] Items<T>(T[] pool, int count, bool canRepeat = true)
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
				var item = Item(pool);

				if (!canRepeat)
				{
					while (items.Contains(item))
					{
						item = Item(pool);
					}
				}

				items[i] = item;
			}

			return items;
		}

		public T[] Items<T>(int count, bool canRepeat = true) where T : Enum
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
				var item = Item<T>();

				if (!canRepeat)
				{
					while (items.Contains(item))
					{
						item = Item<T>();
					}
				}

				items[i] = item;
			}

			return items;
		}
	}
}