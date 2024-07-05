using FamilieLaissInterfaces.Enums;

namespace FamilieLaissInterfaces.Models;

public interface IGraphQlFilterGroup
{
    public Guid Id { get; }

    public string Identifier { get; }

    public int Sort { get; }

    public string DisplayText { get; }
}

public interface IGraphQlFilterCriteria
{
    public Guid Id { get; }

    public Guid GroupId { get; }

    public int Sort { get; }

    public Type FilterInputType { get; }

    public GraphQlFilterType FilterType { get; }

    public string PropertyName { get; }

    public string? PropertyNameFather { get; }

    public void ResetValue();

    public void SetMinValue(object? value);

    public void SetMaxValue(object? value);

    public object? GraphQlFilterInputMin { get; }

    public object? GraphQlFilterInputMax { get; }

    public Dictionary<int, string>? IntValues { get; set; }

    public Dictionary<double, string>? DoubleValues { get; set; }

    public List<string>? StringOnlyValues { get; set; }

    public List<int>? IntOnlyValues { get; set; }

    public List<double>? DoubleOnlyValues { get; set; }

    public bool IsValid { get; set; }

    public bool IsActive { get; set; }

    public object? MinValue { get; }

    public object? MaxValue { get; }

    public string ErrorMessage { get; set; }

    public string DisplayText { get; }
}
