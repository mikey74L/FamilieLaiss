using System;
using System.Collections.Generic;
using System.Text;

namespace UserInteraction.DTO
{
    public class FamilieLaissUserDTOModel
    {
        /// <summary>
        /// The technical id for the user
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The username for the user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The gender of the user
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// The first name for the user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The family name for the user
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// The name of the street where the user lives
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// The house number where the user lives
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// ZIP-Code for User
        /// </summary>
        public string ZIP { get; set; }

        /// <summary>
        /// Name of city where the user lives
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Name of country where the user lives
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// eMail-Adress for the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The security question
        /// </summary>
        public string SecurityQuestion { get; set; }

        /// <summary>
        /// The answer for the security question
        /// </summary>
        public string SecurityAnswer { get; set; }

        /// <summary>
        /// Is eMail-Adress confirmed
        /// </summary>
        public string EmailConfirmed { get; set; }

        /// <summary>
        /// Is access to FamilieLaiss.de granted
        /// </summary>
        public bool IsAllowed { get; set; }

        /// <summary>
        /// Name of the user group this user belongs to
        /// </summary>
        public string UserGroup { get; set; }

        /// <summary>
        /// The count of ratings (For faster Access as property)
        /// </summary>
        public int RatingCount { get; set; }

        /// <summary>
        /// The count of comments (For faster access as property)
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        /// The count of favorites (For faster access as property)
        /// </summary>
        public int FavoriteCount { get; set; }
    }
}
