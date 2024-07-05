using System;
using FamilieLaissInterfaces.Enums;
using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Services;
using FamilieLaissModels.Services;
using Microsoft.AspNetCore.Components;

namespace FamilieLaissServices;

public class ToastService : IToastService
{
    /// <summary>
    /// A event that will be invoked when showing a toast
    /// </summary>
    public event Action<FLToastLevel, RenderFragment, Action<IFlToastSettings>?>? OnShow;

    /// <summary>
    /// A event that will be invoked when clearing all toasts
    /// </summary>
    public event Action? OnClearAll;

    /// <summary>
    /// A event that will be invoked when showing a toast with a custom component
    /// </summary>
    public event Action<Type, IFlToastParameters?, Action<IFlToastSettings>?>? OnShowComponent;

    /// <summary>
    /// A event that will be invoked when clearing toasts
    /// </summary>
    public event Action<FLToastLevel>? OnClearToasts;

    /// <summary>
    /// A event that will be invoked when clearing custom toast components
    /// </summary>
    public event Action? OnClearCustomToasts;

    /// <summary>
    /// A event that will be invoked to clear all queued toasts
    /// </summary>
    public event Action? OnClearQueue;

    /// <summary>
    /// A event that will be invoked to clear queued toast of specified level
    /// </summary>
    public event Action<FLToastLevel>? OnClearQueueToasts;

    /// <summary>
    /// Shows a information toast 
    /// </summary>
    /// <param name="message">Text to display on the toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowInfo(string message, Action<IFlToastSettings>? settings = null)
        => ShowToast(FLToastLevel.Info, message, settings);

    /// <summary>
    /// Shows a information toast 
    /// </summary>
    /// <param name="message">RenderFragment to display on the toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowInfo(RenderFragment message, Action<IFlToastSettings>? settings = null)
        => ShowToast(FLToastLevel.Info, message, settings);

    /// <summary>
    /// Shows a success toast 
    /// </summary>
    /// <param name="message">Text to display on the toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowSuccess(string message, Action<IFlToastSettings>? settings = null)
        => ShowToast(FLToastLevel.Success, message, settings);

    /// <summary>
    /// Shows a success toast 
    /// </summary>
    /// <param name="message">RenderFragment to display on the toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowSuccess(RenderFragment message, Action<IFlToastSettings>? settings = null)
        => ShowToast(FLToastLevel.Success, message, settings);

    /// <summary>
    /// Shows a warning toast 
    /// </summary>
    /// <param name="message">Text to display on the toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowWarning(string message, Action<IFlToastSettings>? settings = null)
        => ShowToast(FLToastLevel.Warning, message, settings);

    /// <summary>
    /// Shows a warning toast 
    /// </summary>
    /// <param name="message">RenderFragment to display on the toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowWarning(RenderFragment message, Action<IFlToastSettings>? settings = null)
        => ShowToast(FLToastLevel.Warning, message, settings);

    /// <summary>
    /// Shows a error toast 
    /// </summary>
    /// <param name="message">Text to display on the toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowError(string message, Action<IFlToastSettings>? settings = null)
        => ShowToast(FLToastLevel.Error, message, settings);

    /// <summary>
    /// Shows a error toast 
    /// </summary>
    /// <param name="message">RenderFragment to display on the toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowError(RenderFragment message, Action<IFlToastSettings>? settings = null)
        => ShowToast(FLToastLevel.Error, message, settings);

    /// <summary>
    /// Shows a toast using the supplied settings
    /// </summary>
    /// <param name="level">Toast level to display</param>
    /// <param name="message">Text to display on the toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowToast(FLToastLevel level, string message, Action<IFlToastSettings>? settings = null)
        => ShowToast(level, builder => builder.AddContent(0, message), settings);


    /// <summary>
    /// Shows a toast using the supplied settings
    /// </summary>
    /// <param name="level">Toast level to display</param>
    /// <param name="message">RenderFragment to display on the toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowToast(FLToastLevel level, RenderFragment message, Action<IFlToastSettings>? settings = null)
        => OnShow?.Invoke(level, message, settings);

    /// <summary>
    /// Shows the toast with the component type
    /// </summary>
    public void ShowToast<TComponent>() where TComponent : IComponent
        => ShowToast(typeof(TComponent), new FlToastParameters(), null);

    /// <summary>
    /// Shows the toast with the component type />,
    /// passing the specified <paramref name="parameters"/> 
    /// </summary>
    /// <param name="contentComponent">Type of component to display.</param>
    /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed.</param>
    /// <param name="settings">Settings to configure the toast component.</param>
    public void ShowToast(Type contentComponent, IFlToastParameters? parameters, Action<IFlToastSettings>? settings)
    {
        if (!typeof(IComponent).IsAssignableFrom(contentComponent))
        {
            throw new ArgumentException($"{contentComponent.FullName} must be a Blazor Component");
        }

        OnShowComponent?.Invoke(contentComponent, parameters, settings);
    }

    /// <summary>
    /// Shows the toast with the component type />,
    /// passing the specified <paramref name="parameters"/> 
    /// </summary>
    /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed.</param>
    public void ShowToast<TComponent>(IFlToastParameters parameters) where TComponent : IComponent
        => ShowToast(typeof(TComponent), parameters, null);

    /// <summary>
    /// Shows a toast using the supplied settings
    /// </summary>
    /// <param name="settings">Toast settings to be used</param>
    public void ShowToast<TComponent>(Action<IFlToastSettings>? settings) where TComponent : IComponent
        => ShowToast(typeof(TComponent), null, settings);

    /// <summary>
    /// Shows a toast using the supplied parameter and settings
    /// </summary>
    /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed.</param>
    /// <param name="settings">Toast settings to be used</param>
    public void ShowToast<TComponent>(IFlToastParameters parameters, Action<IFlToastSettings>? settings)
        where TComponent : IComponent
        => ShowToast(typeof(TComponent), parameters, settings);

    /// <summary>
    /// Removes all toasts
    /// </summary>
    public void ClearAll()
        => OnClearAll?.Invoke();

    /// <summary>
    /// Removes all toasts with a specified <paramref name="toastLevel"/>.
    /// </summary>
    public void ClearToasts(FLToastLevel toastLevel)
        => OnClearToasts?.Invoke(toastLevel);

    /// <summary>
    /// Removes all toasts with toast level warning
    /// </summary>
    public void ClearWarningToasts()
        => OnClearToasts?.Invoke(FLToastLevel.Warning);

    /// <summary>
    /// Removes all toasts with toast level info
    /// </summary>
    public void ClearInfoToasts()
        => OnClearToasts?.Invoke(FLToastLevel.Info);

    /// <summary>
    /// Removes all toasts with toast level success
    /// </summary>
    public void ClearSuccessToasts()
        => OnClearToasts?.Invoke(FLToastLevel.Success);

    /// <summary>
    /// Removes all toasts with toast level error
    /// </summary>
    public void ClearErrorToasts()
        => OnClearToasts?.Invoke(FLToastLevel.Error);

    /// <summary>
    /// Removes all custom component toasts
    /// </summary>
    public void ClearCustomToasts()
        => OnClearCustomToasts?.Invoke();

    /// <summary>
    /// Removes all queued toasts
    /// </summary>
    /// 
    public void ClearQueue()
        => OnClearQueue?.Invoke();

    /// <summary>
    /// Removes all queued toasts with a specified <paramref name="toastLevel"/>.
    /// </summary>
    public void ClearQueueToasts(FLToastLevel toastLevel)
        => OnClearQueueToasts?.Invoke(toastLevel);

    /// <summary>
    /// Removes all queued toasts with toast level warning
    /// </summary>
    public void ClearQueueWarningToasts()
        => OnClearQueueToasts?.Invoke(FLToastLevel.Warning);

    /// <summary>
    /// Removes all queued toasts with toast level info
    /// </summary>
    public void ClearQueueInfoToasts()
        => OnClearQueueToasts?.Invoke(FLToastLevel.Info);

    /// <summary>
    /// Removes all queued toasts with toast level success
    /// </summary>
    public void ClearQueueSuccessToasts()
        => OnClearQueueToasts?.Invoke(FLToastLevel.Success);

    /// <summary>
    /// Removes all queued toasts with toast level error
    /// </summary>
    public void ClearQueueErrorToasts()
        => OnClearQueueToasts?.Invoke(FLToastLevel.Error);
}