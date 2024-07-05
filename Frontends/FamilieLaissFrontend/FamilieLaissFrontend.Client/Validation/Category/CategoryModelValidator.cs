using FamilieLaissInterfaces;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissResources.Resources.Validation.Category;
using FamilieLaissServices.Extensions;
using FluentValidation;

namespace FamilieLaissFrontend.Client.Validation.Category;

public class CategoryModelValidator : AbstractValidator<ICategoryModel>, IValidatorFl<ICategoryModel>
{
    public CategoryModelValidator(IValidationHelperService validationHelper, ICategoryDataService categoryService)
    {
        async Task<bool> CallValidationForGerman(ICategoryModel model, string? value, CancellationToken cancel)
        {
            bool returnValue = true;

            if (model.CategoryType is not null)
            {
                await categoryService.GermanCategoryNameExistsAsync(model.Id, model.CategoryType.Value, value)
                    .HandleSuccess((result) =>
                    {
                        returnValue = !result;

                        return Task.CompletedTask;
                    });
            }

            return returnValue;
        }

        async Task<bool> CallValidationForEnglish(ICategoryModel model, string? value, CancellationToken cancel)
        {
            bool returnValue = true;

            if (model.CategoryType is not null)
            {
                await categoryService.EnglishCategoryNameExistsAsync(model.Id, model.CategoryType.Value, value)
                    .HandleSuccess((result) =>
                    {
                        returnValue = !result;

                        return Task.CompletedTask;
                    });
            }

            return returnValue;
        }

        RuleFor(x => x.NameGerman)
            .MaximumLength(300)
            .WithMessage(validationHelper.GetMaxLengthMessage(CategoryModelValidatorRes.ResourceManager, CategoryModelValidatorRes.CultureInfo,
                nameof(ICategoryModel.NameGerman), 300))
            .NotEmpty().WithMessage(validationHelper.GetRequiredMessage(CategoryModelValidatorRes.ResourceManager, CategoryModelValidatorRes.CultureInfo,
                nameof(ICategoryModel.NameGerman)))
            .MustAsync(CallValidationForGerman)
            .WithMessage(validationHelper.GetAlreadyExistsMessage(CategoryModelValidatorRes.ResourceManager, CategoryModelValidatorRes.CultureInfo,
                nameof(ICategoryModel.NameGerman)));

        RuleFor(x => x.NameEnglish)
            .MaximumLength(300)
            .WithMessage(validationHelper.GetMaxLengthMessage(CategoryModelValidatorRes.ResourceManager, CategoryModelValidatorRes.CultureInfo,
                nameof(ICategoryModel.NameEnglish), 300))
            .NotEmpty().WithMessage(validationHelper.GetRequiredMessage(CategoryModelValidatorRes.ResourceManager, CategoryModelValidatorRes.CultureInfo,
                nameof(ICategoryModel.NameEnglish)))
            .MustAsync(CallValidationForEnglish)
            .WithMessage(validationHelper.GetAlreadyExistsMessage(CategoryModelValidatorRes.ResourceManager, CategoryModelValidatorRes.CultureInfo,
                nameof(ICategoryModel.NameEnglish)));
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result =
            await ValidateAsync(ValidationContext<ICategoryModel>.CreateWithOptions((ICategoryModel)model,
                x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
        {
            return Array.Empty<string>();
        }

        return result.Errors.Select(e => e.ErrorMessage);
    };
}