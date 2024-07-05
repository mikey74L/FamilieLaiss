using Blog.API.Commands.Category;
using MediatR;

namespace Blog.API.GraphQL.Mutations.Blog
{
    [ExtendObjectType(typeof(Mutation))]
    public class MutationsBlog
    {
        [GraphQLDescription("Add a new blog entry")]
        //[Authorize(Policy = "Category.Add")]
        public async Task<AddBlogEntryPayload> AddBlogEntryAsync(AddBlogEntryInput input, [Service] IMediator mediator)
        {
            var NewBlogEntry = await mediator.Send(new MtrMakeNewBlogEntryCmd(input));

            var Result = new AddBlogEntryPayload
            {
                BlogEntry = NewBlogEntry
            };

            return Result;
        }

        [GraphQLDescription("Update existing blog entry")]
        //[Authorize(Policy = "Category.Change")]
        public async Task<UpdateBlogEntryPayload> UpdateBlogEntryAsync(UpdateBlogEntryInput input, [Service] IMediator mediator)
        {
            var updatedBlogEntry = await mediator.Send(new MtrUpdateBlogEntryCmd(input));

            var result = new UpdateBlogEntryPayload
            {
                BlogEntry = updatedBlogEntry
            };

            return result;
        }

        [GraphQLDescription("Delete existing category")]
        //[Authorize(Policy = "Category.Delete")]
        public async Task<DeleteBlogEntryPayload> DeleteBlogEntryAsync(DeleteBlogEntryInput input, [Service] IMediator mediator)
        {
            var deletedBlogEntry = await mediator.Send(new MtrDeleteBlogEntryCmd(input));

            var result = new DeleteBlogEntryPayload
            {
                BlogEntry = deletedBlogEntry
            };

            return result;
        }
    }
}
