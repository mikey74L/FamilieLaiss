namespace Catalog.API.GraphQL.Types.UploadPicture;

public class GraphQlUploadPictureType : ObjectType<Catalog.Domain.Entities.UploadPicture>
{
    protected override void Configure(IObjectTypeDescriptor<Catalog.Domain.Entities.UploadPicture> descriptor)
    {
        descriptor.Field(p => p.Id)
            .Description("The identifier for this upload picture");

        descriptor.Field(p => p.CreateDate)
            .Ignore();
    }
}