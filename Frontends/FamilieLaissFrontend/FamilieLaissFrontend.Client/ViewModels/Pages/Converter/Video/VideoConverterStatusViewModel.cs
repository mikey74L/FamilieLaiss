using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissSharedUI.Helper;
using FamilieLaissSharedUI.ViewModels;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Pages.Converter.Video;

public partial class VideoConverterStatusViewModel : ViewModelBase
{
    #region Services
    private readonly IVideoConvertStatusDataService statusService;
    #endregion

    #region Public Properties
    [ObservableProperty]
    private ExtendedObservableCollection<IVideoConvertStatusModel> _listWaiting = [];
    [ObservableProperty]
    private ExtendedObservableCollection<IVideoConvertStatusModel> _listSuccess = [];
    [ObservableProperty]
    private ExtendedObservableCollection<IVideoConvertStatusModel> _listError = [];
    [ObservableProperty]
    private IVideoConvertStatusModel? _currentConversionItem;
    #endregion

    #region C'tor
    public VideoConverterStatusViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService,
        IVideoConvertStatusDataService statusService) : base(snackbarService, messageBoxService)
    {
        this.statusService = statusService;
    }
    #endregion

    #region Lifecycle
    public override void OnInitialized()
    {
        base.OnInitialized();

        statusService.OpenSubscriptionForWaiting(CallBackForWaitingItems);
        statusService.OpenSubscriptionForSuccess(CallBackForSuccessItems);
        statusService.OpenSubscriptionForError(CallBackForErrorItems);
        statusService.OpenSubscriptionForCurrent(CallBackForCurrentItem);
    }
    #endregion

    #region Private Methods
    private void RemoveFromCurrentItem(IVideoConvertStatusModel modelToRemove)
    {
        if (CurrentConversionItem != null && CurrentConversionItem.Id == modelToRemove.Id)
        {
            CurrentConversionItem = null;
        }
    }

    private void RemoveItemFromWaiting(IVideoConvertStatusModel modelToRemove)
    {
        var foundWaiting = ListWaiting.FirstOrDefault(x => x.Id == modelToRemove.Id);
        if (foundWaiting is not null)
        {
            ListWaiting.Remove(foundWaiting);
        }
    }

    private void AddItemOnlyIfNotExist(ExtendedObservableCollection<IVideoConvertStatusModel> list,
        IVideoConvertStatusModel modelToAdd)
    {
        if (list.All(x => x.Id != modelToAdd.Id))
        {
            list.Add(modelToAdd);
        }
    }
    #endregion

    #region Public Methods
    public void CallBackForWaitingItems(IVideoConvertStatusModel waitingItem)
    {
        AddItemOnlyIfNotExist(ListWaiting, waitingItem);

        NotifyStateChanged();
    }

    public void CallBackForSuccessItems(IVideoConvertStatusModel successItem)
    {
        RemoveFromCurrentItem(successItem);

        AddItemOnlyIfNotExist(ListSuccess, successItem);

        NotifyStateChanged();
    }

    public void CallBackForErrorItems(IVideoConvertStatusModel errorItem)
    {
        RemoveFromCurrentItem(errorItem);

        AddItemOnlyIfNotExist(ListError, errorItem);

        NotifyStateChanged();
    }

    public void CallBackForCurrentItem(IVideoConvertStatusModel currentItem)
    {
        RemoveItemFromWaiting(currentItem);

        CurrentConversionItem = currentItem;
    }
    #endregion

    #region Commands
    [RelayCommand]
    private Task BeforeInternalNavigation(LocationChangingContext context)
    {
        statusService.CloseSubscriptionForWaiting();
        statusService.CloseSubscriptionForSuccess();
        statusService.CloseSubscriptionForError();
        statusService.CloseSubscriptionForCurrent();

        return Task.CompletedTask;
    }
    #endregion

    #region Abstract overrides
    public override void Dispose()
    {
    }
    #endregion
}
