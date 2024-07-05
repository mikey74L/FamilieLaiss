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

public partial class FilterStringOnlyControlViewModel : ViewModelBase, IHandle<AggSetFilter>, IHandle<AggResetFilter>, IHandle<AggResetFilterGroup>, IHandle<AggFilterValuesSet>
{
    #region Services
    private readonly IEventAggregator eventAggregator;
    #endregion

    #region Public Parameter
    public EventCallback<(Guid id, bool hasValue)> ValueChanged;

    public IGraphQlFilterCriteria FilterCriteria = default!;
    #endregion

    #region Public Properties
    [ObservableProperty]
    private string? _selectedValue;
    partial void OnSelectedValueChanged(string? oldValue, string? newValue)
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
    private List<string> _filterItems = [];
    #endregion

    #region C'tor
    public FilterStringOnlyControlViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService,
        IEventAggregator eventAggregator) : base(snackbarService, messageBoxService)
    {
        this.eventAggregator = eventAggregator;
    }
    #endregion

    #region Lifecycle
    public override void OnInitialized()
    {
        base.OnInitialized();

        eventAggregator.Subscribe(this);

        if (FilterCriteria.StringOnlyValues is not null)
        {
            foreach (var item in FilterCriteria.StringOnlyValues)
            {
                FilterItems.Add(item);
            }
        }
    }
    #endregion

    #region Private Members
    private void Validate()
    {
        FilterCriteria.IsValid = true;
    }
    #endregion

    #region Commands
    private void ResetFilterValues()
    {
        SelectedValue = null;

        FilterCriteria.ResetValue();
    }

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

    public Task HandleAsync(AggFilterValuesSet message)
    {
        FilterItems.Clear();

        if (FilterCriteria.StringOnlyValues is not null)
        {
            FilterItems.AddRange(FilterCriteria.StringOnlyValues);
        }

        NotifyStateChanged();

        return Task.CompletedTask;
    }

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
                FilterCriteria.SetMinValue(SelectedValue);
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
