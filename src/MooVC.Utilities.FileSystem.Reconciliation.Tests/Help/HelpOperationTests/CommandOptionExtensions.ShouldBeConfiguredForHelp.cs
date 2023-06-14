namespace MooVC.Utilities.FileSystem.Reconciliation.Help.HelpOperationTests;

using FluentAssertions;
using Microsoft.Extensions.CommandLineUtils;

internal static partial class CommandOptionExtensions
{
    public static void ShouldBeConfiguredForHelp(this CommandOption option)
    {
        _ = option.Should().NotBeNull();
        _ = option.ShortName.Should().Be("h");
        _ = option.LongName.Should().Be("help");
        _ = option.Description.Should().Be("Show help information");
    }
}