using DomainHelper.DomainEvents;

namespace Settings.Domain.DomainEvents;

/// <summary>
/// Event for user setting changed
/// </summary>
/// <remarks>
/// C'tor
/// </remarks>
/// <param name="id">Identifier for user setting</param>
public class DomainEventUserSettingChanged(string id)
    : DomainEventSingle(id)
{
}