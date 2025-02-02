namespace Catalog.API.GraphQL.Types.UploadVideo;

public class GraphQlUploadVideoType : ObjectType<Catalog.Domain.Entities.UploadVideo>
{
    protected override void Configure(IObjectTypeDescriptor<Catalog.Domain.Entities.UploadVideo> descriptor)
    {
        descriptor.Field(p => p.Id)
            .Description("The identifier for this upload video");

        descriptor.Field(p => p.CreateDate)
            .Ignore();
    }
}