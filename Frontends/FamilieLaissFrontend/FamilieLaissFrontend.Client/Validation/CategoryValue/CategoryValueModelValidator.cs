using FamilieLaissInterfaces;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissResources.Resources.Validation.CategoryValue;
using FamilieLaissServices.Extensions;
using FluentValidation;

namespace FamilieLaissFrontend.Client.Validation.CategoryValue;

public class CategoryValueModelValidator : AbstractValidator<ICategoryValueModel>, IValidatorFl<ICategoryValueModel>
{
    public CategoryValueModelValidator(IValidationHelperService validationHelper, ICategoryValueDataService categoryValueService)
    {
        async Task<bool> CallValidationForGerman(ICategoryValueModel model, string? value, CancellationToken cancel)
        {
            bool returnValue = true;

            if (model.CategoryId is not null)
            {
                await categoryValueService.GermanCategoryValueNameExistsAsync(model.Id, model.CategoryId.Value, value)
                    .HandleSuccess((result) =>
                    {
                        returnValue = !result;

                        return Task.CompletedTask;
                    });
            }

            return returnValue;
        }

        async Task<bool> CallValidationForEnglish(ICategoryValueModel model, string? value, CancellationToken cancel)
        {
            bool returnValue = true;

            if (model.CategoryId is not null)
            {
                await categoryValueService.EnglishCategoryValueNameExistsAsync(model.Id, model.CategoryId.Value, value)
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
            .WithMessage(validationHelper.GetMaxLengthMessage(CategoryValueModelValidatorRes.ResourceManager,
                CategoryValueModelValidatorRes.CultureInfo, nameof(ICategoryValueModel.NameGerman), 300))
            .NotEmpty().WithMessage(validationHelper.GetRequiredMessage(CategoryValueModelValidatorRes.ResourceManager,
                CategoryValueModelValidatorRes.CultureInfo, nameof(ICategoryValueModel.NameGerman)))
            .MustAsync(CallValidationForGerman)
            .WithMessage(validationHelper.GetAlreadyExistsMessage(CategoryValueModelValidatorRes.ResourceManager,
                CategoryValueModelValidatorRes.CultureInfo, nameof(ICategoryValueModel.NameGerman)));

        RuleFor(x => x.NameEnglish)
            .MaximumLength(300)
            .WithMessage(validationHelper.GetMaxLengthMessage(CategoryValueModelValidatorRes.ResourceManager,
                CategoryValueModelValidatorRes.CultureInfo, nameof(ICategoryValueModel.NameEnglish), 300))
            .NotEmpty().WithMessage(validationHelper.GetRequiredMessage(CategoryValueModelValidatorRes.ResourceManager,
                CategoryValueModelValidatorRes.CultureInfo, nameof(ICategoryValueModel.NameEnglish)))
            .MustAsync(CallValidationForEnglish)
            .WithMessage(validationHelper.GetAlreadyExistsMessage(CategoryValueModelValidatorRes.ResourceManager,
                CategoryValueModelValidatorRes.CultureInfo, nameof(ICategoryValueModel.NameEnglish)));
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result =
            await ValidateAsync(ValidationContext<ICategoryValueModel>.CreateWithOptions((ICategoryValueModel)model,
                x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
        {
            return Array.Empty<string>();
        }

        return result.Errors.Select(e => e.ErrorMessage);
    };
}