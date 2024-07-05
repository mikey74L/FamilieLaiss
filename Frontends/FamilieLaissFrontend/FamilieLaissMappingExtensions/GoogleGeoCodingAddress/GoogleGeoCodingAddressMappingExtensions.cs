using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Models.Data;
using FamilieLaissModels.Models;

namespace FamilieLaissMappingExtensions.GoogleGeoCodingAddress;

public static class GoogleGeoCodingAddressMappingExtensions
{
    public static IGoogleGeoCodingAddressModel? Map(this IFrGoogleGeoCodingAddress? source)
    {
        if (source is not null)
        {
            var result = new GoogleGeoCodingAddressModel()
            {
                Latitude = source.Latitude,
                Longitude = source.Longitude,
                StreetName = source.StreetName,
                Hnr = source.Hnr,
                Zip = source.Zip,
                City = source.City,
                Country = source.Country
            };

            return result;
        }

        return null;
    }
}
