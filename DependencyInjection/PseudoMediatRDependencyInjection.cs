using Microsoft.Extensions.DependencyInjection;

namespace PseudoMediatR.DependencyInjection
{
    public static class PseudoMediatRDependencyInjection
    {
        /// <summary>
        /// Default configuration setup for injecting the services
        /// </summary>
        public static IServiceCollection AddPseudoMediatR(this IServiceCollection services)
        {
            var configuration = PseudoMediatRDIConfiguration.CreateConfiguration(services);

            configuration.InjectHandlers().InjectSender();

            return services;
        }
        /// <summary>
        /// Configuration setup for injecting the services
        /// </summary>
        public static IServiceCollection AddPseudoMediatR(this IServiceCollection services, Action<PseudoMediatRDIConfiguration> setupConfiguration)
        {
            var configuration = PseudoMediatRDIConfiguration.CreateConfiguration(services);

            setupConfiguration(configuration);

            return services;
        }
    }
}
