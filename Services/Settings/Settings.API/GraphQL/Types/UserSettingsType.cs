using Settings.Domain.Entities;

namespace Settings.API.GraphQL.Types;

public class UserSettingsType : ObjectType<UserSetting>
{
    protected override void Configure(IObjectTypeDescriptor<UserSetting> descriptor)
    {
        descriptor.Field(p => p.Id)
            .Description("The identifier for this user setting");

        descriptor.Field(p => p.CreateDate)
            .Description("When was this user setting added to database");

        descriptor.Field(p => p.ChangeDate)
            .Description("When was this user setting the last time changed in database");
    }
}
