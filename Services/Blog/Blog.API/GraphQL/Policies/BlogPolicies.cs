using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Blog.API.GraphQL.Policies
{
    public static partial class PoliciesExtensions
    {
        public static void ConfigureBlogPolicies(IWebHostEnvironment env, AuthorizationOptions options)
        {
            options.AddPolicy("Blog.Read", policyBaseData =>
            {
                if (!env.IsDevelopment())
                {
                    policyBaseData.RequireClaim("scope", "read:blog");
                }
                else
                {
                    policyBaseData.RequireAssertion(_ => true);
                }
            });

            options.AddPolicy("Blog.Add", policyBaseData =>
            {
                if (!env.IsDevelopment())
                {
                    policyBaseData.RequireClaim("scope", "add:blog");
                }
                else
                {
                    policyBaseData.RequireAssertion(_ => true);
                }
            });

            options.AddPolicy("Blog.Change", policyBaseData =>
            {
                if (!env.IsDevelopment())
                {
                    policyBaseData.RequireClaim("scope", "change:blog");
                }
                else
                {
                    policyBaseData.RequireAssertion(_ => true);
                }
            });

            options.AddPolicy("Blog.Delete", policyBaseData =>
            {
                if (!env.IsDevelopment())
                {
                    policyBaseData.RequireClaim("scope", "delete:blog");
                }
                else
                {
                    policyBaseData.RequireAssertion(_ => true);
                }
            });
        }
    }
}
