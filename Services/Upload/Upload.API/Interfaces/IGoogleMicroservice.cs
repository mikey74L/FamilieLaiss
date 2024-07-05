using Google.DTO;

namespace Upload.API.Interfaces;

public interface IGoogleMicroService
{
    Task<GoogleGeoCodingAdressDTO?> GetGoogleGeoCodingAdressAsync(GoogleGeoCodingRequestDTO request);
}
