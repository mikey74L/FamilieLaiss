using CommunityToolkit.Mvvm.ComponentModel;
using EventAggregator.Blazor;
using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissModels.EventAggregator.Filter;
using FamilieLaissSharedUI.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tocronx.SimpleAsync;

namespace FamilieLaissFrontend.Client.ViewModels.Pages.BaseData.Upload.Picture;

public partial class PictureUploadListDrawersViewModel : ViewModelBase
{
    #region Services
    private readonly IEventAggregator eventAggregator;
    #endregion

    #region Parameters
    public EventCallback<bool> IsSortSidebarVisibleChanged { get; set; }
    public EventCallback<bool> IsFilterSidebarVisibleChanged { get; set; }
    public IGraphQlSortAndFilterService<IUploadPictureModel, UploadPictureSortInput, UploadPictureFilterInput> SortAndFilterService { get; set; } = default!;
    #endregion

    #region Public Properties
    [ObservableProperty]
    private bool _isSortSidebarVisible;
    partial void OnIsSortSidebarVisibleChanged(bool value)
    {
        if (IsSortSidebarVisibleChanged.HasDelegate)
        {
            IsSortSidebarVisibleChanged.InvokeAsync(value).FireAndForget();
        }
    }
    [ObservableProperty]
    private bool _isFilterSidebarVisible;
    partial void OnIsFilterSidebarVisibleChanged(bool value)
    {
        if (IsFilterSidebarVisibleChanged.HasDelegate)
        {
            IsFilterSidebarVisibleChanged.InvokeAsync(value).FireAndForget();
        }
    }

    [ObservableProperty]
    private IGraphQlSortCriteria<UploadPictureSortInput> _selectedSortCriteria = default!;
    partial void OnSelectedSortCriteriaChanged(IGraphQlSortCriteria<UploadPictureSortInput> value)
    {
        SortAndFilterService.SelectedSortCriteria = value;
        eventAggregator.PublishAsync(new AggFilterChanged()).FireAndForget();
    }
    #endregion

    #region C'tor
    public PictureUploadListDrawersViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService,
        IEventAggregator eventAggregator) : base(snackbarService, messageBoxService)
    {
        this.eventAggregator = eventAggregator;
    }
    #endregion

    #region Lifecycle
    public override void OnInitialized()
    {
        base.OnInitialized();

        SelectedSortCriteria = SortAndFilterService.SelectedSortCriteria;
    }
    #endregion

    #region Abstract overrides
    public override void Dispose()
    {
    }
    #endregion
}
