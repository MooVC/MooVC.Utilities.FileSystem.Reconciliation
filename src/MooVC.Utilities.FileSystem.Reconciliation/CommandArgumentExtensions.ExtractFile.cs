namespace MooVC.Utilities.FileSystem.Reconciliation;

using System;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.CommandLineUtils;
using static MooVC.Ensure;

internal static partial class CommandOptionExtensions
{
    public static FileInfo ExtractFile(
        this CommandArgument argument,
        string @default,
        bool enforce = false,
        [CallerArgumentExpression(nameof(argument))] string? parameterName = default)
    {
        string file = IsNotNullOrWhiteSpace(argument.Value, @default: @default);
        var info = new FileInfo(file);

        if (enforce && !info.Exists)
        {
            throw new ArgumentException($"File {file} does not exist.", parameterName);
        }

        return info;
    }
}