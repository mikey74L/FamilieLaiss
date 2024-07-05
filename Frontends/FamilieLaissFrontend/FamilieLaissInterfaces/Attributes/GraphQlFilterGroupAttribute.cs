namespace FamilieLaissInterfaces.Attributes;

[AttributeUsage(AttributeTargets.Interface, AllowMultiple = true)]
public class GraphQlFilterGroupAttribute(string scope, string identifier, int sort) : Attribute
{
    public string Scope { get; } = scope;

    public string Identifier { get; } = identifier;

    public int Sort { get; } = sort;
}
