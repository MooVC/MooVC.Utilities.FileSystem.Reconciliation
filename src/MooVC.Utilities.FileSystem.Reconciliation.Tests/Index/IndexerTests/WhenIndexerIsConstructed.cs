namespace MooVC.Utilities.FileSystem.Reconciliation.Index.IndexerTests;

using System;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using MooVC.Utilities.FileSystem.Reconciliation.Cryptography;
using MooVC.Utilities.FileSystem.Reconciliation.Persistence;
using Moq;
using Xunit;

public sealed class WhenIndexerIsConstructed
{
    [Fact(DisplayName = "Given valid parameters, Then the construction succeeds.")]
    public void GivenValidParametersThenConstructionSucceeds()
    {
        // Arrange

        var fileSystem = new Mock<IFileSystem>();
        var hashingService = new Mock<IHashingService>();
        NullLogger<Indexer> logger = NullLogger<Indexer>.Instance;

        // Act

        Func<Indexer> construct = () => new Indexer(fileSystem.Object, hashingService.Object, logger);

        // Assert

        _ = construct
            .Should()
            .NotThrow();
    }

    [Fact(DisplayName = "Given a null file system, Then an ArgumentNullException is thrown.")]
    public void GivenANullFileSystemThenAnArgumentNullExceptionIsThrown()
    {
        // Arrange

        var hashingService = new Mock<IHashingService>();
        NullLogger<Indexer> logger = NullLogger<Indexer>.Instance;

        // Act

        Func<Indexer> construct = () => new Indexer(default!, hashingService.Object, logger);

        // Assert

        _ = construct
            .Should()
            .Throw<ArgumentNullException>()
            .And
            .ParamName
            .Should()
            .Be("fileSystem");
    }

    [Fact(DisplayName = "Given a null hashing service, Then an ArgumentNullException is thrown.")]
    public void GivenANullHashingServiceThenAnArgumentNullExceptionIsThrown()
    {
        // Arrange

        var fileSystem = new Mock<IFileSystem>();
        NullLogger<Indexer> logger = NullLogger<Indexer>.Instance;

        // Act

        Func<Indexer> construct = () => new Indexer(fileSystem.Object, default!, logger);

        // Assert

        _ = construct
            .Should()
            .Throw<ArgumentNullException>()
            .And
            .ParamName
            .Should()
            .Be("hashingService");
    }

    [Fact(DisplayName = "Given a null logger, Then an ArgumentNullException is thrown.")]
    public void GivenANullLoggerThenAnArgumentNullExceptionIsThrown()
    {
        // Arrange

        var fileSystem = new Mock<IFileSystem>();
        var hashingService = new Mock<IHashingService>();

        // Act

        Func<Indexer> construct = () => new Indexer(fileSystem.Object, hashingService.Object, default!);

        // Assert

        _ = construct
            .Should()
            .Throw<ArgumentNullException>()
            .And
            .ParamName
            .Should()
            .Be("logger");
    }
}