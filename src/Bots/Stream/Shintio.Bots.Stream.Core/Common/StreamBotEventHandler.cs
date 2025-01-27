using Shintio.Bots.Stream.Core.Interfaces;

namespace Shintio.Bots.Stream.Core.Common;

public delegate void StreamBotEventHandler<in TArgs>(IStreamBot bot, TArgs args);