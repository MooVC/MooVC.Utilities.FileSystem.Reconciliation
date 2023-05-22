namespace MooVC.Utilities.FileSystem.Reconciliation.Match;

using System.Collections.Generic;
using MooVC.Utilities.FileSystem.Reconciliation;

internal sealed record Match(Resource Local, IEnumerable<Resource> Remotes)
{
    public bool HasMatches => Remotes.Any();
}