namespace MooVC.Utilities.FileSystem.Reconciliation.Match.MatchOperationTests;

using System;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using MooVC.Utilities.FileSystem.Reconciliation.Persistence;
using Moq;
using Xunit;

public sealed class WhenMatchOperationIsConstructed
{
    [Fact(DisplayName = "Given valid parameters, Then the construction succeeds.")]
    public void GivenValidParametersThenConstructionSucceeds()
    {
        // Arrange

        var matcher = new Mock<IMatch>();
        var store = new Mock<IStore>();
        NullLogger<MatchOperation> logger = NullLogger<MatchOperation>.Instance;

        // Act

        Func<IOperation> construct = () => new MatchOperation(logger, matcher.Object, store.Object);

        // Assert

        _ = construct
            .Should()
            .NotThrow();
    }

    [Fact(DisplayName = "Given a null logger, Then an ArgumentNullException is thrown.")]
    public void GivenANullLoggerThenAnArgumentNullExceptionIsThrown()
    {
        // Arrange

        var matcher = new Mock<IMatch>();
        var store = new Mock<IStore>();

        // Act

        Func<MatchOperation> construct = () => new MatchOperation(null!, matcher.Object, store.Object);

        // Assert

        _ = construct
            .Should()
            .Throw<ArgumentNullException>()
            .And
            .ParamName
            .Should()
            .Be("logger");
    }

    [Fact(DisplayName = "Given a null matcher, Then an ArgumentNullException is thrown.")]
    public void GivenANullMatcherThenAnArgumentNullExceptionIsThrown()
    {
        // Arrange

        var store = new Mock<IStore>();
        NullLogger<MatchOperation> logger = NullLogger<MatchOperation>.Instance;

        // Act

        Func<MatchOperation> construct = () => new MatchOperation(logger, default!, store.Object);

        // Assert

        _ = construct
            .Should()
            .Throw<ArgumentNullException>()
            .And
            .ParamName
            .Should()
            .Be("matcher");
    }

    [Fact(DisplayName = "Given a null store, Then an ArgumentNullException is thrown.")]
    public void GivenANullStoreThenAnArgumentNullExceptionIsThrown()
    {
        // Arrange

        var matcher = new Mock<IMatch>();
        NullLogger<MatchOperation> logger = NullLogger<MatchOperation>.Instance;

        // Act

        Func<MatchOperation> construct = () => new MatchOperation(logger, matcher.Object, default!);

        // Assert

        _ = construct
            .Should()
            .Throw<ArgumentNullException>()
            .And
            .ParamName
            .Should()
            .Be("store");
    }
}