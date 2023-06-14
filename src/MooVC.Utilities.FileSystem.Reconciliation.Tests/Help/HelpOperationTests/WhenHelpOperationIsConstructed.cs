namespace MooVC.Utilities.FileSystem.Reconciliation.Help.HelpOperationTests;

using System;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using MooVC.Utilities.FileSystem.Reconciliation.Index;
using MooVC.Utilities.FileSystem.Reconciliation.Persistence;
using Moq;
using Xunit;

public sealed class WhenHelpOperationIsConstructed
{
    private readonly Mock<ILogger<HelpOperation>> logger;

    public WhenHelpOperationIsConstructed()
    {
        logger = new Mock<ILogger<HelpOperation>>();
    }

    [Fact(DisplayName = "Given valid parameters, Then the construction succeeds.")]
    public void GivenValidParametersThenConstructionSucceeds()
    {
        // Act

        Func<IOperation> construct = () => new HelpOperation(logger.Object);

        // Assert

        _ = construct.Should().NotThrow();
    }

    [Fact(DisplayName = "Given a null ILogger, Then an ArgumentNullException is thrown.")]
    public void GivenANullILoggerThenAnArgumentNullExceptionIsThrown()
    {
        // Arrange

        ILogger<HelpOperation>? logger = default;

        // Act

        Action act = () => _ = new HelpOperation(logger!);

        // Assert

        _ = act.Should().Throw<ArgumentNullException>();
    }
}