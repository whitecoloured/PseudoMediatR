using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace PseudoMediatR.DependencyInjection.Configuration
{
    public sealed class PseudoMediatRConfiguration
    {
        private ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient; //default lifetime
        private Assembly Assembly { get; set; } = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly(); //default assembly

        private readonly IServiceCollection services;

        private PseudoMediatRConfiguration(IServiceCollection services)
        {
            this.services = services;
        }

        /// <summary>
        /// Creation of instance of PseudoMediatRConfiguration
        /// </summary>
        /// <returns>Instance of PseudoMediatrConfiguration</returns>
        public static PseudoMediatRConfiguration CreateConfiguration(IServiceCollection services)
        {
            return new PseudoMediatRConfiguration(services);
        }
        /// <summary>
        /// (Optional) Set lifetime for sender and request handlers. Note that you should call it before InjectHandlers() or InjectHandlers() methods. The default lifetime value is Transient
        /// </summary>
        public PseudoMediatRConfiguration SetLifetime(ServiceLifetime givenLifetime)
        {
            Lifetime = givenLifetime;
            return this;
        }

        /// <summary>
        /// Injection of sender to DI container. Use sender if you need to work with request handers implicitly.
        /// </summary>
        public PseudoMediatRConfiguration InjectSender()
        {
            switch (Lifetime)
            {
                case ServiceLifetime.Transient:
                    {
                        services.AddTransient<ISender, Sender>();
                        break;
                    }
                case ServiceLifetime.Scoped:
                    {
                        services.AddScoped<ISender, Sender>();
                        break;
                    }
                case ServiceLifetime.Singleton:
                    {
                        services.AddSingleton<ISender, Sender>();
                        break;
                    }
                default:
                    break;
            }
            return this;
        }

        /// <summary>
        /// Injection of request handlers to DI container. Use request handlers if you need to work with request handlers explicitly.
        /// </summary>
        public PseudoMediatRConfiguration InjectHandlers()
        {
            var assemblies = Assembly.GetTypes()
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

            switch (Lifetime)
            {
                case ServiceLifetime.Transient:
                    {
                        AddHandlers(assemblies, (interfaceAssembly, classAssembly) => services.AddTransient(interfaceAssembly, classAssembly));
                        break;
                    }
                case ServiceLifetime.Scoped:
                    {
                        AddHandlers(assemblies, (interfaceAssembly, classAssembly) => services.AddScoped(interfaceAssembly, classAssembly));
                        break;
                    }
                case ServiceLifetime.Singleton:
                    {
                        AddHandlers(assemblies, (interfaceAssembly, classAssembly) => services.AddSingleton(interfaceAssembly, classAssembly));
                        break;
                    }
                default:
                    break;
            }

            return this;
        }

        /// <summary>
        /// (Optional) Set assembly where you want to inject the services. Note that you should call it before InjectHandlers() or InjectHandlers() methods.
        /// </summary>
        public PseudoMediatRConfiguration SetAssembly(Assembly givenAssembly)
        {
            Assembly = givenAssembly;
            return this;
        }

        private static void AddHandlers(IEnumerable<(Type interfaceAssembly, Type classAssembly)> assemblies, Action<Type, Type> addHandler)
        {
            foreach (var (interfaceAssembly, classAssembly) in assemblies)
            {
                addHandler(interfaceAssembly, classAssembly);
            }
        }

    }
}
