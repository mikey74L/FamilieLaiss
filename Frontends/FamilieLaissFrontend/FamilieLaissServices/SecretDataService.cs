using FamilieLaissInterfaces.Services;

namespace FamilieLaissServices;

public class SecretDataService : ISecretDataService
{
    public string GoogleMapsApiKey { get; set; } = string.Empty;
}
