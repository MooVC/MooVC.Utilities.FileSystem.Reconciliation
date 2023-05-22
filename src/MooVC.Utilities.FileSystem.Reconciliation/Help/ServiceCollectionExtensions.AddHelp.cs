namespace MooVC.Utilities.FileSystem.Reconciliation.Help;

using Microsoft.Extensions.DependencyInjection;

internal static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddHelp(this IServiceCollection services)
    {
        return services.AddSingleton<IOperation, HelpOperation>();
    }
}