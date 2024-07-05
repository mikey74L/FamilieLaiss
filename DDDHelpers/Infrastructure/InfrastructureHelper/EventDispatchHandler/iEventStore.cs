using DomainHelper.DomainEvents;
using MediatR;
using System.Collections.Concurrent;

namespace InfrastructureHelper.EventDispatchHandler
{
    public interface iEventStore
    {
        ConcurrentBag<DomainEventBase> Store { get; set; }
    }
}
