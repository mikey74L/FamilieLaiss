using DomainHelper.Interfaces;
using InfrastructureHelper.EventDispatchHandler;
using InfrastructureHelper.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceLayerHelper;

public static class UnitOfWorkServiceCollectionExtentions
{
    public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        services.AddScoped<iRepositoryFactory>((serviceProvider) =>
        {
            var factory = serviceProvider.GetRequiredService<IDbContextFactory<TContext>>();

            var eventStore = serviceProvider.GetRequiredService<iEventStore>();

            return new UnitOfWork<TContext>(factory, eventStore);
        });

        services.AddScoped<iUnitOfWork>((serviceProvider) =>
        {
            var factory = serviceProvider.GetRequiredService<IDbContextFactory<TContext>>();

            var eventStore = serviceProvider.GetRequiredService<iEventStore>();

            return new UnitOfWork<TContext>(factory, eventStore);
        });

        services.AddScoped<iUnitOfWork<TContext>>((serviceProvider) =>
        {
            var factory = serviceProvider.GetRequiredService<IDbContextFactory<TContext>>();

            var eventStore = serviceProvider.GetRequiredService<iEventStore>();

            return new UnitOfWork<TContext>(factory, eventStore);
        });

        services.AddSingleton<iEventStore, EventStoreService>();

        return services;
    }
}