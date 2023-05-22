namespace MooVC.Utilities.FileSystem.Reconciliation;

using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

internal static partial class CommandLineApplicationExtensions
{
    public static int TryExecute(this CommandLineApplication application, ILogger logger, params string[] args)
    {
        Action operation = () => application.Execute(args);

        return operation.TryExecute(logger);
    }
}