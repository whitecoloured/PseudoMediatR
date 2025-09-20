using Microsoft.Extensions.DependencyInjection;
using PseudoMediatR.DependencyInjection.Configuration;

namespace PseudoMediatR.DependencyInjection
{
    public static class PseudoMediatRDependencyInjection
    {
        /// <summary>
        /// Default configuration setup for injecting the services
        /// </summary>
        public static IServiceCollection AddPseudoMediatR(this IServiceCollection services)
        {
            var configuration = PseudoMediatRConfiguration.CreateConfiguration(services);

            configuration.InjectSender();

            return services;
        }
        /// <summary>
        /// Configuration setup for injecting the services
        /// </summary>
        public static IServiceCollection AddPseudoMediatR(this IServiceCollection services, Action<PseudoMediatRConfiguration> setupConfiguration)
        {
            var configuration = PseudoMediatRConfiguration.CreateConfiguration(services);

            setupConfiguration(configuration);

            return services;
        }
    }
}
