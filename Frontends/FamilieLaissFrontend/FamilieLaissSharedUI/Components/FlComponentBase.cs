using FamilieLaissSharedUI.Extensions;
using FamilieLaissSharedUI.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissSharedUI.Components;

public class FlComponentBase<TViewModel> : ComponentBase, IDisposable, IView<TViewModel> where TViewModel : IViewModelBase
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

    #endregion

    #region Inject
    [Inject]
    protected TViewModel ViewModel { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;
    #endregion

    #region Overrides for Lifecycle
    protected override void OnInitialized()
    {
        base.OnInitialized();

        ViewModel.PropertyChanged += (_, _) => InvokeAsync(StateHasChanged);

        foreach (var item in ViewModel.QueryStringParameters)
        {
            if (NavigationManager.TryGetQueryString(item.Value, item.Key, out var value))
            {
                ViewModel.SetParameter(item.Key, value);
            }
        }

        ViewModel.OnInitialized();
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        await ViewModel.OnInitializedAsync();
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        ViewModel.OnParametersSet();
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        await ViewModel.OnParametersSetAsync();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        ViewModel.OnAfterRender(firstRender);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        await ViewModel.OnAfterRenderAsync(firstRender);
    }
    #endregion

    #region IDisposable
    public void Dispose()
    {
        ViewModel.PropertyChanged -= (_, _) => InvokeAsync(StateHasChanged);
        ViewModel.Dispose();
    }
    #endregion
}