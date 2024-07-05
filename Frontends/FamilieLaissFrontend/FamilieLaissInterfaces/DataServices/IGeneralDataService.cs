namespace FamilieLaissInterfaces.DataServices;

public interface IGeneralDataService
{
    Task<string> GetGoogleMapsApiKeyAsync();
}
