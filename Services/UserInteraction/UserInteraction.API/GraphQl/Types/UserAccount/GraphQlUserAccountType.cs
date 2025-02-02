using HotChocolate.Types;

namespace UserInteraction.API.GraphQl.Types.UserAccount;

public class GraphQlUserAccountType : ObjectType<Domain.Aggregates.UserAccount>
{
    protected override void Configure(IObjectTypeDescriptor<Domain.Aggregates.UserAccount> descriptor)
    {
        descriptor.Field(p => p.Id)
            .Description("The identifier for this user account");

        descriptor.Field(p => p.ChangeDate)
            .Description("When was this user account last changed");

        descriptor.Field(p => p.CreateDate)
            .Description("When was this user account created");
    }
}