namespace MooVC.Utilities.FileSystem.Reconciliation;

using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

internal static partial class ServiceCollectionExtensions
{
    public static IServiceCollection Defines<TService>(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        return services.Defines<TService, TService>(lifetime: lifetime);
    }

    public static IServiceCollection Defines<TService, TImplementation>(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        ServiceDescriptor? descriptor = services.SingleOrDefault(descriptor => descriptor.ServiceType == typeof(TService));

        _ = descriptor
            .Should()
            .NotBeNull();

        _ = descriptor!
            .Lifetime
            .Should()
            .Be(lifetime);

        _ = descriptor
            .ImplementationType
            .Should()
            .Be<TImplementation>();

        return services;
    }
}