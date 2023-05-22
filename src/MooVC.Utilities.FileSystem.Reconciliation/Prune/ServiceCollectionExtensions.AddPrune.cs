namespace MooVC.Utilities.FileSystem.Reconciliation.Prune;

using Microsoft.Extensions.DependencyInjection;

internal static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddPrune(this IServiceCollection services)
    {
        return services
            .AddPruner()
            .AddSingleton<IOperation, PruneOperation>();
    }
}