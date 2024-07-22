using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventAggregator.Blazor;
using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Services;
using FamilieLaissModels.EventAggregator.Filter;
using FamilieLaissSharedUI.ViewModels;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Controls.Filter;

public partial class FilterGroupControlViewModel(
    ISnackbar snackbarService,
    IMessageBoxService messageBoxService,
    IEventAggregator eventAggregator)
    : ViewModelBase(snackbarService, messageBoxService)
{
    #region Parameters
    public IGraphQlFilterGroup FilterGroup = default!;
    #endregion

    #region Public Properties
    [ObservableProperty]
    private bool _showResetButton;

    [ObservableProperty]
    private Dictionary<Guid, bool> _valueDictFilterItems = [];
    #endregion

    #region Commands
    [RelayCommand]
    public async Task ResetGroupFilter()
    {
        await eventAggregator.PublishAsync(new AggResetFilterGroup() { Id = FilterGroup.Id });

        await eventAggregator.PublishAsync(new AggFilterChanged());
    }

    [RelayCommand]
    public void FilterItemValueChanged((Guid Id, bool hasValue) data)
    {
        ValueDictFilterItems[data.Id] = data.hasValue;

        ShowResetButton = ValueDictFilterItems.Any(x => x.Value);
    }

    #endregion

    #region Abstract overrides
    public override void Dispose()
    {
    }
    #endregion
}
