﻿namespace Catalog.API.GraphQL.Types.CategoryValue;

public class GraphQlCategoryValueType : ObjectType<Domain.Aggregates.CategoryValue>
{
    protected override void Configure(IObjectTypeDescriptor<Domain.Aggregates.CategoryValue> descriptor)
    {
        descriptor.Field(p => p.Id)
            .Description("The identifier for this category value");

        descriptor.Field(p => p.ChangeDate)
            .Description("When was this category value last changed");

        descriptor.Field(p => p.CreateDate)
            .Description("When was this category value created");
    }
}