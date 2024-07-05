using FamilieLaissFrontend.Client.ViewModels.Controls.Filter;
using FamilieLaissFrontend.Client.ViewModels.Controls.PictureControl;
using FamilieLaissFrontend.Client.ViewModels.Controls.UploadControl;
using FamilieLaissFrontend.Client.ViewModels.Controls.User;
using FamilieLaissFrontend.Client.ViewModels.Controls.VideoControl;
using FamilieLaissFrontend.Client.ViewModels.Controls.VideoPlayerControl;
using FamilieLaissFrontend.Client.ViewModels.Dialogs.BaseData.Category;
using FamilieLaissFrontend.Client.ViewModels.Dialogs.BaseData.CategoryValue;
using FamilieLaissFrontend.Client.ViewModels.Dialogs.BaseData.MediaGroup;
using FamilieLaissFrontend.Client.ViewModels.Dialogs.BaseData.MediaItem;
using FamilieLaissFrontend.Client.ViewModels.Dialogs.Choose.Picture;
using FamilieLaissFrontend.Client.ViewModels.Dialogs.PictureInfo;
using FamilieLaissFrontend.Client.ViewModels.Pages.BaseData.Category;
using FamilieLaissFrontend.Client.ViewModels.Pages.BaseData.CategoryValue;
using FamilieLaissFrontend.Client.ViewModels.Pages.BaseData.Media;
using FamilieLaissFrontend.Client.ViewModels.Pages.BaseData.Upload.Picture;
using FamilieLaissFrontend.Client.ViewModels.Pages.BaseData.Upload.Video;
using FamilieLaissFrontend.Client.ViewModels.Pages.Converter.Picture;
using FamilieLaissFrontend.Client.ViewModels.Pages.Converter.Video;
using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissFrontend.Client.Extensions;

public static class FlApplicationViewModelExtension
{
    public static IServiceCollection AddViewModels(this IServiceCollection serviceCollection)
    {
        #region Controls
        #region Filter
        serviceCollection.AddTransient<FilterControlViewModel>();
        serviceCollection.AddTransient<FilterDateRangeControlViewModel>();
        serviceCollection.AddTransient<FilterGroupControlViewModel>();
        serviceCollection.AddTransient<FilterNumberListControlViewModel>();
        serviceCollection.AddTransient<FilterNumberOnlyControlViewModel>();
        serviceCollection.AddTransient<FilterNumberRangeControlViewModel>();
        serviceCollection.AddTransient<FilterStringOnlyControlViewModel>();
        serviceCollection.AddTransient<FilterAppBarControlViewModel<IUploadPictureModel, UploadPictureSortInput, UploadPictureFilterInput>>();
        serviceCollection.AddTransient<FilterAppBarControlViewModel<IUploadVideoModel, UploadVideoSortInput, UploadVideoFilterInput>>();
        #endregion

        #region PictureControl
        serviceCollection.AddTransient<PictureControlActionsViewModel>();
        serviceCollection.AddTransient<PictureControlHeaderViewModel>();
        serviceCollection.AddTransient<PictureControlMediaViewModel>();
        serviceCollection.AddTransient<PictureControlViewModel>();
        #endregion

        #region UploadControl
        serviceCollection.AddTransient<UploadControlViewModel>();
        #endregion

        #region User
        serviceCollection.AddTransient<LoginLogoutControlViewModel>();
        serviceCollection.AddTransient<UserSettingsControlViewModel>();
        #endregion

        #region VideoControl
        serviceCollection.AddTransient<VideoControlActionsViewModel>();
        serviceCollection.AddTransient<VideoControlHeaderViewModel>();
        serviceCollection.AddTransient<VideoControlMediaViewModel>();
        serviceCollection.AddTransient<VideoControlViewModel>();
        #endregion

        #region VideoPlayerControl
        serviceCollection.AddTransient<VideoPlayerControlViewModel>();
        #endregion
        #endregion

        #region Dialogs
        #region BaseData
        #region Category
        serviceCollection.AddTransient<CategoryEditDialogViewModel>();
        #endregion

        #region CategoryValue
        serviceCollection.AddTransient<CategoryValueEditDialogViewModel>();
        #endregion

        #region MediaGroup
        serviceCollection.AddTransient<MediaGroupEditDialogViewModel>();
        #endregion

        #region MediaItem
        serviceCollection.AddTransient<MediaItemEditDialogViewModel>();
        #endregion
        #endregion

        #region PictureInfo
        serviceCollection.AddTransient<PictureInfoDialogViewModel>();
        serviceCollection.AddTransient<PictureInfoGeneralPageViewModel>();
        serviceCollection.AddTransient<PictureInfoGeneralPartViewModel>();
        serviceCollection.AddTransient<PictureInfoMapPageViewModel>();
        #endregion

        #region Choose
        serviceCollection.AddTransient<ChoosePictureDialogViewModel>();
        #endregion
        #endregion

        #region Pages
        #region BaseData
        #region Category
        serviceCollection.AddTransient<CategoryListViewModel>();
        #endregion

        #region CategoryValue
        serviceCollection.AddTransient<CategoryValueListViewModel>();
        #endregion

        #region Media
        serviceCollection.AddTransient<MediaListViewModel>();
        serviceCollection.AddTransient<MediaItemListViewModel>();
        #endregion

        #region Upload
        #region Picture
        serviceCollection.AddTransient<PictureUploadListDrawersViewModel>();
        serviceCollection.AddTransient<PictureUploadListViewModel>();
        serviceCollection.AddTransient<PictureUploadPageViewModel>();
        #endregion

        #region Video
        serviceCollection.AddTransient<VideoUploadListDrawersViewModel>();
        serviceCollection.AddTransient<VideoUploadListViewModel>();
        serviceCollection.AddTransient<VideoUploadPageViewModel>();
        #endregion
        #endregion
        #endregion

        #region Converter
        #region Picture
        serviceCollection.AddTransient<PictureConverterStatusViewModel>();
        serviceCollection.AddTransient<PictureConvertStatusListViewModel>();
        #endregion

        #region Video
        serviceCollection.AddTransient<VideoConverterStatusViewModel>();
        serviceCollection.AddTransient<VideoConvertStatusListViewModel>();
        #endregion
        #endregion
        #endregion

        return serviceCollection;
    }
}
