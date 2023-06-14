namespace MooVC.Utilities.FileSystem.Reconciliation.Index.IndexOperationTests;

using System;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using MooVC.Utilities.FileSystem.Reconciliation.Match;
using MooVC.Utilities.FileSystem.Reconciliation.Persistence;
using Moq;
using Xunit;

public sealed class WhenIndexOperationIsConstructed
{
    private readonly Mock<IIndexer> indexer;
    private readonly Mock<ILogger<IndexOperation>> logger;
    private readonly Mock<IStore> store;

    public WhenIndexOperationIsConstructed()
    {
        indexer = new Mock<IIndexer>();
        logger = new Mock<ILogger<IndexOperation>>();
        store = new Mock<IStore>();
    }

    [Fact(DisplayName = "Given valid parameters, Then the construction succeeds.")]
    public void GivenValidParametersThenConstructionSucceeds()
    {
        // Act

        Func<IOperation> construct = () => new IndexOperation(indexer.Object, logger.Object, store.Object);

        // Assert

        _ = construct.Should().NotThrow();
    }

    [Fact(DisplayName = "Given a null indexer, Then an ArgumentNullException is thrown.")]
    public void GivenANullIndexerThenAnArgumentNullExceptionIsThrown()
    {
        // Arrange

        IIndexer? indexer = default;

        // Act

        Func<IOperation> act = () => new IndexOperation(indexer!, logger.Object, store.Object);

        // Assert

        _ = act.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Given a null logger, Then an ArgumentNullException is thrown.")]
    public void GivenANullLoggerThenAnArgumentNullExceptionIsThrown()
    {
        // Arrange

        ILogger<IndexOperation>? logger = default;

        // Act

        Func<IOperation> act = () => new IndexOperation(indexer.Object, logger!, store.Object);

        // Assert

        _ = act.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "Given a null store, Then an ArgumentNullException is thrown.")]
    public void GivenANullStoreThenAnArgumentNullExceptionIsThrown()
    {
        // Arrange

        IStore? store = default;

        // Act

        Func<IOperation> act = () => new IndexOperation(indexer.Object, logger.Object, store!);

        // Assert

        _ = act.Should().Throw<ArgumentNullException>();
    }
}