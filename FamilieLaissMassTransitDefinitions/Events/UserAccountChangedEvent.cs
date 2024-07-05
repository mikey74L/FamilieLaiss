using FamilieLaissMassTransitDefinitions.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace FamilieLaissMassTransitDefinitions.Events
{
    public class UserAccountChangedEvent : iUserAccountChangedEvent
    {
        #region Properties
        /// <summary>
        /// The user account name (is unique)
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// The first name
        /// </summary>
        public string FirstName { get; private set; }

        /// <summary>
        /// The family name
        /// </summary>
        public string FamilyName { get; private set; }

        /// <summary>
        /// Street-Name
        /// </summary>
        public string Street { get; private set; }

        /// <summary>
        /// House-Number
        /// </summary>
        public string Number { get; private set; }

        /// <summary>
        /// Postal-Code
        /// </summary>
        public string ZIP { get; private set; }

        /// <summary>
        /// City-Name
        /// </summary>
        public string City { get; private set; }

        /// <summary>
        /// Two-Letter ISO-Code
        /// </summary>
        public string Country { get; private set; }

        /// <summary>
        /// Security-Question-ID
        /// </summary>
        public string SecurityQuestion { get; private set; }

        /// <summary>
        /// Security-Answer
        /// </summary>
        public string SecurityAnswer { get; private set; }
        #endregion

        #region C'tor
        /// <param name="userName">The user name</param>
        /// <param name="firstName">The first name</param>
        /// <param name="familyName">The family name</param>
        /// <param name="street">Name of street</param>
        /// <param name="number">House number</param>
        /// <param name="zip">Postal code</param>
        /// <param name="city">Name of city</param>
        /// <param name="country">ID for country</param>
        /// <param name="securityQuestion">ID for security question</param>
        /// <param name="securityAnswer">Security answer</param>
        public UserAccountChangedEvent(string userName, string firstName, string familyName, string street, string number, string zip,
            string city, string country, string securityQuestion, string securityAnswer)
        {
            //Übernehmen der Daten
            UserName = userName;
            FirstName = firstName;
            FamilyName = familyName;
            Street = street;
            Number = number;
            ZIP = zip;
            City = city;
            Country = country;
            SecurityQuestion = securityQuestion;
            SecurityAnswer = securityAnswer;
        }
        #endregion
    }
}
