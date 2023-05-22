namespace MooVC.Utilities.FileSystem.Reconciliation;

using Microsoft.Extensions.Logging;

internal static partial class FuncExtensions
{
    public static async Task<int> TryExecuteAsync(this Func<Task> operation, ILogger logger)
    {
        try
        {
            await operation()
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            return logger.Fail(ex);
        }

        return logger.Success();
    }
}