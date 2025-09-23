using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace PseudoMediatR.DependencyInjection.Configuration
{
    public sealed class PseudoMediatRDIConfiguration
    {
        private ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient; //default lifetime
        private Assembly Assembly { get; set; } = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly(); //default assembly

        private readonly IServiceCollection services;

        private PseudoMediatRDIConfiguration(IServiceCollection services)
        {
            this.services = services;
        }

        /// <summary>
        /// Creation of instance of PseudoMediatRConfiguration
        /// </summary>
        /// <returns>Instance of PseudoMediatrConfiguration</returns>
        public static PseudoMediatRDIConfiguration CreateConfiguration(IServiceCollection services)
        {
            return new PseudoMediatRDIConfiguration(services);
        }
        /// <summary>
        /// (Optional) Set lifetime for sender and request handlers. Note that you should call it before InjectHandlers() or InjectHandlers() methods. The default lifetime value is Transient
        /// </summary>
        public PseudoMediatRDIConfiguration SetLifetime(ServiceLifetime givenLifetime)
        {
            Lifetime = givenLifetime;
            return this;
        }

        /// <summary>
        /// Injection of sender to DI container. Must be called with InjectHandlers(). Use sender if you need to work with request handers implicitly.
        /// </summary>
        public PseudoMediatRDIConfiguration InjectSender()
        {
            services.Add(new ServiceDescriptor(typeof(ISender), typeof(Sender), Lifetime));
            return this;
        }

        /// <summary>
        /// Injection of request handlers to DI container. Can be called without InjectSender(). Use request handlers if you need to work with request handlers explicitly or via sender.
        /// </summary>
        public PseudoMediatRDIConfiguration InjectHandlers()
        {
            var types = Assembly.GetTypes()
                                .Where(p => p.IsClass || !p.IsAbstract)
                                .SelectMany(impl => impl.GetInterfaces()
                                                    .Where(inter => inter.IsGenericType &&
                                                        (inter.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) ||
                                                        inter.GetGenericTypeDefinition() == typeof(IAsyncRequestHandler<,>) &&
                                                        typeof(IRequest<>)
                                                            .MakeGenericType(inter.GenericTypeArguments[1])
                                                            .IsAssignableFrom(inter.GenericTypeArguments[0])
                                                        )
                                                    )
                                                    .Select(inter => (inter, impl))
                                )
                                .ToArray();

            AddHandlers(types, (interfaceType, classType) => services.Add(new ServiceDescriptor(interfaceType, classType, Lifetime)));

            return this;
        }

        /// <summary>
        /// (Optional) Set assembly where you want to inject the services. Note that you should call it before InjectHandlers() or InjectHandlers() methods.
        /// </summary>
        public PseudoMediatRDIConfiguration SetAssembly(Assembly givenAssembly)
        {
            Assembly = givenAssembly;
            return this;
        }

        private static void AddHandlers(IEnumerable<(Type interfaceAssembly, Type classAssembly)> types, Action<Type, Type> addHandler)
        {
            foreach (var (interfaceType, classType) in types)
            {
                addHandler(interfaceType, classType);
            }
        }

    }
}
