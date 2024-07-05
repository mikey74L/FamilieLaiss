using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Services;
using FamilieLaissServices;
using FamilieLaissServices.DataServices;

namespace FamilieLaissFrontend.Client.Extensions;

public static class FlApplicationServicesExtension
{
    public static IServiceCollection AddAppServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IGeneralDataService, GeneralDataService>();
        serviceCollection.AddTransient<ICategoryDataService, CategoryDataService>();
        serviceCollection.AddTransient<ICategoryValueDataService, CategoryValueDataService>();
        serviceCollection.AddTransient<IFileUploadDataService, FileUploadDataService>();
        serviceCollection.AddTransient<IPictureConvertStatusDataService, PictureConvertStatusDataService>();
        serviceCollection.AddTransient<IVideoConvertStatusDataService, VideoConvertStatusDataService>();
        serviceCollection.AddTransient<IUserSettingsDataService, UserSettingsDataService>();
        serviceCollection.AddTransient<IMediaGroupDataService, MediaGroupDataService>();
        serviceCollection.AddTransient<IMediaItemDataService, MediaItemDataService>();

        serviceCollection.AddTransient<IUploadPictureDataService, UploadPictureDataService>();
        serviceCollection.AddTransient<IUploadVideoDataService, UploadVideoDataService>();
        //serviceCollection.AddSingleton<IBlogService, BlogService>();

        serviceCollection.AddTransient<IUrlHelperService, UrlHelperService>();

        serviceCollection.AddSingleton<IValidationHelperService, ValidationHelperService>();

        //serviceCollection.AddTransient<IConvertPictureStatusService, ConvertPictureStatusService>();
        //serviceCollection.AddTransient<IConvertVideoStatusService, ConvertVideoStatusService>();

        serviceCollection.AddTransient<IGraphQlSortAndFilterServiceFactory, GraphQlSortAndFilterServiceFactory>();

        serviceCollection.AddSingleton<IFileUploaderHelperService, FileUploadHelperService>();
        serviceCollection.AddSingleton<IMessageBoxService, MessageBoxService>();
        serviceCollection.AddTransient<IRuntimeEnvironmentService, RuntimeEnvironmentService>();
        serviceCollection.AddSingleton<ISecretDataService, SecretDataService>();
        serviceCollection.AddScoped<IUserSettingsService, UserSettingsService>();
        serviceCollection.AddSingleton<IUserSettingsStoreService, UserSettingsStoreService>();
        serviceCollection.AddEventAggregator();

        return serviceCollection;
    }
}