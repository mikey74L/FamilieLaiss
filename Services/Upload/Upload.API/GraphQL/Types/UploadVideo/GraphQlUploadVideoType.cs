using HotChocolate.Types;

namespace Upload.API.GraphQL.Types.UploadVideo;

public class GraphQlUploadVideoType : ObjectType<Domain.Entities.UploadVideo>
{
    protected override void Configure(IObjectTypeDescriptor<Domain.Entities.UploadVideo> descriptor)
    {
        descriptor.Field(p => p.Id)
            .Description("The identifier for this upload video");

        descriptor.Field(p => p.CreateDate)
            .Description("When was this upload video created");
    }
}