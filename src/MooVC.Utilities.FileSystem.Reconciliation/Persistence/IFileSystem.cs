namespace MooVC.Utilities.FileSystem.Reconciliation.Persistence;

using System.Collections.Generic;

internal interface IFileSystem
{
    Task DeleteAsync(string filePath, CancellationToken cancellationToken = default);

    Task<IEnumerable<FileInfo>> ListFilesAsync(
        DirectoryInfo directory,
        string searchPattern,
        SearchOption searchOption,
        CancellationToken cancellationToken = default);

    Task<Stream> OpenAsync(string filePath, FileMode mode = FileMode.Open, CancellationToken cancellationToken = default);
}