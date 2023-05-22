namespace MooVC.Utilities.FileSystem.Reconciliation;

using Microsoft.Extensions.CommandLineUtils;

internal static partial class CommandLineApplicationExtensions
{
    public static CommandLineApplication WithHelp(this CommandLineApplication application)
    {
        _ = application.HelpOption("-h|--help");

        return application;
    }
}