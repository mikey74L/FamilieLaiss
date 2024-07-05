using FamilieLaissInterfaces.Enums;
using FamilieLaissInterfaces.Models;

namespace FamilieLaissModels.Services;

public class GraphQlSortCriteria<TSortInput> : IGraphQlSortCriteria<TSortInput> where TSortInput : class
{
    public required Guid Id { get; init; }

    public required GraphQlSortDirection SortDirection { get; init; }

    public required string DisplayText { get; init; }

    public required TSortInput GraphQlSortInput { get; init; }

    public required int DisplaySort { get; init; }

    public required bool InitialSelect { get; init; }

    public override bool Equals(object o)
    {
        var other = o as GraphQlSortCriteria<TSortInput>;
        return other?.Id == Id;
    }

    public override int GetHashCode() => DisplayText?.GetHashCode() ?? 0;

    public override string ToString() => DisplayText;
}
