using DomainHelper.DomainEvents;

namespace Settings.Domain.DomainEvents;

/// <summary>
/// Event for user setting deleted
/// </summary>
/// <remarks>
/// C'tor
/// </remarks>
/// <param name="id">Identifier for user setting</param>
public class DomainEventUserSettingDeleted(string id)
    : DomainEventSingle(id)
{
}