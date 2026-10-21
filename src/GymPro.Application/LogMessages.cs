using Microsoft.Extensions.Logging;

namespace GymPro.Application;

/// <summary>
/// Source-generated-style logging helpers. Hand-written rather than using
/// [LoggerMessage] so the analyzer sees a cached delegate and no boxing.
/// </summary>
internal static partial class LogMessages
{
    private static readonly Action<ILogger, int, Exception?> ListedMembersCore =
        LoggerMessage.Define<int>(
            LogLevel.Debug,
            new EventId(1000, nameof(ListedMembers)),
            "Listed {Count} members.");

    public static void ListedMembers(this ILogger logger, int count)
        => ListedMembersCore(logger, count, null);
}
