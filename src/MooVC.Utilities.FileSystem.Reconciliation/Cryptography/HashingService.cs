namespace MooVC.Utilities.FileSystem.Reconciliation.Cryptography;

using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using MooVC.Utilities.FileSystem.Reconciliation.Persistence;
using static MooVC.Ensure;

internal sealed class HashingService
    : IHashingService
{
    private const int DefaultBufferSize = 1_048_576;
    private readonly HashAlgorithm algorithm;
    private readonly int bufferSize;
    private readonly IFileSystem fileSystem;

    public HashingService(HashAlgorithm algorithm, IFileSystem fileSystem, int bufferSize = DefaultBufferSize)
    {
        this.algorithm = IsNotNull(algorithm, message: "The hashing algorithm must be provided.");
        this.bufferSize = InRange(bufferSize, message: "The buffer size must be a positive number.", start: 1);
        this.fileSystem = IsNotNull(fileSystem, message: "The file system service must be provided.");
    }

    public async Task<IEnumerable<byte>> ComputeHashAsync(string filePath, CancellationToken cancellationToken = default)
    {
        Stream stream = await fileSystem
            .OpenAsync(filePath, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        using (stream)
        {
            using Stream buffer = new BufferedStream(stream, bufferSize);

            return await algorithm
                .ComputeHashAsync(buffer, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
    }
}