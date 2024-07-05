using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissInterfaces.DataServices;

public interface IUploadPictureDataService
{
    Task<IApiResult<IEnumerable<IUploadPictureModel>>> GetUploadPicturesForUploadViewAsync(IReadOnlyList<UploadPictureSortInput> sortCriterias, UploadPictureFilterInput? filterCriteria);
    Task<IApiResult<IEnumerable<IUploadPictureModel>>> GetUploadPicturesForChooseViewAsync();

    Task<IApiResult<int>> GetUploadPictureCount();

    Task<IUploadPictureExifInfoFilterData?> GetUploadPictureExifInfoFilterData();

    Task<IApiResult<IUploadPictureModel?>> DeleteUploadPictureAsync(IUploadPictureModel model);

    Task<IApiResult<IEnumerable<IUploadPictureModel>>> DeleteAllUploadPicturesAsync(IEnumerable<IUploadPictureModel> modelsToDelete);
}