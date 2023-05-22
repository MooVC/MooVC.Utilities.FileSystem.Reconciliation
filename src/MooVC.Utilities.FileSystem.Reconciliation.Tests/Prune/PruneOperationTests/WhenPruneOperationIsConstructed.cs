namespace MooVC.Utilities.FileSystem.Reconciliation.Prune.PruneOperationTests;

using System;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using MooVC.Utilities.FileSystem.Reconciliation.Persistence;
using MooVC.Utilities.FileSystem.Reconciliation.Prune;
using Moq;
using Xunit;

public sealed class WhenPruneOperationIsConstructed
{
    [Fact(DisplayName = "Given valid parameters, Then the construction succeeds.")]
    public void GivenValidParametersThenConstructionSucceeds()
    {
        // Arrange

        ILogger<PruneOperation> logger = Mock.Of<ILogger<PruneOperation>>();
        IPrune pruner = Mock.Of<IPrune>();
        IStore store = Mock.Of<IStore>();

        // Act

        Func<IOperation> act = () => new PruneOperation(logger, pruner, store);

        // Assert

        _ = act
            .Should()
            .NotThrow();
    }

    [Fact(DisplayName = "Given a null logger, Then ArgumentNullException is thrown.")]
    public void GivenNullLoggerThenArgumentNullExceptionIsThrown()
    {
        // Arrange

        ILogger<PruneOperation>? logger = default;
        IPrune pruner = Mock.Of<IPrune>();
        IStore store = Mock.Of<IStore>();

        // Act

        Action act = () => _ = new PruneOperation(logger!, pruner, store);

        // Assert

        _ = act
            .Should()
            .Throw<ArgumentNullException>()
            .And
            .ParamName
            .Should()
            .Be("logger");
    }

    [Fact(DisplayName = "Given a null pruner, Then ArgumentNullException is thrown.")]
    public void GivenNullPrunerThenArgumentNullExceptionIsThrown()
    {
        // Arrange

        ILogger<PruneOperation> logger = Mock.Of<ILogger<PruneOperation>>();
        IPrune? pruner = default;
        IStore store = Mock.Of<IStore>();

        // Act

        Action act = () => _ = new PruneOperation(logger, pruner!, store);

        // Assert

        _ = act
            .Should()
            .Throw<ArgumentNullException>()
            .And
            .ParamName
            .Should()
            .Be("pruner");
    }

    [Fact(DisplayName = "Given a null store, Then ArgumentNullException is thrown.")]
    public void GivenNullStoreThenArgumentNullExceptionIsThrown()
    {
        // Arrange

        ILogger<PruneOperation> logger = Mock.Of<ILogger<PruneOperation>>();
        IPrune pruner = Mock.Of<IPrune>();
        IStore? store = default;

        // Act

        Action act = () => _ = new PruneOperation(logger, pruner, store!);

        // Assert

        _ = act
            .Should()
            .Throw<ArgumentNullException>()
            .And
            .ParamName
            .Should()
            .Be("store");
    }
}