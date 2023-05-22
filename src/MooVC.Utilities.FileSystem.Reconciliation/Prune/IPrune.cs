namespace MooVC.Utilities.FileSystem.Reconciliation.Prune;

using System.Collections.Generic;
using System.Threading.Tasks;
using MooVC.Utilities.FileSystem.Reconciliation;
using MooVC.Utilities.FileSystem.Reconciliation.Match;

internal interface IPrune
{
    Task<IEnumerable<Resource>> PruneAsync(IEnumerable<Match> matches, CancellationToken cancellationToken = default);

    Task<IEnumerable<Resource>> PruneAsync(IEnumerable<Resource> resources, CancellationToken cancellationToken = default);
}