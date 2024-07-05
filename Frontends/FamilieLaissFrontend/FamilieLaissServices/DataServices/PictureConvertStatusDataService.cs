using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissMappingExtensions.PictureConvertStatus;
using System;

namespace FamilieLaissServices.DataServices;

public class PictureConvertStatusDataService(IFamilieLaissClient familieLaissClient) : BaseDataService(familieLaissClient), IPictureConvertStatusDataService
{
    private IDisposable? _subscriptionForWaiting;
    private IDisposable? _subscriptionForError;
    private IDisposable? _subscriptionForSuccess;
    private IDisposable? _subscriptionForCurrent;

    public void OpenSubscriptionForWaiting(Action<IPictureConvertStatusModel>? callBackForWaiting)
    {
        _subscriptionForWaiting = Client.PictureConvertStatusWaitingSubscription.Watch().Subscribe(result =>
        {
            if (result.Data is null)
            {
                return;
            }

            var mappedModel = result.Data.OnPictureConvertStatusWaiting.Map();

            callBackForWaiting?.Invoke(mappedModel);
        });
    }

    public void OpenSubscriptionForError(Action<IPictureConvertStatusModel>? callBackForError)
    {
        _subscriptionForError = Client.PictureConvertStatusErrorSubscription.Watch().Subscribe(result =>
        {
            if (result.Data is null)
            {
                return;
            }

            var mappedModel = result.Data.OnPictureConvertStatusError.Map();

            callBackForError?.Invoke(mappedModel);
        });
    }

    public void OpenSubscriptionForSuccess(Action<IPictureConvertStatusModel>? callBackForSuccess)
    {
        _subscriptionForSuccess = Client.PictureConvertStatusSuccessSubscription.Watch().Subscribe(result =>
        {
            if (result.Data is null)
            {
                return;
            }

            var mappedModel = result.Data.OnPictureConvertStatusSuccess.Map();

            callBackForSuccess?.Invoke(mappedModel);
        });
    }

    public void OpenSubscriptionForCurrent(Action<IPictureConvertStatusModel>? callBackForCurrent)
    {
        _subscriptionForCurrent = Client.PictureConvertStatusCurrentSubscription.Watch().Subscribe(result =>
        {
            if (result.Data is null)
            {
                return;
            }

            var mappedModel = result.Data.OnPictureConvertStatusCurrent.Map();

            callBackForCurrent?.Invoke(mappedModel);
        });
    }

    public void CloseSubscriptionForWaiting()
    {
        _subscriptionForWaiting?.Dispose();
    }

    public void CloseSubscriptionForError()
    {
        _subscriptionForError?.Dispose();
    }

    public void CloseSubscriptionForSuccess()
    {
        _subscriptionForSuccess?.Dispose();
    }

    public void CloseSubscriptionForCurrent()
    {
        _subscriptionForCurrent?.Dispose();
    }
}