namespace FamilieLaissInterfaces.SignalRServices;

public interface IConvertPictureStatusService : ISignalRBaseService
{
    Task GetWaitingEntriesOverEvents();

    Task GetErrorEntriesOverEvents();

    Task GetSuccessEntriesOverEvents();
}
