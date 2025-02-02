using HotChocolate.Types;

namespace UserInteraction.API.GraphQl.Types.Favorite;

public class GraphQlFavoriteType : ObjectType<Domain.Aggregates.Favorite>
{
    protected override void Configure(IObjectTypeDescriptor<Domain.Aggregates.Favorite> descriptor)
    {
        descriptor.Field(p => p.Id)
            .Description("The identifier for this favorite");

        descriptor.Field(p => p.CreateDate)
            .Description("When was this favorite created");
    }
}