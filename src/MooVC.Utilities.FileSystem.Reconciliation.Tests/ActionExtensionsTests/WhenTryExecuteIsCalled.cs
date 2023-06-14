namespace MooVC.Utilities.FileSystem.Reconciliation.ActionExtensionsTests;

using System;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public sealed class WhenTryExecuteIsCalled
{
    [Fact(DisplayName = "Given an operation, When no exception is thrown, Then it should return the success code.")]
    public void GivenAnOperationWhenNoExceptionIsThrownThenASuccessCodeIsReturned()
    {
        // Arrange

        ILogger logger = Mock.Of<ILogger>();
        Action operation = () => { };

        // Act

        int result = operation.TryExecute(logger);

        // Assert

        _ = result.Should().Be(ILoggerExtensions.ExitStateSuccess);
    }

    [Fact(DisplayName = "Given an operation, When a exception is thrown, Then it should return the failure code.")]
    public void GivenAnOperationWhenAnExceptionThrownThenAFailureCodeIsReturned()
    {
        // Arrange

        ILogger logger = Mock.Of<ILogger>();
        Action operation = () => throw new InvalidOperationException("Test exception");

        // Act

        int result = operation.TryExecute(logger);

        // Assert

        _ = result.Should().Be(ILoggerExtensions.ExitStateFail);
    }
}