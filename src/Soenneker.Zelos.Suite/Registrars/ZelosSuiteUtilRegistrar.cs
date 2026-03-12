using Microsoft.Extensions.DependencyInjection;
using Soenneker.Zelos.Container.Util.Registrars;

namespace Soenneker.Zelos.Suite.Registrars;

/// <summary>
/// A singular package for Zelos, the file-based json document database engine
/// </summary>
public static class ZelosSuiteUtilRegistrar
{
    public static IServiceCollection AddZelosSuiteUtilAsSingleton(this IServiceCollection services)
    {
        services.AddZelosContainerUtilAsSingleton();

        return services;
    }
}
