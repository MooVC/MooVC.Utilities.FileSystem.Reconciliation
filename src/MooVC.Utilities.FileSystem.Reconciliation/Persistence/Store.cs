namespace MooVC.Utilities.FileSystem.Reconciliation.Persistence;

using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static MooVC.Ensure;

internal sealed class Store
    : IStore
{
    private readonly JsonSerializerOptions options;

    public Store(JsonSerializerOptions options)
    {
        this.options = IsNotNull(options, message: "The serialization options must be provided");
    }

    public async Task<IEnumerable<T>> GetAsync<T>(string path, CancellationToken cancellationToken = default)
    {
        _ = IsNotNullOrWhiteSpace(path, message: "The path to the file must be provided.");
        _ = Satisfies(path, _ => Path.Exists(path), message: $"File {path} does not exist.");

        using var file = new FileStream(path, FileMode.Open);

        T[]? content = await JsonSerializer
            .DeserializeAsync<T[]>(file, options: options, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (content is null)
        {
            return Enumerable.Empty<T>();
        }

        return content;
    }

    public async Task SaveAsync<T>(IEnumerable<T> items, string path, CancellationToken cancellationToken = default)
    {
        _ = IsNotNullOrWhiteSpace(path, message: "The path to the file must be provided.");
        _ = IsNotNull(items, message: $"The items must be provided.");

        using var file = new FileStream(path, FileMode.OpenOrCreate);

        T[] content = items.ToArray();

        await JsonSerializer
            .SerializeAsync(file, content, options: options, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}