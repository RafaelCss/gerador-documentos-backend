using GeradorDocumentos.Dominio.Contrato;
using GeradorDocumentos.Infra.Mediator;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GeradorDocumento.Ioc.Infra.MediatorExtensions;

public static class MediatorExtensions
{
    public static IServiceCollection AddMediatorHandlers(
        this IServiceCollection services ,
        params Assembly[] assemblies)
    {
        if (assemblies.Length == 0)
            assemblies = AppDomain.CurrentDomain.GetAssemblies();

        var handlerInterface = typeof(IRequestHandler<,>);

        foreach (var assembly in assemblies)
        {
            var handlers = assembly.GetTypes()
                .Where(t => t is { IsAbstract: false, IsInterface: false })
                .SelectMany(t => t.GetInterfaces() , (type , iface) => new { type , iface })
                .Where(x =>
                    x.iface.IsGenericType &&
                    x.iface.GetGenericTypeDefinition() == handlerInterface)
                .Select(x => new
                {
                    ServiceType = x.iface ,
                    ImplementationType = x.type
                });

            foreach (var handler in handlers)
            {
                services.AddScoped(handler.ServiceType , handler.ImplementationType);
            }
        }

        services.AddScoped<IMediator , Mediator>();
        return services;
    }
}