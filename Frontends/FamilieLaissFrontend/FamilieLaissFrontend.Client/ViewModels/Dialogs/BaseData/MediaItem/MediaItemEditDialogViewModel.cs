using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventAggregator.Blazor;
using FamilieLaissEnums;
using FamilieLaissFrontend.Client.Dialogs.Choose.Picture;
using FamilieLaissFrontend.Client.Dialogs.Choose.Video;
using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Enums;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissModels.EventAggregator.MediaItem;
using FamilieLaissModels.Models.MediaItem;
using FamilieLaissResources.Resources.ViewModels.Dialogs.BaseData.MediaItem;
using FamilieLaissServices.Extensions;
using FamilieLaissSharedUI.ViewModels;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Dialogs.BaseData.MediaItem;

public partial class MediaItemEditDialogViewModel(
    ISnackbar snackbarService,
    IMessageBoxService messageBoxService,
    IValidatorFl<IMediaItemModel> validator,
    IMediaItemDataService mediaItemService,
    ICategoryDataService categoryService,
    IEventAggregator eventAggregator,
    IDialogService dialogService)
    : ViewModelBase(snackbarService, messageBoxService)
{
    #region Services

    public readonly IValidatorFl<IMediaItemModel> Validator = validator;

    #endregion

    #region Parameters

    public MudDialogInstance MudDialog { get; set; } = default!;
    public bool IsInEditMode { get; set; }
    public IMediaItemModel? Model { get; set; }
    public long? MediaGroupId { get; set; }

    #endregion

    #region Public Properties

    public MudForm? Form { get; set; }

    [ObservableProperty] private EnumMediaType? _currentMediaType;

    [ObservableProperty] private IUploadPictureModel? _selectedPicture;

    [ObservableProperty] private IUploadVideoModel? _selectedVideo;

    [ObservableProperty] private List<ICategoryValueModel> _categoryValues = [];

    public IEnumerable<ICategoryValueModel>? SelectedCategoryValues { get; set; }

    #endregion

    #region Lifecycle

    public override async Task OnInitializedAsync()
    {
        if (!IsInEditMode)
        {
            Model = new MediaItemModel();
            Model.MediaGroupId = MediaGroupId;
            Model.OnlyFamily = false;
        }
        else
        {
            IsLoading = true;

            if (Model is not null)
            {
                await mediaItemService.GetMediaItemAsync(Model.Id)
                    .HandleSuccess(async (result) =>
                    {
                        if (Model.MediaType == EnumMediaType.Picture)
                        {
                            await categoryService.GetPhotoCategoriesWithValuesAsync()
                                .HandleStatus(APIResultErrorType.NoError, (resultCategories) =>
                                {
                                    CategoryValues.Clear();

                                    if (resultCategories is not null)
                                    {
                                        foreach (var category in resultCategories.ToList()
                                                     .OrderBy(x => x.LocalizedName))
                                        {
                                            if (category.CategoryValues is null) continue;
                                            foreach (var categoryValue in category.CategoryValues.OrderBy(x =>
                                                         x.LocalizedName))
                                            {
                                                categoryValue.Category = category;
                                                CategoryValues.Add(categoryValue);
                                            }
                                        }
                                    }

                                    if (Model.MediaItemCategoryValues is not null)
                                    {
                                        var catValues = new List<ICategoryValueModel>();
                                        foreach (var mediaItemCategoryValue in Model.MediaItemCategoryValues)
                                        {
                                            if (mediaItemCategoryValue.CategoryValue?.Id is not null)
                                            {
                                                catValues.Add(CategoryValues.First(x =>
                                                    x.Id == mediaItemCategoryValue.CategoryValue.Id));
                                            }
                                        }
                                    }

                                    Model = result;
                                    if (Model?.MediaType is null) return Task.CompletedTask;
                                    CurrentMediaType = Model.MediaType;

                                    if (CurrentMediaType == EnumMediaType.Picture)
                                    {
                                        SelectedPicture = Model.UploadPicture;
                                    }

                                    return Task.CompletedTask;
                                })
                                .HandleStatus(APIResultErrorType.NotAuthorized, (_) =>
                                {
                                    ShowErrorToast(MediaItemEditDialogViewModelRes.ToastLoadingErrorNotAuthorized);

                                    return Task.CompletedTask;
                                })
                                .HandleStatus(APIResultErrorType.ServerError, (_) =>
                                {
                                    ShowErrorToast(MediaItemEditDialogViewModelRes.ToastLoadingErrorServerError);

                                    return Task.CompletedTask;
                                })
                                .HandleStatus(APIResultErrorType.CommunicationError, (_) =>
                                {
                                    ShowErrorToast(MediaItemEditDialogViewModelRes.ToastLoadingErrorCommunication);

                                    return Task.CompletedTask;
                                });
                        }
                    })
                    .HandleStatus(APIResultErrorType.NotAuthorized, (_) =>
                    {
                        ShowErrorToast(MediaItemEditDialogViewModelRes.ToastLoadingErrorNotAuthorized);

                        return Task.CompletedTask;
                    })
                    .HandleStatus(APIResultErrorType.ServerError, (_) =>
                    {
                        ShowErrorToast(MediaItemEditDialogViewModelRes.ToastLoadingErrorServerError);

                        return Task.CompletedTask;
                    })
                    .HandleStatus(APIResultErrorType.CommunicationError, (_) =>
                    {
                        ShowErrorToast(MediaItemEditDialogViewModelRes.ToastLoadingErrorCommunication);

                        return Task.CompletedTask;
                    });
            }

            IsLoading = false;
        }
    }

    #endregion

    #region Abstract overrides

    public override void Dispose()
    {
    }

    #endregion

    #region Commands

    [RelayCommand]
    private async Task SaveAsync(bool stayInDialog)
    {
        IsSaving = true;

        if (Form is not null && Model is not null)
        {
            await Form.Validate();

            if (Form.IsValid)
            {
                if (!IsInEditMode)
                {
                    await mediaItemService
                        .AddMediaItemAsync(Model, SelectedCategoryValues?.Select(x => x.Id).ToList() ?? new())
                        .HandleStatus(APIResultErrorType.NoError, async () =>
                        {
                            ShowSuccessToast(string.Format(MediaItemEditDialogViewModelRes.ToastSaveAddSuccess,
                                Model.MediaType == EnumMediaType.Picture
                                    ? Model.UploadPicture?.Filename
                                    : Model.LocalizedName));

                            await eventAggregator.PublishAsync(new AggMediaItemCreated() { MediaItem = Model });
                        })
                        .HandleStatus(APIResultErrorType.NotAuthorized, () =>
                        {
                            ShowErrorToast(string.Format(
                                MediaItemEditDialogViewModelRes.ToastSaveAddErrorNotAuthorized,
                                Model.MediaType == EnumMediaType.Picture
                                    ? Model.UploadPicture?.Filename
                                    : Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.Conflict, () =>
                        {
                            ShowErrorToast(string.Format(
                                MediaItemEditDialogViewModelRes.ToastSaveAddErrorConflict,
                                Model.MediaType == EnumMediaType.Picture
                                    ? Model.UploadPicture?.Filename
                                    : Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.ServerError, () =>
                        {
                            ShowErrorToast(string.Format(
                                MediaItemEditDialogViewModelRes.ToastSaveAddErrorServer,
                                Model.MediaType == EnumMediaType.Picture
                                    ? Model.UploadPicture?.Filename
                                    : Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.CommunicationError, () =>
                        {
                            ShowErrorToast(string.Format(
                                MediaItemEditDialogViewModelRes.ToastSaveAddErrorCommunication,
                                Model.MediaType == EnumMediaType.Picture
                                    ? Model.UploadPicture?.Filename
                                    : Model.LocalizedName));

                            return Task.CompletedTask;
                        });
                }
                else
                {
                    await mediaItemService.UpdateMediaItemAsync(Model,
                            SelectedCategoryValues?.Select(x => x.Id).ToList() ?? new())
                        .HandleStatus(APIResultErrorType.NoError, async () =>
                        {
                            ShowSuccessToast(string.Format(MediaItemEditDialogViewModelRes.ToastSaveEditSuccess,
                                Model.LocalizedName));

                            await eventAggregator.PublishAsync(new AggMediaItemChanged() { MediaItem = Model });
                        })
                        .HandleStatus(APIResultErrorType.NotAuthorized, () =>
                        {
                            ShowErrorToast(string.Format(
                                MediaItemEditDialogViewModelRes.ToastSaveEditErrorNotAuthorized,
                                Model.MediaType == EnumMediaType.Picture
                                    ? Model.UploadPicture?.Filename
                                    : Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.NotFound, () =>
                        {
                            ShowErrorToast(string.Format(
                                MediaItemEditDialogViewModelRes.ToastSaveEditErrorNotFound,
                                Model.MediaType == EnumMediaType.Picture
                                    ? Model.UploadPicture?.Filename
                                    : Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.Conflict, () =>
                        {
                            ShowErrorToast(string.Format(
                                MediaItemEditDialogViewModelRes.ToastSaveEditErrorConflict,
                                Model.MediaType == EnumMediaType.Picture
                                    ? Model.UploadPicture?.Filename
                                    : Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.ServerError, () =>
                        {
                            ShowErrorToast(string.Format(
                                MediaItemEditDialogViewModelRes.ToastSaveEditErrorServer,
                                Model.MediaType == EnumMediaType.Picture
                                    ? Model.UploadPicture?.Filename
                                    : Model.LocalizedName));

                            return Task.CompletedTask;
                        })
                        .HandleStatus(APIResultErrorType.CommunicationError, () =>
                        {
                            ShowErrorToast(string.Format(
                                MediaItemEditDialogViewModelRes.ToastSaveEditErrorCommunication,
                                Model.MediaType == EnumMediaType.Picture
                                    ? Model.UploadPicture?.Filename
                                    : Model.LocalizedName));

                            return Task.CompletedTask;
                        });
                }

                if (stayInDialog)
                {
                    CategoryValues.Clear();
                    CurrentMediaType = null;
                    SelectedPicture = null;
                    SelectedVideo = null;
                    Model = new MediaItemModel();
                }
                else
                {
                    MudDialog.Close();
                }
            }
        }

        SaveMode = stayInDialog ? EnumSaveMode.SaveAndContinue : EnumSaveMode.Save;

        IsSaving = false;
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        if (Form is { IsTouched: true })
        {
            var title = !IsInEditMode
                ? MediaItemEditDialogViewModelRes.AlertCancelAddTitle
                : MediaItemEditDialogViewModelRes.AlertCancelEditTitle;
            var message = !IsInEditMode
                ? MediaItemEditDialogViewModelRes.AlertCancelAddMessage
                : MediaItemEditDialogViewModelRes.AlertCancelEditMessage;
            var buttonCancel = !IsInEditMode
                ? MediaItemEditDialogViewModelRes.AlertCancelAddCancel
                : MediaItemEditDialogViewModelRes.AlertCancelEditCancel;
            var buttonConfirm = !IsInEditMode
                ? MediaItemEditDialogViewModelRes.AlertCancelAddConfirm
                : MediaItemEditDialogViewModelRes.AlertCancelEditConfirm;

            var dialogResult =
                await QuestionConfirmRed(title, message, buttonConfirm, buttonCancel, Icons.Material.Filled.Cancel);

            if (dialogResult.HasValue && dialogResult.Value)
            {
                MudDialog.Cancel();
            }
        }
        else
        {
            MudDialog.Cancel();
        }
    }

    [RelayCommand]
    private async Task ShowChoosePictureDialogAsync()
    {
        var dialogOptions = GetDialogOptions();
        dialogOptions.MaxWidth = MaxWidth.ExtraExtraLarge;

        var dialog =
            await dialogService.ShowAsync<ChoosePictureDialog>(MediaItemEditDialogViewModelRes.ChoosePicture,
                dialogOptions);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await categoryService.GetPhotoCategoriesWithValuesAsync()
                .HandleStatus(APIResultErrorType.NoError, (resultCategories) =>
                {
                    CategoryValues.Clear();

                    if (resultCategories is not null)
                    {
                        foreach (var category in resultCategories.ToList().OrderBy(x => x.LocalizedName))
                        {
                            if (category.CategoryValues is not null)
                            {
                                foreach (var categoryValue in category.CategoryValues.OrderBy(x => x.LocalizedName))
                                {
                                    categoryValue.Category = category;
                                    CategoryValues.Add(categoryValue);
                                }
                            }
                        }
                    }

                    SelectedPicture = (IUploadPictureModel)result.Data;
                    CurrentMediaType = EnumMediaType.Picture;
                    if (Model is not null)
                    {
                        Model.MediaType = CurrentMediaType;
                        Model.UploadPicture = SelectedPicture;
                    }

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.NotAuthorized, (_) =>
                {
                    ShowErrorToast(MediaItemEditDialogViewModelRes.ToastLoadingErrorNotAuthorized);

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.ServerError, (_) =>
                {
                    ShowErrorToast(MediaItemEditDialogViewModelRes.ToastLoadingErrorServerError);

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.CommunicationError, (_) =>
                {
                    ShowErrorToast(MediaItemEditDialogViewModelRes.ToastLoadingErrorCommunication);

                    return Task.CompletedTask;
                });
        }
    }

    [RelayCommand]
    private async Task ShowChooseVideoDialogAsync()
    {
        var dialogOptions = GetDialogOptions();
        dialogOptions.MaxWidth = MaxWidth.ExtraExtraLarge;

        var dialog =
            await dialogService.ShowAsync<ChooseVideoDialog>(MediaItemEditDialogViewModelRes.ChooseVideo,
                dialogOptions);
        var result = await dialog.Result;
        if (!result.Canceled)
        {
            await categoryService.GetVideoCategoriesWithValuesAsync()
                .HandleStatus(APIResultErrorType.NoError, (resultCategories) =>
                {
                    CategoryValues.Clear();

                    if (resultCategories is not null)
                    {
                        foreach (var category in resultCategories.ToList().OrderBy(x => x.LocalizedName))
                        {
                            if (category.CategoryValues is null) continue;
                            foreach (var categoryValue in category.CategoryValues.OrderBy(x => x.LocalizedName))
                            {
                                categoryValue.Category = category;
                                CategoryValues.Add(categoryValue);
                            }
                        }
                    }

                    SelectedVideo = (IUploadVideoModel)result.Data;
                    CurrentMediaType = EnumMediaType.Video;
                    if (Model is null) return Task.CompletedTask;
                    Model.MediaType = CurrentMediaType;
                    Model.UploadVideo = SelectedVideo;

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.NotAuthorized, (_) =>
                {
                    ShowErrorToast(MediaItemEditDialogViewModelRes.ToastLoadingErrorNotAuthorized);

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.ServerError, (_) =>
                {
                    ShowErrorToast(MediaItemEditDialogViewModelRes.ToastLoadingErrorServerError);

                    return Task.CompletedTask;
                })
                .HandleStatus(APIResultErrorType.CommunicationError, (_) =>
                {
                    ShowErrorToast(MediaItemEditDialogViewModelRes.ToastLoadingErrorCommunication);

                    return Task.CompletedTask;
                });
        }
    }

    #endregion
}