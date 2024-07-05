namespace FamilieLaissInterfaces.SignalRServices;

public interface IConvertVideoStatusService: ISignalRBaseService
{
    Task GetWaitingEntriesOverEvents();

    Task GetErrorEntriesOverEvents();

    Task GetSuccessEntriesOverEvents();
}