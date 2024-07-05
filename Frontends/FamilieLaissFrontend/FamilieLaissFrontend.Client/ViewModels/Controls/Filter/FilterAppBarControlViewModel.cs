using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventAggregator.Blazor;
using FamilieLaissInterfaces.Enums;
using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissModels.EventAggregator.Filter;
using FamilieLaissResources.Resources.ViewModels.Controls.Filter;
using FamilieLaissSharedUI.ViewModels;
using MudBlazor;

namespace FamilieLaissFrontend.Client.ViewModels.Controls.Filter;

public partial class FilterAppBarControlViewModel<TModel, TSortInput, TFilterInputType> : ViewModelBase, IHandle<AggFilterChanged> where TModel : IBaseModel where TSortInput : class where TFilterInputType : class
{
    #region Services
    private IEventAggregator eventAggregator;
    #endregion

    #region Parameters
    public IGraphQlSortAndFilterService<TModel, TSortInput, TFilterInputType> SortAndFilterService { get; set; } = default!;
    #endregion

    #region Public Properties
    [ObservableProperty]
    private string _currentSortCriteria = string.Empty;

    [ObservableProperty]
    private string _currentFilterCriteria = string.Empty;
    #endregion

    #region C'tor
    public FilterAppBarControlViewModel(ISnackbar snackbarService, IMessageBoxService messageBoxService,
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

        SetCurrentSortCriteria();
        SetCurrentFilterCriteria();
    }
    #endregion

    #region Private Methods
    private void SetCurrentSortCriteria()
    {
        CurrentSortCriteria = SortAndFilterService.SelectedSortCriteria.DisplayText;
    }

    private string CreateFilterCriteriaNumberRange(IGraphQlFilterCriteria filterCriteria)
    {
        var propertyName = filterCriteria.DisplayText;

        if (filterCriteria.MinValue is not null && filterCriteria.MaxValue is null)
        {
            return $"{propertyName} >= {filterCriteria.MinValue}";
        }
        if (filterCriteria.MaxValue is not null && filterCriteria.MinValue is null)
        {
            return $"{propertyName} <= {filterCriteria.MaxValue}";
        }
        if (filterCriteria.MinValue is not null && filterCriteria.MaxValue is not null)
        {
            return $"{propertyName} >= {filterCriteria.MinValue} & <= {filterCriteria.MaxValue}";
        }

        return string.Empty;
    }

    private string CreateFilterCriteriaStringOnly(IGraphQlFilterCriteria filterCriteria)
    {
        var propertyName = filterCriteria.DisplayText;

        if (filterCriteria.MinValue is not null)
        {
            return $"{propertyName} = \"{filterCriteria.MinValue}\"";
        }

        return string.Empty;
    }

    private string CreateFilterCriteriaNumberOnly(IGraphQlFilterCriteria filterCriteria)
    {
        var propertyName = filterCriteria.DisplayText;

        if (filterCriteria.MinValue is not null)
        {
            return $"{propertyName} = \"{filterCriteria.MinValue}\"";
        }

        return string.Empty;
    }

    private string CreateFilterCriteriaDateRange(IGraphQlFilterCriteria filterCriteria)
    {
        var propertyName = filterCriteria.DisplayText;

        if (filterCriteria.MinValue is not null && filterCriteria.MaxValue is not null)
        {
            return $"{propertyName} >= \"{((DateTimeOffset)filterCriteria.MinValue).DateTime.ToShortDateString()}\" & <= {((DateTimeOffset)filterCriteria.MaxValue).DateTime.ToShortDateString()}";
        }

        return string.Empty;
    }

    private string CreateFilterCriteriaValueListNumber(IGraphQlFilterCriteria filterCriteria)
    {
        var propertyName = filterCriteria.DisplayText;

        if (filterCriteria.MinValue is not null)
        {
            if (filterCriteria.FilterType == GraphQlFilterType.ValueListInt)
            {
                return $"{propertyName} = \"{filterCriteria.IntValues?.SingleOrDefault(x => x.Key == (int)filterCriteria.MinValue).Value}\"";
            }
            if (filterCriteria.FilterType == GraphQlFilterType.ValueListDouble)
            {
                return $"{propertyName} = \"{filterCriteria.DoubleValues?.SingleOrDefault(x => x.Key == (double)filterCriteria.MinValue).Value}\"";
            }
        }

        return string.Empty;
    }

    private string CreateFilterCriteria(IGraphQlFilterCriteria filterCriteria)
    {
        if (filterCriteria.FilterType == GraphQlFilterType.NumberRange)
        {
            return CreateFilterCriteriaNumberRange(filterCriteria);
        }

        if (filterCriteria.FilterType == GraphQlFilterType.ValueListInt || filterCriteria.FilterType == GraphQlFilterType.ValueListDouble)
        {
            return CreateFilterCriteriaValueListNumber(filterCriteria);
        }

        if (filterCriteria.FilterType == GraphQlFilterType.ValueListStringOnly)
        {
            return CreateFilterCriteriaStringOnly(filterCriteria);
        }

        if (filterCriteria.FilterType == GraphQlFilterType.DateRange)
        {
            return CreateFilterCriteriaDateRange(filterCriteria);
        }

        if (filterCriteria.FilterType == GraphQlFilterType.ValueListIntOnly || filterCriteria.FilterType == GraphQlFilterType.ValueListDoubleOnly)
        {
            return CreateFilterCriteriaNumberOnly(filterCriteria);
        }
        return string.Empty;
    }

    private void SetCurrentFilterCriteria()
    {
        if (SortAndFilterService.GetGraphQlFilterCriteria() is null)
        {
            CurrentFilterCriteria = FilterAppBarControlViewModelRes.InfoTextNoFilter;
        }
        else
        {
            CurrentFilterCriteria = "";
            foreach (var group in SortAndFilterService.FilterGroups.OrderBy(x => x.Sort))
            {
                foreach (var criteria in SortAndFilterService.FilterCriterias.Where(x => x.GroupId == group.Id && x.IsActive).OrderBy(x => x.Sort))
                {
                    if (CurrentFilterCriteria.Length > 0)
                    {
                        CurrentFilterCriteria += ", ";
                    }

                    CurrentFilterCriteria += CreateFilterCriteria(criteria);
                }
            }
        }
    }
    #endregion

    #region Commands
    [RelayCommand]
    public async Task ShowEditFilterCriteria()
    {
        await eventAggregator.PublishAsync(new AggEditFilterCriteria());
    }

    [RelayCommand]
    public async Task ShowEditSortCriteria()
    {
        await eventAggregator.PublishAsync(new AggEditSortCriteria());
    }
    #endregion

    #region Event-Aggregator
    public Task HandleAsync(AggFilterChanged message)
    {
        SetCurrentSortCriteria();
        SetCurrentFilterCriteria();

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
