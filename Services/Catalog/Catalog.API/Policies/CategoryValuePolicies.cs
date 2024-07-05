using Microsoft.AspNetCore.Authorization;

namespace Catalog.API.Policies
{
    public static partial class PoliciesExtensions
    {
        public static void ConfigureCategoryValuePolicies(IWebHostEnvironment env, AuthorizationOptions options)
        {
            options.AddPolicy("CategoryValue.Read", policyBaseData =>
            {
                if (!env.IsDevelopment())
                {
                    policyBaseData.RequireClaim("scope", "read:category-value");
                }
                else
                {
                    policyBaseData.RequireAssertion(_ => true);
                }
            });

            options.AddPolicy("CategoryValue.Add", policyBaseData =>
            {
                if (!env.IsDevelopment())
                {
                    policyBaseData.RequireClaim("scope", "add:category-value");
                }
                else
                {
                    policyBaseData.RequireAssertion(_ => true);
                }
            });

            options.AddPolicy("CategoryValue.Change", policyBaseData =>
            {
                if (!env.IsDevelopment())
                {
                    policyBaseData.RequireClaim("scope", "change:category-value");
                }
                else
                {
                    policyBaseData.RequireAssertion(_ => true);
                }
            });

            options.AddPolicy("CategoryValue.Delete", policyBaseData =>
            {
                if (!env.IsDevelopment())
                {
                    policyBaseData.RequireClaim("scope", "delete:category-value");
                }
                else
                {
                    policyBaseData.RequireAssertion(_ => true);
                }
            });

            options.AddPolicy("CategoryValue.Validate", policyBaseData =>
            {
                if (!env.IsDevelopment())
                {
                    policyBaseData.RequireClaim("scope", "validation:category-value");
                }
                else
                {
                    policyBaseData.RequireAssertion(_ => true);
                }
            });
        }
    }
}
