namespace MooVC.Utilities.FileSystem.Reconciliation.Cryptography.HashingServiceTests;

using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MooVC.Utilities.FileSystem.Reconciliation.Persistence;
using Moq;
using Xunit;

public sealed class WhenComputeHashAsyncIsCalled
    : IDisposable
{
    private const string FilePath = "TestFile.txt";
    private readonly HashAlgorithm algorithm;
    private readonly IFileSystem fileSystem;
    private readonly HashingService service;

    public WhenComputeHashAsyncIsCalled()
    {
        algorithm = SHA256.Create();
        fileSystem = new Mock<IFileSystem>().Object;
        service = new HashingService(algorithm, fileSystem);
    }

    public void Dispose()
    {
        algorithm.Dispose();
    }

    [Fact(DisplayName = "Given a valid stream, Then the hash is computed correctly.")]
    public async Task GivenAValidStreamThenTheHashIsComputedCorrectly()
    {
        // Arrange

        byte[] data = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        using var stream = new MemoryStream(data);

        var fileSystem = Mock.Get(this.fileSystem);

        _ = fileSystem
            .Setup(fileSystem => fileSystem.OpenAsync(FilePath, It.IsAny<FileMode>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(stream);

        // Act

        IEnumerable<byte> result = await service.ComputeHashAsync(FilePath);

        // Assert

        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(SHA256.HashData(data));
    }

    [Fact(DisplayName = "Given a cancelled operation, Then a TaskCanceledException is thrown.")]
    public async Task GivenACancelledOperationThenAOperationCanceledExceptionIsThrown()
    {
        // Arrange

        byte[] data = RandomNumberGenerator.GetBytes(16);
        using var stream = new MemoryStream(data);

        var fileSystem = Mock.Get(this.fileSystem);

        _ = fileSystem
            .Setup(fileSystem => fileSystem.OpenAsync(FilePath, It.IsAny<FileMode>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string filePath, FileMode mode, CancellationToken cancellationToken) =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                return stream;
            });

        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        // Act

        Func<Task> act = async () => await service.ComputeHashAsync(FilePath, cancellationTokenSource.Token);

        // Assert

        _ = await Assert.ThrowsAsync<OperationCanceledException>(act);
    }
}