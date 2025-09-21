namespace Shintio.Bots.Stream.Core.Common;

public enum ChatterType
{
    /// <summary>The standard user-type representing a standard viewer.</summary>
    Viewer,

    /// <summary>User-type representing viewers with channel-specific moderation powers.</summary>
    Moderator,

    /// <summary>User-type representing the broadcaster of the channel</summary>
    Broadcaster,
}