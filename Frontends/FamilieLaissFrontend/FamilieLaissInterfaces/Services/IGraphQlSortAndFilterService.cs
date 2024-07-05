using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissInterfaces.Services;

public interface IGraphQlSortAndFilterService<TModel, TSortInput, TFilterInput> where TModel : IBaseModel where TSortInput : class where TFilterInput : class
{
    public List<IGraphQlSortCriteria<TSortInput>> SortCriterias { get; }

    public IGraphQlSortCriteria<TSortInput> SelectedSortCriteria { get; set; }

    public IReadOnlyList<TSortInput> GraphQlSortCriteria { get; }

    public List<IGraphQlFilterGroup> FilterGroups { get; }

    public List<IGraphQlFilterCriteria> FilterCriterias { get; }

    public Func<Task>? SelectedSortCriteriaChanged { get; set; }

    public TFilterInput? GetGraphQlFilterCriteria();

    public Task SetFilterDataOnly<TFilterData>(string propertyName, List<TFilterData> data);

    public Task SetFilterData<TFilterData>(string propertyName, Dictionary<TFilterData, string> data) where TFilterData : notnull;
}
