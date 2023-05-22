namespace MooVC.Utilities.FileSystem.Reconciliation.Match.ServiceCollectionExtensionsTests;

using Microsoft.Extensions.DependencyInjection;
using Xunit;

public sealed class WhenAddMatchIsCalled
{
    [Fact(DisplayName = "Given an empty services collection, Then the services collection contains the MatchOperation and the Matcher.")]
    public void GivenAnEmptyServicesCollectionThenTheServicesCollectionContainsTheMatchOperationAndMatcher()
    {
        // Arrange

        IServiceCollection services = new ServiceCollection();

        // Act

        _ = services.AddMatch();

        // Assert

        _ = services.Defines<IMatch, Matcher>();
        _ = services.Defines<IOperation, MatchOperation>();
    }
}