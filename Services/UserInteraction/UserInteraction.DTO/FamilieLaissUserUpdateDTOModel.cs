namespace UserInteraction.DTO
{
    /// <summary>
    /// User-Admin Class for update operations from client to backend
    /// </summary>
    public class FamilieLaissUserUpdateDTOModel
    {
        /// <summary>
        /// The first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The family name
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// Name of street
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// House number
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Postal code
        /// </summary>
        public string ZIP { get; set; }

        /// <summary>
        /// Name of city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// ID for country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// ID for security question
        /// </summary>
        public string SecurityQuestion { get; set; }

        /// <summary>
        /// Security answer
        /// </summary>
        public string SecurityAnswer { get; set; }
    }
}
