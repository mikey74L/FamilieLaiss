using DomainHelper.DomainEvents;
using DomainHelper.Interfaces;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainHelper.AbstractClasses;

public abstract class DomainEntity
{
    #region Private Members

    private List<DomainEventBase> _domainEvents;

    #endregion

    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    public DomainEntity()
    {
        _domainEvents = [];
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Adds a domain event to the list of domain events
    /// </summary>
    /// <param name="domainEvent"></param>
    protected void AddDomainEvent(DomainEventBase domainEvent)
    {
        //Deklaration
        bool EventFound = false;

        //Ermitteln ob es sich um ein Single-Event handelt. Wenn nicht, kann
        //das Event einfach hinzugefügt werden. Wenn ja muss ermittelt werden
        //ob es schon ein Event dieses Typs mit dem gleichen Primary-Key (ID) gibt
        if (domainEvent.MultipleEventsAllowed)
        {
            _domainEvents.Add(domainEvent);
        }
        else
        {
            foreach (var ExistingEvent in _domainEvents)
            {
                if (domainEvent.GetType() == ExistingEvent.GetType() && domainEvent.ID == ExistingEvent.ID)
                {
                    EventFound = true;
                    break;
                }
            }

            if (!EventFound)
            {
                _domainEvents.Add(domainEvent);
            }
        }
    }

    /// <summary>
    /// Remove a domain event from the list of domain events
    /// </summary>
    /// <param name="domainEvent"></param>
    protected void RemoveDomainEvent(DomainEventBase domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    #endregion

    #region Public Fields

    /// <summary>
    /// List of domain events for mediatr
    /// </summary>
    [GraphQLIgnore]
    public IEnumerable<DomainEventBase> DomainEvents => _domainEvents;

    #endregion
}

public abstract class EntityBase : DomainEntity
{
    #region C'tor

    public EntityBase() : base()
    {
    }

    #endregion
}

public abstract class EntityBase<T> : EntityBase, iEntityBase, iEntityDeleted
{
    #region C'tor

    /// <summary>
    /// C'tor
    /// </summary>
    public EntityBase() : base()
    {
    }

    #endregion

    #region Public abstract Methods

    /// <summary>
    /// Wird vom Change-Tracker aufgerufen wenn die Entity hinzugefügt wurde
    /// </summary>
    public abstract Task EntityAddedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams);

    /// <summary>
    /// Wird vom Change-Tracker aufgerufen wenn die Entity gelöscht wurde
    /// </summary>
    public abstract Task EntityDeletedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams);

    #endregion

    #region Public Properties

    /// <summary>
    /// The technical identifier for this entity 
    /// </summary>
    [GraphQLNonNullType]
    public T Id { get; protected set; }

    #endregion
}

public abstract class EntityCreation<T> : EntityBase<T>, IEntityCreation
{
    #region Public Methods

    public void SetCreateDate()
    {
        CreateDate = DateTimeOffset.UtcNow;
    }
    #endregion

    #region Public Properties

    /// <summary>
    /// When was this entity created
    /// </summary>
    [GraphQLNonNullType]
    public DateTimeOffset? CreateDate { get; protected set; }

    #endregion
}

public abstract class EntityModify<T> : EntityCreation<T>, iEntityModify
{
    #region Public Methods

    /// <summary>
    /// Set current timestamp as change date for entity
    /// </summary>
    public void SetChangeDate()
    {
        ChangeDate = DateTimeOffset.UtcNow;
    }

    #endregion

    #region Public Abstract Methods

    /// <summary>
    /// Wird vom Change-Tracker aufgerufen wenn die Entity geändert wurde
    /// </summary>
    public abstract Task EntityModifiedAsync(DbContext dbContext, IDictionary<string, object> dictContextParams);

    #endregion

    #region Public Properties

    /// <summary>
    /// When was this entity last changed
    /// </summary>
    public DateTimeOffset? ChangeDate { get; protected set; }

    #endregion
}