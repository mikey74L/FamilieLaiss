﻿using Microsoft.AspNetCore.Authorization;

namespace Catalog.API.Policies
{
    public static partial class PoliciesExtensions
    {
        public static void ConfigureMediaGroupPolicies(IWebHostEnvironment env, AuthorizationOptions options)
        {
            options.AddPolicy("MediaGroup.Read", policyBaseData =>
            {
                if (!env.IsDevelopment())
                {
                    policyBaseData.RequireClaim("scope", "read:media-group");
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
                    policyBaseData.RequireClaim("scope", "add:media-group");
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
                    policyBaseData.RequireClaim("scope", "change:media-group");
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
                    policyBaseData.RequireClaim("scope", "delete:media-group");
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
                    policyBaseData.RequireClaim("scope", "validation:media-group");
                }
                else
                {
                    policyBaseData.RequireAssertion(_ => true);
                }
            });
        }
    }
}
