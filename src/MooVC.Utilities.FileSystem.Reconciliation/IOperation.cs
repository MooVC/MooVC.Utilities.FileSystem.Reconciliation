namespace MooVC.Utilities.FileSystem.Reconciliation;

using Microsoft.Extensions.CommandLineUtils;

internal interface IOperation
{
    void Register(CommandLineApplication application, CancellationToken cancellationToken = default);
}