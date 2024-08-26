using System;
using System.Collections.Generic;
using System.Linq;
using Shintio.DependencyInjection.Interfaces;

namespace Shintio.DependencyInjection.Common
{
	public sealed class ServiceProvider
	{
		private readonly IServiceCollection _serviceCollection;

		private readonly Dictionary<Type, object> _services;

		public ServiceProvider(IServiceCollection serviceCollection, Dictionary<Type, object> services)
		{
			_serviceCollection = serviceCollection;
			_services = services;
			_services.Add(serviceCollection.GetType(), _serviceCollection);
			_services.Add(typeof(ServiceProvider), this);
		}

		public IEnumerable<Type> GetAllServicesTypes()
		{
			return _serviceCollection.GetAllServices();
		}

		public object? GetService(Type serviceType)
		{
			var implementationType = _serviceCollection.GetService(serviceType);
			if (implementationType == null)
			{
				return null;
			}

			if (_services.ContainsKey(implementationType))
			{
				return _services[implementationType];
			}

			var constructors = implementationType.GetConstructors()
				.OrderByDescending(c => c.GetParameters().Length);

			foreach (var constructor in constructors)
			{
				var services = new List<object>();
				foreach (var parameter in constructor.GetParameters())
				{
					var parameterService = GetService(parameter.ParameterType);
					if (parameterService == null)
					{
						break;
					}

					services.Add(parameterService);
				}

				if (services.Count == constructor.GetParameters().Length)
				{
					return _services[implementationType] = constructor.Invoke(services.ToArray());
				}
			}

			throw new Exception($"No constructor found for {serviceType.Name}");
		}
	}
}