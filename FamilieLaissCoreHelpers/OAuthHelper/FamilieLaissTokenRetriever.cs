using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace FamilieLaissCoreHelpers.OAuthHelper
{
    /// <summary> 
    /// Defines some common token retrieval strategies 
    /// </summary> 
    public static class TokenRetrieval
    {
        /// <summary> 
        /// Reads the token from the authrorization header. 
        /// </summary> 
        /// <param name="scheme">The scheme (defaults to Bearer).</param> 
        /// <returns></returns> 
        public static Func<HttpRequest, string> FromAuthorizationHeader(string scheme = "Bearer")
        {
            return (request) =>
            {
                string authorization = request.Headers["Authorization"].FirstOrDefault();


                if (string.IsNullOrEmpty(authorization))
                {
                    return null;
                }


                if (authorization.StartsWith(scheme + " ", StringComparison.OrdinalIgnoreCase))
                {
                    return authorization.Substring(scheme.Length + 1).Trim();
                }


                return null;
            };
        }

        /// <summary> 
        /// Reads the token from a query string parameter. 
        /// </summary> 
        /// <param name="name">The name (defaults to access_token).</param> 
        /// <returns></returns> 
        public static Func<HttpRequest, string> FromQueryString(string name = "access_token")
        {
            return (request) =>
                request.Query[name].FirstOrDefault();
            
        }

        /// <summary>
        /// Reads the token first from authorization header and when not found from query string
        /// </summary>
        /// <param name="name">The name (defaults to acces_token).</param>
        /// <param name="scheme">The scheme (defaults to Bearer).</param>
        /// <returns></returns>
        public static Func<HttpRequest, string> FromAuthorizationHeaderAndThenFromQueryString(string scheme = "Bearer", string name = "access_token")
        {
            return (request) =>
            {
                //Zuerst überprüfen ob das Token im Request-Header enthalten ist.
                //Das ist der normale Weg für API
                //Wenn es dort kein Token gibt, dann wird das Token über den
                //Query-String ermittelt
                var FuncFromAuthorization = FromAuthorizationHeader(scheme);
                string TokenFromAuthorization = FuncFromAuthorization(request);

                if (!string.IsNullOrEmpty(TokenFromAuthorization))
                {
                    return TokenFromAuthorization;
                }
                else
                {
                    var FuncFromQueryString = FromQueryString(name);
                    return FuncFromQueryString(request);
                }
            };
        }
    }
}
