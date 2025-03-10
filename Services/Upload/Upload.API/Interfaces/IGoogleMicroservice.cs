using Google.DTO;

namespace Upload.API.Interfaces;

public interface IGoogleMicroService
{
    Task<GoogleGeoCodingAdressDTO?> GetGoogleGeoCodingAddressAsync(GoogleGeoCodingRequestDTO request);
}