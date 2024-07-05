#nullable disable
using FamilieLaissModels.Models.UserSettings;

namespace FamilieLaissGraphQLDataLayer.ResponceTypes.UserSettings;

public class GetAllUserSettingsResponce
{
    public List<UserSettingsModel> UserSettings { get; set; }
}

public class GetUserSettingsForUserResponce : GetAllUserSettingsResponce
{
}

public class GetUpdateUserSettingsResponce
{
    public UserSettingsModel UserSettings { get; set; }
}
