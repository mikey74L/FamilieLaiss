namespace FLBackEnd.API.GraphQL.Types.UserSetting;

public class GraphQlUserSettingType : ObjectType<Domain.Entities.UserSetting>
{
    protected override void Configure(IObjectTypeDescriptor<Domain.Entities.UserSetting> descriptor)
    {
        descriptor.Field(p => p.Id)
        .Description("The identifier for this user setting");

        descriptor.Field(p => p.ChangeDate)
            .Description("When was this user setting last changed");

        descriptor.Field(p => p.CreateDate)
            .Description("When was this user setting created");
    }
}
