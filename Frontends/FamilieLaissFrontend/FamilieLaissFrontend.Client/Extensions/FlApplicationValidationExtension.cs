using FamilieLaissFrontend.Client.Validation.Category;
using FamilieLaissFrontend.Client.Validation.CategoryValue;
using FamilieLaissFrontend.Client.Validation.MediaGroup;
using FamilieLaissFrontend.Client.Validation.MediaItem;
using FamilieLaissInterfaces;
using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissFrontend.Client.Extensions;

public static class FlApplicationValidationExtension
{
    public static IServiceCollection AddFluentValidators(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IValidatorFl<ICategoryModel>, CategoryModelValidator>();
        serviceCollection.AddTransient<IValidatorFl<ICategoryValueModel>, CategoryValueModelValidator>();
        serviceCollection.AddTransient<IValidatorFl<IMediaGroupModel>, MediaGroupModelValidator>();
        serviceCollection.AddTransient<IValidatorFl<IMediaItemModel>, MediaItemModelValidator>();
        //serviceCollection.AddTransient<IValidatorFl<IBlogItemModel>, BlogItemModelValidator>();

        return serviceCollection;
    }
}