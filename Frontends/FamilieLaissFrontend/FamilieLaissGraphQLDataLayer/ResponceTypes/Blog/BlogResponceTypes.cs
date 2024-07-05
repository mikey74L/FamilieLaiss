#nullable disable
using FamilieLaissModels.Models.Blog;

namespace FamilieLaissGraphQLDataLayer.ResponceTypes.Blog;

public class GetAllBlogEntriesResponce
{
    public List<BlogItemModel> BlogEntries { get; set; }
}

public class GetAllBlogEntriesFilterResponce : GetAllBlogEntriesResponce
{
}

public class GetBlogEntryResponce : GetAllBlogEntriesResponce
{
}

public class BlogGetMinMaxDateResponce
{
    public BlogMinMaxDateModel BlogGetMinAndMaxDate { get; set; }
}

public class MutationResult
{
    public BlogItemModel BlogEntry { get; set; }
}

public class AddBlogEntryResponce
{
    public MutationResult AddBlogEntry { get; set; }
}

public class UpdateBlogEntryResponce
{
    public MutationResult UpdateBlogEntry { get; set; }
}

public class DeleteBlogEntryResponce
{
    public MutationResult DeleteBlogEntry { get; set; }
}