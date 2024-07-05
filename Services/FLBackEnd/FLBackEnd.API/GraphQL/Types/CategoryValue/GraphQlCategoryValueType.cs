namespace FLBackEnd.API.GraphQL.Types.CategoryValue;

public class GraphQlCategoryValueType : ObjectType<Domain.Entities.CategoryValue>
{
    protected override void Configure(IObjectTypeDescriptor<Domain.Entities.CategoryValue> descriptor)
    {
        descriptor.Field(p => p.Id)
            .Description("The identifier for this category value");

        descriptor.Field(p => p.ChangeDate)
            .Description("When was this category value last changed");

        descriptor.Field(p => p.CreateDate)
            .Description("When was this category value created");
    }
}