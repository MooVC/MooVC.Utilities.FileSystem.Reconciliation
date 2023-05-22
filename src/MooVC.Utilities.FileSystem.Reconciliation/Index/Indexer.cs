namespace MooVC.Utilities.FileSystem.Reconciliation.Index;

using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Logging;
using MooVC.Utilities.FileSystem.Reconciliation;
using MooVC.Utilities.FileSystem.Reconciliation.Cryptography;
using MooVC.Utilities.FileSystem.Reconciliation.Persistence;
using static MooVC.Ensure;

internal sealed class Indexer
    : IIndexer
{
    private readonly IFileSystem fileSystem;
    private readonly IHashingService hashingService;
    private readonly ILogger logger;

    public Indexer(IFileSystem fileSystem, IHashingService hashingService, ILogger<Indexer> logger)
    {
        this.fileSystem = IsNotNull(fileSystem, message: "The file system service must be provided.");
        this.hashingService = IsNotNull(hashingService, message: "The hashing service must be provided.");
        this.logger = IsNotNull(logger, message: "The logger must be provided.");
    }

    public async Task<IEnumerable<Resource>> IndexAsync(
        DirectoryInfo directory,
        string searchPattern,
        SearchOption searchOption,
        CancellationToken cancellationToken = default)
    {
        _ = IsNotNull(directory, message: "The directory in which the files reside must be provided.");

        IEnumerable<FileInfo> files = await fileSystem
            .ListFilesAsync(directory, searchPattern, searchOption, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return await GenerateIndexAsync(files, cancellationToken)
            .ConfigureAwait(false);
    }

    private async Task<IEnumerable<Resource>> GenerateIndexAsync(IEnumerable<FileInfo> files, CancellationToken cancellationToken)
    {
        var indexes = new List<Resource>();
        ulong missed = 0;

        foreach (FileInfo file in files)
        {
            try
            {
                Resource index = await CreateIndexAsync(file, cancellationToken)
                    .ConfigureAwait(false);

                indexes.Add(index);
            }
            catch (Exception ex)
            {
                missed++;

                logger.LogError(ex, "{FullName} could not be indexed due to {Message}.", file.FullName, ex.Message);
            }
        }

        logger.LogInformation("{Count} files processed with {Missed} skipped due to failure.", indexes.Count, missed);

        return indexes.ToArray();
    }

    private async Task<Resource> CreateIndexAsync(FileInfo file, CancellationToken cancellationToken)
    {
        IEnumerable<byte> hash = await hashingService
            .ComputeHashAsync(file.FullName, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        string location = Path.GetDirectoryName(file.FullName)!;
        string name = Path.GetFileNameWithoutExtension(file.Name);
        string type = Path.GetExtension(file.Name);

        return new Resource(new Guid(hash.ToArray()), location, name, type[1..]);
    }
}