namespace MooVC.Utilities.FileSystem.Reconciliation;

using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MooVC.Utilities.FileSystem.Reconciliation.Cryptography;
using MooVC.Utilities.FileSystem.Reconciliation.Help;
using MooVC.Utilities.FileSystem.Reconciliation.Index;
using MooVC.Utilities.FileSystem.Reconciliation.Match;
using MooVC.Utilities.FileSystem.Reconciliation.Persistence;
using MooVC.Utilities.FileSystem.Reconciliation.Prune;
using Serilog;

public sealed class Program
{
    public static int Main(string[] args)
    {
        using IHost host = CreateHost();

        CommandLineApplication application = host.CreateApplication();
        ILogger<Program> logger = host.Services.GetRequiredService<ILogger<Program>>();

        return application.TryExecute(logger, args);
    }

    private static IHost CreateHost()
    {
        return Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, builder) => builder.AddJsonFile("appsettings.json", optional: true))
            .UseSerilog((context, configuration) => configuration
                .ReadFrom
                .Configuration(context.Configuration))
            .ConfigureServices(services => services
                .AddFileSystem()
                .AddHashing()
                .AddHelp()
                .AddIndex()
                .AddMatch()
                .AddPrune()
                .AddStore())
            .Build();
    }
}