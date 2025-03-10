using Catalog.Domain.Aggregates;

namespace Catalog.API.GraphQL.Types.Media;

public class GraphQlMediaGroupType : ObjectType<MediaGroup>
{
    protected override void Configure(IObjectTypeDescriptor<MediaGroup> descriptor)
    {
        descriptor.Field(p => p.Id)
            .Description("The identifier for this media group");

        descriptor.Field(p => p.ChangeDate)
            .Description("When was this media group last changed");

        descriptor.Field(p => p.CreateDate)
            .Description("When was this media group created");
    }
}