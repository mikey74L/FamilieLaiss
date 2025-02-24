using CommunityToolkit.Mvvm.ComponentModel;
using FamilieLaissEnums;
using FamilieLaissInterfaces.Services;
using FamilieLaissSharedUI.Interfaces;
using MudBlazor;

namespace FamilieLaissSharedUI.ViewModels;

public abstract partial class ViewModelBase(ISnackbar snackbarService, IMessageBoxService messageBoxService)
    : ObservableObject, IDisposable, IViewModelBase
{
    #region Protected Fields
    protected System.Timers.Timer? LoadingTimer;
    #endregion

    #region Properties
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsBusy))]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsBusy))]
    public partial bool IsSaving { get; set; }

    [ObservableProperty]
    public partial EnumSaveMode SaveMode { get; set; }

    [ObservableProperty]
    public partial bool HasError { get; set; }

    public bool IsBusy => IsLoading || IsSaving;

    public Dictionary<string, Type> QueryStringParameters { get; } = [];
    #endregion

    #region Lifecycle Methods
    //This Methods will be called from FlBaseComponent
    public virtual void OnInitialized()
    {
    }

    public virtual Task OnInitializedAsync()
    {
        return Task.CompletedTask;
    }

    public virtual void OnParametersSet()
    {
    }

    public virtual Task OnParametersSetAsync()
    {
        return Task.CompletedTask;
    }

    public virtual void OnAfterRender(bool firstRender)
    {
    }

    public virtual Task OnAfterRenderAsync(bool firstRender)
    {
        return Task.CompletedTask;
    }
    #endregion

    #region Toast
    protected void ShowErrorToast(string message)
    {
        snackbarService.Add(message, Severity.Error);
    }

    protected void ShowInfoToast(string message)
    {
        snackbarService.Add(message, Severity.Info);
    }

    protected void ShowWarningToast(string message)
    {
        snackbarService.Add(message, Severity.Warning);
    }

    protected void ShowSuccessToast(string message)
    {
        snackbarService.Add(message, Severity.Success);
    }
    #endregion

    #region MessageBox
    protected Task<bool?> Question(string title, string message, string yesButtonText, string noButtonText, bool yesButtonRed, bool noButtonRed)
    {
        return messageBoxService.Question(title, message, yesButtonText, noButtonText, yesButtonRed, noButtonRed);
    }

    protected Task<bool?> Question(string title, string message, string yesButtonText, string noButtonText,
        bool yesButtonRed, bool noButtonRed,
        string yesButtonIcon = "", string noButtonIcon = "")
    {
        return messageBoxService.Question(title, message, yesButtonText, noButtonText, yesButtonRed, noButtonRed, yesButtonIcon, noButtonIcon);
    }

    protected Task<bool?> QuestionConfirmRed(string title, string message, string yesButtonText, string noButtonText)
    {
        return messageBoxService.QuestionConfirmRed(title, message, yesButtonText, noButtonText);
    }

    protected Task<bool?> QuestionConfirmRed(string title, string message, string yesButtonText, string noButtonText,
        string yesButtonIcon = "", string noButtonIcon = "")
    {
        return messageBoxService.QuestionConfirmRed(title, message, yesButtonText, noButtonText, yesButtonIcon,
            noButtonIcon);
    }

    protected Task<bool?> QuestionConfirmWithCancel(string title, string message, string yesButtonText, string noButtonText, string cancelButtonText,
        bool yesButtonRed, bool noButtopnRed, bool cancelButtonRed)
    {
        return messageBoxService.QuestionWithCancel(title, message, yesButtonText, noButtonText, cancelButtonText, yesButtonRed, noButtopnRed, cancelButtonRed);
    }

    protected Task<bool?> Message(string title, string message, string buttonText, bool buttonRed)
    {
        return messageBoxService.Message(title, message, buttonText, buttonRed);
    }

    protected Task<bool?> Message(string title, string message, string buttonText, bool buttonRed, string buttonIcon)
    {
        return messageBoxService.Message(title, message, buttonText, buttonRed, buttonIcon);
    }
    #endregion

    #region Dialog
    protected DialogOptions GetDialogOptions(bool? closeButton = null, bool? closeOnEscape = null,
        DialogPosition? dialogPosition = null, MaxWidth? maxWidth = null)
    {
        DialogOptions dialogOptions = new()
        {
            CloseButton = closeButton,
            CloseOnEscapeKey = closeOnEscape,
            Position = dialogPosition,
            MaxWidth = maxWidth,
            BackgroundClass = "blury-dialog"
        };

        return dialogOptions;
    }
    #endregion

    #region Loading Debounce
    private void DisposeLoadingTimer()
    {
        if (LoadingTimer is not null)
        {
            LoadingTimer.Elapsed -= LoadingTimer_Elapsed;
            LoadingTimer.Stop();
            LoadingTimer.Dispose();
            LoadingTimer = null;
        }
    }

    protected void StartLoading()
    {
        DisposeLoadingTimer();
        LoadingTimer = new(500);
        LoadingTimer.Elapsed += LoadingTimer_Elapsed;
        LoadingTimer.Enabled = true;
        LoadingTimer.Start();
    }

    private void LoadingTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        DisposeLoadingTimer();
        DebouncedLoading();
    }
    #endregion

    #region Parameters
    public virtual void SetParameter(string name, object value)
    {
    }
    #endregion

    #region Overridable Methods
    protected virtual void NotifyStateChanged() => OnPropertyChanged((string?)null);

    protected virtual void DebouncedLoading()
    {
    }
    #endregion

    #region Abstract Methods
    public abstract void Dispose();
    #endregion
}
