using Google.DTO;

namespace FLBackEnd.API.Interfaces;

public interface IGoogleMicroService
{
    Task<GoogleGeoCodingAdressDTO?> GetGoogleGeoCodingAddressAsync(GoogleGeoCodingRequestDTO request);
}