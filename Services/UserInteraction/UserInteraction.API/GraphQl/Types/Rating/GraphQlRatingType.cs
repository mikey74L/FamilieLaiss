using HotChocolate.Types;

namespace UserInteraction.API.GraphQl.Types.Rating;

public class GraphQlRatingType : ObjectType<Domain.Aggregates.Rating>
{
    protected override void Configure(IObjectTypeDescriptor<Domain.Aggregates.Rating> descriptor)
    {
        descriptor.Field(p => p.Id)
            .Description("The identifier for this rating");

        descriptor.Field(p => p.CreateDate)
            .Description("When was this rating created");
    }
}