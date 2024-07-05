using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.Models;

public class GoogleGeoCodingAddressModel : IGoogleGeoCodingAddressModel
{
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
    public string? StreetName { get; set; }
    public string? Hnr { get; set; }
    public string? Zip { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
}
