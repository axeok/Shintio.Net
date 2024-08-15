using System.Drawing;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shintio.Database.Comparers;
using Shintio.Database.Converters;
using Shintio.Database.Extensions;
using Shintio.Json.Interfaces;

namespace Shintio.Database.Common;

public abstract class BaseDbContext : DbContext
{
	private readonly HashSet<(Type, object)> _objectToJsonConverters = new();

	protected virtual int StringMaxLength => 255;
	protected virtual string JsonColumnType => "json";
	protected virtual string StringColumnType => "varchar(255)";

	protected readonly IJson Json;

	public BaseDbContext(DbContextOptions options, IJson json) : base(options)
	{
		Json = json;
	}

	protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
	{
		configurationBuilder
			.Properties<string>()
			.HaveMaxLength(StringMaxLength);

		configurationBuilder.AddDefaultConverter<Color, ColorToInt32Converter>();
		configurationBuilder.AddDefaultConverter<DateTime, DateTimeUtcConverter>();
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// SetOwnedTypes(modelBuilder, new[] { typeof(ValueObject) });

		RegisterObjectToJsonConverters(modelBuilder);
	}

	protected void AddObjectToJsonConverter<TObject>(Func<TObject> getDefaultValue)
	{
		_objectToJsonConverters.Add((typeof(TObject), getDefaultValue));
	}

	protected void SetListConversion<TEntity, TProperty>(
		ModelBuilder modelBuilder,
		Expression<Func<TEntity, List<TProperty>>> propertyExpression
	) where TEntity : class
	{
		modelBuilder.Entity<TEntity>()
			.Property(propertyExpression)
			.HasConversion(CreateObjectJsonValueConverter(() => new List<TProperty>()), new ListComparer<TProperty>())
			.HasColumnType(JsonColumnType);
	}

	protected void SetDictionaryConversion<TEntity, TKey, TValue>(
		ModelBuilder modelBuilder,
		Expression<Func<TEntity, Dictionary<TKey, TValue>>> propertyExpression
	) where TEntity : class where TKey : notnull
	{
		modelBuilder.Entity<TEntity>()
			.Property(propertyExpression)
			.HasConversion(CreateObjectJsonValueConverter(() => new Dictionary<TKey, TValue>()),
				new DictionaryComparer<TKey, TValue>())
			.HasColumnType(JsonColumnType);
	}

	private void SetOwnedTypes(ModelBuilder modelBuilder, IEnumerable<Type> ownedTypes)
	{
		var types = ownedTypes.ToArray();

		var entities = modelBuilder.Model.GetEntityTypes()
			.SelectMany(entity => entity.GetNavigations()
					.Where(p => types.Any(t =>
							(p.ClrType.IsSubclassOf(t) || p.ClrType == t)
						// && !(p.ClrType.IsSubclassOf(typeof(DataCollection)) || p.ClrType == typeof(DataCollection))
					)),
				(e, p) => (e.Name, p.ClrType, p.Name)
			).ToList();

		foreach (var (entity, type, name) in entities)
		{
			// TODO: вынести
			// if (type == typeof(Transform) || type == typeof(Vector3))
			// {
			// 	continue;
			// }

			modelBuilder.Owned(type);
		}
	}

	private void RegisterObjectToJsonConverters(ModelBuilder modelBuilder)
	{
		foreach (var (type, getDefaultValue) in _objectToJsonConverters)
		{
			var entities = modelBuilder.Model.GetEntityTypes()
				.SelectMany(entity => entity.GetNavigations()
						.Where(p => p.ClrType == type),
					(e, p) => (e.Name, p.Name)
				).ToList();

			if (entities.Count <= 0)
			{
				continue;
			}

			var converter = CreateObjectJsonValueConverter(type, getDefaultValue);

			foreach (var (entity, name) in entities)
			{
				modelBuilder.Entity(entity)
					.Property(name)
					.HasConversion(converter);
			}
		}
	}

	private ValueConverterFactory CreateObjectJsonValueConverterFactory(Type type, object getDefaultValue)
	{
		return (ValueConverterFactory)Activator
			.CreateInstance(
				typeof(ObjectToJsonConverterFactory<>).MakeGenericType(type),
				[Json, getDefaultValue]
			)!;
	}

	private ValueConverter CreateObjectJsonValueConverter(Type type, object getDefaultValue)
	{
		return CreateObjectJsonValueConverterFactory(type, getDefaultValue).Create();
	}

	private ValueConverter CreateObjectJsonValueConverter<TObject>(Func<TObject> getDefaultValue)
	{
		return CreateObjectJsonValueConverterFactory(typeof(TObject), getDefaultValue).Create();
	}
}