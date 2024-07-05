using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissMappingExtensions.Category;
using FamilieLaissMappingExtensions.CategoryValue;
using StrawberryShake;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FamilieLaissServices.DataServices;

public class CategoryValueDataService(IFamilieLaissClient familieLaissClient) : BaseDataService(familieLaissClient), ICategoryValueDataService

{
    #region Validation

    public async Task<IApiResult<bool>> EnglishCategoryValueNameExistsAsync(long id, long categoryId,
        string? englishName)
    {
        try
        {
            var response = await Client.EnglishCategoryValueNameExists.ExecuteAsync(categoryId, englishName ?? "", id);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.EnglishCategoryValueNameExists);
            }

            return CreateApiResultForError<bool>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<bool>(ex);
        }
    }

    public async Task<IApiResult<bool>> GermanCategoryValueNameExistsAsync(long id, long categoryId, string? germanName)
    {
        try
        {
            var response = await Client.GermanCategoryValueNameExists.ExecuteAsync(categoryId, germanName ?? "", id);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.GermanCategoryValueNameExists);
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

    public async Task<IApiResult<ICategoryModel>> GetCategoryValuesForCategoryAsync(long categoryId)
    {
        try
        {
            var response = await Client.GetCategoryValuesForCategory.ExecuteAsync(categoryId);

            if (response.IsSuccessResult() && response.Data is not null && response.Data.Categories.Any())
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

    public async Task<IApiResult<ICategoryValueModel>> GetCategoryValueAsync(long id)
    {
        try
        {
            var response = await Client.GetCategoryValue.ExecuteAsync(id);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                return CreateApiResult(response.Data.CategoryValues.First().Map());
            }

            return CreateApiResultForError<ICategoryValueModel>(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateApiResultForCommunicationError<ICategoryValueModel>(ex);
        }
    }

    #endregion

    #region CRUD

    public async Task<IApiResult> AddCategoryValueAsync(ICategoryValueModel model)
    {
        if (model.CategoryId is null)
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
                await Client.AddCategoryValue.ExecuteAsync(model.CategoryId.Value, model.NameGerman, model.NameEnglish);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                model.Id = response.Data.AddCategoryValue.CategoryValue.Id;
                model.CreateDate = response.Data.AddCategoryValue.CategoryValue.CreateDate;

                return CreateSimpleApiResult();
            }

            return CreateSimpleApiResultForError(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateSimpleApiResultForCommunicationError(ex);
        }
    }

    public async Task<IApiResult> UpdateCategoryValueAsync(ICategoryValueModel model)
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
                await Client.UpdateCategoryValue.ExecuteAsync(model.Id, model.NameGerman, model.NameEnglish);

            if (response.IsSuccessResult() && response.Data is not null)
            {
                model.ChangeDate = response.Data.UpdateCategoryValue.CategoryValue.ChangeDate;

                return CreateSimpleApiResult();
            }

            return CreateSimpleApiResultForError(response.Errors);
        }
        catch (Exception ex)
        {
            return CreateSimpleApiResultForCommunicationError(ex);
        }
    }

    public async Task<IApiResult> DeleteCategoryValueAsync(ICategoryValueModel model)
    {
        try
        {
            var response = await Client.DeleteCategoryValue.ExecuteAsync(model.Id);

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