namespace MooVC.Utilities.FileSystem.Reconciliation.Prune.PruneOperationTests;

using System;
using FluentAssertions;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using MooVC.Utilities.FileSystem.Reconciliation.Persistence;
using MooVC.Utilities.FileSystem.Reconciliation.Prune;
using Moq;
using Xunit;

public sealed class WhenRegisterIsCalled
{
    [Fact(DisplayName = "Given an application, Then the command is registered.")]
    public void GivenAnApplicationThenTheCommandIsRegistered()
    {
        // Arrange

        ILogger<PruneOperation> logger = Mock.Of<ILogger<PruneOperation>>();
        IPrune pruner = Mock.Of<IPrune>();
        IStore store = Mock.Of<IStore>();
        var operation = new PruneOperation(logger, pruner, store);
        var application = new CommandLineApplication(throwOnUnexpectedArg: false);

        // Act

        operation.Register(application);

        // Assert

        _ = application.Commands.Should().Contain(cmd => cmd.Name == "prune");
    }

    [Fact(DisplayName = "Given an application, Then the command arguments are configured correctly.")]
    public void GivenAnApplicationThenTheCommandArgumentsAreConfiguredCorrectly()
    {
        // Arrange

        ILogger<PruneOperation> logger = Mock.Of<ILogger<PruneOperation>>();
        IPrune pruner = Mock.Of<IPrune>();
        IStore store = Mock.Of<IStore>();
        var operation = new PruneOperation(logger, pruner, store);
        var application = new CommandLineApplication(throwOnUnexpectedArg: false);

        // Act

        operation.Register(application);

        CommandArgument? local = application
            .Commands
            .Single(cmd => cmd.Name == "prune")
            .Arguments
            .SingleOrDefault(arg => arg.Name == "local");

        CommandArgument? output = application
            .Commands
            .Single(cmd => cmd.Name == "prune")
            .Arguments
            .SingleOrDefault(arg => arg.Name == "output");

        // Assert

        _ = local.Should().NotBeNull();
        _ = output.Should().NotBeNull();
    }

    [Fact(DisplayName = "Given a null application, Then ArgumentNullException is thrown.")]
    public void GivenNullApplicationThenArgumentNullExceptionIsThrown()
    {
        // Arrange

        ILogger<PruneOperation> logger = Mock.Of<ILogger<PruneOperation>>();
        IPrune pruner = Mock.Of<IPrune>();
        IStore store = Mock.Of<IStore>();
        var operation = new PruneOperation(logger, pruner, store);
        CommandLineApplication? application = default;

        // Act

        Action act = () => operation.Register(application!);

        // Assert

        _ = act
            .Should()
            .Throw<ArgumentNullException>()
            .And
            .ParamName
            .Should()
            .Be("application");
    }
}