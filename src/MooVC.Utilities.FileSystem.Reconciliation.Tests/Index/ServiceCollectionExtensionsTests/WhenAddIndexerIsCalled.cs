namespace MooVC.Utilities.FileSystem.Reconciliation.Index.ServiceCollectionExtensionsTests;

using Microsoft.Extensions.DependencyInjection;
using Xunit;

public sealed class WhenAddIndexerIsCalled
{
    [Fact(DisplayName = "Given an empty services collection, Then the services collection contains the Indexer.")]
    public void GivenAnEmptyServicesCollectionThenTheServicesCollectionContainsTheIndexer()
    {
        // Arrange

        IServiceCollection services = new ServiceCollection();

        // Act

        _ = services.AddIndexer();

        // Assert

        _ = services.Defines<IIndexer, Indexer>();
    }
}