namespace Catalog.API.GraphQL.Types.Category;

public class GraphQlCategoryType : ObjectType<Domain.Aggregates.Category>
{
    protected override void Configure(IObjectTypeDescriptor<Domain.Aggregates.Category> descriptor)
    {
        descriptor.Field(p => p.Id)
            .Description("The identifier for this category");

        descriptor.Field(p => p.ChangeDate)
            .Description("When was this category last changed");

        descriptor.Field(p => p.CreateDate)
            .Description("When was this category created");
    }
}