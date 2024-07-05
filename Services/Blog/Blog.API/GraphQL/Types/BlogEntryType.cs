using Blog.Domain.Entities;

namespace Blog.API.GraphQL.Types
{
    public class BlogEntryType : ObjectType<BlogEntry>
    {
        protected override void Configure(IObjectTypeDescriptor<BlogEntry> descriptor)
        {
            descriptor.Field(p => p.Id)
                .Description("The identifier for this blog entry");

            descriptor.Field(p => p.ChangeDate)
                .Description("When was this blog entry last changed");

            descriptor.Field(p => p.CreateDate)
                .Description("When was this blog entry created");

            descriptor.Field(p => p.DomainEvents)
                .Ignore();
        }
    }

    [GraphQLDescription("Blog min und max dates")]
    public class BlogMaxMinDateInfo
    {
        [GraphQLDescription("Max blog date")]
        public DateTimeOffset? MaxCreateDate { get; set; }

        [GraphQLDescription("Min blog date")]
        public DateTimeOffset? MinCreateDate { get; set; }
    }
}
