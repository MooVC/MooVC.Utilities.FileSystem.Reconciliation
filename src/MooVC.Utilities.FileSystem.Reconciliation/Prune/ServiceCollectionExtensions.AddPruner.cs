namespace MooVC.Utilities.FileSystem.Reconciliation.Prune;

using Microsoft.Extensions.DependencyInjection;

internal static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddPruner(this IServiceCollection services)
    {
        return services.AddSingleton<IPrune, Pruner>();
    }
}