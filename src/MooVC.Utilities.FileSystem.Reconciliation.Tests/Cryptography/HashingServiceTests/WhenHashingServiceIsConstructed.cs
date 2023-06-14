namespace MooVC.Utilities.FileSystem.Reconciliation.Cryptography.HashingServiceTests;

using System;
using System.Security.Cryptography;
using FluentAssertions;
using MooVC.Utilities.FileSystem.Reconciliation.Persistence;
using Moq;
using Xunit;

public sealed class WhenHashingServiceIsConstructed
    : IDisposable
{
    private readonly HashAlgorithm algorithm;
    private readonly IFileSystem fileSystem;

    public WhenHashingServiceIsConstructed()
    {
        algorithm = SHA256.Create();
        fileSystem = new Mock<IFileSystem>().Object;
    }

    public void Dispose()
    {
        algorithm.Dispose();
    }

    [Fact(DisplayName = "Given a null algorithm, Then an ArgumentNullException is thrown.")]
    public void GivenAllValuesThenAnInstanceIsCreated()
    {
        // Act

        Func<IHashingService> construct = () => new HashingService(algorithm, fileSystem);

        // Assert

        _ = construct.Should().NotThrow();
    }

    [Fact(DisplayName = "Given a null algorithm, Then an ArgumentNullException is thrown.")]
    public void GivenANullAlgorithmThenAnArgumentNullExceptionIsThrown()
    {
        // Arrange

        HashAlgorithm? algorithm = default;

        // Act

        Func<IHashingService> act = () => new HashingService(algorithm!, fileSystem);

        // Assert

        _ = act.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Given a null file system, Then an ArgumentNullException is thrown.")]
    public void GivenANullFileSystemThenAnArgumentNullExceptionIsThrown()
    {
        // Arrange

        IFileSystem? fileSystem = default;

        // Act

        Func<IHashingService> act = () => new HashingService(algorithm, fileSystem!);

        // Assert

        _ = act.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Given an invalid buffer size, Then an ArgumentOutOfRangeException is thrown.")]
    public void GivenAnInvalidBufferSizeThenAnArgumentOutOfRangeExceptionIsThrown()
    {
        // Arrange

        int invalidBufferSize = 0;

        // Act

        Func<IHashingService> act = () => new HashingService(algorithm, fileSystem, invalidBufferSize);

        // Assert

        _ = act.Should().Throw<ArgumentOutOfRangeException>();
    }
}