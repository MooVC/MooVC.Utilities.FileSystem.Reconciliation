namespace MooVC.Utilities.FileSystem.Reconciliation.Index.ServiceCollectionExtensionsTests;

using Microsoft.Extensions.DependencyInjection;
using Xunit;

public sealed class WhenAddIndexIsCalled
{
    [Fact(DisplayName = "Given an empty services collection, Then the services collection contains the IndexOperation and the Indexer.")]
    public void GivenAnEmptyServicesCollectionThenTheServicesCollectionContainsTheIndexOperationAndIndexer()
    {
        // Arrange

        IServiceCollection services = new ServiceCollection();

        // Act

        _ = services.AddIndex();

        // Assert

        _ = services.Defines<IIndexer, Indexer>();
        _ = services.Defines<IOperation, IndexOperation>();
    }
}