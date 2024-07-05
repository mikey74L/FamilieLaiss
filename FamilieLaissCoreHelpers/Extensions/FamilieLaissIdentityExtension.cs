using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using IdentityModel;

namespace FamilieLaissCoreHelpers.Extensions
{
    public static class FamilieLaissIdentityExtension
    {
        /// <summary>
        /// Get the current username from identity claims
        /// </summary>
        /// <param name="identity">The identity</param>
        /// <returns>The username</returns>
        public static string GetUserName(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            if (claimsIdentity == null)
            {
                return null;
            }

            return claimsIdentity.Claims.First(x => x.Type == JwtClaimTypes.PreferredUserName).Value;
        }
    }
}
