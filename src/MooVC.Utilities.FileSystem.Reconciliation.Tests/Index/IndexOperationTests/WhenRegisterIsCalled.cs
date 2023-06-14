namespace MooVC.Utilities.FileSystem.Reconciliation.Index.IndexOperationTests;

using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MooVC.Utilities.FileSystem.Reconciliation.Persistence;
using MooVC.Utilities.FileSystem.Reconciliation.Prune;
using Moq;
using Xunit;

public sealed class WhenRegisterIsCalled
{
    [Fact(DisplayName = "Given a CommandLineApplication, Then the index command is registered.")]
    public void GivenACommandLineApplicationThenTheIndexCommandIsRegistered()
    {
        // Arrange

        var indexer = new Mock<IIndexer>();
        var store = new Mock<IStore>();
        NullLogger<IndexOperation> logger = NullLogger<IndexOperation>.Instance;
        var operation = new IndexOperation(indexer.Object, logger, store.Object);
        var application = new CommandLineApplication();

        // Act

        operation.Register(application);

        // Assert

        _ = application
            .Commands
            .Should()
            .ContainSingle(command => command.Name == "index");
    }

    [Fact(DisplayName = "Given a CommandLineApplication with the index command, Then the command arguments and options are registered.")]
    public void GivenACommandLineApplicationWithTheIndexCommandThenTheCommandArgumentsAndOptionsAreRegistered()
    {
        // Arrange

        var indexer = new Mock<IIndexer>();
        var store = new Mock<IStore>();
        NullLogger<IndexOperation> logger = NullLogger<IndexOperation>.Instance;
        var operation = new IndexOperation(indexer.Object, logger, store.Object);
        var application = new CommandLineApplication();

        // Act

        operation.Register(application);

        CommandLineApplication command = application
            .Commands
            .Single(cmd => cmd.Name == "index");

        // Assert

        _ = command.Arguments.Should().Contain(argument => argument.Name == "source");
        _ = command.Arguments.Should().Contain(argument => argument.Name == "output");
        _ = command.Options.Should().Contain(option => option.Template == "-p|--pattern");
        _ = command.Options.Should().Contain(option => option.Template == "-o|--option");
    }

    [Fact(DisplayName = "Given a null application, Then ArgumentNullException is thrown.")]
    public void GivenNullApplicationThenArgumentNullExceptionIsThrown()
    {
        // Arrange

        var indexer = new Mock<IIndexer>();
        var store = new Mock<IStore>();
        NullLogger<IndexOperation> logger = NullLogger<IndexOperation>.Instance;
        var operation = new IndexOperation(indexer.Object, logger, store.Object);
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