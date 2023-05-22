namespace MooVC.Utilities.FileSystem.Reconciliation;

using System.Runtime.CompilerServices;
using Microsoft.Extensions.CommandLineUtils;

internal static partial class CommandOptionExtensions
{
    public static string Extract(this CommandOption option, string @default)
    {
        if (option.HasValue())
        {
            return option.Value();
        }

        return @default;
    }

    public static T Extract<T>(this CommandOption option, T @default, [CallerArgumentExpression(nameof(option))] string? parameterName = default)
        where T : struct
    {
        if (option.HasValue())
        {
            string value = option.Value();

            if (!Enum.TryParse<T>(value, out T parsed))
            {
                throw new ArgumentException($"Value {value} is not defined.", parameterName);
            }

            return parsed;
        }

        return @default;
    }
}