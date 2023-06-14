namespace MooVC.Utilities.FileSystem.Reconciliation.Prune;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MooVC.Linq;
using MooVC.Utilities.FileSystem.Reconciliation.Match;
using MooVC.Utilities.FileSystem.Reconciliation.Persistence;
using static MooVC.Ensure;

internal sealed class Pruner
    : IPrune
{
    private readonly ILogger<Pruner> logger;
    private readonly IFileSystem fileSystem;

    public Pruner(IFileSystem fileSystem, ILogger<Pruner> logger)
    {
        this.logger = IsNotNull(logger, message: "The logger must be provided.");
        this.fileSystem = IsNotNull(fileSystem, message: "The file system service must be provided.");
    }

    public async Task<IEnumerable<Resource>> PruneAsync(IEnumerable<Match> matches, CancellationToken cancellationToken = default)
    {
        if (matches.IsEmpty())
        {
            return Enumerable.Empty<Resource>();
        }

        Resource[] targets = matches
            .Where(predicate: match => match.HasMatches)
            .Select(match => match.Local)
            .ToArray();

        await PerformPruneAsync(targets, cancellationToken)
            .ConfigureAwait(false);

        return targets;
    }

    public async Task<IEnumerable<Resource>> PruneAsync(IEnumerable<Resource> resources, CancellationToken cancellationToken = default)
    {
        if (resources.IsEmpty())
        {
            return Enumerable.Empty<Resource>();
        }

        var targets = new List<Resource>();

        foreach (IGrouping<Guid, Resource> duplicates in resources
            .GroupBy(index => index.Id)
            .Where(resource => resource.Count() > 1))
        {
            targets.AddRange(duplicates.Skip(1));
        }

        await PerformPruneAsync(targets, cancellationToken)
            .ConfigureAwait(false);

        return targets.ToArray();
    }

    private async Task PerformPruneAsync(IEnumerable<Resource> resources, CancellationToken cancellationToken)
    {
        ulong missed = 0;

        foreach (Resource resource in resources)
        {
            try
            {
                logger.LogTrace("Deleting {File}.", resource.Path);

                await fileSystem
                    .DeleteAsync(resource.Path, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                logger.LogDebug("Deleted {File}.", resource.Path);
            }
            catch (Exception ex)
            {
                missed++;

                logger.LogError(ex, "{File} could not be deleted due to {Message}.", resource.Path, ex.Message);
            }
        }

        logger.LogInformation("{Count} files pruned with {Missed} skipped due to failure.", resources.Count(), missed);
    }
}