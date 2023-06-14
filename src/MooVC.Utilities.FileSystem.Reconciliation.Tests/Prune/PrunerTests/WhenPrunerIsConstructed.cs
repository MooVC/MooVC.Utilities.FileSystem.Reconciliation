namespace MooVC.Utilities.FileSystem.Reconciliation.Prune.PrunerTests;

using System;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using MooVC.Utilities.FileSystem.Reconciliation.Persistence;
using Moq;
using Xunit;

public sealed class WhenPrunerIsConstructed
{
    [Fact(DisplayName = "Given valid parameters, Then the construction succeeds.")]
    public void GivenValidParametersThenConstructionSucceeds()
    {
        // Arrange

        ILogger<Pruner> logger = Mock.Of<ILogger<Pruner>>();
        IFileSystem fileSystem = Mock.Of<IFileSystem>();

        // Act

        Func<IPrune> act = () => new Pruner(fileSystem, logger);

        // Assert

        _ = act.Should().NotThrow();
    }

    [Fact(DisplayName = "Given a null logger, Then ArgumentNullException is thrown.")]
    public void GivenNullLoggerThenArgumentNullExceptionIsThrown()
    {
        // Arrange

        ILogger<Pruner>? logger = default;
        IFileSystem fileSystem = Mock.Of<IFileSystem>();

        // Act

        Func<IPrune> act = () => new Pruner(fileSystem, logger!);

        // Assert

        _ = act
            .Should()
            .Throw<ArgumentNullException>()
            .And
            .ParamName
            .Should()
            .Be("logger");
    }

    [Fact(DisplayName = "Given a null file system, Then ArgumentNullException is thrown.")]
    public void GivenNullFileSystemThenArgumentNullExceptionIsThrown()
    {
        // Arrange

        ILogger<Pruner> logger = Mock.Of<ILogger<Pruner>>();
        IFileSystem? fileSystem = default;

        // Act

        Func<IPrune> act = () => new Pruner(fileSystem!, logger);

        // Assert

        _ = act
            .Should()
            .Throw<ArgumentNullException>()
            .And
            .ParamName
            .Should()
            .Be("fileSystem");
    }
}