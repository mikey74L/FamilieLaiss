using System.ComponentModel;
using FamilieLaissSharedUI.Extensions;
using FamilieLaissSharedUI.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissSharedUI.Components;

public class FlComponentBase<TViewModel>(TViewModel viewModel, NavigationManager navigationManager)
    : ComponentBase, IDisposable, IView<TViewModel>
    where TViewModel : IViewModelBase
{
    #region Properties

    protected string FullPageHeightGrid =>
        "calc(100vh - var(--mud-appbar-height) - var(--fl-content-border-size) - var(--fl-additional-substract-content-page-height) - var(--fl-grid-additional))";

    protected string FullPageHeightCalculated(int additionalSubtractValue)
    {
        return
            $"calc(100vh - var(--mud-appbar-height) - var(--fl-content-border-size) - var(--fl-additional-substract-content-page-height) - {additionalSubtractValue}px)";
    }

    protected bool IsWebAssembly => OperatingSystem.IsBrowser();

    private readonly TViewModel _viewModel = viewModel;
    #endregion

    #region Private Methods

    private void PropertyChangedHandler(object? sender, PropertyChangedEventArgs e)
    {
        InvokeAsync(StateHasChanged);
    }
    #endregion
    
    #region Overrides for Lifecycle
    protected override void OnInitialized()
    {
        base.OnInitialized();

        _viewModel.PropertyChanged += PropertyChangedHandler;

        foreach (var item in _viewModel.QueryStringParameters)
        {
            if (navigationManager.TryGetQueryString(item.Value, item.Key, out var value))
            {
                _viewModel.SetParameter(item.Key, value);
            }
        }

        _viewModel.OnInitialized();
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await _viewModel.OnInitializedAsync();
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        _viewModel.OnParametersSet();
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        await _viewModel.OnParametersSetAsync();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        _viewModel.OnAfterRender(firstRender);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        await _viewModel.OnAfterRenderAsync(firstRender);
    }
    #endregion

    #region Public Properties
    public TViewModel ViewModel => _viewModel;
    
    public NavigationManager NavigationManager => navigationManager;
    #endregion
    
    #region IDisposable
    public void Dispose()
    {
        _viewModel.PropertyChanged -= PropertyChangedHandler;
        _viewModel.Dispose();
    }
    #endregion
}