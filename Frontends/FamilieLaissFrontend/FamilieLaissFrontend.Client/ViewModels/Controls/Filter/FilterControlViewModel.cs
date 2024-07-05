using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventAggregator.Blazor;
using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Services;
using FamilieLaissModels.EventAggregator.Filter;
using FamilieLaissSharedUI.ViewModels;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Controls.Filter;

public partial class FilterControlViewModel : ViewModelBase
{
    #region Services
    private readonly IEventAggregator eventAggregator;
    #endregion

    #region Parameters
    public List<IGraphQlFilterCriteria> FilterCriterias { get; set; } = default!;
    #endregion

    #region Public Properties
    [ObservableProperty]
    private bool _isfilterValueSet;
    #endregion

    #region C'tor
    public FilterControlViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService,
        IEventAggregator eventAggregator) : base(snackbarService, messageBoxService)
    {
        this.eventAggregator = eventAggregator;
    }
    #endregion

    #region Commands
    [RelayCommand]
    private async Task SetFilter()
    {
        await eventAggregator.PublishAsync(new AggSetFilter());

        IsfilterValueSet = true;

        if (!FilterCriterias.Any(x => !x.IsValid))
        {
            await eventAggregator.PublishAsync(new AggFilterChanged());
        }
    }

    [RelayCommand]
    private async Task ResetFilter()
    {
        if (IsfilterValueSet)
        {
            IsfilterValueSet = false;

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
