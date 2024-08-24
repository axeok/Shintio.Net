using LibGit2Sharp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shintio.Git.Services;
using Shintio.Json.Interfaces;
using Shintio.Json.Newtonsoft.Common;
using Shintio.Json.Nodes;
using Shintio.Net;

var app = new TestApp(ConfigureServices);
await app.PrepareAsync();

var logger = app.Host.Services.GetRequiredService<ILogger<GitService>>();
var service = new GitService("test-repo", "https://github.com/SciSharp/LLamaSharp.git",
	message => logger.LogInformation(message));


await service.Initialize();
using var repo = service.GetRepository();


foreach (var commit in repo.Commits)
{
	var parent = commit.Parents.FirstOrDefault();
	foreach (var change in repo.Diff.Compare<Patch>(parent.Tree, commit.Tree))
	{
		Console.WriteLine(change.Patch);
	}

	break;
}


await app.RunAsync();

return;

static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
	// services.AddSingleton<IJson, NewtonsoftJson>();
}