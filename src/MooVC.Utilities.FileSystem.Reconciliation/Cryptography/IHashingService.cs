namespace MooVC.Utilities.FileSystem.Reconciliation.Cryptography;

using System.Collections.Generic;
using System.Threading.Tasks;

internal interface IHashingService
{
    Task<IEnumerable<byte>> ComputeHashAsync(string filePath, CancellationToken cancellationToken = default);
}