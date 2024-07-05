namespace FLBackEnd.API.GraphQL.Types.Media;

public class GraphQlMediaGroupType : ObjectType<Domain.Entities.MediaGroup>
{
    protected override void Configure(IObjectTypeDescriptor<Domain.Entities.MediaGroup> descriptor)
    {
        descriptor.Field(p => p.Id)
            .Description("The identifier for this media group");

        descriptor.Field(p => p.ChangeDate)
            .Description("When was this media group last changed");

        descriptor.Field(p => p.CreateDate)
            .Description("When was this media group created");
    }
}
