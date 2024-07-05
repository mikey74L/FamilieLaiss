namespace FamilieLaissInterfaces.SignalRServices;

public interface ISignalRBaseService
{
    Task StartHubConnectionAsync();

    Task StopHubConnectionAsync();
}
