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

public partial class FilterDateRangeControlViewModel(
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
    private DateRange _dateRangeValue = new(null, null);
    partial void OnDateRangeValueChanged(DateRange value)
    {
        if (ValueChanged.HasDelegate)
        {
            if (DateRangeValue.Start.HasValue || DateRangeValue.End.HasValue)
            {
                ValueChanged.InvokeAsync((FilterCriteria.Id, true)).FireAndForget();
            }
            else
            {
                ValueChanged.InvokeAsync((FilterCriteria.Id, false)).FireAndForget();
            }
        }
    }

    public MudDateRangePicker PickerControl = default!;
    #endregion

    #region Lifecycle
    public override void OnInitialized()
    {
        base.OnInitialized();

        eventAggregator.Subscribe(this);
    }
    #endregion

    #region Private Methods
    private void Validate()
    {
        if (DateRangeValue.Start is not null && DateRangeValue.End is not null)
        {
            if (DateRangeValue.Start >= DateRangeValue.End)
            {
                FilterCriteria.IsValid = false;
                FilterCriteria.ErrorMessage = FilterDateRangeControlViewModelRes.ValidationErrorMaxGreaterMin;

                return;
            }
        }

        FilterCriteria.IsValid = true;
    }

    private void ResetFilterValues()
    {
        DateRangeValue = new DateRange(null, null);

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
    public Task HandleAsync(AggSetFilter message)
    {
        Validate();

        if (FilterCriteria.IsValid)
        {
            if (DateRangeValue.Start is null && DateRangeValue.End is null)
            {
                FilterCriteria.ResetValue();
            }
            else
            {
                if (DateRangeValue.Start is not null && DateRangeValue.End is not null)
                {
                    FilterCriteria.SetMinValue(new DateTimeOffset(DateRangeValue.Start.Value.ToUniversalTime()));
                    FilterCriteria.SetMaxValue(new DateTimeOffset(DateRangeValue.End.Value.ToUniversalTime()));
                }
            }
        }

        return Task.CompletedTask;
    }

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
    #endregion

    #region Abstract Overrides
    public override void Dispose()
    {
        eventAggregator.Unsubscribe(this);
    }
    #endregion
}
