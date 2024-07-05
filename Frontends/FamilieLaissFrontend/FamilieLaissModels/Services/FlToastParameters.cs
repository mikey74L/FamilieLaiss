using FamilieLaissInterfaces.Models;

namespace FamilieLaissModels.Services;

public class FlToastParameters : IFlToastParameters
{
    private readonly Dictionary<string, object> _parameters = new();

    public IFlToastParameters Add(string parameterName, object value)
    {
        _parameters[parameterName] = value;
        return this;
    }

    public T Get<T>(string parameterName)
    {
        if (_parameters.TryGetValue(parameterName, out var value))
        {
            return (T)value;
        }

        throw new KeyNotFoundException($"{parameterName} does not exist in toast parameters");
    }

    public Dictionary<string, object> GetDictionary()
    {
        return _parameters;
    }

    public T? TryGet<T>(string parameterName)
    {
        if (_parameters.TryGetValue(parameterName, out var value))
        {
            return (T)value;
        }

        return default;
    }
}