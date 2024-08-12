using Shintio.DependencyInjection.Common;

namespace Shintio.DependencyInjection.Interfaces
{
	public interface IServiceCollection
	{
		IServiceCollection AddSingleton<TService, TImplementation>()
			where TService : class where TImplementation : class, TService;

		IServiceCollection AddSingleton(Type serviceType, Type implementationType);

		Type? GetService(Type serviceType);
		IEnumerable<Type> GetAllServices();

		ServiceProvider BuildServiceProvider();
	}
}