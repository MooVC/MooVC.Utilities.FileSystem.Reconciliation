namespace MooVC.Utilities.FileSystem.Reconciliation.Match.ServiceCollectionExtensionsTests;

using Microsoft.Extensions.DependencyInjection;
using Xunit;

public sealed class WhenAddMatcherIsCalled
{
    [Fact(DisplayName = "Given an empty services collection, Then the services collection contains the Indexer.")]
    public void GivenAnEmptyServicesCollectionThenTheServicesCollectionContainsTheMatcher()
    {
        // Arrange

        IServiceCollection services = new ServiceCollection();

        // Act

        _ = services.AddMatcher();

        // Assert

        _ = services.Defines<IMatch, Matcher>();
    }
}