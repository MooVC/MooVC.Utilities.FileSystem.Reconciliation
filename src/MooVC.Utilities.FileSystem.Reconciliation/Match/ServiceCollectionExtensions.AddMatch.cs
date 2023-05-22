namespace MooVC.Utilities.FileSystem.Reconciliation.Match;

using Microsoft.Extensions.DependencyInjection;

internal static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddMatch(this IServiceCollection services)
    {
        return services
            .AddMatcher()
            .AddSingleton<IOperation, MatchOperation>();
    }
}