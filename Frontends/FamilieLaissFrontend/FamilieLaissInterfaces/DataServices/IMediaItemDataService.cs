using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissInterfaces.DataServices;

public interface IMediaItemDataService
{
    Task<IApiResult<IEnumerable<IMediaItemModel>>> GetMediaItemsForGroupAsync(IMediaGroupModel mediaGroup);

    Task<IApiResult<IMediaItemModel>> GetMediaItemAsync(long mediaItemId);

    Task<IApiResult> AddMediaItemAsync(IMediaItemModel model, List<long> assignedCategoryValues);

    Task<IApiResult> UpdateMediaItemAsync(IMediaItemModel model, List<long> assignedCategoryValues);

    Task<IApiResult> DeleteMediaItemAsync(IMediaItemModel model, bool keepUploadItem);

    Task<IApiResult<bool>> GermanMediaItemNameExistsAsync(long id, long mediaGroupId, string? germanName);

    Task<IApiResult<bool>> EnglishMediaItemNameExistsAsync(long id, long mediaGroupId, string? englishName);
}
