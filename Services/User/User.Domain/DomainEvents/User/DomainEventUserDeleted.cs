using DomainHelper.DomainEvents;

namespace User.Domain.DomainEvents.User;

public class DomainEventUserDeleted(string id) : DomainEventSingle(id)
{
}
