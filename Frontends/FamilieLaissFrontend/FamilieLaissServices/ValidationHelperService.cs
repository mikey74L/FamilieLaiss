using FamilieLaissInterfaces.Services;
using System.Globalization;
using System.Resources;

namespace FamilieLaissServices
{
    public class ValidationHelperService : IValidationHelperService
    {
        public string GetAlreadyExistsMessage(ResourceManager resManager, CultureInfo? cultureInfo, string propertyName)
        {
            return resManager.GetString($"{propertyName}_AlreadyExists", cultureInfo)!;
        }

        public string GetMaxLengthMessage(ResourceManager resManager, CultureInfo? cultureInfo, string propertyName, int maxLength)
        {
            var locString = resManager.GetString($"{propertyName}_MaxLength", cultureInfo)!;

            return string.Format(locString, maxLength);
        }

        public string GetRequiredMessage(ResourceManager resManager, CultureInfo? cultureInfo, string propertyName)
        {
            return resManager.GetString($"{propertyName}_Required", cultureInfo)!;
        }
    }
}
