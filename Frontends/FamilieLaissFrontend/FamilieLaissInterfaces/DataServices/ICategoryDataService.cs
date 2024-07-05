using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissInterfaces.DataServices;

public interface ICategoryDataService
{
    Task<IApiResult<IEnumerable<ICategoryModel>>> GetPhotoCategoriesWithValuesAsync();

    Task<IApiResult<IEnumerable<ICategoryModel>>> GetVideoCategoriesWithValuesAsync();

    Task<IApiResult<IEnumerable<ICategoryModel>>> GetAllCategoriesAsync();

    Task<IApiResult<ICategoryModel>> GetCategoryAsync(long id);

    Task<IApiResult> AddCategoryAsync(ICategoryModel model);

    Task<IApiResult> UpdateCategoryAsync(ICategoryModel model);

    Task<IApiResult> DeleteCategoryAsync(ICategoryModel model);

    Task<IApiResult<bool>> GermanCategoryNameExistsAsync(long id, EnumCategoryType categoryType, string? germanName);

    Task<IApiResult<bool>> EnglishCategoryNameExistsAsync(long id, EnumCategoryType categoryType, string? englishName);
}