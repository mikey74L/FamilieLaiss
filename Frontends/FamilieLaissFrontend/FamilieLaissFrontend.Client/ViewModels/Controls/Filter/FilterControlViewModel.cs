using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventAggregator.Blazor;
using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Services;
using FamilieLaissModels.EventAggregator.Filter;
using FamilieLaissSharedUI.ViewModels;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Controls.Filter;

public partial class FilterControlViewModel(
    ISnackbar snackbarService,
    IMessageBoxService messageBoxService,
    IEventAggregator eventAggregator)
    : ViewModelBase(snackbarService, messageBoxService)
{
    #region Parameters
    public List<IGraphQlFilterCriteria> FilterCriterias { get; set; } = default!;
    #endregion

    #region Public Properties
    [ObservableProperty]
    private bool _isFilterValueSet;
    #endregion

    #region Commands
    [RelayCommand]
    private async Task SetFilter()
    {
        await eventAggregator.PublishAsync(new AggSetFilter());

        IsFilterValueSet = true;

        if (FilterCriterias.All(x => x.IsValid))
        {
            await eventAggregator.PublishAsync(new AggFilterChanged());
        }
    }

    [RelayCommand]
    private async Task ResetFilter()
    {
        if (IsFilterValueSet)
        {
            IsFilterValueSet = false;

            await eventAggregator.PublishAsync(new AggResetFilter());

            await eventAggregator.PublishAsync(new AggFilterChanged());
        }
    }
    #endregion

    #region Abstract overrides
    public override void Dispose()
    {
    }
    #endregion
}
