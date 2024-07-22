using Catalog.Domain.Aggregates;

namespace Catalog.API.GraphQL.Types.Media;

public class GraphQlMediaItemType : ObjectType<MediaItem>
{
    protected override void Configure(IObjectTypeDescriptor<MediaItem> descriptor)
    {
        descriptor.Field(p => p.Id)
            .Description("The identifier for this media item");

        descriptor.Field(p => p.ChangeDate)
            .Description("When was this media item last changed");

        descriptor.Field(p => p.CreateDate)
            .Description("When was this media item created");
    }
}