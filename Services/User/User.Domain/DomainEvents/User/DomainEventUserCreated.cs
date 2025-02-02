using DomainHelper.DomainEvents;

namespace User.Domain.DomainEvents.User;

public class DomainEventUserCreated(string id) : DomainEventSingle(id)
{
}
