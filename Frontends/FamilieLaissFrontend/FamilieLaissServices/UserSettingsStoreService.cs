using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;

namespace FamilieLaissServices;

public class UserSettingsStoreService : IUserSettingsStoreService
{
    public IUserSettingsModel? OriginalUserSettings { get; set; }
    public IUserSettingsModel? CurrentUserSettings { get; set; }
}
