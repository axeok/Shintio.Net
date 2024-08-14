using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shintio.Net;

var app = new TestApp(ConfigureServices);
await app.PrepareAsync();
await app.RunAsync();

return;

static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
}