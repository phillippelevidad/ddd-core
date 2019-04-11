using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;
using System;

namespace Core.Infra
{
    public static class CoreConfiguration
    {
        public static IServiceCollection AddCore(this IServiceCollection services, params Assembly[] assembliesWithCommandsQueriesAndDomainEventHandlers)
        {
            if (assembliesWithCommandsQueriesAndDomainEventHandlers.Length == 0)
                assembliesWithCommandsQueriesAndDomainEventHandlers = AppDomain.CurrentDomain.GetAssemblies();

            services.AddScoped<ICommandDispatcher, MediatorCommandDispatcher>();
            services.AddScoped<IDomainEventDispatcher, MediatorDomainEventDispatcher>();
            services.AddScoped<IQueryDispatcher, MediatorQueryDispatcher>();
            services.AddMediatR(assembliesWithCommandsQueriesAndDomainEventHandlers);

            return services;
        }
    }
}