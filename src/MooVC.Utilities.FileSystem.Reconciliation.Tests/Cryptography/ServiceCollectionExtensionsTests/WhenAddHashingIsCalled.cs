namespace MooVC.Utilities.FileSystem.Reconciliation.Cryptography.ServiceCollectionExtensionsTests;

using System.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public sealed class WhenAddHashingIsCalled
{
    [Fact(DisplayName = "Given an empty services collection, Then the services collection contains the HashAlgorithm and HashingService.")]
    public void GivenAnEmptyServicesCollectionThenTheServicesCollectionContainsHashAlgorithmAndHashingService()
    {
        // Arrange

        IServiceCollection services = new ServiceCollection();

        // Act

        _ = services.AddHashing();

        // Assert

        _ = services.Defines<IHashingService, HashingService>();
    }
}