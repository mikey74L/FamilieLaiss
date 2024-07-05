using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissMappingExtensions.Category;
using StrawberryShake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilieLaissServices.DataServices;

public class CategoryDataService(IFamilieLaissClient familieLaissClient)
    : BaseDataService(familieLaissClient), ICategoryDataService
{
    #region Validation

    public async Task<IApiResult<bool>> EnglishCategoryNameExistsAsync(long id, EnumCategoryType categoryType,
        string? englishName)
    {
        try
        {
            var response = await Client.EnglishCategoryNameExists.ExecuteAsync(categoryType, englishName ?? "", id);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.EnglishCategoryNameExists);
            }

            return CreateApiResultForError<bool>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<bool>(ex);
        }
    }

    public async Task<IApiResult<bool>> GermanCategoryNameExistsAsync(long id, EnumCategoryType categoryType,
        string? germanName)
    {
        try
        {
            var response = await Client.GermanCategoryNameExists.ExecuteAsync(categoryType, germanName ?? "", id);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.GermanCategoryNameExists);
            }

            return CreateApiResultForError<bool>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<bool>(ex);
        }
    }

    #endregion

    #region Query
    public async Task<IApiResult<IEnumerable<ICategoryModel>>> GetPhotoCategoriesWithValuesAsync()
    {
        try
        {
            var response = await Client.GetAllPhotoCategories.ExecuteAsync();

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.Categories.Map());
            }

            return CreateApiResultForError<IEnumerable<ICategoryModel>>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<IEnumerable<ICategoryModel>>(ex);
        }
    }

    public async Task<IApiResult<IEnumerable<ICategoryModel>>> GetVideoCategoriesWithValuesAsync()
    {
        try
        {
            var response = await Client.GetAllVideoCategories.ExecuteAsync();

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.Categories.Map());
            }

            return CreateApiResultForError<IEnumerable<ICategoryModel>>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<IEnumerable<ICategoryModel>>(ex);
        }
    }

    public async Task<IApiResult<IEnumerable<ICategoryModel>>> GetAllCategoriesAsync()
    {
        try
        {
            var response = await Client.GetAllCategories.ExecuteAsync();

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.Categories.Map());
            }

            return CreateApiResultForError<IEnumerable<ICategoryModel>>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<IEnumerable<ICategoryModel>>(ex);
        }
    }

    public async Task<IApiResult<ICategoryModel>> GetCategoryAsync(long id)
    {
        try
        {
            var response = await Client.GetCategory.ExecuteAsync(id);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.Categories.First().Map());
            }

            return CreateApiResultForError<ICategoryModel>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<ICategoryModel>(ex);
        }
    }

    #endregion

    #region CRUD

    public async Task<IApiResult> AddCategoryAsync(ICategoryModel model)
    {
        if (model.CategoryType is null)
        {
            return CreateSimpleApiResultForBadRequest();
        }

        if (model.NameGerman is null)
        {
            return CreateSimpleApiResultForBadRequest();
        }

        if (model.NameEnglish is null)
        {
            return CreateSimpleApiResultForBadRequest();
        }

        try
        {
            var response =
                await Client.AddCategory.ExecuteAsync(model.CategoryType.Value, model.NameGerman, model.NameEnglish);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                model.Id = response.Data.AddCategory.Category.Id;
                model.CreateDate = response.Data.AddCategory.Category.CreateDate;

                return CreateSimpleApiResult();
            }

            return CreateSimpleApiResultForError(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateSimpleApiResultForCommunicationError(ex);
        }
    }

    public async Task<IApiResult> UpdateCategoryAsync(ICategoryModel model)
    {
        if (model.NameGerman is null)
        {
            return CreateSimpleApiResultForBadRequest();
        }

        if (model.NameEnglish is null)
        {
            return CreateSimpleApiResultForBadRequest();
        }

        try
        {
            var response =
                await Client.UpdateCategory.ExecuteAsync(model.Id, model.NameGerman, model.NameEnglish);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                model.ChangeDate = response.Data.UpdateCategory.Category.ChangeDate;

                return CreateSimpleApiResult();
            }

            return CreateSimpleApiResultForError(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateSimpleApiResultForCommunicationError(ex);
        }
    }

    public async Task<IApiResult> DeleteCategoryAsync(ICategoryModel model)
    {
        try
        {
            var response = await Client.DeleteCategory.ExecuteAsync(model.Id);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateSimpleApiResult();
            }

            return CreateSimpleApiResultForError(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateSimpleApiResultForCommunicationError(ex);
        }
    }

    #endregion
}