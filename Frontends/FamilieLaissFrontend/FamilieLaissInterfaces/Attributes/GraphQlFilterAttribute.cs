using FamilieLaissInterfaces.Enums;

namespace FamilieLaissInterfaces.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class GraphQlFilterAttribute(string scope, string groupIdentifier, int sort, GraphQlFilterType filterType) : Attribute
{
    public string Scope { get; } = scope;

    public int Sort { get; } = sort;

    public string GroupIdentifier { get; } = groupIdentifier;

    public GraphQlFilterType FilterType { get; } = filterType;
}

[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
public class GraphQlFilterInputTypeAttribute(Type filterInputType) : Attribute
{
    public Type GraphQlFilterInputType { get; } = filterInputType;
}
