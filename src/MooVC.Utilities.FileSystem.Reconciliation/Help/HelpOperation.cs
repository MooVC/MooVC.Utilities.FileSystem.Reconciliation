namespace MooVC.Utilities.FileSystem.Reconciliation.Help;

using System.Threading;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using static MooVC.Ensure;

internal sealed class HelpOperation
    : IOperation
{
    private readonly ILogger logger;

    public HelpOperation(ILogger<HelpOperation> logger)
    {
        this.logger = IsNotNull(logger, message: "The logger must be provided.");
    }

    public void Register(CommandLineApplication application, CancellationToken cancellationToken = default)
    {
        _ = IsNotNull(application, message: "The command line applicaiton must be provided.");

        _ = application
            .WithHelp()
            .Register(logger, () => application.ShowHelp());
    }
}