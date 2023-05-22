namespace MooVC.Utilities.FileSystem.Reconciliation.Index;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using MooVC.Utilities.FileSystem.Reconciliation.Persistence;
using static MooVC.Ensure;

internal sealed class IndexOperation
    : IOperation
{
    private const string IndexPath = "./Index.json";
    private const string SearchPattern = "*.jpg|*.mp4";
    private const string SourcePath = "./";
    private const SearchOption Option = SearchOption.AllDirectories;

    private readonly IIndexer indexer;
    private readonly ILogger logger;
    private readonly IStore store;
    private CancellationToken cancellationToken;
    private CommandArgument? outputPath;
    private CommandOption? searchPattern;
    private CommandOption? searchOption;
    private CommandArgument? sourcePath;

    public IndexOperation(IIndexer indexer, ILogger<IndexOperation> logger, IStore store)
    {
        this.indexer = IsNotNull(indexer, message: "The indexer must be provided.");
        this.logger = IsNotNull(logger, message: "The logger must be provided.");
        this.store = IsNotNull(store, message: "The store must be provided.");
    }

    public void Register(CommandLineApplication application, CancellationToken cancellationToken = default)
    {
        _ = IsNotNull(application, message: "The command line applicaiton must be provided.");

        this.cancellationToken = cancellationToken;

        _ = application.Command("index", Configure);
    }

    private void Configure(CommandLineApplication application)
    {
        sourcePath = application.Argument("source", $"Path to the root folder from which files are to be indexed (Default: {SourcePath}).");
        outputPath = application.Argument("output", $"Path to the location to which the index will be saved (Default: {IndexPath}).");

        searchPattern = application.Option(
            "-p|--pattern",
            $"Pattern to be applied when searching for files (Default: {SearchPattern}).",
            CommandOptionType.SingleValue);

        searchOption = application.Option(
            "-o|--option",
            $"Search option to be applied when searching for files (Default: {Option}).",
            CommandOptionType.SingleValue);

        _ = application
            .WithHelp()
            .Register(logger, ExecuteAsync);
    }

    private async Task ExecuteAsync()
    {
        if (this.sourcePath is null || this.outputPath is null || this.searchPattern is null || this.searchOption is null)
        {
            throw new InvalidOperationException("The index command has not been configured.");
        }

        DirectoryInfo sourcePath = this.sourcePath.ExtractDirectory(SourcePath, enforce: true);
        FileInfo outputPath = this.outputPath.ExtractFile(IndexPath);
        string searchPattern = this.searchPattern.Extract(SearchPattern);
        SearchOption searchOption = this.searchOption.Extract(Option);

        IEnumerable<Resource> index = await indexer
            .IndexAsync(sourcePath, searchPattern, searchOption, cancellationToken)
            .ConfigureAwait(false);

        await store
            .SaveAsync(index, outputPath.FullName, cancellationToken)
            .ConfigureAwait(false);
    }
}