using Shintio.DependencyInjection.Common;

namespace Shintio.DependencyInjection.Extensions
{
	public static class ServiceProviderExtensions
	{
		public static object GetRequiredService(this ServiceProvider provider, Type serviceType)
		{
			var service = provider.GetService(serviceType);
			if (service == null)
			{
				throw new InvalidOperationException($"Unable to resolve service for type '{serviceType.FullName}'.");
			}

			return service;
		}

		public static T GetRequiredService<T>(this ServiceProvider provider) where T : notnull
		{
			return (T)provider.GetRequiredService(typeof(T));
		}
	}
}