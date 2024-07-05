using Microsoft.AspNetCore.Authorization;

namespace FLBackEnd.API.GraphQL.Policies;

public static partial class PoliciesExtensions
{
    public static void ConfigureCategoryPolicies(IWebHostEnvironment env, AuthorizationOptions options)
    {
        options.AddPolicy("Category.Read", policyBaseData =>
        {
            if (!env.IsDevelopment())
            {
                policyBaseData.RequireClaim("permissions", "read:category");
            }
            else
            {
                policyBaseData.RequireAssertion(_ => true);
            }
        });

        options.AddPolicy("Category.Add", policyBaseData =>
        {
            if (!env.IsDevelopment())
            {
                policyBaseData.RequireClaim("permissions", "add:category");
            }
            else
            {
                policyBaseData.RequireAssertion(_ => true);
            }
        });

        options.AddPolicy("Category.Change", policyBaseData =>
        {
            if (!env.IsDevelopment())
            {
                policyBaseData.RequireClaim("permissions", "change:category");
            }
            else
            {
                policyBaseData.RequireAssertion(_ => true);
            }
        });

        options.AddPolicy("Category.Delete", policyBaseData =>
        {
            if (!env.IsDevelopment())
            {
                policyBaseData.RequireClaim("permissions", "delete:category");
            }
            else
            {
                policyBaseData.RequireAssertion(_ => true);
            }
        });

        options.AddPolicy("Category.Validate", policyBaseData =>
        {
            if (!env.IsDevelopment())
            {
                policyBaseData.RequireClaim("permissions", "validation:category");
            }
            else
            {
                policyBaseData.RequireAssertion(_ => true);
            }
        });
    }
}