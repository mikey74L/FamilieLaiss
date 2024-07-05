using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissInterfaces.DataServices
{
    public interface IMediaGroupDataService
    {
        Task<IApiResult<IEnumerable<IMediaGroupModel>>> GetAllMediaGroupsAsync();

        Task<IApiResult<IMediaGroupModel>> GetMediaGroupAsync(long id);

        //Task<IEnumerable<IMediaGroupMediaItemCountInfo>> GetGroupItemsCountAsync(EnumMediaType mediaType);

        Task<IApiResult> AddMediaGroupAsync(IMediaGroupModel model);

        Task<IApiResult> UpdateMediaGroupAsync(IMediaGroupModel model);

        Task<IApiResult> DeleteMediaGroupAsync(IMediaGroupModel model, bool keepUploadItems);

        Task<IApiResult<bool>> GermanMediaGroupNameExistsAsync(long id, string? germanName);

        Task<IApiResult<bool>> EnglishMediaGroupNameExistsAsync(long id, string? englishName);
    }
}
