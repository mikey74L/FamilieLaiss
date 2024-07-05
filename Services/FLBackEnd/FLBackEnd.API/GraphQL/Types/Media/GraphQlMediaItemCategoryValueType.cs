namespace FLBackEnd.API.GraphQL.Types.Media;

public class GraphQlMediaItemCategoryValueType : ObjectType<Domain.Entities.MediaItemCategoryValue>
{
    protected override void Configure(IObjectTypeDescriptor<Domain.Entities.MediaItemCategoryValue> descriptor)
    {
        descriptor.Field(p => p.Id)
            .Description("The identifier for this media item category value");

        descriptor.Field(p => p.CreateDate)
            .Description("When was this media item category value created");
    }
}
