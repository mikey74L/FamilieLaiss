using HotChocolate;

namespace Upload.API.Models;

[GraphQLDescription("The filter data info for upload picture exif info")]
public class UploadPictureExifInfoFilterData
{
    [GraphQLDescription("The distinct list of distinct makes")]
    public List<string> Make { get; set; } = [];

    [GraphQLDescription("The distinct list of distinct models")]
    public List<string> Model { get; set; } = [];

    [GraphQLDescription("The distinct list of fnumbers")]
    public List<double> FNumbers { get; set; } = [];

    [GraphQLDescription("The distinct list of ISO sensitivities")]
    public List<int> IsoSensitivities { get; set; } = [];

    [GraphQLDescription("The distinct list of exposure times")]
    public List<double> ExposureTimes { get; set; } = [];

    [GraphQLDescription("The distinct list of shutter speeds")]
    public List<double> ShutterSpeeds { get; set; } = [];

    [GraphQLDescription("The distinct list of focal lengths")]
    public List<double> FocalLengths { get; set; } = [];
}