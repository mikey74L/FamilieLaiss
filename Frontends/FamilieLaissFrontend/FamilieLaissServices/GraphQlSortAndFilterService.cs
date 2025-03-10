using EventAggregator.Blazor;
using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Attributes;
using FamilieLaissInterfaces.Enums;
using FamilieLaissInterfaces.Extensions;
using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissModels.EventAggregator.Filter;
using FamilieLaissModels.Services;
using FamilieLaissResources.Resources.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Tocronx.SimpleAsync;

namespace FamilieLaissServices;

public abstract class GraphQlSortAndFilterService
{

}

public class GraphQlSortAndFilterService<TModel, TSortInput, TFilterInput> : IGraphQlSortAndFilterService<TModel, TSortInput, TFilterInput> where TModel : IBaseModel where TSortInput : class where TFilterInput : class
{
    #region Private Members
    private string Scope { get; }
    private readonly Type _genericTypeModel;
    private readonly IEventAggregator _eventAggregator;
    private readonly Func<string, Dictionary<int, string>>? _provideNumberList;
    #endregion

    #region C'tor
    public GraphQlSortAndFilterService(string scope, IEventAggregator eventAggregator, Func<string, Dictionary<int, string>>? provideNumberList)
    {
        this.Scope = scope;
        this._eventAggregator = eventAggregator;
        this._provideNumberList = provideNumberList;

        _genericTypeModel = typeof(TModel);

        var sortCriterias = GetSortingCriterias(_genericTypeModel);

        SortCriterias.AddRange(sortCriterias.OrderBy(x => x.DisplaySort));

        _selectedSortCriteria = sortCriterias.Single(x => x.InitialSelect);

        GetFilterGroups(_genericTypeModel);
        GetFilterCriterias(_genericTypeModel);
    }
    #endregion

    #region Sorting
    #region Private Methods
    private string GetResourceIdentifierSorting(Type type, PropertyInfo propInfo, GraphQlSortDirection sortDirection)
    {
        return $"Sort.{type.Name}.{propInfo.Name}.{(sortDirection == GraphQlSortDirection.Ascending ? "Asc" : "Desc")}";
    }

    private Type? GetSortInputType(Type modelType)
    {
        var foundSortInputTypeAttr = modelType.GetCustomAttribute<GraphQlSortInputTypeAttribute>();
        return foundSortInputTypeAttr?.GraphQlSortInputType;
    }

    private object? CreateSortInputObject(Type modelType)
    {
        var foundSortInputType = GetSortInputType(modelType);
        if (foundSortInputType is not null)
        {
            return Activator.CreateInstance(foundSortInputType);
        }

        return null;
    }

    private void SetSortCriteriaOnSortInput(Type modelType, object sortInputObject,
        GraphQlSortDirection sortDirection, string propertyName)
    {
        var foundSortInputType = GetSortInputType(modelType);

        if (foundSortInputType is not null)
        {
            var propertyInfoToSet = foundSortInputType.GetProperty(propertyName);

            if (propertyInfoToSet is not null)
            {
                var sortValue = sortDirection == GraphQlSortDirection.Ascending ? SortEnumType.Asc : SortEnumType.Desc;

                propertyInfoToSet.SetValue(sortInputObject, sortValue);
            }
        }
    }

    private string GetLocalizedText(string identifier)
    {
        return GraphQlSortAndFilterServiceRes.ResourceManager.GetString(identifier, GraphQlSortAndFilterServiceRes.CultureInfo)!;
    }

    private List<GraphQlSortCriteria<TSortInput>> GetSortingCriterias(Type searchType, string? propertyPathFather = null)
    {
        if (!searchType.IsInterface && searchType is not IBaseModel)
        {
            return [];
        }

        var sortCriterias = new List<GraphQlSortCriteria<TSortInput>>();

        foreach (var propInfo in searchType.GetProperties())
        {
            if (propInfo.PropertyType is IBaseModel)
            {
                sortCriterias.AddRange(GetSortingCriterias(propInfo.PropertyType, propInfo.Name.ToLowerFirstChar()));
            }
            else
            {
                foreach (var attribute in propInfo.GetCustomAttributes<GraphQlSortAttribute>().Where(x => x.Scope == this.Scope))
                {
                    var resourceIdentifier = GetResourceIdentifierSorting(searchType, propInfo, attribute.SortDirection);

                    var sortInputObject = CreateSortInputObject(searchType);
                    if (sortInputObject is not null)
                    {
                        SetSortCriteriaOnSortInput(searchType, sortInputObject, attribute.SortDirection, propInfo.Name);

                        var newSortCriteria = new GraphQlSortCriteria<TSortInput>()
                        {
                            Id = Guid.NewGuid(),
                            GraphQlSortInput = (TSortInput)sortInputObject,
                            DisplayText = GetLocalizedText(resourceIdentifier),
                            DisplaySort = attribute.Sort,
                            InitialSelect = attribute.InitialSelected,
                            SortDirection = attribute.SortDirection
                        };

                        sortCriterias.Add(newSortCriteria);
                    }
                }
            }
        }

        return sortCriterias;
    }
    #endregion

    #region Public Properties
    public List<IGraphQlSortCriteria<TSortInput>> SortCriterias { get; } = [];

    private IGraphQlSortCriteria<TSortInput> _selectedSortCriteria;
    public IGraphQlSortCriteria<TSortInput> SelectedSortCriteria
    {
        get => _selectedSortCriteria;
        set
        {
            _selectedSortCriteria = value;

            if (SelectedSortCriteriaChanged is not null)
            {
                SelectedSortCriteriaChanged.Invoke().FireAndForget();
            }
        }
    }

    public Func<Task>? SelectedSortCriteriaChanged { get; set; }

    public IReadOnlyList<TSortInput> GraphQlSortCriteria => [SelectedSortCriteria.GraphQlSortInput];

    #endregion
    #endregion

    #region Filter
    #region Private Methods
    private string GetResourceIdentifierFilterGroupName(Type type, string groupIdentifier)
    {
        return $"Filter.GroupName.{type.Name}.{groupIdentifier}";
    }

    private string GetResourceIdentifierFilter(Type type, PropertyInfo propInfo)
    {
        return $"Filter.Item.{type.Name}.{propInfo.Name}";
    }

    private Type? GetFilterInputType(Type modelType)
    {
        var foundFilterInputTypeAttr = modelType.GetCustomAttribute<GraphQlFilterInputTypeAttribute>();
        return foundFilterInputTypeAttr?.GraphQlFilterInputType;
    }

    private object? CreateFilterInputObject(Type modelType)
    {
        var foundFilterInputType = GetFilterInputType(modelType);
        if (foundFilterInputType is not null)
        {
            return Activator.CreateInstance(foundFilterInputType);
        }

        return null;
    }

    private void GetFilterGroups(Type searchType)
    {
        foreach (var filterGroupAttr in searchType.GetCustomAttributes<GraphQlFilterGroupAttribute>().Where(x => x.Scope == Scope))
        {
            var groupResourceIdentifier = GetResourceIdentifierFilterGroupName(searchType, filterGroupAttr.Identifier);

            _filterGroups.Add(
                new GraphQlFilterGroup()
                {
                    Id = Guid.NewGuid(),
                    Identifier = filterGroupAttr.Identifier,
                    Sort = filterGroupAttr.Sort,
                    DisplayText = GetLocalizedText(groupResourceIdentifier)
                });
        }

        foreach (var propInfoNavProp in searchType.GetProperties().Where(x => x.PropertyType.GetInterfaces().Contains(typeof(IBaseModel))))
        {
            GetFilterGroups(propInfoNavProp.PropertyType);
        }
    }

    private void GetFilterCriterias(Type searchType, string? fatherProperty = null)
    {
        if (!searchType.IsInterface && searchType is not IBaseModel)
        {
            return;
        }

        //Alle Properties des direkten Models auswerten die gefiltert werden sollen
        foreach (var propInfo in searchType.GetProperties())
        {
            foreach (var attribute in propInfo.GetCustomAttributes<GraphQlFilterAttribute>().Where(x => x.Scope == this.Scope))
            {
                var resourceIdentifier = GetResourceIdentifierFilter(searchType, propInfo);
                Dictionary<int, string>? localizedValues = null;
                if (attribute.FilterType == GraphQlFilterType.ValueListInt)
                {
                    if (_provideNumberList is not null)
                    {
                        var numberList = _provideNumberList.Invoke(propInfo.Name);

                        localizedValues = [];
                        foreach (var item in numberList)
                        {
                            localizedValues.Add(item.Key, item.Value);
                        }
                    }
                }

                var newFilterCriteria = new GraphQlFilterCriteria()
                {
                    Id = Guid.NewGuid(),
                    GroupId = FilterGroups.Single(x => x.Identifier == attribute.GroupIdentifier).Id,
                    Sort = attribute.Sort,
                    FilterType = attribute.FilterType,
                    PropertyName = propInfo.Name,
                    PropertyNameFather = fatherProperty,
                    IntValues = localizedValues,
                    DisplayText = GetLocalizedText(resourceIdentifier),
                    FilterInputType = GetFilterInputType(searchType) ?? throw new Exception("Could not define FilterInputType")
                };

                FilterCriterias.Add(newFilterCriteria);
            }
        }

        //Alle direkten Navigation Properties ermitteln die gefiltert werden sollen
        foreach (var propInfoNavProp in searchType.GetProperties().Where(x => x.PropertyType.GetInterfaces().Contains(typeof(IBaseModel))))
        {
            GetFilterCriterias(propInfoNavProp.PropertyType, propInfoNavProp.Name);
        }
    }

    private object? GetGraphQlFilterForNavigationProperties(Type typeForFather, Type typeForNavProp, string fatherPropertyName)
    {
        var filterInputTypeNavProp = GetFilterInputType(typeForNavProp);

        if (filterInputTypeNavProp is not null)
        {
            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(filterInputTypeNavProp);
            var subFilterCriterias = Activator.CreateInstance(constructedListType);

            if (subFilterCriterias is not null)
            {
                var addMethod = subFilterCriterias.GetType().GetMethod("Add", BindingFlags.Instance | BindingFlags.Public);

                if (addMethod is not null)
                {
                    foreach (var subFilterCriteria in FilterCriterias
                        .Where(x => x.GraphQlFilterInputMin is not null && x.FilterInputType == filterInputTypeNavProp)
                        .Select(x => x.GraphQlFilterInputMin))
                    {
                        if (subFilterCriteria is not null)
                        {
                            addMethod.Invoke(subFilterCriterias, new object[] { subFilterCriteria });
                        }
                    }
                    foreach (var subFilterCriteria in FilterCriterias
                        .Where(x => x.GraphQlFilterInputMax is not null && x.FilterInputType == filterInputTypeNavProp)
                        .Select(x => x.GraphQlFilterInputMax))
                    {
                        if (subFilterCriteria is not null)
                        {
                            addMethod.Invoke(subFilterCriterias, new object[] { subFilterCriteria });
                        }
                    }

                    var filterObject = CreateFilterInputObject(typeForNavProp);

                    if (filterObject is not null)
                    {
                        foreach (var propInfoNavProp in typeForNavProp.GetProperties().Where(x => x.PropertyType.GetInterfaces().Contains(typeof(IBaseModel))))
                        {
                            var filterObjectNav = GetGraphQlFilterForNavigationProperties(typeForNavProp, propInfoNavProp.PropertyType, propInfoNavProp.Name);

                            if (filterObjectNav is not null)
                            {
                                addMethod.Invoke(subFilterCriterias, new object[] { filterObjectNav });
                            }
                        }

                        if (((IList)subFilterCriterias).Count > 0)
                        {
                            var asReadOnlyMethod = subFilterCriterias.GetType().GetMethod("AsReadOnly", BindingFlags.Instance | BindingFlags.Public);

                            if (asReadOnlyMethod is not null)
                            {
                                var readOnlyList = asReadOnlyMethod.Invoke(subFilterCriterias, null);

                                var propInfoForAnd = filterObject.GetType().GetProperty("And");

                                if (propInfoForAnd is not null)
                                {
                                    propInfoForAnd.SetValue(filterObject, readOnlyList);
                                }

                                var fatherFilterObject = CreateFilterInputObject(typeForFather);

                                if (fatherFilterObject is not null)
                                {
                                    var navPropInfo = fatherFilterObject.GetType().GetProperty(fatherPropertyName);

                                    if (navPropInfo is not null)
                                    {
                                        navPropInfo.SetValue(fatherFilterObject, filterObject);
                                    }

                                    return fatherFilterObject;
                                }
                            }
                        }
                    }
                }
            }
        }

        return null;
    }
    #endregion

    #region Public Methods
    public TFilterInput? GetGraphQlFilterCriteria()
    {
        var filterInputType = GetFilterInputType(_genericTypeModel);

        if (filterInputType is not null)
        {
            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(filterInputType);
            var subFilterCriterias = Activator.CreateInstance(constructedListType);

            if (subFilterCriterias is not null)
            {
                var addMethod = subFilterCriterias.GetType().GetMethod("Add", BindingFlags.Instance | BindingFlags.Public);

                if (addMethod is not null)
                {
                    foreach (var subFilterCriteria in FilterCriterias
                        .Where(x => x.GraphQlFilterInputMin is not null && x.FilterInputType == filterInputType)
                        .Select(x => x.GraphQlFilterInputMin))
                    {
                        if (subFilterCriteria is not null)
                        {
                            addMethod.Invoke(subFilterCriterias, new object[] { subFilterCriteria });
                        }
                    }
                    foreach (var subFilterCriteria in FilterCriterias
                        .Where(x => x.GraphQlFilterInputMax is not null && x.FilterInputType == filterInputType)
                        .Select(x => x.GraphQlFilterInputMax))
                    {
                        if (subFilterCriteria is not null)
                        {
                            addMethod.Invoke(subFilterCriterias, new object[] { subFilterCriteria });
                        }
                    }

                    var mainFilterObject = CreateFilterInputObject(_genericTypeModel);

                    if (mainFilterObject is not null)
                    {
                        foreach (var propInfoNavProp in _genericTypeModel.GetProperties().Where(x => x.PropertyType.GetInterfaces().Contains(typeof(IBaseModel))))
                        {
                            var filterObjectNav = GetGraphQlFilterForNavigationProperties(_genericTypeModel, propInfoNavProp.PropertyType, propInfoNavProp.Name);

                            if (filterObjectNav is not null)
                            {
                                addMethod.Invoke(subFilterCriterias, new object[] { filterObjectNav });
                            }
                        }

                        if (((IList)subFilterCriterias).Count > 0)
                        {
                            var asReadOnlyMethod = subFilterCriterias.GetType().GetMethod("AsReadOnly", BindingFlags.Instance | BindingFlags.Public);

                            if (asReadOnlyMethod is not null)
                            {
                                var readOnlyList = asReadOnlyMethod.Invoke(subFilterCriterias, null);

                                var propInfoForAnd = mainFilterObject.GetType().GetProperty("And");

                                if (propInfoForAnd is not null)
                                {
                                    propInfoForAnd.SetValue(mainFilterObject, readOnlyList);
                                }
                            }

                            return (TFilterInput)mainFilterObject;
                        }
                    }
                }
            }
        }

        return null;
    }

    public async Task SetFilterDataOnly<TFilterData>(string propertyName, List<TFilterData> data)
    {
        if (typeof(TFilterData) == typeof(string))
        {
            var foundCriteria = FilterCriterias.SingleOrDefault(x => x.PropertyName == propertyName && x.FilterType == GraphQlFilterType.ValueListStringOnly);

            if (foundCriteria is not null)
            {
                foundCriteria.StringOnlyValues = data.Cast<string>().ToList();

                await _eventAggregator.PublishAsync(new AggFilterValuesSet());
            }
        }

        if (typeof(TFilterData) == typeof(int))
        {
            var foundCriteria = FilterCriterias.SingleOrDefault(x => x.PropertyName == propertyName && x.FilterType == GraphQlFilterType.ValueListIntOnly);

            if (foundCriteria is not null)
            {
                foundCriteria.IntOnlyValues = data.Cast<int>().ToList();

                await _eventAggregator.PublishAsync(new AggFilterValuesSet());
            }
        }

        if (typeof(TFilterData) == typeof(double))
        {
            var foundCriteria = FilterCriterias.SingleOrDefault(x => x.PropertyName == propertyName && x.FilterType == GraphQlFilterType.ValueListDoubleOnly);

            if (foundCriteria is not null)
            {
                foundCriteria.DoubleOnlyValues = data.Cast<double>().ToList();

                await _eventAggregator.PublishAsync(new AggFilterValuesSet());
            }
        }
    }

    public async Task SetFilterData<TFilterData>(string propertyName, Dictionary<TFilterData, string> data) where TFilterData : notnull
    {
        if (typeof(TFilterData) == typeof(double))
        {
            var foundCriteria = FilterCriterias.SingleOrDefault(x => x.PropertyName == propertyName && x.FilterType == GraphQlFilterType.ValueListDouble);

            if (foundCriteria is not null)
            {
                foundCriteria.DoubleValues ??= [];

                foreach (var item in data)
                {
                    foundCriteria.DoubleValues?.Add(Convert.ToDouble(item.Key), item.Value);
                }

                await _eventAggregator.PublishAsync(new AggFilterValuesSet());
            }
        }
    }
    #endregion

    #region Public Properties
    private readonly List<IGraphQlFilterGroup> _filterGroups = [];
    public List<IGraphQlFilterGroup> FilterGroups => _filterGroups;

    public List<IGraphQlFilterCriteria> FilterCriterias { get; } = [];

    #endregion
    #endregion
}
