using System;
using System.Collections.Generic;
using System.Text;

namespace FamilieLaissMassTransitDefinitions.Contracts.Events
{
    /// <summary>
    /// User account changed event (Event-Class for MassTransit)
    /// </summary>
    public interface iUserAccountChangedEvent
    {
        /// <summary>
        /// The user account name (is unique)
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// The first name
        /// </summary>
        string FirstName { get; }

        /// <summary>
        /// The family name
        /// </summary>
        string FamilyName { get; }

        /// <summary>
        /// Street-Name
        /// </summary>
        string Street { get; }

        /// <summary>
        /// House-Number
        /// </summary>
        string Number { get; }

        /// <summary>
        /// Postal-Code
        /// </summary>
        string ZIP { get; }

        /// <summary>
        /// City-Name
        /// </summary>
        string City { get; }

        /// <summary>
        /// Two-Letter ISO-Code
        /// </summary>
        string Country { get; }

        /// <summary>
        /// Security-Question-ID
        /// </summary>
        string SecurityQuestion { get; }

        /// <summary>
        /// Security-Answer
        /// </summary>
        string SecurityAnswer { get; }
    }
}
