namespace MooVC.Utilities.FileSystem.Reconciliation.Match;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MooVC.Collections.Generic;
using MooVC.Utilities.FileSystem.Reconciliation;

internal sealed class Matcher
    : IMatch
{
    public Task<IEnumerable<Match>> MatchAsync(
        IEnumerable<Resource> local,
        IEnumerable<Resource> remote,
        CancellationToken cancellationToken = default)
    {
        IEnumerable<Match> matches = Match(local, remote);

        return Task.FromResult(matches);
    }

    private static IEnumerable<Match> Match(IEnumerable<Resource> local, IEnumerable<Resource> remote)
    {
        if (local is null || !local.Any())
        {
            return Enumerable.Empty<Match>();
        }

        var index = remote
            .Snapshot()
            .GroupBy(remote => remote.Id)
            .ToDictionary(remote => remote.Key, remote => remote.ToArray());

        return local
            .Select(local => Match(local, index))
            .ToArray();
    }

    private static Match Match(Resource local, Dictionary<Guid, Resource[]> index)
    {
        _ = index.TryGetValue(local.Id, out Resource[]? matches);

        return new Match(local, matches.Snapshot());
    }
}