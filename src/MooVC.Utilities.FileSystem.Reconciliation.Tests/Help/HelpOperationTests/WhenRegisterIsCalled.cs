namespace MooVC.Utilities.FileSystem.Reconciliation.Help.HelpOperationTests;

using System;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public sealed class WhenRegisterIsCalled
{
    private readonly Mock<ILogger<HelpOperation>> logger;
    private readonly HelpOperation operation;

    public WhenRegisterIsCalled()
    {
        logger = new Mock<ILogger<HelpOperation>>();
        operation = new HelpOperation(logger.Object);
    }

    [Fact(DisplayName = "Given a null CommandLineApplication, Then an ArgumentNullException is thrown.")]
    public void GivenANullCommandLineApplicationThenAnArgumentNullExceptionIsThrown()
    {
        // Arrange

        CommandLineApplication? application = default;
        using var cancellationTokenSource = new CancellationTokenSource();

        // Act

        void Act()
        {
            operation.Register(application!, cancellationTokenSource.Token);
        }

        // Assert

        _ = Assert.Throws<ArgumentNullException>(Act);
    }

    [Fact(DisplayName = "Given a valid CommandLineApplication, Then the help operation is registered correctly.")]
    public void GivenAValidCommandLineApplicationThenTheHelpOperationIsRegisteredCorrectly()
    {
        // Arrange

        using var cancellationTokenSource = new CancellationTokenSource();
        var application = new CommandLineApplication();

        // Act

        operation.Register(application, cancellationTokenSource.Token);

        // Assert

        application
            .OptionHelp
            .ShouldBeConfiguredForHelp();
    }

    [Fact(DisplayName = "Given a valid CommandLineApplication and cancelled CancellationToken, Then the help operation is registered correctly.")]
    public void GivenAValidCommandLineApplicationAndCancelledCancellationTokenThenTheHelpOperationIsRegisteredCorrectly()
    {
        // Arrange

        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();
        var application = new CommandLineApplication();

        // Act

        operation.Register(application, cancellationTokenSource.Token);

        // Assert

        application
            .OptionHelp
            .ShouldBeConfiguredForHelp();
    }
}