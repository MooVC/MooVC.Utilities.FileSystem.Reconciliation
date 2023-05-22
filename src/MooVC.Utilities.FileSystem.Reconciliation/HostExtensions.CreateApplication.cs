namespace MooVC.Utilities.FileSystem.Reconciliation;

using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal static partial class HostExtensions
{
    public static CommandLineApplication CreateApplication(this IHost host)
    {
        var application = new CommandLineApplication
        {
            Name = "MooVC File System Reconciliation Utility",
        };

        foreach (IOperation operation in host.Services.GetServices<IOperation>())
        {
            operation.Register(application, CancellationToken.None);
        }

        return application;
    }
}