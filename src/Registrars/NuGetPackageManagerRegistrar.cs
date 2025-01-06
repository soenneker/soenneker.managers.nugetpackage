using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Managers.NuGetPackage.Abstract;

namespace Soenneker.Managers.NuGetPackage.Registrars;

/// <summary>
/// Handles building, packaging, and publishing .NET projects to NuGet
/// </summary>
public static class NuGetPackageManagerRegistrar
{
    /// <summary>
    /// Adds <see cref="INuGetPackageManager"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddNuGetPackageManagerAsSingleton(this IServiceCollection services)
    {
        services.TryAddSingleton<INuGetPackageManager, NuGetPackageManager>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="INuGetPackageManager"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddNuGetPackageManagerAsScoped(this IServiceCollection services)
    {
        services.TryAddScoped<INuGetPackageManager, NuGetPackageManager>();

        return services;
    }
}
