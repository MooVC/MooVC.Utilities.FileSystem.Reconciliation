namespace MooVC.Utilities.FileSystem.Reconciliation.Prune.PrunerTests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using MooVC.Utilities.FileSystem.Reconciliation.Persistence;
using Moq;
using Xunit;
using Match = MooVC.Utilities.FileSystem.Reconciliation.Match.Match;

public sealed class WhenPruneAsyncIsCalled
{
    [Fact(DisplayName = "Given an empty collection of matches, Then it should return an empty collection.")]
    public async Task GivenAnEmptyCollectionOfMatchesThenAnEmptyCollectionIsReturned()
    {
        // Arrange

        IEnumerable<Match> matches = Enumerable.Empty<Match>();
        CancellationToken cancellationToken = CancellationToken.None;
        ILogger<Pruner> logger = Mock.Of<ILogger<Pruner>>();
        IFileSystem fileSystem = Mock.Of<IFileSystem>();
        var pruner = new Pruner(fileSystem, logger);

        // Act

        IEnumerable<Resource> result = await pruner.PruneAsync(matches, cancellationToken);

        // Assert

        _ = result.Should().BeEmpty();
    }

    [Fact(DisplayName = "Given a collection of matches with no matches, Then it should return an empty collection.")]
    public async Task GivenAnCollectionOfMatchesWhenThereAreNoMatchesThenAnEmptyCollectionIsReturned()
    {
        // Arrange

        IEnumerable<Match> matches = new List<Match>
        {
            new Match(new Resource(Guid.NewGuid(), "path1", "file1", "txt"), Enumerable.Empty<Resource>()),
            new Match(new Resource(Guid.NewGuid(), "path2", "file2", "txt"), Enumerable.Empty<Resource>()),
        };

        CancellationToken cancellationToken = CancellationToken.None;
        ILogger<Pruner> logger = Mock.Of<ILogger<Pruner>>();
        IFileSystem fileSystem = Mock.Of<IFileSystem>();
        var pruner = new Pruner(fileSystem, logger);

        // Act

        IEnumerable<Resource> result = await pruner.PruneAsync(matches, cancellationToken);

        // Assert

        _ = result.Should().BeEmpty();
    }

    [Fact(DisplayName = "Given a collection of matches with matches, Then it should perform pruning and return the targets.")]
    public async Task GivenACollectionOfMatchesWhenMatchesExistThenTheTargetsAreReturned()
    {
        // Arrange

        var match1Id = Guid.NewGuid();
        var match2Id = Guid.NewGuid();

        IEnumerable<Match> matches = new List<Match>
        {
            new Match(
                new Resource(match1Id, "path1", "file1", "txt"),
                new[] { new Resource(Guid.NewGuid(), "path2", "file2", "txt") }),
            new Match(
                new Resource(match2Id, "path3", "file3", "txt"),
                new[]
                {
                    new Resource(match2Id, "path4", "file4", "txt"),
                    new Resource(match2Id, "path5", "file5", "txt"),
                }),
        };

        CancellationToken cancellationToken = CancellationToken.None;

        ILogger<Pruner> logger = Mock.Of<ILogger<Pruner>>();
        IFileSystem fileSystem = Mock.Of<IFileSystem>(fs =>
            fs.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()) == Task.CompletedTask);

        var pruner = new Pruner(fileSystem, logger);

        // Act

        IEnumerable<Resource> result = await pruner.PruneAsync(matches, cancellationToken);

        // Assert

        _ = result.Should().HaveCount(2);
        _ = result.Should().ContainSingle(resource => resource.Id == match1Id && resource.Location == "path1" && resource.Name == "file1" && resource.Type == "txt");
        _ = result.Should().ContainSingle(resource => resource.Id == match2Id && resource.Location == "path3" && resource.Name == "file3" && resource.Type == "txt");
    }

    [Fact(DisplayName = "Given an empty collection of resources, Then it should return an empty collection.")]
    public async Task GivenAnEmptyCollectionOfResourcesThenAnEmptyCollectionIsReturned()
    {
        // Arrange

        IEnumerable<Resource> resources = Enumerable.Empty<Resource>();
        CancellationToken cancellationToken = CancellationToken.None;
        ILogger<Pruner> logger = Mock.Of<ILogger<Pruner>>();
        IFileSystem fileSystem = Mock.Of<IFileSystem>();
        var pruner = new Pruner(fileSystem, logger);

        // Act

        IEnumerable<Resource> result = await pruner.PruneAsync(resources, cancellationToken);

        // Assert

        _ = result.Should().BeEmpty();
    }

    [Fact(DisplayName = "Given a collection of resources with no duplicates, Then it should return an empty collection.")]
    public async Task GivenCollectionOfResourcesWhenNoDuplicatesExistThenAnEmptyCollectionIsReturned()
    {
        // Arrange

        IEnumerable<Resource> resources = new List<Resource>
        {
            new Resource(Guid.NewGuid(), "path1", "file1", "txt"),
            new Resource(Guid.NewGuid(), "path2", "file2", "txt"),
        };

        CancellationToken cancellationToken = CancellationToken.None;
        ILogger<Pruner> logger = Mock.Of<ILogger<Pruner>>();
        IFileSystem fileSystem = Mock.Of<IFileSystem>();
        var pruner = new Pruner(fileSystem, logger);

        // Act

        IEnumerable<Resource> result = await pruner.PruneAsync(resources, cancellationToken);

        // Assert

        _ = result.Should().BeEmpty();
    }

    [Fact(DisplayName = "Given a collection of resources with duplicates, Then it should perform pruning and return the targets.")]
    public async Task GivenCollectionOfResourcesWhenDuplicatesExistThenTheTargetsAreReturned()
    {
        // Arrange

        CancellationToken cancellationToken = CancellationToken.None;
        var duplicateId = Guid.NewGuid();
        IFileSystem fileSystem = Mock.Of<IFileSystem>(fs => fs.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()) == Task.CompletedTask);
        ILogger<Pruner> logger = Mock.Of<ILogger<Pruner>>();
        var pruner = new Pruner(fileSystem, logger);

        IEnumerable<Resource> resources = new List<Resource>
        {
            new Resource(Guid.NewGuid(), "path1", "file1", "txt"),
            new Resource(duplicateId, "path2", "file2", "txt"),
            new Resource(duplicateId, "path3", "file3", "txt"),
            new Resource(Guid.NewGuid(), "path4", "file4", "txt"),
            new Resource(duplicateId, "path5", "file5", "txt"),
        };

        // Act

        IEnumerable<Resource> result = await pruner.PruneAsync(resources, cancellationToken);

        // Assert

        _ = result.Should().HaveCount(2);
        _ = result.Should().ContainSingle(resource => resource.Id == duplicateId && resource.Location == "path3" && resource.Name == "file3" && resource.Type == "txt");
        _ = result.Should().ContainSingle(resource => resource.Id == duplicateId && resource.Location == "path5" && resource.Name == "file5" && resource.Type == "txt");
    }
}