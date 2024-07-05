using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.Models.Blog;

public class BlogMinMaxDateModel : IBlogMinMaxDateModel
{
    public DateTimeOffset? MinCreateDate { get; set; }
    public DateTimeOffset? MaxCreateDate { get; set; }
}