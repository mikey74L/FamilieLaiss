using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissResources.Resources.Validation.MediaItem;
using FamilieLaissServices.Extensions;
using FluentValidation;

namespace FamilieLaissFrontend.Client.Validation.MediaItem;

public class MediaItemModelValidator : AbstractValidator<IMediaItemModel>, IValidatorFl<IMediaItemModel>
{
    public MediaItemModelValidator(IValidationHelperService validationHelper, IMediaItemDataService mediaItemService)
    {
        async Task<bool> CallValidationForGerman(IMediaItemModel model, string? value, CancellationToken cancel)
        {
            bool returnValue = true;

            await mediaItemService.GermanMediaItemNameExistsAsync(model.Id, model.MediaGroup?.Id ?? 0, value)
                .HandleSuccess((result) =>
                {
                    returnValue = !result;

                    return Task.CompletedTask;
                });

            return returnValue;
        }

        async Task<bool> CallValidationForEnglish(IMediaItemModel model, string? value, CancellationToken cancel)
        {
            bool returnValue = true;

            await mediaItemService.EnglishMediaItemNameExistsAsync(model.Id, model.MediaGroup?.Id ?? 0, value)
                .HandleSuccess((result) =>
                {
                    returnValue = !result;

                    return Task.CompletedTask;
                });

            return returnValue;
        }

        RuleFor(x => x.NameGerman)
            .MaximumLength(200)
              .WithMessage(validationHelper.GetMaxLengthMessage(MediaItemModelValidatorRes.ResourceManager, MediaItemModelValidatorRes.CultureInfo, nameof(IMediaItemModel.NameGerman), 200))
            .NotEmpty()
              .When(model => model.MediaType == EnumMediaType.Video)
              .WithMessage(validationHelper.GetRequiredMessage(MediaItemModelValidatorRes.ResourceManager, MediaItemModelValidatorRes.CultureInfo, nameof(IMediaItemModel.NameGerman)))
            .MustAsync(CallValidationForGerman)
              .When(model => model.MediaType == EnumMediaType.Video)
              .WithMessage(validationHelper.GetAlreadyExistsMessage(MediaItemModelValidatorRes.ResourceManager, MediaItemModelValidatorRes.CultureInfo, nameof(IMediaItemModel.NameGerman)));
        RuleFor(x => x.NameEnglish)
            .MaximumLength(200)
              .WithMessage(validationHelper.GetMaxLengthMessage(MediaItemModelValidatorRes.ResourceManager, MediaItemModelValidatorRes.CultureInfo, nameof(IMediaItemModel.NameEnglish), 200))
            .NotEmpty()
              .When(model => model.MediaType == EnumMediaType.Video)
              .WithMessage(validationHelper.GetRequiredMessage(MediaItemModelValidatorRes.ResourceManager, MediaItemModelValidatorRes.CultureInfo, nameof(IMediaItemModel.NameEnglish)))
            .MustAsync(CallValidationForEnglish)
              .When(model => model.MediaType == EnumMediaType.Video)
              .WithMessage(validationHelper.GetAlreadyExistsMessage(MediaItemModelValidatorRes.ResourceManager, MediaItemModelValidatorRes.CultureInfo, nameof(IMediaItemModel.NameEnglish)));
        RuleFor(x => x.DescriptionGerman)
            .MaximumLength(2000)
              .WithMessage(validationHelper.GetMaxLengthMessage(MediaItemModelValidatorRes.ResourceManager, MediaItemModelValidatorRes.CultureInfo, nameof(IMediaItemModel.DescriptionGerman), 2000))
            .NotEmpty()
              .When(model => model.MediaType == EnumMediaType.Video)
              .WithMessage(validationHelper.GetRequiredMessage(MediaItemModelValidatorRes.ResourceManager, MediaItemModelValidatorRes.CultureInfo, nameof(IMediaItemModel.DescriptionGerman)));
        RuleFor(x => x.DescriptionEnglish)
            .MaximumLength(2000)
              .WithMessage(validationHelper.GetMaxLengthMessage(MediaItemModelValidatorRes.ResourceManager, MediaItemModelValidatorRes.CultureInfo, nameof(IMediaItemModel.DescriptionEnglish), 2000))
            .NotEmpty()
              .When(model => model.MediaType == EnumMediaType.Video)
              .WithMessage(validationHelper.GetRequiredMessage(MediaItemModelValidatorRes.ResourceManager, MediaItemModelValidatorRes.CultureInfo, nameof(IMediaItemModel.DescriptionEnglish)));
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<IMediaItemModel>.CreateWithOptions((IMediaItemModel)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
        {
            return Array.Empty<string>();
        }

        return result.Errors.Select(e => e.ErrorMessage);
    };
}
