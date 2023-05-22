namespace MooVC.Utilities.FileSystem.Reconciliation.Persistence;

using System.Collections.Generic;
using System.Threading.Tasks;

internal interface IStore
{
    Task<IEnumerable<T>> GetAsync<T>(string path, CancellationToken cancellationToken = default);

    Task SaveAsync<T>(IEnumerable<T> items, string path, CancellationToken cancellationToken = default);
}