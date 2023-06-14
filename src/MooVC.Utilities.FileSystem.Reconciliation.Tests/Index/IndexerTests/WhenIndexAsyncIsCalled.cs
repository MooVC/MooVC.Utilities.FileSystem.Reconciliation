namespace MooVC.Utilities.FileSystem.Reconciliation.Tests.Index;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using MooVC.Utilities.FileSystem.Reconciliation;
using MooVC.Utilities.FileSystem.Reconciliation.Cryptography;
using MooVC.Utilities.FileSystem.Reconciliation.Index;
using MooVC.Utilities.FileSystem.Reconciliation.Persistence;
using Moq;
using Xunit;

public sealed class WhenIndexAsyncIsCalled
{
    private const string Directory = @"C:\Files\";
    private const string Fullname = $"{Name}.{Type}";
    private const string Location = $"{Directory}{Fullname}";
    private const string Name = "TestFile";
    private const string SearchPattern = $"*.{Type}";
    private const string Type = "txt";

    private readonly Mock<IFileSystem> fileSystem;
    private readonly Mock<IHashingService> hashingService;
    private readonly Mock<ILogger<Indexer>> logger;
    private readonly Indexer indexer;

    public WhenIndexAsyncIsCalled()
    {
        fileSystem = new Mock<IFileSystem>();
        hashingService = new Mock<IHashingService>();
        logger = new Mock<ILogger<Indexer>>();
        indexer = new Indexer(fileSystem.Object, hashingService.Object, logger.Object);
    }

    [Fact(DisplayName = "Given a valid directory and search pattern, Then an index containing resources is returned.")]
    public async Task GivenAValidDirectoryAndSearchPatternThenAnIndexContainingResourcesIsReturned()
    {
        // Arrange

        var file = new FileInfo(Fullname);
        DirectoryInfo directory = file.Directory!;
        SearchOption searchOption = SearchOption.AllDirectories;

        _ = fileSystem
            .Setup(fs => fs.ListFilesAsync(directory, SearchPattern, searchOption, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { file });

        byte[] expectedHash = RandomNumberGenerator.GetBytes(16);

        _ = hashingService
            .Setup(hs => hs.ComputeHashAsync(file.FullName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedHash);

        // Act

        IEnumerable<Resource> result = await indexer.IndexAsync(directory, SearchPattern, searchOption);

        // Assert

        _ = result.Should().NotBeNull();
        _ = result.Should().HaveCount(1);

        Resource resource = result.First();

        _ = resource.Name.Should().Be(Name);
        _ = resource.Type.Should().Be(Type);
        _ = resource.Path.Should().Be(file.FullName);
    }

    [Fact(DisplayName = "Given a null directory, Then an ArgumentNullException is thrown.")]
    public async Task GivenANullDirectoryThenAnArgumentNullExceptionIsThrown()
    {
        // Arrange

        DirectoryInfo? directory = default;
        SearchOption searchOption = SearchOption.AllDirectories;

        // Act

        Func<Task> act = async () => await indexer.IndexAsync(directory!, SearchPattern, searchOption);

        // Assert

        _ = await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Theory(DisplayName = "Given a valid directory and search pattern, Then the search pattern is propagated to the file system.")]
    [InlineData("*.txt")]
    [InlineData("*.*")]
    [InlineData("*.pdf")]
    [InlineData("")]
    public async Task GivenAValidDirectoryAndASearchPatternThenTheSearchPatternIsPropagatedToTheFileSystem(string searchPattern)
    {
        // Arrange

        var directory = new DirectoryInfo(Directory);
        SearchOption searchOption = SearchOption.AllDirectories;

        _ = fileSystem
            .Setup(fs => fs.ListFilesAsync(directory, searchPattern, searchOption, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<FileInfo>());

        // Act

        _ = await indexer.IndexAsync(directory, searchPattern, searchOption);

        // Assert

        fileSystem.Verify(fs => fs.ListFilesAsync(directory, searchPattern, searchOption, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory(DisplayName = "Given a valid directory and search option, Then the search option is propagated to the file system.")]
    [InlineData(SearchOption.TopDirectoryOnly)]
    [InlineData(SearchOption.AllDirectories)]
    [InlineData((SearchOption)5)]
    public async Task GivenAValidDirectoryAndASearchOptionThenTheSearchOptionIsPropagatedToTheFileSystem(SearchOption searchOption)
    {
        // Arrange

        var directory = new DirectoryInfo(Directory);

        _ = fileSystem
            .Setup(fs => fs.ListFilesAsync(directory, SearchPattern, searchOption, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<FileInfo>());

        // Act

        _ = await indexer.IndexAsync(directory, SearchPattern, searchOption);

        // Assert

        fileSystem.Verify(fs => fs.ListFilesAsync(directory, SearchPattern, searchOption, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Given a valid directory and search pattern, Then the returned resource contains the hash provided by the hashing service.")]
    public async Task GivenAValidDirectoryThenTheResourceContainsTheHashProvidedByHashingServiceAsItsId()
    {
        // Arrange

        var directory = new DirectoryInfo(Directory);
        SearchOption searchOption = SearchOption.AllDirectories;

        var fileInfo = new FileInfo(Location);

        byte[] expectedHash = RandomNumberGenerator.GetBytes(16);

        _ = fileSystem
            .Setup(fs => fs.ListFilesAsync(directory, SearchPattern, searchOption, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { fileInfo });

        _ = hashingService
            .Setup(hs => hs.ComputeHashAsync(fileInfo.FullName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedHash);

        // Act

        IEnumerable<Resource> result = await indexer.IndexAsync(directory, SearchPattern, searchOption);

        // Assert

        _ = result.Should().NotBeNull();
        _ = result.Should().HaveCount(1);
        _ = result.First().Id.ToByteArray().Should().BeEquivalentTo(expectedHash);
    }
}