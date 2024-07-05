namespace FLBackEnd.API.GraphQL.Types.PictureConvertStatus;

public class GraphQlPictureConvertType : ObjectType<Domain.Entities.PictureConvertStatus>
{
    protected override void Configure(IObjectTypeDescriptor<Domain.Entities.PictureConvertStatus> descriptor)
    {
        descriptor.Field(p => p.Id)
            .Description("The identifier for this picture convert status item");
    }
}