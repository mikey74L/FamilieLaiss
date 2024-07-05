using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissMappingExtensions.VideoConvertStatus;
using System;

namespace FamilieLaissServices.DataServices;

public class VideoConvertStatusDataService(IFamilieLaissClient familieLaissClient) : BaseDataService(familieLaissClient), IVideoConvertStatusDataService
{
    private IDisposable? _subscriptionForWaiting;
    private IDisposable? _subscriptionForError;
    private IDisposable? _subscriptionForSuccess;
    private IDisposable? _subscriptionForCurrent;

    public void OpenSubscriptionForWaiting(Action<IVideoConvertStatusModel>? callBackForWaiting)
    {
        _subscriptionForWaiting = Client.VideoConvertStatusWaitingSubscription.Watch().Subscribe(result =>
        {
            if (result.Data is null)
            {
                return;
            }

            var mappedModel = result.Data.OnVideoConvertStatusWaiting.Map();

            callBackForWaiting?.Invoke(mappedModel);
        });
    }

    public void OpenSubscriptionForError(Action<IVideoConvertStatusModel>? callBackForError)
    {
        _subscriptionForError = Client.VideoConvertStatusErrorSubscription.Watch().Subscribe(result =>
        {
            if (result.Data is null)
            {
                return;
            }

            var mappedModel = result.Data.OnVideoConvertStatusError.Map();

            callBackForError?.Invoke(mappedModel);
        });
    }

    public void OpenSubscriptionForSuccess(Action<IVideoConvertStatusModel>? callBackForSuccess)
    {
        _subscriptionForSuccess = Client.VideoConvertStatusSuccessSubscription.Watch().Subscribe(result =>
        {
            if (result.Data is null)
            {
                return;
            }

            var mappedModel = result.Data.OnVideoConvertStatusSuccess.Map();

            callBackForSuccess?.Invoke(mappedModel);
        });
    }

    public void OpenSubscriptionForCurrent(Action<IVideoConvertStatusModel>? callBackForCurrent)
    {
        _subscriptionForCurrent = Client.VideoConvertStatusCurrentSubscription.Watch().Subscribe(result =>
        {
            if (result.Data is null)
            {
                return;
            }

            var mappedModel = result.Data.OnVideoConvertStatusCurrent.Map();

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