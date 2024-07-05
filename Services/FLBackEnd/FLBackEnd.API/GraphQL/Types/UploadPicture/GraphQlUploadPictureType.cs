namespace FLBackEnd.API.GraphQL.Types.UploadPicture;

public class GraphQlUploadPictureType : ObjectType<Domain.Entities.UploadPicture>
{
    protected override void Configure(IObjectTypeDescriptor<Domain.Entities.UploadPicture> descriptor)
    {
        descriptor.Field(p => p.Id)
            .Description("The identifier for this upload picture");

        descriptor.Field(p => p.CreateDate)
            .Description("When was this upload picture created");
    }
}