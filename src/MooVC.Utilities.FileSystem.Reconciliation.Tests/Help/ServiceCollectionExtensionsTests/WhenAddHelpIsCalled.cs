namespace MooVC.Utilities.FileSystem.Reconciliation.Help.ServiceCollectionExtensionsTests;

using System.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public sealed class WhenAddHelpIsCalled
{
    [Fact(DisplayName = "Given an empty services collection, Then the services collection contains the HashAlgorithm and IHashingService.")]
    public void GivenAnEmptyServicesCollectionThenTheServicesCollectionContainsTheHelpOperation()
    {
        // Arrange

        IServiceCollection services = new ServiceCollection();

        // Act

        _ = services.AddHelp();

        // Assert

        _ = services.Defines<IOperation, HelpOperation>();
    }
}