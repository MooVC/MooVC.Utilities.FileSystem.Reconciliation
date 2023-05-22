namespace MooVC.Utilities.FileSystem.Reconciliation.Match;

using System.Collections.Generic;
using System.Threading.Tasks;
using MooVC.Utilities.FileSystem.Reconciliation;

internal interface IMatch
{
    Task<IEnumerable<Match>> MatchAsync(
        IEnumerable<Resource> local,
        IEnumerable<Resource> remote,
        CancellationToken cancellationToken = default);
}