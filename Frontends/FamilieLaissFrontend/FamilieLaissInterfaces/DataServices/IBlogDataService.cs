using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissInterfaces.DataServices;

public interface IBlogDataService
{
    Task<IEnumerable<IBlogItemModel>> GetAllBlogEntriesAsync(DateTimeOffset? filterMinDate, DateTimeOffset? filterMaxDate);

    Task<IBlogItemModel> GetBlogEntryAsync(long id);

    Task<IBlogMinMaxDateModel> GetBlogMinMaxDates();

    Task<IBlogItemModel> AddBlogEntryAsync(IBlogItemModel model);

    Task<IBlogItemModel> UpdateBlogEntryAsync(IBlogItemModel model);

    Task<IBlogItemModel> DeleteBlogEntryAsync(IBlogItemModel model);
}
