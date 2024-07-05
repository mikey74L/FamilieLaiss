using FamilieLaissInterfaces;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissResources.Resources.Validation.MediaGroup;
using FamilieLaissServices.Extensions;
using FluentValidation;

namespace FamilieLaissFrontend.Client.Validation.MediaGroup;

public class MediaGroupModelValidator : AbstractValidator<IMediaGroupModel>, IValidatorFl<IMediaGroupModel>
{
    public MediaGroupModelValidator(IValidationHelperService validationHelper, IMediaGroupDataService mediaGroupService)
    {
        async Task<bool> CallValidationForGerman(IMediaGroupModel model, string? value, CancellationToken cancel)
        {
            bool returnValue = true;

            await mediaGroupService.GermanMediaGroupNameExistsAsync(model.Id, value)
                .HandleSuccess((result) =>
                {
                    returnValue = !result;

                    return Task.CompletedTask;
                });

            return returnValue;
        }

        async Task<bool> CallValidationForEnglish(IMediaGroupModel model, string? value, CancellationToken cancel)
        {
            bool returnValue = true;

            await mediaGroupService.EnglishMediaGroupNameExistsAsync(model.Id, value)
                .HandleSuccess((result) =>
                {
                    returnValue = !result;

                    return Task.CompletedTask;
                });

            return returnValue;
        }

        RuleFor(x => x.EventDateForInput)
            .NotEmpty().WithMessage(validationHelper.GetRequiredMessage(MediaGroupModelValidatorRes.ResourceManager, MediaGroupModelValidatorRes.CultureInfo, nameof(IMediaGroupModel.EventDateForInput)));

        RuleFor(x => x.NameGerman)
            .MaximumLength(300).WithMessage(validationHelper.GetMaxLengthMessage(MediaGroupModelValidatorRes.ResourceManager, MediaGroupModelValidatorRes.CultureInfo, nameof(IMediaGroupModel.NameGerman), 300))
            .NotEmpty().WithMessage(validationHelper.GetRequiredMessage(MediaGroupModelValidatorRes.ResourceManager, MediaGroupModelValidatorRes.CultureInfo, nameof(IMediaGroupModel.NameGerman)))
            .MustAsync(CallValidationForGerman).WithMessage(validationHelper.GetAlreadyExistsMessage(MediaGroupModelValidatorRes.ResourceManager, MediaGroupModelValidatorRes.CultureInfo, nameof(IMediaGroupModel.NameGerman)));

        RuleFor(x => x.NameEnglish)
            .MaximumLength(300).WithMessage(validationHelper.GetMaxLengthMessage(MediaGroupModelValidatorRes.ResourceManager, MediaGroupModelValidatorRes.CultureInfo, nameof(IMediaGroupModel.NameEnglish), 300))
            .NotEmpty().WithMessage(validationHelper.GetRequiredMessage(MediaGroupModelValidatorRes.ResourceManager, MediaGroupModelValidatorRes.CultureInfo, nameof(IMediaGroupModel.NameEnglish)))
            .MustAsync(CallValidationForEnglish).WithMessage(validationHelper.GetAlreadyExistsMessage(MediaGroupModelValidatorRes.ResourceManager, MediaGroupModelValidatorRes.CultureInfo, nameof(IMediaGroupModel.NameEnglish)));

        RuleFor(x => x.DescriptionGerman)
            .MaximumLength(3000).WithMessage(validationHelper.GetMaxLengthMessage(MediaGroupModelValidatorRes.ResourceManager, MediaGroupModelValidatorRes.CultureInfo, nameof(IMediaGroupModel.DescriptionGerman), 3000))
            .NotEmpty().WithMessage(validationHelper.GetRequiredMessage(MediaGroupModelValidatorRes.ResourceManager, MediaGroupModelValidatorRes.CultureInfo, nameof(IMediaGroupModel.DescriptionGerman)));

        RuleFor(x => x.DescriptionEnglish)
            .MaximumLength(3000).WithMessage(validationHelper.GetMaxLengthMessage(MediaGroupModelValidatorRes.ResourceManager, MediaGroupModelValidatorRes.CultureInfo, nameof(IMediaGroupModel.DescriptionEnglish), 3000))
            .NotEmpty().WithMessage(validationHelper.GetRequiredMessage(MediaGroupModelValidatorRes.ResourceManager, MediaGroupModelValidatorRes.CultureInfo, nameof(IMediaGroupModel.DescriptionEnglish)));

        RuleFor(x => x.EventDateForInput)
            .NotEmpty().WithMessage(validationHelper.GetRequiredMessage(MediaGroupModelValidatorRes.ResourceManager, MediaGroupModelValidatorRes.CultureInfo, nameof(IMediaGroupModel.EventDateForInput)));
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<IMediaGroupModel>.CreateWithOptions((IMediaGroupModel)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
        {
            return Array.Empty<string>();
        }

        return result.Errors.Select(e => e.ErrorMessage);
    };
}