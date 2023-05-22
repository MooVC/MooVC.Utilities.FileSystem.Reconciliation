namespace MooVC.Utilities.FileSystem.Reconciliation;

using Microsoft.Extensions.Logging;

internal static partial class ILoggerExtensions
{
    public const int ExitStateFail = -1;

    public static int Fail(this ILogger logger, Exception cause)
    {
        logger.LogCritical(cause, "A critical failure has prevented successful execution of the requested operation.");

        return ExitStateFail;
    }
}