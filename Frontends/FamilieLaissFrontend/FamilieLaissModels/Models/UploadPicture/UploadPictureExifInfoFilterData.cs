using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.Models.UploadPicture;

public class UploadPictureExifInfoFilterData : IUploadPictureExifInfoFilterData
{
    public List<string> Makes { get; set; } = [];
    public List<string> Models { get; set; } = [];
    public List<double> FNumbers { get; set; } = [];
    public List<int> ISOSensitivities { get; set; } = [];

    public List<double> ExposureTimes { get; set; } = [];
    public List<double> ShutterSpeeds { get; set; } = [];
    public List<double> FocalLengths { get; set; } = [];
}
