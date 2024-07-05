using System.ComponentModel;

namespace FamilieLaissSharedUI.Interfaces;

public interface IViewModelBase : INotifyPropertyChanged
{
    void OnInitialized();

    Task OnInitializedAsync();

    void OnParametersSet();

    Task OnParametersSetAsync();

    void OnAfterRender(bool firstRender);

    Task OnAfterRenderAsync(bool firstRender);

    Dictionary<string, Type> QueryStringParameters { get; }

    void SetParameter(string name, object value);

    void Dispose();
}