using System.Globalization;
using System.Resources;

namespace FamilieLaissInterfaces.Services
{
    public interface IValidationHelperService
    {
        string GetMaxLengthMessage(ResourceManager resManager, CultureInfo? cultureInfo, string propertyName, int maxLength);

        string GetRequiredMessage(ResourceManager resManager, CultureInfo? cultureInfo, string propertyName);

        string GetAlreadyExistsMessage(ResourceManager resManager, CultureInfo? cultureInfo, string propertyName);
    }
}
