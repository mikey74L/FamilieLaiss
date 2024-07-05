using FamilieLaissInterfaces.Services;
using MudBlazor;
using System.Threading.Tasks;

namespace FamilieLaissServices;

public class MessageBoxService : IMessageBoxService
{
    public MudMessageBox MsgBoxQuestion { get; set; } = default!;
    public MudMessageBox MsgBoxMessage { get; set; } = default!;
    public MudMessageBox MsgBoxQuestionCancel { get; set; } = default!;

    public string MessageBoxTitle { get; set; } = string.Empty;
    public string MessageBoxMessage { get; set; } = string.Empty;
    public string YesButtonText { get; set; } = string.Empty;
    public string YesButtonIcon { get; set; } = string.Empty;
    public string NoButtonText { get; set; } = string.Empty;
    public string NoButtonIcon { get; set; } = string.Empty;
    public string CancelButtonText { get; set; } = string.Empty;
    public string CancelButtonIcon { get; set; } = string.Empty;
    public bool YesButtonRed { get; set; }
    public bool NoButtonRed { get; set; }
    public bool CancelButtonRed { get; set; }

    public Task<bool?> Message(string title, string message, string buttonText, bool buttonRed)
    {
        return Message(title, message, buttonText, buttonRed, "");
    }

    public async Task<bool?> Message(string title, string message, string buttonText, bool buttonRed, string buttonIcon)
    {
        MessageBoxTitle = title;
        MessageBoxMessage = message;
        YesButtonText = buttonText;
        YesButtonIcon = buttonIcon;
        YesButtonRed = buttonRed;

        return await MsgBoxMessage.Show();
    }

    public Task<bool?> Question(string title, string message, string yesButtonText, string noButtonText, bool yesButtonRed, bool noButtonRed)
    {
        return Question(title, message, yesButtonText, noButtonText, yesButtonRed, noButtonRed, "", "");
    }

    public async Task<bool?> Question(string title, string message, string yesButtonText, string noButtonText,
        bool yesButtonRed, bool noButtonRed, string yesButtonIcon = "", string noButtonIcon = "")
    {
        MessageBoxTitle = title;
        MessageBoxMessage = message;
        YesButtonText = yesButtonText;
        YesButtonIcon = yesButtonIcon;
        NoButtonText = noButtonText;
        NoButtonIcon = noButtonIcon;
        YesButtonRed = yesButtonRed;
        NoButtonRed = noButtonRed;

        return await MsgBoxQuestion.Show();
    }

    public Task<bool?> QuestionConfirmRed(string title, string message, string yesButtonText, string noButtonText)
    {
        return QuestionConfirmRed(title, message, yesButtonText, noButtonText, "", "");
    }

    public async Task<bool?> QuestionConfirmRed(string title, string message, string yesButtonText, string noButtonText, string yesButtonIcon = "", string noButtonIcon = "")
    {
        MessageBoxTitle = title;
        MessageBoxMessage = message;
        YesButtonText = yesButtonText;
        YesButtonIcon = yesButtonIcon;
        NoButtonText = noButtonText;
        NoButtonIcon = noButtonIcon;
        YesButtonRed = true;
        NoButtonRed = false;

        return await MsgBoxQuestion.Show();
    }

    public Task<bool?> QuestionWithCancel(string title, string message, string yesButtonText, string noButtonText, string cancelButtonText,
        bool yesButtonRed, bool noButtonRed, bool cancelButtonRed)
    {
        return QuestionWithCancel(title, message, yesButtonText, noButtonText, cancelButtonText, yesButtonRed, noButtonRed, cancelButtonRed, "", "", "");
    }

    public async Task<bool?> QuestionWithCancel(string title, string message, string yesButtonText, string noButtonText, string cancelButtonText,
        bool yesButtonRed, bool noButtonRed, bool cancelButtonRed,
        string yesButtonIcon = "", string noButtonIcon = "", string cancelButtonIcon = "")
    {
        MessageBoxTitle = title;
        MessageBoxMessage = message;
        YesButtonText = yesButtonText;
        YesButtonIcon = yesButtonIcon;
        YesButtonRed = yesButtonRed;
        NoButtonText = noButtonText;
        NoButtonIcon = noButtonIcon;
        NoButtonRed = noButtonRed;
        CancelButtonText = cancelButtonText;
        CancelButtonIcon = cancelButtonIcon;
        CancelButtonRed = cancelButtonRed;

        return await MsgBoxQuestionCancel.Show();
    }
}
