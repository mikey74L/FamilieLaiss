using Catalog.Domain.Aggregates;

namespace Catalog.API.GraphQL.Types.Media;

public class GraphQlMediaItemCategoryValueType : ObjectType<MediaItemCategoryValue>
{
    protected override void Configure(IObjectTypeDescriptor<MediaItemCategoryValue> descriptor)
    {
        descriptor.Field(p => p.Id)
            .Description("The identifier for this media item category value");

        descriptor.Field(p => p.CreateDate)
            .Description("When was this media item category value created");
    }
}