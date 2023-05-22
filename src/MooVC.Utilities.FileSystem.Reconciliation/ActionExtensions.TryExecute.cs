namespace MooVC.Utilities.FileSystem.Reconciliation;

using Microsoft.Extensions.Logging;

internal static partial class ActionExtensions
{
    public static int TryExecute(this Action operation, ILogger logger)
    {
        try
        {
            operation();
        }
        catch (Exception ex)
        {
            return logger.Fail(ex);
        }

        return logger.Success();
    }
}