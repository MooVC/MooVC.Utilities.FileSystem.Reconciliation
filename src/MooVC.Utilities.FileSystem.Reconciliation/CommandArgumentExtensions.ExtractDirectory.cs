namespace MooVC.Utilities.FileSystem.Reconciliation;

using System;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.CommandLineUtils;
using static MooVC.Ensure;

internal static partial class CommandArgumentExtensions
{
    public static DirectoryInfo ExtractDirectory(
        this CommandArgument argument,
        string @default,
        bool enforce = false,
        [CallerArgumentExpression(nameof(argument))] string? parameterName = default)
    {
        string directory = IsNotNullOrWhiteSpace(argument.Value, @default: @default);
        var info = new DirectoryInfo(directory);

        if (enforce && !info.Exists)
        {
            throw new ArgumentException($"Directory {directory} does not exist.", parameterName);
        }

        return info;
    }
}