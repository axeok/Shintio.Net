using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shintio.Hosting;

namespace Shintio.Net;

public class TestApp : ShintioApp
{
	public TestApp(Action<HostBuilderContext, IServiceCollection> configureServices) : base(configureServices)
	{
	}

	protected override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
	{
		var configurationRoot = context.Configuration;
	}

	protected override Task PrepareInternalAsync()
	{
		return Task.CompletedTask;
	}

	protected override Task BeforeRun()
	{
		return Task.CompletedTask;
	}
}