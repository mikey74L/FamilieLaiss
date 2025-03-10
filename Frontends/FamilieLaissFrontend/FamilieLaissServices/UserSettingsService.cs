using FamilieLaissInterfaces.DataServices;
using FamilieLaissInterfaces.Models;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissInterfaces.Services;
using FamilieLaissServices.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace FamilieLaissServices;

public class UserSettingsService(IUserSettingsStoreService storeService,
    IUserSettingsDataService dataService) : IUserSettingsService
{
    private async Task<string> GetUserIdFromAuthState(Task<AuthenticationState>? taskAuthState)
    {
        if (taskAuthState is not null)
        {
            var authState = await taskAuthState;
            var user = authState.User;

            var userId = user.Claims.FirstOrDefault(x => x.Type == "https://familielaiss.de/user_id")?.Value ?? "";

            return userId;
        }

        return "";
    }

    public async Task LoadUserSettings(Task<AuthenticationState>? taskAuthState)
    {
        await dataService.GetUserSettingsForUserAsync(await GetUserIdFromAuthState(taskAuthState))
            .HandleSuccess((data) =>
            {
                storeService.OriginalUserSettings = data;
                storeService.CurrentUserSettings = storeService.OriginalUserSettings?.Clone();

                return Task.CompletedTask;
            })
            .HandleErrors((_) => Task.CompletedTask);
    }

    public void ResetUserSettings()
    {
        storeService.CurrentUserSettings = storeService.OriginalUserSettings?.Clone();
    }

    public Task<IApiResult> SaveUserSettings()
    {
        return dataService.UpdateUserSettingsForUser(storeService.CurrentUserSettings);
    }

    public async Task<IUserSettingsModel?> GetCurrentUserSettings(Task<AuthenticationState>? taskAuthState)
    {
        if (storeService.CurrentUserSettings is null)
        {
            await LoadUserSettings(taskAuthState);
        }

        return storeService.CurrentUserSettings;
    }
}
