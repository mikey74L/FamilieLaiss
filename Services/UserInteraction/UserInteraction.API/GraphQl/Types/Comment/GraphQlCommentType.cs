using HotChocolate.Types;

namespace UserInteraction.API.GraphQl.Types.Comment;

public class GraphQlCommentType : ObjectType<Domain.Aggregates.Comment>
{
    protected override void Configure(IObjectTypeDescriptor<Domain.Aggregates.Comment> descriptor)
    {
        descriptor.Field(p => p.Id)
            .Description("The identifier for this comment");

        descriptor.Field(p => p.CreateDate)
            .Description("When was this comment created");
    }
}