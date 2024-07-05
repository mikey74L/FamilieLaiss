using AutoMapper;
using Upload.Domain.ValueObjects;
using Upload.DTO.ValueData;

namespace Upload.API.AutoMapper;

public class GeoCodingAddressProfile: Profile
{
    public GeoCodingAddressProfile()
    {
        CreateMap<GoogleGeoCodingAddress, GoogleGeoCodingAddressDto>();
    }
}