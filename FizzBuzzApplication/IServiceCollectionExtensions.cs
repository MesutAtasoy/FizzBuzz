using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace FizzBuzzApplication
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection DecorateWithDispatchProxy<TInterface, TProxy>(this IServiceCollection services)
            where TInterface : class
            where TProxy : DispatchProxy
        {
            MethodInfo createMethod;
            try
            {
                createMethod = typeof(TProxy)
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .First(info => !info.IsGenericMethod && info.ReturnType == typeof(TInterface));
            }
            catch (Exception e)
            {
                throw new Exception("An error has occured while finding create method in given interface");
            }

            var argInfos = createMethod.GetParameters();
            
            var descriptorsToDecorate = services.Where(s => s.ServiceType == typeof(TInterface)).ToList();

            if (descriptorsToDecorate.Count == 0)
            {
                throw new InvalidOperationException("There are no  services are present in ServiceCollection");
            }

            foreach (var descriptor in descriptorsToDecorate)
            {
                var decorated = ServiceDescriptor.Describe(typeof(TInterface), sp =>
                    {
                        var decoratorInstance = createMethod.Invoke(null,
                            argInfos.Select(
                                    info => info.ParameterType ==
                                            (descriptor.ServiceType ?? descriptor.ImplementationType)
                                        ? sp.CreateInstance(descriptor)
                                        : sp.GetRequiredService(info.ParameterType))
                                .ToArray());

                        return (TInterface) decoratorInstance;
                    },
                    descriptor.Lifetime);

                services.Remove(descriptor);
                services.Add(decorated);
            }

            return services;
        }

        private static object CreateInstance(this IServiceProvider services, ServiceDescriptor descriptor)
        {
            if (descriptor.ImplementationInstance != null)
            {
                return descriptor.ImplementationInstance;
            }

            if (descriptor.ImplementationFactory != null)
            {
                return descriptor.ImplementationFactory(services);
            }

            return ActivatorUtilities.GetServiceOrCreateInstance(services, descriptor.ImplementationType);
        }
    }
}