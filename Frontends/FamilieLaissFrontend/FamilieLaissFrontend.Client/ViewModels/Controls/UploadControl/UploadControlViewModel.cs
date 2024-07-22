using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FamilieLaissEnums;
using FamilieLaissInterfaces;
using FamilieLaissInterfaces.Models.Components;
using FamilieLaissInterfaces.Services;
using FamilieLaissResources.Resources.ViewModels.Controls.UploadControl;
using FamilieLaissSharedUI.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Controls.UploadControl;

public partial class UploadControlViewModel : ViewModelBase
{
    #region Services

    private readonly IGlobalFunctions _globalFunctions;
    private readonly IFileUploaderHelperService _fileUploadHelperService;

    #endregion

    #region Private Fields

    private double _maxFileSize;

    #endregion

    #region Parameters

    public bool UploadMultipleFiles { get; set; }
    public int UploadFileMaxSelectCount { get; set; }
    public EnumUploadType UploadType { get; set; }
    public int ChunkSize { get; set; }
    public EventCallback UploadFinished { get; set; }
    public EventCallback UploadStarted { get; set; }
    public EventCallback UploadFinishedWithFailure { get; set; }
    public EventCallback UploadFinishedWithSuccess { get; set; }
    public EventCallback<string> FileUploadedWithSuccess { get; set; }
    public EventCallback<string> FileUploadedWithError { get; set; }
    public EventCallback<int> UploadCountChanged { get; set; }

    #endregion

    #region Public Properties

    [ObservableProperty] private string _idInputFile;

    [ObservableProperty] private string _allowedFileExtensions;

    public string StatusText
    {
        get
        {
            var returnValue = IsUploading
                ? UploadFileInfoRes.StatusControlUploading
                : UploadFileInfoRes.StatusControlStandard;

            return returnValue;
        }
    }

    [ObservableProperty] private bool _isUploading;

    [ObservableProperty] private List<IUploadFileInfo> _uploadFileInfoList = [];

    #endregion

    #region C'tor

    public UploadControlViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService,
        IGlobalFunctions globalFunctions, IFileUploaderHelperService fileUploadHelperService) : base(snackbarService,
        messageBoxService)
    {
        this._globalFunctions = globalFunctions;
        this._fileUploadHelperService = fileUploadHelperService;

        UploadMultipleFiles = false;
        UploadType = EnumUploadType.Picture;
        UploadFileMaxSelectCount = 40;
        IdInputFile = "TestId";
        IsUploading = false;
        ChunkSize = 1024 * 100;
        AllowedFileExtensions = ".jpeg,.jpg,.png,.bmp";
    }

    #endregion

    #region Lifecycle

    public override void OnParametersSet()
    {
        base.OnParametersSet();

        switch (UploadType)
        {
            case EnumUploadType.Picture:
                AllowedFileExtensions = ".jpeg,.jpg,.png,.bmp";
                _maxFileSize = (double)1024 * 1024 * 30;
                if (ChunkSize == 1024 * 100)
                {
                    ChunkSize = 1024 * 1024;
                }

                break;
            case EnumUploadType.Video:
                AllowedFileExtensions = ".mp4,.mov,.m4v,.avi,.mpg,.mpeg,.mts,.wmv";
                _maxFileSize = (double)1024 * 1024 * 1024 * 5;
                if (ChunkSize == 1024 * 100)
                {
                    ChunkSize = 1024 * 1024 * 2;
                }

                break;
        }
    }

    #endregion

    #region Private Methods

    private async Task UploadFile(IUploadFileInfo fileInfoToUpload)
    {
        var uploadResult =
            await _fileUploadHelperService.UploadFileAsync(fileInfoToUpload, ChunkSize, UploadType, NotifyStateChanged);

        if (!uploadResult)
        {
            fileInfoToUpload.SetErrorState();

            ShowErrorToast(string.Format(UploadControlViewModelRes.ToastUploadError,
                fileInfoToUpload.FileName));

            await FileUploadedWithError.InvokeAsync(fileInfoToUpload.FileName);
        }
        else
        {
            fileInfoToUpload.SetSuccessState();

            ShowSuccessToast(string.Format(UploadControlViewModelRes.ToastUploadSuccess,
                fileInfoToUpload.FileName));

            await FileUploadedWithSuccess.InvokeAsync(fileInfoToUpload.FileName);
        }
    }

    #endregion

    #region Commands

    [RelayCommand]
    private async Task RemoveItemAsync(long id)
    {
        bool removeItem = true;

        var itemToRemove = UploadFileInfoList.Single(x => x.Id == id);

        if (!itemToRemove.IsUploaded)
        {
            var result = await QuestionConfirmRed(
                UploadControlViewModelRes.AlertRemoveItemTitle,
                UploadControlViewModelRes.AlertRemoveItemMessage,
                UploadControlViewModelRes.AlertRemoveItemConfirm,
                UploadControlViewModelRes.AlertRemoveItemCancel);

            removeItem = result is not null && result.Value;
        }

        if (removeItem)
        {
            await itemToRemove.DisposeData();

            UploadFileInfoList.Remove(itemToRemove);

            await UploadCountChanged.InvokeAsync(UploadFileInfoList.Count(x => x.Status == EnumUploadStatus.Added));
        }
    }

    [RelayCommand]
    private async Task StartUploadAsync()
    {
        IsUploading = true;

        await UploadStarted.InvokeAsync();

        foreach (var itemToUpload in UploadFileInfoList.Where(x => !x.IsUploaded || x.WithError))
        {
            await UploadFile(itemToUpload);

            await UploadCountChanged.InvokeAsync(UploadFileInfoList.Count(x => x.Status == EnumUploadStatus.Added));
        }

        IsUploading = false;

        if (UploadFileInfoList.Count(x => x.WithError) > 0)
        {
            await UploadFinishedWithFailure.InvokeAsync();
        }
        else
        {
            await UploadFinishedWithSuccess.InvokeAsync();
        }

        await UploadFinished.InvokeAsync();
    }

    [RelayCommand]
    private async Task ClearListAsync()
    {
        bool clearList = true;

        if (UploadFileInfoList.Any(x => !x.IsUploaded))
        {
            var result = await QuestionConfirmRed(
                UploadControlViewModelRes.AlertClearListTitle,
                UploadControlViewModelRes.AlertClearListMessage,
                UploadControlViewModelRes.AlertClearListConfirm,
                UploadControlViewModelRes.AlertClearListCancel);

            clearList = result is not null && result.Value;
        }

        if (clearList)
        {
            foreach (var itemToRemove in UploadFileInfoList)
            {
                await itemToRemove.DisposeData();
            }

            UploadFileInfoList.Clear();

            await UploadCountChanged.InvokeAsync(UploadFileInfoList.Count(x => x.Status == EnumUploadStatus.Added));
        }
    }

    [RelayCommand]
    private async Task SelectFileAsync(InputFileChangeEventArgs args)
    {
        var fileListTemp = new List<IBrowserFile>();

        if (UploadMultipleFiles)
        {
            foreach (var item in args.GetMultipleFiles(UploadFileMaxSelectCount))
            {
                fileListTemp.Add(item);
            }
        }
        else
        {
            fileListTemp.Add(args.File);
        }

        var allowedTypesList = AllowedFileExtensions.Split(",").ToList();

        foreach (var item in fileListTemp)
        {
            var couldAddFile = false;

            foreach (var fileTypeItem in allowedTypesList)
            {
                if (item.Name.ToUpper().Split('.')[1] == fileTypeItem.ToUpper().Replace(".", ""))
                {
                    couldAddFile = true;
                }
            }

            if (!couldAddFile)
            {
                ShowWarningToast(string.Format(UploadControlViewModelRes.ToastFiletypeWarning, item.Name));
            }

            if (couldAddFile && item.Size > _maxFileSize)
            {
                ShowWarningToast(string.Format(UploadControlViewModelRes.ToastFilesizeWarning, item.Name,
                    _globalFunctions.GetFileSizeAsString(item.Size)));

                couldAddFile = false;
            }

            if (couldAddFile)
            {
                var uploadFile =
                    await _fileUploadHelperService.CreateUploadFileAsync(item, UploadType);

                if (uploadFile is not null)
                {
                    UploadFileInfoList.Add(uploadFile);

                    var count = UploadFileInfoList.Count(x => x.Status == EnumUploadStatus.Added);
                    await UploadCountChanged.InvokeAsync(count);
                }
                else
                {
                    ShowErrorToast(string.Format(UploadControlViewModelRes.ToastGetIdError, item.Name));
                }
            }
        }

        NotifyStateChanged();
    }

    #endregion

    #region Abstract overrides

    public override void Dispose()
    {
    }

    #endregion
}