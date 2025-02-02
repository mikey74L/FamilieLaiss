using HotChocolate.Types;

namespace UserInteraction.API.GraphQl.Types.UserInteractionInfo;

public class GraphQlUserInteractionInfoType : ObjectType<Domain.Aggregates.UserInteractionInfo>
{
    protected override void Configure(IObjectTypeDescriptor<Domain.Aggregates.UserInteractionInfo> descriptor)
    {
        descriptor.Field(p => p.Id)
            .Description("The identifier for this user interaction info");

        descriptor.Field(p => p.ChangeDate)
            .Description("When was this user interaction info last changed");

        descriptor.Field(p => p.CreateDate)
            .Description("When was this user interaction info created");
    }
}