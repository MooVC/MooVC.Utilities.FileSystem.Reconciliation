namespace MooVC.Utilities.FileSystem.Reconciliation.Match.MatcherTests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MooVC.Utilities.FileSystem.Reconciliation;
using Xunit;

public sealed class WhenMatchAsyncIsCalled
{
    [Fact(DisplayName = "Given local and remote resources with matches, Then matches are returned.")]
    public async Task GivenLocalAndRemoteResourcesWithMatchesThenMatchesAreReturned()
    {
        // Arrange

        var local = new List<Resource>
        {
            new Resource(Guid.NewGuid(), "Location1", "File1", "jpg"),
            new Resource(Guid.NewGuid(), "Location2", "File2", "mp4"),
        };

        var remote = new List<Resource>
        {
            new Resource(local[0].Id, "RemoteLocation1", "RemoteFile1", "jpg"),
            new Resource(local[1].Id, "RemoteLocation2", "RemoteFile2", "mp4"),
        };

        var matcher = new Matcher();

        // Act

        IEnumerable<Match> matches = await matcher.MatchAsync(local, remote);

        // Assert

        _ = matches.Should().NotBeNullOrEmpty().And.HaveCount(local.Count);

        foreach (Match match in matches)
        {
            _ = match.Local.Should().BeOneOf(local);
            _ = match.Remotes.Should().NotBeNullOrEmpty().And.ContainSingle();
            _ = match.HasMatches.Should().BeTrue();
        }
    }

    [Fact(DisplayName = "Given local and remote resources without matches, Then no matches are returned.")]
    public async Task GivenLocalAndRemoteResourcesWithoutMatchesThenNoMatchesAreReturned()
    {
        // Arrange

        var local = new List<Resource>
        {
            new Resource(Guid.NewGuid(), "Location1", "File1", "jpg"),
            new Resource(Guid.NewGuid(), "Location2", "File2", "mp4"),
        };

        var remote = new List<Resource>
        {
            new Resource(Guid.NewGuid(), "RemoteLocation1", "RemoteFile1", "jpg"),
            new Resource(Guid.NewGuid(), "RemoteLocation2", "RemoteFile2", "mp4"),
        };

        var matcher = new Matcher();

        // Act

        IEnumerable<Match> matches = await matcher.MatchAsync(local, remote);

        // Assert

        _ = matches.Should().NotBeNull().And.HaveCount(local.Count);

        foreach (Match match in matches)
        {
            _ = match.Local.Should().BeOneOf(local);
            _ = match.Remotes.Should().BeEmpty();
            _ = match.HasMatches.Should().BeFalse();
        }
    }

    [Fact(DisplayName = "Given empty local resources, Then an empty set of matches is returned.")]
    public async Task GivenEmptyLocalResourcesThenEmptySetOfMatchesIsReturned()
    {
        // Arrange

        IEnumerable<Resource> local = Enumerable.Empty<Resource>();

        var remote = new List<Resource>
        {
            new Resource(Guid.NewGuid(), "RemoteLocation1", "RemoteFile1", "jpg"),
            new Resource(Guid.NewGuid(), "RemoteLocation2", "RemoteFile2", "mp4"),
        };

        var matcher = new Matcher();

        // Act
        IEnumerable<Match> matches = await matcher.MatchAsync(local, remote);

        // Assert

        _ = matches.Should().NotBeNull().And.BeEmpty();
    }

    [Fact(DisplayName = "Given null local resources, Then an empty set of matches is returned.")]
    public async Task GivenNullLocalResourcesThenEmptySetOfMatchesIsReturned()
    {
        // Arrange
        IEnumerable<Resource>? local = default;

        var remote = new List<Resource>
        {
            new Resource(Guid.NewGuid(), "RemoteLocation1", "RemoteFile1", "jpg"),
            new Resource(Guid.NewGuid(), "RemoteLocation2", "RemoteFile2", "mp4"),
        };

        var matcher = new Matcher();

        // Act

        IEnumerable<Match> matches = await matcher.MatchAsync(local!, remote);

        // Assert

        _ = matches.Should().NotBeNull().And.BeEmpty();
    }

    [Fact(DisplayName = "Given empty remote resources, Then no matches are returned.")]
    public async Task GivenEmptyRemoteResourcesThenNoMatchesAreReturned()
    {
        // Arrange

        var local = new List<Resource>
        {
            new Resource(Guid.NewGuid(), "Location1", "File1", "jpg"),
            new Resource(Guid.NewGuid(), "Location2", "File2", "mp4"),
        };

        IEnumerable<Resource> remote = Enumerable.Empty<Resource>();

        var matcher = new Matcher();

        // Act

        IEnumerable<Match> matches = await matcher.MatchAsync(local, remote);

        // Assert

        _ = matches.Should().NotBeNull().And.HaveCount(local.Count);

        foreach (Match match in matches)
        {
            _ = match.Local.Should().BeOneOf(local);
            _ = match.Remotes.Should().BeEmpty();
            _ = match.HasMatches.Should().BeFalse();
        }
    }

    [Fact(DisplayName = "Given null remote resources, Then no matches are returned.")]
    public async Task GivenNullRemoteResourcesThenNoMatchesAreReturned()
    {
        // Arrange

        var local = new List<Resource>
        {
            new Resource(Guid.NewGuid(), "Location1", "File1", "jpg"),
            new Resource(Guid.NewGuid(), "Location2", "File2", "mp4"),
        };

        IEnumerable<Resource>? remote = default;

        var matcher = new Matcher();

        // Act

        IEnumerable<Match> matches = await matcher.MatchAsync(local, remote!);

        // Assert

        _ = matches.Should().NotBeNull().And.HaveCount(local.Count);

        foreach (Match match in matches)
        {
            _ = match.Local.Should().BeOneOf(local);
            _ = match.Remotes.Should().BeEmpty();
            _ = match.HasMatches.Should().BeFalse();
        }
    }
}