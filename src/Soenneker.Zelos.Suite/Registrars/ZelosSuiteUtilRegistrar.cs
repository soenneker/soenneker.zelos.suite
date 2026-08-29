using Microsoft.Extensions.DependencyInjection;
using Soenneker.Zelos.Container.Util.Registrars;

namespace Soenneker.Zelos.Suite.Registrars;

/// <summary>
/// A singular package for Zelos, the file-based json document database engine
/// </summary>
public static class ZelosSuiteUtilRegistrar
{
    /// <summary>
    /// Registers Zelos Suite Util with a singleton lifetime.
    /// </summary>
    /// <param name="services">Service collection that receives the registration.</param>
    /// <returns>The same service collection, so additional registrations can be chained.</returns>
    public static IServiceCollection AddZelosSuiteUtilAsSingleton(this IServiceCollection services)
    {
        services.AddZelosContainerUtilAsSingleton();

        return services;
    }
}
