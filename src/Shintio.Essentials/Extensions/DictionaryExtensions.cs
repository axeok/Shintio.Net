using System.Collections.Generic;

namespace Shintio.Essentials.Extensions
{
	public static class DictionaryExtensions
	{
		public static Dictionary<T2, T1> Flip<T1, T2>(this Dictionary<T1, T2> dictionary) 
			where T1 : notnull
			where T2 : notnull
		{
			var flipped = new Dictionary<T2, T1>();
			
			foreach (var (key, value) in dictionary)
			{
				flipped.Add(value, key);
			}

			return flipped;
		}

		public static Dictionary<T2, T1> SwapKeyValues<T1, T2>(this Dictionary<T1, T2> dictionary)
			where T1 : notnull
			where T2 : notnull
			=> Flip(dictionary); 
		
		public static void AddRange<TKey, TValue>(
			this Dictionary<TKey, TValue> dictionary,
			Dictionary<TKey, TValue> otherDictionary
		)
		{
			foreach (var item in otherDictionary)
			{
				dictionary.Add(item.Key, item.Value);
			}
		}
	}
}