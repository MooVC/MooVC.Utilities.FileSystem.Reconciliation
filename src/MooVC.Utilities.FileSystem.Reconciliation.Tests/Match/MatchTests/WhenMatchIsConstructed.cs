namespace MooVC.Utilities.FileSystem.Reconciliation.Match.MatchTests;

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

public sealed class WhenMatchIsConstructed
{
    [Fact(DisplayName = "Given a local resource and an empty remote list, Then the object is constructed successfully and HasMatches is false.")]
    public void GivenALocalResourceAndAnEmptyRemoteListThenTheObjectIsConstructedSuccessfullyAndHasMatchesIsFalse()
    {
        // Arrange

        var local = new Resource(Guid.NewGuid(), @"C:\", "Local", "txt");
        IEnumerable<Resource> remotes = Enumerable.Empty<Resource>();

        // Act

        var match = new Match(local, remotes);

        // Assert

        _ = match.Local.Should().Be(local);
        _ = match.Remotes.Should().BeEmpty();
        _ = match.HasMatches.Should().BeFalse();
    }

    [Fact(DisplayName = "Given a local resource and a non-empty remote list, Then the object is constructed successfully and HasMatches is true.")]
    public void GivenALocalResourceAndANonEmptyRemoteListThenTheObjectIsConstructedSuccessfullyAndHasMatchesIsTrue()
    {
        // Arrange

        var local = new Resource(Guid.NewGuid(), @"C:\", "Local", "txt");

        var remotes = new List<Resource>
        {
            new Resource(Guid.NewGuid(), @"C:\", "Remote1", "txt"),
            new Resource(Guid.NewGuid(), @"C:\", "Remote2", "txt"),
        };

        // Act

        var match = new Match(local, remotes);

        // Assert

        _ = match.Local.Should().Be(local);
        _ = match.Remotes.Should().BeEquivalentTo(remotes);
        _ = match.HasMatches.Should().BeTrue();
    }
}