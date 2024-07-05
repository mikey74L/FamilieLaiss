using Microsoft.AspNetCore.Authorization;

namespace FLBackEnd.API.GraphQL.Policies;

public static partial class PoliciesExtensions
{
    public static void ConfigureMediaGroupPolicies(IWebHostEnvironment env, AuthorizationOptions options)
    {
        options.AddPolicy("MediaGroup.Read", policyBaseData =>
        {
            if (!env.IsDevelopment())
            {
                policyBaseData.RequireClaim("permissions", "read:media-group");
            }
            else
            {
                policyBaseData.RequireAssertion(_ => true);
            }
        });

        options.AddPolicy("MediaGroup.Add", policyBaseData =>
        {
            if (!env.IsDevelopment())
            {
                policyBaseData.RequireClaim("permissions", "add:media-group");
            }
            else
            {
                policyBaseData.RequireAssertion(_ => true);
            }
        });

        options.AddPolicy("MediaGroup.Change", policyBaseData =>
        {
            if (!env.IsDevelopment())
            {
                policyBaseData.RequireClaim("permissions", "change:media-group");
            }
            else
            {
                policyBaseData.RequireAssertion(_ => true);
            }
        });

        options.AddPolicy("MediaGroup.Delete", policyBaseData =>
        {
            if (!env.IsDevelopment())
            {
                policyBaseData.RequireClaim("permissions", "delete:media-group");
            }
            else
            {
                policyBaseData.RequireAssertion(_ => true);
            }
        });

        options.AddPolicy("MediaGroup.Validate", policyBaseData =>
        {
            if (!env.IsDevelopment())
            {
                policyBaseData.RequireClaim("permissions", "validation:media-group");
            }
            else
            {
                policyBaseData.RequireAssertion(_ => true);
            }
        });
    }
}