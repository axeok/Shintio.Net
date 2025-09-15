using Shintio.Bots.Stream.Core.Interfaces;

namespace Shintio.Bots.Stream.Core.Common;

public delegate Task StreamBotEventHandler<in TArgs>(IStreamBot bot, TArgs args);