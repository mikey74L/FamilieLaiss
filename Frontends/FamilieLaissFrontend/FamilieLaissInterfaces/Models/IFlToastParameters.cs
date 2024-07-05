namespace FamilieLaissInterfaces.Models;

public interface IFlToastParameters
{
    public Dictionary<string, object> GetDictionary();

    public IFlToastParameters Add(string parameterName, object value);

    public T Get<T>(string parameterName);

    public T? TryGet<T>(string parameterName);
}