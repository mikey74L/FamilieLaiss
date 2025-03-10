using FamilieLaissInterfaces;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissResources.Resources.Validation.Blog;
using FluentValidation;

namespace FamilieLaissFrontend.Client.Validation.Blog;

public class BlogItemModelValidator : AbstractValidator<IBlogItemModel>, IValidatorFl<IBlogItemModel>
{
    public BlogItemModelValidator(IValidationHelperService validationHelper)
    {
        RuleFor(x => x.HeaderGerman)
            .MaximumLength(300).WithMessage(validationHelper.GetMaxLengthMessage(BlogItemModelValidatorRes.ResourceManager, BlogItemModelValidatorRes.CultureInfo, nameof(IBlogItemModel.HeaderGerman), 200))
            .NotEmpty().WithMessage(validationHelper.GetRequiredMessage(BlogItemModelValidatorRes.ResourceManager, BlogItemModelValidatorRes.CultureInfo, nameof(IBlogItemModel.HeaderGerman)));

        RuleFor(x => x.HeaderEnglish)
            .MaximumLength(300).WithMessage(validationHelper.GetMaxLengthMessage(BlogItemModelValidatorRes.ResourceManager, BlogItemModelValidatorRes.CultureInfo, nameof(IBlogItemModel.HeaderEnglish), 200))
            .NotEmpty().WithMessage(validationHelper.GetRequiredMessage(BlogItemModelValidatorRes.ResourceManager, BlogItemModelValidatorRes.CultureInfo, nameof(IBlogItemModel.HeaderEnglish)));

        RuleFor(x => x.TextGerman)
          .NotEmpty().WithMessage(validationHelper.GetRequiredMessage(BlogItemModelValidatorRes.ResourceManager, BlogItemModelValidatorRes.CultureInfo, nameof(IBlogItemModel.TextGerman)));

        RuleFor(x => x.TextEnglish)
         .NotEmpty().WithMessage(validationHelper.GetRequiredMessage(BlogItemModelValidatorRes.ResourceManager, BlogItemModelValidatorRes.CultureInfo, nameof(IBlogItemModel.TextEnglish)));
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<IBlogItemModel>.CreateWithOptions((IBlogItemModel)model, x => x.IncludeProperties(propertyName)));

        if (result.IsValid)
        {
            return Array.Empty<string>();
        }

        return result.Errors.Select(e => e.ErrorMessage);
    };
}
