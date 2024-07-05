using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissInterfaces.DataServices
{
    public interface IUserSettingsDataService
    {
        Task<IApiResult<IUserSettingsModel>> GetUserSettingsForUserAsync(string id);

        Task<IApiResult> UpdateUserSettingsForUser(IUserSettingsModel? model);
    }
}
