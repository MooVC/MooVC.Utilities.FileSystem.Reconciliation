namespace MooVC.Utilities.FileSystem.Reconciliation.Index;

using System.Collections.Generic;
using MooVC.Utilities.FileSystem.Reconciliation;

internal interface IIndexer
{
    Task<IEnumerable<Resource>> IndexAsync(
        DirectoryInfo directory,
        string searchPattern,
        SearchOption searchOption,
        CancellationToken cancellationToken = default);
}