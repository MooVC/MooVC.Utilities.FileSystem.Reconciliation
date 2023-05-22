namespace MooVC.Utilities.FileSystem.Reconciliation.Prune;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using MooVC.Utilities.FileSystem.Reconciliation.Persistence;
using static MooVC.Ensure;

internal sealed class PruneOperation
    : IOperation
{
    private const string IndexPath = "./Index.json";
    private const string OutputPath = "./Pruned.json";

    private readonly IPrune pruner;
    private readonly ILogger logger;
    private readonly IStore store;
    private CancellationToken cancellationToken;
    private CommandArgument? indexPath;
    private CommandArgument? outputPath;

    public PruneOperation(ILogger<PruneOperation> logger, IPrune pruner, IStore store)
    {
        this.logger = IsNotNull(logger, message: "The logger must be provided.");
        this.pruner = IsNotNull(pruner, message: "The pruner must be provided.");
        this.store = IsNotNull(store, message: "The store must be provided.");
    }

    public void Register(CommandLineApplication application, CancellationToken cancellationToken = default)
    {
        _ = IsNotNull(application, message: "The command line applicaiton must be provided.");

        this.cancellationToken = cancellationToken;

        _ = application.Command("prune", Configure);
    }

    private void Configure(CommandLineApplication application)
    {
        indexPath = application.Argument("local", $"Path to the index (Default: {IndexPath}).");
        outputPath = application.Argument("output", $"Path to the location to which the pruned results will be saved (Default: {OutputPath}).");

        _ = application
            .WithHelp()
            .Register(logger, ExecuteAsync);
    }

    private async Task ExecuteAsync()
    {
        if (this.indexPath is null || this.outputPath is null)
        {
            throw new InvalidOperationException("The prune command has not been configured.");
        }

        FileInfo indexPath = this.indexPath.ExtractFile(IndexPath, enforce: true);
        FileInfo outputPath = this.outputPath.ExtractFile(OutputPath);

        IEnumerable<Resource> index = await store
            .GetAsync<Resource>(indexPath.FullName, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        IEnumerable<Resource> pruned = await pruner
            .PruneAsync(index, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        await store
            .SaveAsync(pruned, outputPath.FullName, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}