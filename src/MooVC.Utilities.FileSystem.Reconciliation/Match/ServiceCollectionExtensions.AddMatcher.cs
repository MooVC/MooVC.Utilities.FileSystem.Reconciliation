namespace MooVC.Utilities.FileSystem.Reconciliation.Match;

using Microsoft.Extensions.DependencyInjection;

internal static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddMatcher(this IServiceCollection services)
    {
        return services.AddSingleton<IMatch, Matcher>();
    }
}