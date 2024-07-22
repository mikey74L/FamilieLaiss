using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventAggregator.Blazor;
using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Services;
using FamilieLaissModels.EventAggregator.Filter;
using FamilieLaissSharedUI.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tocronx.SimpleAsync;

namespace FamilieLaissFrontend.Client.ViewModels.Controls.Filter;

public class FilterItemNumberValue
{
    public object Value { get; init; } = default!;
    public string DisplayText { get; init; } = string.Empty;
}

public partial class FilterNumberListControlViewModel(
    ISnackbar snackbarService,
    IMessageBoxService messageBoxService,
    IEventAggregator eventAggregator)
    : ViewModelBase(snackbarService, messageBoxService), IHandle<AggSetFilter>, IHandle<AggResetFilter>,
        IHandle<AggResetFilterGroup>, IHandle<AggFilterValuesSet>
{
    #region Parameters
    public IGraphQlFilterCriteria FilterCriteria { get; set; } = default!;
    public EventCallback<(Guid id, bool hasValue)> ValueChanged { get; set; }
    #endregion

    #region Public Properties
    [ObservableProperty]
    private FilterItemNumberValue? _selectedValue;
    partial void OnSelectedValueChanged(FilterItemNumberValue? value)
    {
        if (ValueChanged.HasDelegate)
        {
            if (SelectedValue is not null)
            {
                ValueChanged.InvokeAsync((FilterCriteria.Id, true)).FireAndForget();
            }
            else
            {
                ValueChanged.InvokeAsync((FilterCriteria.Id, false)).FireAndForget();
            }
        }
    }

    [ObservableProperty]
    private List<FilterItemNumberValue> _filterItems = [];
    #endregion

    #region Lifecycle
    public override void OnInitialized()
    {
        base.OnInitialized();

        eventAggregator.Subscribe(this);

        SetFilterValues();
    }
    #endregion

    #region Private Methods
    private void SetFilterValues()
    {
        FilterItems.Clear();

        if (FilterCriteria.IntValues is not null)
        {
            foreach (var item in FilterCriteria.IntValues)
            {
                FilterItems.Add(new FilterItemNumberValue() { Value = item.Key, DisplayText = item.Value });
            }
        }

        if (FilterCriteria.DoubleValues is not null)
        {
            foreach (var item in FilterCriteria.DoubleValues)
            {
                FilterItems.Add(new FilterItemNumberValue() { Value = item.Key, DisplayText = item.Value });
            }
        }
    }


    private void Validate()
    {
        FilterCriteria.IsValid = true;
    }

    private void ResetFilterValues()
    {
        SelectedValue = null;

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
            if (SelectedValue is null)
            {
                FilterCriteria.ResetValue();
            }
            else
            {
                FilterCriteria.SetMinValue(SelectedValue.Value);
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

    public Task HandleAsync(AggFilterValuesSet message)
    {
        SetFilterValues();

        NotifyStateChanged();

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
