using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissInterfaces.Services;

public interface IUserSettingsStoreService
{
    public IUserSettingsModel? OriginalUserSettings { get; set; }
    public IUserSettingsModel? CurrentUserSettings { get; set; }
}
