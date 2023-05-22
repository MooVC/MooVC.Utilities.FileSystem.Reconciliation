namespace MooVC.Utilities.FileSystem.Reconciliation.Index;

using Microsoft.Extensions.DependencyInjection;

internal static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddIndexer(this IServiceCollection services)
    {
        return services.AddSingleton<IIndexer, Indexer>();
    }
}