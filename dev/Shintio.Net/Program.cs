using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shintio.Bots.Telegram.Common;
using Shintio.Bots.Telegram.Configuration;
using Shintio.Bots.Telegram.Extensions;
using Shintio.Bots.Telegram.Services;
using Shintio.Net;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole();

builder.Services.AddTelegramBot<TestBot>();

if (builder.Environment.EnvironmentName is { } environment)
{
    builder.Configuration.AddJsonFile($"appsettings.{environment}.json");
}

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.Configure<TelegramSecrets>(builder.Configuration.GetSection("Telegram"));

using var host = builder.Build();
await host.RunAsync();