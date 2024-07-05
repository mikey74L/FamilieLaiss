using DomainHelper.AbstractClasses;
using DomainHelper.DomainEvents;
using DomainHelper.Interfaces;
using InfrastructureHelper.EventDispatchHandler;
using InfrastructureHelper.Exceptions;
using InfrastructureHelper.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfrastructureHelper.UnitOfWork;

public class UnitOfWork<TContext> : iRepositoryFactory, iUnitOfWork<TContext>, iUnitOfWork
    where TContext : DbContext, IDisposable
{
    #region Private Members

    private Dictionary<Type, object> repositories;
    private readonly iEventStore eventStore;
    private Dictionary<string, object> contextParameters;

    #endregion

    #region C'tor

    public UnitOfWork(TContext context, iEventStore eventStore)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));

        this.eventStore = eventStore;
    }

    public UnitOfWork(IDbContextFactory<TContext> factory, iEventStore eventStore) : this(factory.CreateDbContext(),
        eventStore)
    {
    }

    #endregion

    #region Interface iRepositoryFactory

    public iReadRepository<TEntity> GetReadOnlyRepository<TEntity>() where TEntity : EntityBase
    {
        if (repositories == null)
        {
            repositories = [];
        }

        var type = typeof(TEntity);
        if (!repositories.ContainsKey(type))
        {
            repositories[type] = new ReadRepository<TEntity>(Context);
        }

        return (iReadRepository<TEntity>)repositories[type];
    }

    public iRepository<TEntity> GetRepository<TEntity>() where TEntity : EntityBase
    {
        if (repositories == null)
        {
            repositories = [];
        }

        var type = typeof(TEntity);
        if (!repositories.ContainsKey(type))
        {
            repositories[type] = new Repository<TEntity>(Context);
        }

        return (iRepository<TEntity>)repositories[type];
    }

    #endregion

    #region Interface iUnitOfWork

    public TContext Context { get; }

    public void AddContextParameter(string key, object value)
    {
        contextParameters.Add(key, value);
    }

    private async Task<int> SaveChangesAsync(IEnumerable<EntityEntry> changedEntities)
    {
        List<DomainEventBase> domainEvents = [];
        List<iEntityBase> addedEntities = [];
        List<iEntityModify> modifiedEntities = [];
        List<iEntityDeleted> deletedEntities = [];

        try
        {
            foreach (var changedEntity in changedEntities)
            {
                if (changedEntity.State == EntityState.Added)
                {
                    if (changedEntity.Entity is iEntityBase entityAdded)
                    {
                        addedEntities.Add(entityAdded);
                    }
                    if (changedEntity.Entity is IEntityCreation entityCreation)
                    {
                        entityCreation.SetCreateDate();
                    }
                }

                if (changedEntity.State == EntityState.Modified)
                {
                    if (changedEntity.Entity is iEntityModify entitychanged)
                    {
                        modifiedEntities.Add(entitychanged);
                        entitychanged.SetChangeDate();
                    }
                }

                if (changedEntity.State == EntityState.Deleted)
                {
                    if (changedEntity.Entity is iEntityDeleted entityDeleted)
                    {
                        deletedEntities.Add(entityDeleted);
                    }
                }
            }

            foreach (var Item in changedEntities)
            {
                if (Item.Entity is DomainEntity entity)
                {
                    domainEvents.AddRange(entity.DomainEvents);
                }
            }

            var result = await Context.SaveChangesAsync();


            foreach (var entity in addedEntities)
            {
                await entity.EntityAddedAsync(Context, contextParameters);
            }

            foreach (var entity in modifiedEntities)
            {
                await entity.EntityModifiedAsync(Context, contextParameters);
            }

            foreach (var entity in deletedEntities)
            {
                await entity.EntityDeletedAsync(Context, contextParameters);
            }

            foreach (var @event in domainEvents)
            {
                if (!@event.MultipleEventsAllowed)
                {
                    var eventFound = eventStore.Store.Any(foundEvent => @event.GetType() == foundEvent.GetType() &&
                                                                         @event.ID == foundEvent.ID);

                    if (!eventFound)
                    {
                        eventStore.Store.Add(@event);
                    }
                }
                else
                {
                    eventStore.Store.Add(@event);
                }
            }

            return result;
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: "23505" })
        {
            throw new DataDuplicatedValueException(ex.Message);
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        int result = 0;

        var changedEntities = Context.ChangeTracker.Entries();

        var transaction = await Context.Database.BeginTransactionAsync();

        try
        {
            while (changedEntities.Any(x => x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted))
            {
                result += await SaveChangesAsync(changedEntities);

                changedEntities = Context.ChangeTracker.Entries();
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        return result;
    }

    #endregion

    #region Interface IDisposable

    public void Dispose()
    {
        Context?.Dispose();
    }

    #endregion
}