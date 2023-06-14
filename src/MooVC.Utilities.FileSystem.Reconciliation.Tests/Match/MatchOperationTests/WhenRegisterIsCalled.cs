namespace MooVC.Utilities.FileSystem.Reconciliation.Match.MatchOperationTests;

using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging.Abstractions;
using MooVC.Utilities.FileSystem.Reconciliation.Index;
using MooVC.Utilities.FileSystem.Reconciliation.Persistence;
using Moq;
using Xunit;

public sealed class WhenRegisterIsCalled
{
    [Fact(DisplayName = "Given a CommandLineApplication, Then the match command is registered.")]
    public void GivenACommandLineApplicationThenTheMatchCommandIsRegistered()
    {
        // Arrange

        var matcher = new Mock<IMatch>();
        var store = new Mock<IStore>();
        NullLogger<MatchOperation> logger = NullLogger<MatchOperation>.Instance;
        var operation = new MatchOperation(logger, matcher.Object, store.Object);
        var application = new CommandLineApplication();

        // Act

        operation.Register(application);

        // Assert

        _ = application
            .Commands
            .Should()
            .ContainSingle(command => command.Name == "match");
    }

    [Fact(DisplayName = "Given a CommandLineApplication with the match command, Then the command arguments are registered.")]
    public void GivenACommandLineApplicationWithTheMatchCommandThenTheCommandArgumentsAreRegistered()
    {
        // Arrange

        var matcher = new Mock<IMatch>();
        var store = new Mock<IStore>();
        NullLogger<MatchOperation> logger = NullLogger<MatchOperation>.Instance;
        var operation = new MatchOperation(logger, matcher.Object, store.Object);
        var application = new CommandLineApplication();

        // Act

        operation.Register(application);

        CommandLineApplication command = application
            .Commands
            .Single(cmd => cmd.Name == "match");

        // Assert

        _ = command.Arguments.Should().Contain(argument => argument.Name == "local");
        _ = command.Arguments.Should().Contain(argument => argument.Name == "remote");
        _ = command.Arguments.Should().Contain(argument => argument.Name == "output");
    }

    [Fact(DisplayName = "Given a null application, Then ArgumentNullException is thrown.")]
    public void GivenNullApplicationThenArgumentNullExceptionIsThrown()
    {
        // Arrange

        var matcher = new Mock<IMatch>();
        var store = new Mock<IStore>();
        NullLogger<MatchOperation> logger = NullLogger<MatchOperation>.Instance;
        var operation = new MatchOperation(logger, matcher.Object, store.Object);
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