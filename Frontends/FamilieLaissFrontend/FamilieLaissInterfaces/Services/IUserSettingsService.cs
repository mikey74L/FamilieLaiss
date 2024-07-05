using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Models.Data;
using Microsoft.AspNetCore.Components.Authorization;

namespace FamilieLaissInterfaces.Services;

public interface IUserSettingsService
{
    public Task<IUserSettingsModel?> GetCurrentUserSettings(Task<AuthenticationState>? taskAuthState);

    public Task LoadUserSettings(Task<AuthenticationState>? taskAuthState);

    public Task<IApiResult> SaveUserSettings();

    public void ResetUserSettings();
}
