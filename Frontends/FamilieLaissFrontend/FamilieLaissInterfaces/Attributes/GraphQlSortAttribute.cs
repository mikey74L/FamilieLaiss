using FamilieLaissInterfaces.Enums;

namespace FamilieLaissInterfaces.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class GraphQlSortAttribute(string scope, int sort, bool initialSelected, GraphQlSortDirection sortDirection) : Attribute
{
    public string Scope { get; } = scope;

    public int Sort { get; } = sort;

    public bool InitialSelected { get; } = initialSelected;

    public GraphQlSortDirection SortDirection { get; } = sortDirection;
}

[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
public class GraphQlSortInputTypeAttribute(Type sortInputType) : Attribute
{
    public Type GraphQlSortInputType { get; } = sortInputType;
}