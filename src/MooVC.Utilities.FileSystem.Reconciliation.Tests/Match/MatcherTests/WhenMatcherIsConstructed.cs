namespace MooVC.Utilities.FileSystem.Reconciliation.Match.MatcherTests;

using System;
using FluentAssertions;
using Xunit;

public sealed class WhenMatcherIsConstructed
{
    [Fact(DisplayName = "Given valid parameters, Then the construction succeeds.")]
    public void GivenValidParametersThenConstructionSucceeds()
    {
        // Arrange & Act

        Func<IMatch> construct = () => new Matcher();

        // Assert

        _ = construct
            .Should()
            .NotThrow();
    }
}