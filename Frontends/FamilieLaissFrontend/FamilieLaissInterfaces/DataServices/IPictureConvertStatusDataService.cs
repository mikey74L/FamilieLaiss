using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissInterfaces.DataServices;

public interface IPictureConvertStatusDataService
{
    void OpenSubscriptionForWaiting(Action<IPictureConvertStatusModel>? callBackForWaiting);
    void OpenSubscriptionForError(Action<IPictureConvertStatusModel>? callBackForError);
    void OpenSubscriptionForSuccess(Action<IPictureConvertStatusModel>? callBackForSuccess);
    void OpenSubscriptionForCurrent(Action<IPictureConvertStatusModel>? callBackForCurrent);

    void CloseSubscriptionForWaiting();
    void CloseSubscriptionForError();
    void CloseSubscriptionForSuccess();
    void CloseSubscriptionForCurrent();
}