using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace User.API.GraphQL.Policies
{
    public static partial class PoliciesExtensions
    {
        public static void ConfigureCountryPolicies(IWebHostEnvironment env, AuthorizationOptions options)
        {
            //Policy zum Lesen von Countries hinzufügen
            options.AddPolicy("Country.Read", policyBaseData =>
            {
                if (!env.IsDevelopment())
                {
                    policyBaseData.RequireClaim("scope", "read:country");
                }
                else
                {
                    policyBaseData.RequireAssertion(_ => true);
                }
            });
        }
    }
}
