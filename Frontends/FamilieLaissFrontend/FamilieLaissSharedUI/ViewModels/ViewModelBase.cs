using CommunityToolkit.Mvvm.ComponentModel;
using FamilieLaissEnums;
using FamilieLaissInterfaces.Services;
using FamilieLaissSharedUI.Interfaces;
using MudBlazor;

namespace FamilieLaissSharedUI.ViewModels;

public abstract partial class ViewModelBase : ObservableObject, IDisposable, IViewModelBase
{
    #region Private Service Members
    private readonly ISnackbar snackbarService;
    private readonly IMessageBoxService messageBoxService;
    #endregion

    #region Protected Fields
    protected System.Timers.Timer? loadingTimer;
    #endregion

    #region Properties
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsBusy))]
    private bool _isLoading;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsBusy))]
    private bool _isSaving;

    [ObservableProperty]
    private EnumSaveMode _saveMode;

    [ObservableProperty]
    private bool _hasError;

    public bool IsBusy => IsLoading || IsSaving;

    public Dictionary<string, Type> QueryStringParameters { get; } = [];
    #endregion

    #region C'tor
    public ViewModelBase(ISnackbar snackbarService, IMessageBoxService messageBoxService)
    {
        this.snackbarService = snackbarService;
        this.messageBoxService = messageBoxService;

    }
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
    protected DialogOptions GetDialogOptions()
    {
        DialogOptions dialogOptions = new()
        {
            ClassBackground = "blury-dialog"
        };

        return dialogOptions;
    }
    #endregion

    #region Loading Debounce
    private void DisposeLoadingTimer()
    {
        if (loadingTimer is not null)
        {
            loadingTimer.Elapsed -= LoadingTimer_Elapsed;
            loadingTimer.Stop();
            loadingTimer.Dispose();
            loadingTimer = null;
        }
    }

    protected void StartLoading()
    {
        DisposeLoadingTimer();
        loadingTimer = new(500);
        loadingTimer.Elapsed += LoadingTimer_Elapsed;
        loadingTimer.Enabled = true;
        loadingTimer.Start();
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
