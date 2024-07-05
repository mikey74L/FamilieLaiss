using MudBlazor;

namespace FamilieLaissInterfaces.Services;

public interface IMessageBoxService
{
    MudMessageBox MsgBoxQuestion { get; set; }
    MudMessageBox MsgBoxMessage { get; set; }
    MudMessageBox MsgBoxQuestionCancel { get; set; }
    string MessageBoxTitle { get; set; }
    string MessageBoxMessage { get; set; }
    string YesButtonText { get; set; }
    string YesButtonIcon { get; set; }
    string NoButtonText { get; set; }
    string NoButtonIcon { get; set; }
    string CancelButtonText { get; set; }
    string CancelButtonIcon { get; set; }
    bool YesButtonRed { get; set; }
    bool NoButtonRed { get; set; }
    bool CancelButtonRed { get; set; }

    Task<bool?> Question(string title, string message, string yesButtonText, string noButtonText,
        bool yesButtonRed, bool noButtonRed);

    Task<bool?> Question(string title, string message, string yesButtonText, string noButtonText,
        bool yesButtonRed, bool noButtonRed,
        string yesButtonIcon = "", string noButtonIcon = "");

    Task<bool?> QuestionWithCancel(string title, string message, string yesButtonText, string noButtonText, string cancelButtonText,
        bool yesButtonRed, bool noButtonRed, bool cancelButtonRed);

    Task<bool?> QuestionWithCancel(string title, string message, string yesButtonText, string noButtonText, string cancelButtonText,
        bool yesButtonRed, bool noButtonRed, bool cancelButtonRed,
        string yesButtonIcon = "", string noButtonIcon = "", string cancelButtonIcon = "");

    Task<bool?> QuestionConfirmRed(string title, string message, string yesButtonText, string noButtonText);

    Task<bool?> QuestionConfirmRed(string title, string message, string yesButtonText, string noButtonText,
            string yesButtonIcon = "", string noButtonIcon = "");

    Task<bool?> Message(string title, string message, string buttonText, bool buttonRed);

    Task<bool?> Message(string title, string message, string buttonText, bool buttonRed, string buttonIcon);
}
