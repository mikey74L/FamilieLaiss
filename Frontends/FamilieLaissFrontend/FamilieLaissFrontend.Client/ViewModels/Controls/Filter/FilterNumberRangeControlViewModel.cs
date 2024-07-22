using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventAggregator.Blazor;
using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Services;
using FamilieLaissModels.EventAggregator.Filter;
using FamilieLaissResources.Resources.ViewModels.Controls.Filter;
using FamilieLaissSharedUI.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tocronx.SimpleAsync;

namespace FamilieLaissFrontend.Client.ViewModels.Controls.Filter;

public partial class FilterNumberRangeControlViewModel(
    ISnackbar snackbarService,
    IMessageBoxService messageBoxService,
    IEventAggregator eventAggregator)
    : ViewModelBase(snackbarService, messageBoxService), IHandle<AggSetFilter>, IHandle<AggResetFilter>,
        IHandle<AggResetFilterGroup>
{
    #region Parameters
    public IGraphQlFilterCriteria FilterCriteria { get; set; } = default!;
    public EventCallback<(Guid id, bool hasValue)> ValueChanged { get; set; }
    #endregion

    #region Public Properties
    [ObservableProperty]
    private int? _intValueFrom;

    [ObservableProperty]
    private int? _intValueTo;
    #endregion

    #region Lifecycle
    public override void OnInitialized()
    {
        base.OnInitialized();

        eventAggregator.Subscribe(this);
    }
    #endregion

    #region Private Methods
    private void CheckHasValue()
    {
        if (ValueChanged.HasDelegate)
        {
            if (IntValueFrom.HasValue || IntValueTo.HasValue)
            {
                ValueChanged.InvokeAsync((FilterCriteria.Id, true)).FireAndForget();
            }
            else
            {
                ValueChanged.InvokeAsync((FilterCriteria.Id, false)).FireAndForget();
            }
        }
    }

    private void Validate()
    {
        if (IntValueFrom is not null && IntValueTo is not null)
        {
            if (IntValueFrom.Value >= IntValueTo)
            {
                FilterCriteria.IsValid = false;
                FilterCriteria.ErrorMessage = FilterNumberRangeControlViewModelRes.ValidationErrorMaxGreaterMin;

                return;
            }
        }

        FilterCriteria.IsValid = true;
    }

    private void ResetFilterValues()
    {
        IntValueFrom = null;
        IntValueTo = null;

        FilterCriteria.ResetValue();
    }
    #endregion

    #region Commands
    [RelayCommand]
    public async Task ResetFilter()
    {
        ResetFilterValues();

        await eventAggregator.PublishAsync(new AggFilterChanged());
    }
    #endregion

    #region Event-Aggregator
    public Task HandleAsync(AggResetFilter message)
    {
        ResetFilterValues();

        return Task.CompletedTask;
    }

    public Task HandleAsync(AggResetFilterGroup message)
    {
        if (message.Id == FilterCriteria.GroupId)
        {
            ResetFilterValues();
        }

        return Task.CompletedTask;
    }

    public Task HandleAsync(AggSetFilter message)
    {
        Validate();

        if (FilterCriteria.IsValid)
        {
            if (IntValueFrom is null && IntValueTo is null)
            {
                FilterCriteria.ResetValue();
            }
            else
            {
                FilterCriteria.SetMinValue(IntValueFrom);
                FilterCriteria.SetMaxValue(IntValueTo);
            }
        }

        return Task.CompletedTask;
    }
    #endregion

    #region Abstract overrides
    public override void Dispose()
    {
        eventAggregator.Unsubscribe(this);
    }
    #endregion
}
