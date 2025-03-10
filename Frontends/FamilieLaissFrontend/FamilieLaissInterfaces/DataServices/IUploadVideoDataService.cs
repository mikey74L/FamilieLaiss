using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissInterfaces.DataServices;

public interface IUploadVideoDataService
{
    Task<IApiResult<IEnumerable<IUploadVideoModel>>> GetUploadVideosForUploadViewAsync(
        IReadOnlyList<UploadVideoSortInput> sortCriterias, UploadVideoFilterInput? filterCriteria);

    Task<IApiResult<IEnumerable<IUploadVideoModel>>> GetUploadVideosForChooseViewAsync();

    Task<IApiResult<int>> GetUploadVideoCount();

    Task<IApiResult<IUploadVideoModel?>> DeleteUploadVideoAsync(IUploadVideoModel model);

    Task<IApiResult<IEnumerable<IUploadVideoModel>>> DeleteAllUploadVideosAsync(
        IEnumerable<IUploadVideoModel> modelsToDelete);
}