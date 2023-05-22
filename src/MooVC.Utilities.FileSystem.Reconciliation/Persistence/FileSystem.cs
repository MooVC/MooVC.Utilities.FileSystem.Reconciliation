namespace MooVC.Utilities.FileSystem.Reconciliation.Persistence;

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

internal sealed class FileSystem
    : IFileSystem
{
    public Task DeleteAsync(string filePath, CancellationToken cancellationToken = default)
    {
        File.Delete(filePath);

        return Task.CompletedTask;
    }

    public Task<IEnumerable<FileInfo>> ListFilesAsync(
        DirectoryInfo directory,
        string searchPattern,
        SearchOption searchOption,
        CancellationToken cancellationToken = default)
    {
        IEnumerable<FileInfo> files = directory.GetFiles(searchPattern, searchOption);

        return Task.FromResult(files);
    }

    public Task<Stream> OpenAsync(string filePath, FileMode mode = FileMode.Open, CancellationToken cancellationToken = default)
    {
        Stream stream = new FileStream(filePath, mode);

        return Task.FromResult(stream);
    }
}