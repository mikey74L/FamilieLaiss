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

public partial class FilterNumberOnlyControlViewModel(
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
    private int? _selectedValueInt;
    partial void OnSelectedValueIntChanged(int? value)
    {
        CheckHasValue();
    }

    [ObservableProperty]
    private double? _selectedValueDouble;
    partial void OnSelectedValueDoubleChanged(double? value)
    {
        CheckHasValue();
    }

    [ObservableProperty]
    private List<int?> _filterItemsInt = [];

    [ObservableProperty]
    private List<double?> _filterItemsDouble = [];
    #endregion

    #region Lifecycle
    public override void OnInitialized()
    {
        base.OnInitialized();

        eventAggregator.Subscribe(this);

        if (FilterCriteria.IntOnlyValues is not null)
        {
            foreach (var item in FilterCriteria.IntOnlyValues)
            {
                FilterItemsInt.Add(item);
            }
        }

        if (FilterCriteria.DoubleOnlyValues is not null)
        {
            foreach (var item in FilterCriteria.DoubleOnlyValues)
            {
                FilterItemsDouble.Add(item);
            }
        }
    }
    #endregion

    #region Private Methods
    private void CheckHasValue()
    {
        if (ValueChanged.HasDelegate)
        {
            if (SelectedValueInt is not null || SelectedValueDouble is not null)
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
        FilterCriteria.IsValid = true;
    }

    private void ResetFilterValues()
    {
        SelectedValueInt = null;
        SelectedValueDouble = null;

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
            if (SelectedValueInt is null && SelectedValueDouble is null)
            {
                FilterCriteria.ResetValue();
            }
            else
            {
                if (SelectedValueInt is not null)
                {
                    FilterCriteria.SetMinValue(SelectedValueInt);
                }
                if (SelectedValueDouble is not null)
                {
                    FilterCriteria.SetMinValue(SelectedValueDouble);
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

    public Task HandleAsync(AggFilterValuesSet message)
    {
        FilterItemsInt.Clear();

        if (FilterCriteria.IntOnlyValues is not null)
        {
            FilterItemsInt.AddRange(FilterCriteria.IntOnlyValues.Cast<int?>());
        }
        if (FilterCriteria.DoubleOnlyValues is not null)
        {
            FilterItemsDouble.AddRange(FilterCriteria.DoubleOnlyValues.Cast<double?>());
        }

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
