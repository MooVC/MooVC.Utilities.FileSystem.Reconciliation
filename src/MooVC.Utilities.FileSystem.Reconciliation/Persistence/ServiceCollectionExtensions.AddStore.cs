namespace MooVC.Utilities.FileSystem.Reconciliation.Persistence;

using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

internal static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddStore(this IServiceCollection services)
    {
        return services
            .AddSingleton(JsonSerializerOptions.Default)
            .AddSingleton<IStore, Store>();
    }
}