namespace VideoConvert.API.GraphQL.Types.VideoConvertStatus;

public class GraphQlVideoConvertStatusType : ObjectType<Domain.Entities.VideoConvertStatus>
{
    protected override void Configure(IObjectTypeDescriptor<Domain.Entities.VideoConvertStatus> descriptor)
    {
        descriptor.Field(p => p.Id)
            .Description("The identifier for this video convert status item");
    }
}