using DomainHelper.DomainEvents;
using MediatR;
using System.Collections.Concurrent;

namespace InfrastructureHelper.EventDispatchHandler
{
    public class EventStoreService : iEventStore
    {
        #region C'tor
        public EventStoreService()
        {
            Store = new ConcurrentBag<DomainEventBase>();
        }
        #endregion

        #region Interface iEventStore
        public ConcurrentBag<DomainEventBase> Store { get; set; }
        #endregion
    }
}
