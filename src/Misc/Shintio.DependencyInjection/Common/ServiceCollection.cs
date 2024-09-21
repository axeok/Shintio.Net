using System;
using System.Collections.Generic;
using Shintio.DependencyInjection.Interfaces;

namespace Shintio.DependencyInjection.Common
{
	public class ServiceCollection : IServiceCollection
	{
		private readonly Dictionary<Type, Type> _servicesMap = new Dictionary<Type, Type>();
		private readonly Dictionary<Type, object> _implementations = new Dictionary<Type, object>();

		public ServiceCollection()
		{
			AddSingleton<IServiceCollection, ServiceCollection>();
			AddSingleton<ServiceProvider, ServiceProvider>();
		}

		public IServiceCollection AddSingleton<TService, TImplementation>()
			where TService : class
			where TImplementation : class, TService
		{
			_servicesMap[typeof(TService)] = typeof(TImplementation);

			return this;
		}

		public IServiceCollection AddSingleton(Type serviceType, Type implementationType)
		{
			_servicesMap[serviceType] = implementationType;

			return this;
		}

		public IServiceCollection AddSingleton<TService>(TService implementation)
			where TService : class
		{
			var serviceType = typeof(TService);
			
			_servicesMap[serviceType] = implementation.GetType();
			_implementations[serviceType] = implementation;

			return this;
		}

		public IEnumerable<Type> GetAllServices()
		{
			return _servicesMap.Keys;
		}

		public ServiceProvider BuildServiceProvider()
		{
			return new ServiceProvider(this, _implementations);
		}

		public Type? GetService(Type serviceType)
		{
			return _servicesMap.TryGetValue(serviceType, out var implementationType) ? implementationType : null;
		}
	}
}