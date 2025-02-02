using HotChocolate.Types;

namespace User.API.GraphQL.Types;

public class UserType : ObjectType<User.Domain.Aggregates.UserAccount>
{
    protected override void Configure(IObjectTypeDescriptor<User.Domain.Aggregates.UserAccount> descriptor)
    {
        descriptor.Field(p => p.Id)
            .Description("The identifier for this user");

        descriptor.Field(p => p.CreateDate)
            .Description("When was this user created");

        descriptor.Field(p => p.ChangeDate)
            .Description("When was this user last changed");
    }
}
