using System;

#if NETCOREAPP2_2_OR_GREATER
using System.ComponentModel.DataAnnotations;
#endif

namespace Shintio.Essentials.Common
{
	public abstract class Entity : Entity<int>
	{
	}

	public abstract class Entity<TId> where TId : notnull
	{
		public delegate void EntityDelegate<in T>(T entity) where T : Entity<TId>;

		public delegate void EntityDelegate<in TEntity, in TValue>(TEntity entity, TValue value)
			where TEntity : Entity<TId>;

		public delegate void EntityDelegate<in TEntity, in TValue, in TOldValue>(
			TEntity entity,
			TValue value,
			TOldValue oldValue
		)
			where TEntity : Entity<TId>;

		private const string ProxyPrefix = "Castle.Proxies.";

#if NETCOREAPP2_2_OR_GREATER
		[Key]
#endif
		public TId Id { get; set; } = default(TId)!;

		public virtual bool IsTransient()
		{
			return Id.Equals(default(TId));
		}

		public override bool Equals(object? obj)
		{
			if (!(obj is Entity<TId> item))
			{
				return false;
			}

			if (ReferenceEquals(this, item))
			{
				return true;
			}

			if ((
				    GetType() != item.GetType() &&
				    GetUnProxiedType() != item.GetUnProxiedType()
			    ) ||
			    IsTransient() ||
			    item.IsTransient()
			   )
			{
				return false;
			}

			return item.Id.Equals(Id);
		}

		public override int GetHashCode()
		{
			return (GetUnProxiedType().ToString() + Id).GetHashCode();
		}

		public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
		{
			return left?.Equals(right) ?? Equals(right, null);
		}

		public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
		{
			return !(left == right);
		}

		public bool IsProxy() => GetType().ToString().Contains(ProxyPrefix);

		public Type GetUnProxiedType()
		{
			var type = GetType();

			return IsProxy() ? type.BaseType! : type;
		}

		public override string ToString()
		{
			return $"{GetUnProxiedType().Name} {Id}";
		}
	}
}