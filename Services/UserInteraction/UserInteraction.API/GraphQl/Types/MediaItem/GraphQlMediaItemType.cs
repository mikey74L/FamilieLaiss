using HotChocolate.Types;

namespace UserInteraction.API.GraphQl.Types.MediaItem;

public class GraphQlMediaItemType : ObjectType<Domain.Aggregates.MediaItem>
{
    protected override void Configure(IObjectTypeDescriptor<Domain.Aggregates.MediaItem> descriptor)
    {
        descriptor.Field(p => p.Id)
            .Description("The identifier for this media item");

        descriptor.Field(p => p.CreateDate)
            .Description("When was this media item created");
    }
}