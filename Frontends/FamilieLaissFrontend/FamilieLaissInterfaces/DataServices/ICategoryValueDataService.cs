using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissInterfaces.DataServices;

public interface ICategoryValueDataService
{
    Task<IApiResult<ICategoryModel>> GetCategoryValuesForCategoryAsync(long categoryId);

    Task<IApiResult<ICategoryValueModel>> GetCategoryValueAsync(long categoryValueId);

    Task<IApiResult> AddCategoryValueAsync(ICategoryValueModel model);

    Task<IApiResult> UpdateCategoryValueAsync(ICategoryValueModel model);

    Task<IApiResult> DeleteCategoryValueAsync(ICategoryValueModel model);

    Task<IApiResult<bool>> GermanCategoryValueNameExistsAsync(long id, long categoryId, string? germanName);

    Task<IApiResult<bool>> EnglishCategoryValueNameExistsAsync(long id, long categoryId, string? englishName);
}