﻿namespace MooVC.Utilities.FileSystem.Reconciliation.Cryptography;

using System.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection;

internal static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddHashing(this IServiceCollection services)
    {
        return services
            .AddSingleton<HashAlgorithm>(_ => MD5.Create())
            .AddSingleton<IHashingService, HashingService>();
    }
}