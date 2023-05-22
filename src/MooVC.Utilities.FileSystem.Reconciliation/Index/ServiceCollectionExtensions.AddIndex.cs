namespace MooVC.Utilities.FileSystem.Reconciliation.Index;

using Microsoft.Extensions.DependencyInjection;

internal static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddIndex(this IServiceCollection services)
    {
        return services
            .AddIndexer()
            .AddSingleton<IOperation, IndexOperation>();
    }
}