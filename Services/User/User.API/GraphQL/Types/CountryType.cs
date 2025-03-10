using HotChocolate.Types;
using User.Domain.Aggregates;

namespace User.API.GraphQL.Types;

public class CountryType : ObjectType<Country>
{
    protected override void Configure(IObjectTypeDescriptor<Country> descriptor)
    {
        descriptor.Field(p => p.Id)
            .Description("The identifier for this country");

        descriptor.Field(p => p.CreateDate)
            .Description("When was this country added to database");
    }
}
