namespace MooVC.Utilities.FileSystem.Reconciliation.Match;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using MooVC.Utilities.FileSystem.Reconciliation.Persistence;
using static MooVC.Ensure;

internal sealed class MatchOperation
    : IOperation
{
    private const string LocalPath = "./Local.json";
    private const string OutputPath = "./Matches.json";
    private const string RemotePath = "./Remote.json";

    private readonly IMatch matcher;
    private readonly ILogger logger;
    private readonly IStore store;
    private CancellationToken cancellationToken;
    private CommandArgument? localPath;
    private CommandArgument? outputPath;
    private CommandArgument? remotePath;

    public MatchOperation(ILogger<MatchOperation> logger, IMatch matcher, IStore store)
    {
        this.logger = IsNotNull(logger, message: "The logger must be provided.");
        this.matcher = IsNotNull(matcher, message: "The matcher must be provided.");
        this.store = IsNotNull(store, message: "The store must be provided.");
    }

    public void Register(CommandLineApplication application, CancellationToken cancellationToken = default)
    {
        _ = IsNotNull(application, message: "The command line applicaiton must be provided.");

        this.cancellationToken = cancellationToken;

        _ = application.Command("match", Configure);
    }

    private void Configure(CommandLineApplication application)
    {
        localPath = application.Argument("local", $"Path to the local index (Default: {LocalPath}).");
        remotePath = application.Argument("remote", $"Path to the remote index (Default: {RemotePath}).");
        outputPath = application.Argument("output", $"Path to the location to which the match results will be saved (Default: {OutputPath}).");

        _ = application
            .WithHelp()
            .Register(logger, ExecuteAsync);
    }

    private async Task ExecuteAsync()
    {
        if (this.localPath is null || this.remotePath is null || this.outputPath is null)
        {
            throw new InvalidOperationException("The match command has not been configured.");
        }

        FileInfo localPath = this.localPath.ExtractFile(LocalPath, enforce: true);
        FileInfo outputPath = this.outputPath.ExtractFile(OutputPath);
        FileInfo remotePath = this.remotePath.ExtractFile(RemotePath, enforce: true);

        IEnumerable<Resource> local = await store
            .GetAsync<Resource>(localPath.FullName, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        IEnumerable<Resource> remote = await store
            .GetAsync<Resource>(remotePath.FullName, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        IEnumerable<Match> matches = await matcher
            .MatchAsync(local, remote, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        await store
            .SaveAsync(matches, outputPath.FullName, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}