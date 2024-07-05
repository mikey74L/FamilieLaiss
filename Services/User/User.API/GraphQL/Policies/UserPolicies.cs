using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace User.API.GraphQL.Policies
{
    public static partial class PoliciesExtensions
    {
        public static void ConfigureUserPolicies(IWebHostEnvironment env, AuthorizationOptions options)
        {
            //Policy zum Lesen von Usern hinzufügen
            options.AddPolicy("User.Read", policyBaseData =>
            {
                if (!env.IsDevelopment())
                {
                    policyBaseData.RequireClaim("scope", "read:user");
                }
                else
                {
                    policyBaseData.RequireAssertion(_ => true);
                }
            });

            //Policy zum Hinzufügen von Usern hinzufügen
            options.AddPolicy("User.Add", policyBaseData =>
            {
                if (!env.IsDevelopment())
                {
                    policyBaseData.RequireClaim("scope", "add:user");
                }
                else
                {
                    policyBaseData.RequireAssertion(_ => true);
                }
            });

            //Policy zum Ändern von Usern hinzufügen
            options.AddPolicy("User.Change", policyBaseData =>
            {
                if (!env.IsDevelopment())
                {
                    policyBaseData.RequireClaim("scope", "change:user");
                }
                else
                {
                    policyBaseData.RequireAssertion(_ => true);
                }
            });

            //Policy zum Löschen von Usern hinzufügen
            options.AddPolicy("User.Delete", policyBaseData =>
            {
                if (!env.IsDevelopment())
                {
                    policyBaseData.RequireClaim("scope", "delete:user");
                }
                else
                {
                    policyBaseData.RequireAssertion(_ => true);
                }
            });
        }
    }
}
