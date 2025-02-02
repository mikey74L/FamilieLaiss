using DomainHelper.DomainEvents;

namespace User.Domain.DomainEvents.User;

public class DomainEventUserChanged(string id) : DomainEventSingle(id)
{
}
