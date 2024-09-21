using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Shintio.Essentials.Interfaces;

namespace Shintio.Essentials.Common
{
	public abstract class DataCollection : ValueObject, IDataCollection
	{
		private static readonly Dictionary<Type, int> Ids = new Dictionary<Type, int>();

		private static readonly Dictionary<Type, Dictionary<string, DataCollection>> AllValues =
			new Dictionary<Type, Dictionary<string, DataCollection>>();

		private static readonly Dictionary<Type, ReadOnlyCollection<FieldInfo>> AllFields =
			new Dictionary<Type, ReadOnlyCollection<FieldInfo>>();

		private readonly int _id;

		protected DataCollection()
		{
			var type = GetType();

			Ids.TryAdd(type, 0);

			_id = Ids[type]++;

			Key = GetKeyByField();
		}

		public string Key { get; }

		public override string ToString() => Key;

		protected override IEnumerable<object> GetEqualityComponents()
		{
			yield return Key;
		}

		#region Map

		public static IEnumerable<KeyValuePair<string, DataCollection>> GetMap(Type type)
		{
			return GetMapInternal(type);
		}

		public static IEnumerable<string> GetKeys(Type type)
		{
			return GetMapInternal(type).Keys;
		}

		public static IEnumerable<DataCollection> GetValues(Type type)
		{
			return GetMapInternal(type).Values;
		}

		public static IEnumerable<KeyValuePair<string, T>> GetMap<T>() where T : DataCollection
			=> GetMapInternal(typeof(T))
				.Where(p => p.Value is T)
				.Select(p => new KeyValuePair<string, T>(p.Key, (p.Value as T)!));

		public static IEnumerable<string> GetKeys<T>() where T : DataCollection
			=> GetKeys(typeof(T));

		public static IEnumerable<T> GetValues<T>() where T : DataCollection
			=> GetValues(typeof(T)).Cast<T>();

		private static Dictionary<string, DataCollection> GetMapInternal(Type type)
		{
			if (!AllValues.ContainsKey(type))
			{
				AllValues[type] = GetFields(type).Select(f => f.GetValue(null)).Cast<DataCollection>().ToDictionary(
					v => v.Key,
					v => v
				);
			}

			return AllValues[type];
		}

		#endregion

		#region Parse

		public static T? TryParse<T>(string key) where T : notnull, DataCollection =>
			GetMapInternal(typeof(T)).TryGetValue(key, out var value) ? value as T : null;

		public static DataCollection? TryParse(Type type, string key) =>
			type.IsSubclassOf(typeof(DataCollection))
				? GetMapInternal(type).TryGetValue(key, out var value) ? value : null
				: null;

		public static T TryParseOrDefault<T>(string key) where T : DataCollection
		{
			var map = GetMapInternal(typeof(T));

			return (T)(map.TryGetValue(key, out var value) ? value : map.FirstOrDefault().Value);
		}

		public static DataCollection TryParseOrDefault(Type type, string key)
		{
			var map = GetMapInternal(type);

			return map.TryGetValue(key, out var value) ? value : map.FirstOrDefault().Value;
		}

		#endregion

		protected virtual string GetKeyByField() => GetFields(GetType()).ElementAt(_id).Name;

		private static IEnumerable<FieldInfo> GetFields(Type type)
		{
			if (!AllFields.ContainsKey(type))
			{
				AllFields[type] = type.IsSubclassOf(typeof(DataCollection)) || type == typeof(DataCollection)
					? type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
						.Where(f => f.FieldType == type)
						.ToList().AsReadOnly()
					: new List<FieldInfo>().AsReadOnly();
			}

			return AllFields[type];
		}
	}
}