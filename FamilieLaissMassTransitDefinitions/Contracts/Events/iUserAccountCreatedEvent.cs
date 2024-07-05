namespace FamilieLaissMassTransitDefinitions.Contracts.Events
{
    /// <summary>
    /// User account created event (Event-Class for MassTransit)
    /// </summary>
    public interface iUserAccountCreatedEvent
    {
        /// <summary>
        /// The identifier for the user
        /// </summary>
        string ID { get; }

        /// <summary>
        /// The user account name (is unique)
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// eMail-Adress
        /// </summary>
        string Email { get; }
    }
}
