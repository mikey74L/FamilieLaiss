using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.DataServices;
using StrawberryShake;
using System.Threading.Tasks;

namespace FamilieLaissServices.DataServices;

public class GeneralDataService(IFamilieLaissClient familieLaissClient)
    : BaseDataService(familieLaissClient), IGeneralDataService
{
    public async Task<string> GetGoogleMapsApiKeyAsync()
    {
        var response = await Client.GetGoogleMapsApiKey.ExecuteAsync();

        if (response.IsSuccessResult() && response.Data is not null)
        {
            return response.Data.GoogleMapsApiKey;
        }

        return "";
    }
}
