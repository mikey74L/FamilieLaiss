using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissInterfaces.DataServices;

public interface IVideoConvertStatusDataService
{
    void OpenSubscriptionForWaiting(Action<IVideoConvertStatusModel>? callBackForWaiting);
    void OpenSubscriptionForError(Action<IVideoConvertStatusModel>? callBackForError);
    void OpenSubscriptionForSuccess(Action<IVideoConvertStatusModel>? callBackForSuccess);
    void OpenSubscriptionForCurrent(Action<IVideoConvertStatusModel>? callBackForCurrent);

    void CloseSubscriptionForWaiting();
    void CloseSubscriptionForError();
    void CloseSubscriptionForSuccess();
    void CloseSubscriptionForCurrent();
}