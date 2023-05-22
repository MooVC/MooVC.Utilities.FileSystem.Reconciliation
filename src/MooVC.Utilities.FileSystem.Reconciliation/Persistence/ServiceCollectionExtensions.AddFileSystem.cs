namespace MooVC.Utilities.FileSystem.Reconciliation.Persistence;

using Microsoft.Extensions.DependencyInjection;

internal static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddFileSystem(this IServiceCollection services)
    {
        return services.AddSingleton<IFileSystem, FileSystem>();
    }
}