using DomainHelper.DomainEvents;

namespace Settings.Domain.DomainEvents;

/// <summary>
/// Event for user setting created
/// </summary>
/// <remarks>
/// C'tor
/// </remarks>
/// <param name="id">Identifier for user setting</param>
public class DomainEventUserSettingCreated(string id)
    : DomainEventSingle(id)
{
}