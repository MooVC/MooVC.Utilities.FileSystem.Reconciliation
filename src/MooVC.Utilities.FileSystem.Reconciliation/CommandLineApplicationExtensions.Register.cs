namespace MooVC.Utilities.FileSystem.Reconciliation;

using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

internal static partial class CommandLineApplicationExtensions
{
    public static CommandLineApplication Register(this CommandLineApplication application, ILogger logger, Action operation)
    {
        application.OnExecute(() => operation.TryExecute(logger));

        return application;
    }

    public static CommandLineApplication Register(this CommandLineApplication application, ILogger logger, Func<Task> operation)
    {
        application.OnExecute(async () => await operation
            .TryExecuteAsync(logger)
            .ConfigureAwait(false));

        return application;
    }
}