using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using Shintio.Essentials.Attributes;
using Shintio.Essentials.Extensions;
using Shintio.Essentials.Interfaces;

namespace Shintio.Essentials.Common
{
	public abstract class DataCollection : ValueObject, IDataCollection
	{
		private static readonly Dictionary<Type, Dictionary<Type, int>> Ids = new Dictionary<Type, Dictionary<Type, int>>();

		private static readonly Dictionary<Type, Dictionary<string, DataCollection>> AllValues =
			new Dictionary<Type, Dictionary<string, DataCollection>>();

		private static readonly Dictionary<Type, Dictionary<Type, ReadOnlyCollection<FieldInfo>>> AllFields =
			new Dictionary<Type, Dictionary<Type, ReadOnlyCollection<FieldInfo>>>();

		private readonly int _id;
		private readonly Type _mainType;
		private readonly Type _nestedType;

		protected DataCollection()
		{
			_mainType = GetMainType();
			
			var skipFrames = 1;
			MethodBase frameMethod;
			do
			{
				frameMethod = new StackFrame(skipFrames).GetMethod();
				skipFrames++;
			} while (frameMethod.IsConstructor);
			
			_nestedType = frameMethod.DeclaringType!;

			Ids.TryAdd(_mainType, new Dictionary<Type, int>());
			Ids[_mainType].TryAdd(_nestedType, 0);
			_id = Ids[_mainType][_nestedType]++;

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
				AllValues[type] = GetFields(type).Values.Flatten().Select(f => f.GetValue(null)).Cast<DataCollection>().ToDictionary(
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

		protected virtual string GetKeyByField() => $"{GetPrefix(_nestedType)}{GetFields(_mainType)[_nestedType].ElementAt(_id).Name}";
		
		private string GetPrefix(Type nestedType)
		{
			var prefix = "";

			var type = nestedType;
			while (type != null && !typeof(DataCollection).IsAssignableFrom(type))
			{
				prefix = $"{type.Name}_" + prefix;
				type = type.DeclaringType;
			}
			
			return prefix;
		}

		private static Dictionary<Type, ReadOnlyCollection<FieldInfo>> GetFields(Type mainType)
		{
			if (!AllFields.ContainsKey(mainType))
			{
				if (!mainType.IsSubclassOf(typeof(DataCollection)) && mainType != typeof(DataCollection))
				{
					return new Dictionary<Type, ReadOnlyCollection<FieldInfo>>();
				}
				
				var result = new Dictionary<Type, ReadOnlyCollection<FieldInfo>>();
				var types = GetAllTypes(mainType);
				foreach (var type in types)
				{
					result[type] = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
						.Where(f => f.FieldType == mainType)
						.ToList().AsReadOnly();
				}
				
				AllFields[mainType] = result;
			}

			return AllFields[mainType];
		}
		
		private static List<Type> GetAllTypes(Type type)
		{
			return type.GetNestedTypes().SelectMany(GetAllTypes).Prepend(type).ToList();
		}

		private static Type GetMainType(Type type)
		{
			return type.BaseType?.GetCustomAttribute<MainDataCollectionTypeAttribute>() != null
				? type.BaseType
				: type;
		}
		
		public Type GetMainType()
		{
			return GetMainType(GetType());
		}
	}
}