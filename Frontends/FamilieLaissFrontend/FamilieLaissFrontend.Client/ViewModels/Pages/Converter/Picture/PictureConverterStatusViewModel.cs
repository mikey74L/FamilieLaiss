using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissSharedUI.Helper;
using FamilieLaissSharedUI.ViewModels;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Pages.Converter.Picture;

public partial class PictureConverterStatusViewModel : ViewModelBase
{
    #region Services
    private readonly IPictureConvertStatusDataService statusService;
    #endregion

    #region Public Properties
    [ObservableProperty]
    private ExtendedObservableCollection<IPictureConvertStatusModel> _listWaiting = [];
    [ObservableProperty]
    private ExtendedObservableCollection<IPictureConvertStatusModel> _listSuccess = [];
    [ObservableProperty]
    private ExtendedObservableCollection<IPictureConvertStatusModel> _listError = [];
    [ObservableProperty]
    private IPictureConvertStatusModel? _currentConversionItem;
    #endregion

    #region C'tor
    public PictureConverterStatusViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService,
        IPictureConvertStatusDataService statusService) : base(snackbarService, messageBoxService)
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
    private void RemoveFromCurrentItem(IPictureConvertStatusModel modelToRemove)
    {
        if (CurrentConversionItem != null && CurrentConversionItem.Id == modelToRemove.Id)
        {
            CurrentConversionItem = null;
        }
    }

    private void RemoveItemFromWaiting(IPictureConvertStatusModel modelToRemove)
    {
        var foundWaiting = ListWaiting.FirstOrDefault(x => x.Id == modelToRemove.Id);
        if (foundWaiting is not null)
        {
            ListWaiting.Remove(foundWaiting);
        }
    }

    private void AddItemOnlyIfNotExist(ExtendedObservableCollection<IPictureConvertStatusModel> list,
        IPictureConvertStatusModel modelToAdd)
    {
        if (list.All(x => x.Id != modelToAdd.Id))
        {
            list.Add(modelToAdd);
        }
    }
    #endregion

    #region Public Methods
    public void CallBackForWaitingItems(IPictureConvertStatusModel waitingItem)
    {
        AddItemOnlyIfNotExist(ListWaiting, waitingItem);

        NotifyStateChanged();
    }

    public void CallBackForSuccessItems(IPictureConvertStatusModel successItem)
    {
        RemoveFromCurrentItem(successItem);

        AddItemOnlyIfNotExist(ListSuccess, successItem);

        NotifyStateChanged();
    }

    public void CallBackForErrorItems(IPictureConvertStatusModel errorItem)
    {
        RemoveFromCurrentItem(errorItem);

        AddItemOnlyIfNotExist(ListError, errorItem);

        NotifyStateChanged();
    }

    public void CallBackForCurrentItem(IPictureConvertStatusModel currentItem)
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
