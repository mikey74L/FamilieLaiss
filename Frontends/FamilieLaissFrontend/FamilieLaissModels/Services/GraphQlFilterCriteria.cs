using FamilieLaissInterfaces.Enums;
using FamilieLaissInterfaces.Models;

namespace FamilieLaissModels.Services;

public class GraphQlFilterGroup : IGraphQlFilterGroup
{
    public required Guid Id { get; init; }

    public required int Sort { get; init; }

    public required string DisplayText { get; init; }

    public required string Identifier { get; init; }
}

public class GraphQlFilterCriteria : IGraphQlFilterCriteria
{
    public required Guid Id { get; init; }

    public required Guid GroupId { get; init; }

    public required GraphQlFilterType FilterType { get; init; }

    public required string PropertyName { get; init; }

    public string? PropertyNameFather { get; set; }

    public object? GraphQlFilterInputMin { get; set; }

    public object? GraphQlFilterInputMax { get; set; }

    public required string DisplayText { get; init; }

    public required int Sort { get; init; }

    public bool IsValid { get; set; } = true;

    public bool IsActive { get; set; } = false;

    public string ErrorMessage { get; set; } = string.Empty;

    public object? MinValue { get; set; }

    public object? MaxValue { get; set; }

    public required Type FilterInputType { get; init; }

    public Dictionary<int, string>? IntValues { get; set; }

    public Dictionary<double, string>? DoubleValues { get; set; }

    public List<string>? StringOnlyValues { get; set; }

    public List<int>? IntOnlyValues { get; set; }

    public List<double>? DoubleOnlyValues { get; set; }

    private void CreateNewMinFilterInput()
    {
        var newObject = Activator.CreateInstance(FilterInputType);

        if (newObject is not null)
        {
            GraphQlFilterInputMin = newObject;
        }

    }

    private void CreateNewMaxFilterInput()
    {
        var newObject = Activator.CreateInstance(FilterInputType);

        if (newObject is not null)
        {
            GraphQlFilterInputMax = newObject;
        }

    }

    private void SetValue(object? targetFilterInput, object value, bool isMaxValue)
    {
        if (targetFilterInput is not null)
        {
            var typeOfFilterInput = targetFilterInput.GetType();

            var propInfo = typeOfFilterInput.GetProperty(PropertyName);

            if (propInfo is not null)
            {
                var typeOfProperty = propInfo.PropertyType;
                var newValue = Activator.CreateInstance(typeOfProperty);

                if (newValue is not null)
                {
                    if (isMaxValue && FilterType == GraphQlFilterType.NumberRange)
                    {
                        var propInfoLte = typeOfProperty.GetProperty("Lte");

                        if (propInfoLte is not null)
                        {
                            if (propInfoLte.PropertyType.IsGenericType && propInfoLte.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                var basePropertyType = propInfoLte.PropertyType.GetGenericArguments()[0];
                                propInfoLte.SetValue(newValue, Convert.ChangeType(value, basePropertyType));
                            }
                        }
                    }

                    if (isMaxValue && FilterType == GraphQlFilterType.DateRange)
                    {
                        var propInfoLte = typeOfProperty.GetProperty("Lte");

                        if (propInfoLte is not null)
                        {
                            if (propInfoLte.PropertyType.IsGenericType && propInfoLte.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                var basePropertyType = propInfoLte.PropertyType.GetGenericArguments()[0];
                                propInfoLte.SetValue(newValue, Convert.ChangeType(value, basePropertyType));
                            }
                        }
                    }

                    if (!isMaxValue && FilterType == GraphQlFilterType.NumberRange)
                    {
                        var propInfoGte = typeOfProperty.GetProperty("Gte");

                        if (propInfoGte is not null)
                        {
                            if (propInfoGte.PropertyType.IsGenericType && propInfoGte.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                var basePropertyType = propInfoGte.PropertyType.GetGenericArguments()[0];
                                propInfoGte.SetValue(newValue, Convert.ChangeType(value, basePropertyType));
                            }
                        }
                    }

                    if (!isMaxValue && FilterType == GraphQlFilterType.DateRange)
                    {
                        var propInfoGte = typeOfProperty.GetProperty("Gte");

                        if (propInfoGte is not null)
                        {
                            if (propInfoGte.PropertyType.IsGenericType && propInfoGte.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                var basePropertyType = propInfoGte.PropertyType.GetGenericArguments()[0];
                                propInfoGte.SetValue(newValue, Convert.ChangeType(value, basePropertyType));
                            }
                        }
                    }

                    if (!isMaxValue && (FilterType == GraphQlFilterType.ValueListInt || FilterType == GraphQlFilterType.ValueListDouble))
                    {
                        var propInfoEq = typeOfProperty.GetProperty("Eq");

                        if (propInfoEq is not null)
                        {
                            if (propInfoEq.PropertyType.IsGenericType && propInfoEq.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                var basePropertyType = propInfoEq.PropertyType.GetGenericArguments()[0];
                                propInfoEq.SetValue(newValue, Convert.ChangeType(value, basePropertyType));
                            }
                            else
                            {
                                var basePropertyType = propInfoEq.PropertyType;
                                propInfoEq.SetValue(newValue, Convert.ChangeType(value, basePropertyType));
                            }
                        }
                    }

                    if ((!isMaxValue && FilterType == GraphQlFilterType.ValueListIntOnly) || FilterType == GraphQlFilterType.ValueListDoubleOnly)
                    {
                        var propInfoEq = typeOfProperty.GetProperty("Eq");

                        if (propInfoEq is not null)
                        {
                            if (propInfoEq.PropertyType.IsGenericType && propInfoEq.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                var basePropertyType = propInfoEq.PropertyType.GetGenericArguments()[0];
                                propInfoEq.SetValue(newValue, Convert.ChangeType(value, basePropertyType));
                            }
                            else
                            {
                                var basePropertyType = propInfoEq.PropertyType;
                                propInfoEq.SetValue(newValue, Convert.ChangeType(value, basePropertyType));
                            }
                        }
                    }

                    if (!isMaxValue && FilterType == GraphQlFilterType.ValueListStringOnly)
                    {
                        var propInfoEq = typeOfProperty.GetProperty("Eq");

                        if (propInfoEq is not null)
                        {
                            if (propInfoEq.PropertyType.IsGenericType && propInfoEq.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                var basePropertyType = propInfoEq.PropertyType.GetGenericArguments()[0];
                                propInfoEq.SetValue(newValue, Convert.ChangeType(value, basePropertyType));
                            }
                            else
                            {
                                propInfoEq.SetValue(newValue, value);
                            }
                        }
                    }

                    propInfo.SetValue(targetFilterInput, newValue);
                }
            }
        }
    }

    public void ResetValue()
    {
        GraphQlFilterInputMin = null;
        GraphQlFilterInputMax = null;
        MinValue = null;
        MaxValue = null;

        IsActive = false;
    }

    public void SetMaxValue(object? value)
    {
        if (value is null)
        {
            GraphQlFilterInputMax = null;
            MaxValue = null;

            if (GraphQlFilterInputMin is null && GraphQlFilterInputMax is null)
            {
                IsActive = false;
            }
        }
        else
        {
            CreateNewMaxFilterInput();

            SetValue(GraphQlFilterInputMax, value, true);
            MaxValue = value;

            IsActive = true;
        }
    }

    public void SetMinValue(object? value)
    {
        if (value is null)
        {
            GraphQlFilterInputMin = null;
            MinValue = null;

            if (GraphQlFilterInputMin is null && GraphQlFilterInputMax is null)
            {
                IsActive = false;
            }
        }
        else
        {
            CreateNewMinFilterInput();

            SetValue(GraphQlFilterInputMin, value, false);
            MinValue = value;

            IsActive = true;
        }
    }
}
