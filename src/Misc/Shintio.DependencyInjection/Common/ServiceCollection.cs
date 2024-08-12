using Shintio.DependencyInjection.Interfaces;

namespace Shintio.DependencyInjection.Common
{
	public class ServiceCollection : IServiceCollection
	{
		private readonly Dictionary<Type, Type> _servicesMap = new Dictionary<Type, Type>();

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

		public IEnumerable<Type> GetAllServices()
		{
			return _servicesMap.Keys;
		}

		public ServiceProvider BuildServiceProvider()
		{
			return new ServiceProvider(this);
		}

		public Type? GetService(Type serviceType)
		{
			return _servicesMap.TryGetValue(serviceType, out var implementationType) ? implementationType : null;
		}
	}
}