namespace MooVC.Utilities.FileSystem.Reconciliation;

using Microsoft.Extensions.Logging;

internal static partial class ILoggerExtensions
{
    public const int ExitStateSuccess = 0;

    public static int Success(this ILogger logger)
    {
        logger.LogTrace("The operation has completed successfully.");

        return ExitStateSuccess;
    }
}