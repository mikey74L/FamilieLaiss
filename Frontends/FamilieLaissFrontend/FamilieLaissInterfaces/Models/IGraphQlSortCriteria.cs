using FamilieLaissInterfaces.Enums;

namespace FamilieLaissInterfaces.Models;

public interface IGraphQlSortCriteria<TSortInput> where TSortInput : class
{
    public Guid Id { get; }

    public GraphQlSortDirection SortDirection { get; }

    public TSortInput GraphQlSortInput { get; }

    public string DisplayText { get; }
}
