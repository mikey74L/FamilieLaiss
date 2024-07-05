using FamilieLaissInterfaces.Models.Data;
using FamilieLaissResources.Resources.Models.UploadPicture;

namespace FamilieLaissModels.Models.UploadPicture;

public class UploadPictureExifInfoModel : IUploadPictureExifInfoModel
{
    public string? Make { get; set; }
    public string? Model { get; set; }
    public double? ResolutionX { get; set; }
    public double? ResolutionY { get; set; }
    public short? ResolutionUnit { get; set; }
    public short? Orientation { get; set; }
    public DateTimeOffset? DdlRecorded { get; set; }
    public double? ExposureTime { get; set; }
    public short? ExposureProgram { get; set; }
    public short? ExposureMode { get; set; }
    public double? FNumber { get; set; }
    public int? IsoSensitivity { get; set; }
    public double? ShutterSpeed { get; set; }
    public short? MeteringMode { get; set; }
    public short? FlashMode { get; set; }
    public double? FocalLength { get; set; }
    public short? SensingMode { get; set; }
    public short? WhiteBalanceMode { get; set; }
    public short? Sharpness { get; set; }
    public double? GpsLongitude { get; set; }
    public double? GpsLatitude { get; set; }
    public short? Contrast { get; set; }
    public short? Saturation { get; set; }

    private static string GetLocalizedText(string identifier)
    {
        return UploadPictureExifInfoModelRes.ResourceManager.GetString(identifier, UploadPictureExifInfoModelRes.CultureInfo) ?? "";
    }

    private string ResolutionUnitText
    {
        get
        {
            string result = "";

            switch (ResolutionUnit)
            {
                case 2:
                    result = GetLocalizedText("ResolutionUnit.R2");
                    break;
                case 3:
                    result = GetLocalizedText("ResolutionUnit.R3");
                    break;
                default:
                    result = "Not defined";
                    break;
            }

            return result;
        }
    }

    public string ResolutionXText => $"{ResolutionX} {ResolutionUnitText}";

    public string ResolutionYText => $"{ResolutionY} {ResolutionUnitText}";

    public string FNumberText => $"F/{FNumber}";

    public static string GetFocalLengthText(double? focalLength)
    {
        return focalLength.HasValue ? $"{focalLength} mm" : "";
    }

    public string FocalLengthText => GetFocalLengthText(FocalLength);

    public string IsoSensitivityText => $"ISO-{IsoSensitivity}";

    public static string GetShutterSpeedText(double? shutterSpeed)
    {
        return shutterSpeed.HasValue ? $"1/{Math.Round(Math.Pow(2, shutterSpeed.Value))}" : "";
    }

    public string ShutterSpeedText => GetShutterSpeedText(ShutterSpeed);

    public static string GetExposureTimeText(double? exposureTime)
    {
        return exposureTime != null ? $"1/{1 / Convert.ToDouble(exposureTime)}" : "";
    }

    public string ExposureTimeText => GetExposureTimeText(ExposureTime);

    public static Dictionary<int, string> GetNumberValues(string propertyName)
    {
        Dictionary<int, string> result = [];

        if (propertyName == nameof(ExposureProgram))
        {
            result.Add(1, GetLocalizedText("ExposureProgram.P1"));
            result.Add(2, GetLocalizedText("ExposureProgram.P2"));
            result.Add(3, GetLocalizedText("ExposureProgram.P3"));
            result.Add(4, GetLocalizedText("ExposureProgram.P4"));
            result.Add(5, GetLocalizedText("ExposureProgram.P5"));
            result.Add(6, GetLocalizedText("ExposureProgram.P6"));
            result.Add(7, GetLocalizedText("ExposureProgram.P7"));
            result.Add(8, GetLocalizedText("ExposureProgram.P8"));
        }

        if (propertyName == nameof(ExposureMode))
        {
            result.Add(0, GetLocalizedText("ExposureMode.M0"));
            result.Add(1, GetLocalizedText("ExposureMode.M1"));
            result.Add(2, GetLocalizedText("ExposureMode.M2"));
        }

        if (propertyName == nameof(Contrast))
        {
            result.Add(0, GetLocalizedText("Contrast.C0"));
            result.Add(1, GetLocalizedText("Contrast.C1"));
            result.Add(2, GetLocalizedText("Contrast.C2"));
        }

        if (propertyName == nameof(Saturation))
        {
            result.Add(0, GetLocalizedText("Saturation.S0"));
            result.Add(1, GetLocalizedText("Saturation.S1"));
            result.Add(2, GetLocalizedText("Saturation.S2"));
        }

        if (propertyName == nameof(MeteringMode))
        {
            result.Add(0, GetLocalizedText("MeteringMode.M0"));
            result.Add(1, GetLocalizedText("MeteringMode.M1"));
            result.Add(2, GetLocalizedText("MeteringMode.M2"));
            result.Add(3, GetLocalizedText("MeteringMode.M3"));
            result.Add(4, GetLocalizedText("MeteringMode.M4"));
            result.Add(5, GetLocalizedText("MeteringMode.M5"));
            result.Add(6, GetLocalizedText("MeteringMode.M6"));
            result.Add(255, GetLocalizedText("MeteringMode.M255"));
        }

        if (propertyName == nameof(FlashMode))
        {
            result.Add(0, GetLocalizedText("FlashMode.F0"));
            result.Add(1, GetLocalizedText("FlashMode.F1"));
            result.Add(5, GetLocalizedText("FlashMode.F5"));
            result.Add(7, GetLocalizedText("FlashMode.F7"));
            result.Add(9, GetLocalizedText("FlashMode.F9"));
            result.Add(13, GetLocalizedText("FlashMode.F13"));
            result.Add(15, GetLocalizedText("FlashMode.F15"));
            result.Add(16, GetLocalizedText("FlashMode.F16"));
            result.Add(24, GetLocalizedText("FlashMode.F24"));
            result.Add(25, GetLocalizedText("FlashMode.F25"));
            result.Add(29, GetLocalizedText("FlashMode.F29"));
            result.Add(31, GetLocalizedText("FlashMode.F31"));
            result.Add(32, GetLocalizedText("FlashMode.F32"));
            result.Add(65, GetLocalizedText("FlashMode.F65"));
            result.Add(69, GetLocalizedText("FlashMode.F69"));
            result.Add(71, GetLocalizedText("FlashMode.F71"));
            result.Add(73, GetLocalizedText("FlashMode.F73"));
            result.Add(77, GetLocalizedText("FlashMode.F77"));
            result.Add(79, GetLocalizedText("FlashMode.F79"));
            result.Add(89, GetLocalizedText("FlashMode.F89"));
            result.Add(93, GetLocalizedText("FlashMode.F93"));
            result.Add(95, GetLocalizedText("FlashMode.F95"));
        }

        if (propertyName == nameof(Sharpness))
        {
            result.Add(0, GetLocalizedText("Sharpness.S0"));
            result.Add(1, GetLocalizedText("Sharpness.S1"));
            result.Add(2, GetLocalizedText("Sharpness.S2"));
        }

        if (propertyName == nameof(WhiteBalanceMode))
        {
            result.Add(0, GetLocalizedText("WhiteBalanceMode.W0"));
            result.Add(1, GetLocalizedText("WhiteBalanceMode.W1"));
        }

        if (propertyName == nameof(Orientation))
        {
            result.Add(1, GetLocalizedText("Orientation.O1"));
            result.Add(2, GetLocalizedText("Orientation.O2"));
            result.Add(3, GetLocalizedText("Orientation.O3"));
            result.Add(4, GetLocalizedText("Orientation.O4"));
            result.Add(5, GetLocalizedText("Orientation.O5"));
            result.Add(6, GetLocalizedText("Orientation.O6"));
            result.Add(7, GetLocalizedText("Orientation.O7"));
            result.Add(8, GetLocalizedText("Orientation.O8"));
        }

        if (propertyName == nameof(SensingMode))
        {
            result.Add(1, GetLocalizedText("SensingMode.S1"));
            result.Add(2, GetLocalizedText("SensingMode.S2"));
            result.Add(3, GetLocalizedText("SensingMode.S3"));
            result.Add(4, GetLocalizedText("SensingMode.S4"));
            result.Add(5, GetLocalizedText("SensingMode.S5"));
            result.Add(7, GetLocalizedText("SensingMode.S7"));
            result.Add(8, GetLocalizedText("SensingMode.S8"));
        }

        if (propertyName == nameof(ResolutionUnit))
        {
            result.Add(2, GetLocalizedText("ResolutionUnit.R2"));
            result.Add(3, GetLocalizedText("ResolutionUnit.R3"));
        }

        return result;
    }

    public string OrientationText
    {
        get
        {
            string result = "";

            switch (Orientation)
            {
                case 1:
                    result = GetLocalizedText("Orientation.O1");
                    break;
                case 2:
                    result = GetLocalizedText("Orientation.O2");
                    break;
                case 3:
                    result = GetLocalizedText("Orientation.O3");
                    break;
                case 4:
                    result = GetLocalizedText("Orientation.O4");
                    break;
                case 5:
                    result = GetLocalizedText("Orientation.O5");
                    break;
                case 6:
                    result = GetLocalizedText("Orientation.O6");
                    break;
                case 7:
                    result = GetLocalizedText("Orientation.O7");
                    break;
                case 8:
                    result = GetLocalizedText("Orientation.O8");
                    break;
                default:
                    result = "Not defined";
                    break;
            }

            return result;
        }
    }

    public string ExposureProgrammText
    {
        get
        {
            string result = "";

            switch (ExposureProgram)
            {
                case 1:
                    result = GetLocalizedText("ExposureProgram.P1");
                    break;
                case 2:
                    result = GetLocalizedText("ExposureProgram.P2");
                    break;
                case 3:
                    result = GetLocalizedText("ExposureProgram.P3");
                    break;
                case 4:
                    result = GetLocalizedText("ExposureProgram.P4");
                    break;
                case 5:
                    result = GetLocalizedText("ExposureProgram.P5");
                    break;
                case 6:
                    result = GetLocalizedText("ExposureProgram.P6");
                    break;
                case 7:
                    result = GetLocalizedText("ExposureProgram.P7");
                    break;
                case 8:
                    result = GetLocalizedText("ExposureProgram.P8");
                    break;
                default:
                    result = "Not defined";
                    break;
            }

            return result;
        }
    }

    public string ExposureModeText
    {
        get
        {
            string result = "";

            switch (ExposureMode)
            {
                case 0:
                    result = GetLocalizedText("ExposureMode.M0");
                    break;
                case 1:
                    result = GetLocalizedText("ExposureMode.M1");
                    break;
                case 2:
                    result = GetLocalizedText("ExposureMode.M2");
                    break;
                default:
                    result = "Not defined";
                    break;
            }

            return result;
        }
    }

    public string MeteringModeText
    {
        get
        {
            string result = "";

            switch (MeteringMode)
            {
                case 0:
                    result = GetLocalizedText("MeteringMode.M0");
                    break;
                case 1:
                    result = GetLocalizedText("MeteringMode.M1");
                    break;
                case 2:
                    result = GetLocalizedText("MeteringMode.M2");
                    break;
                case 3:
                    result = GetLocalizedText("MeteringMode.M3");
                    break;
                case 4:
                    result = GetLocalizedText("MeteringMode.M4");
                    break;
                case 5:
                    result = GetLocalizedText("MeteringMode.M5");
                    break;
                case 6:
                    result = GetLocalizedText("MeteringMode.M6");
                    break;
                case 255:
                    result = GetLocalizedText("MeteringMode.M255");
                    break;
                default:
                    result = "Not defined";
                    break;
            }

            return result;
        }
    }

    public string FlashModeText
    {
        get
        {
            string result = "";

            switch (FlashMode)
            {
                case 0:
                    result = GetLocalizedText("FlashMode.F0");
                    break;
                case 1:
                    result = GetLocalizedText("FlashMode.F1");
                    break;
                case 5:
                    result = GetLocalizedText("FlashMode.F5");
                    break;
                case 7:
                    result = GetLocalizedText("FlashMode.F7");
                    break;
                case 9:
                    result = GetLocalizedText("FlashMode.F9");
                    break;
                case 13:
                    result = GetLocalizedText("FlashMode.F13");
                    break;
                case 15:
                    result = GetLocalizedText("FlashMode.F15");
                    break;
                case 16:
                    result = GetLocalizedText("FlashMode.F16");
                    break;
                case 24:
                    result = GetLocalizedText("FlashMode.F24");
                    break;
                case 25:
                    result = GetLocalizedText("FlashMode.F25");
                    break;
                case 29:
                    result = GetLocalizedText("FlashMode.F29");
                    break;
                case 31:
                    result = GetLocalizedText("FlashMode.F31");
                    break;
                case 32:
                    result = GetLocalizedText("FlashMode.F32");
                    break;
                case 65:
                    result = GetLocalizedText("FlashMode.F65");
                    break;
                case 69:
                    result = GetLocalizedText("FlashMode.F69");
                    break;
                case 71:
                    result = GetLocalizedText("FlashMode.F71");
                    break;
                case 73:
                    result = GetLocalizedText("FlashMode.F73");
                    break;
                case 77:
                    result = GetLocalizedText("FlashMode.F77");
                    break;
                case 79:
                    result = GetLocalizedText("FlashMode.F79");
                    break;
                case 89:
                    result = GetLocalizedText("FlashMode.F89");
                    break;
                case 93:
                    result = GetLocalizedText("FlashMode.F93");
                    break;
                case 95:
                    result = GetLocalizedText("FlashMode.F95");
                    break;
                default:
                    result = "Not defined";
                    break;
            }

            return result;
        }
    }

    public string SensingModeText
    {
        get
        {
            string result = "";

            switch (SensingMode)
            {
                case 1:
                    result = GetLocalizedText("SensingMode.S1");
                    break;
                case 2:
                    result = GetLocalizedText("SensingMode.S2");
                    break;
                case 3:
                    result = GetLocalizedText("SensingMode.S3");
                    break;
                case 4:
                    result = GetLocalizedText("SensingMode.S4");
                    break;
                case 5:
                    result = GetLocalizedText("SensingMode.S5");
                    break;
                case 7:
                    result = GetLocalizedText("SensingMode.S7");
                    break;
                case 8:
                    result = GetLocalizedText("SensingMode.S8");
                    break;
                default:
                    result = "Not defined";
                    break;
            }

            return result;
        }
    }

    public string WhiteBalanceModeText
    {
        get
        {
            string result = "";

            switch (WhiteBalanceMode)
            {
                case 0:
                    result = GetLocalizedText("WhiteBalanceMode.W0");
                    break;
                case 1:
                    result = GetLocalizedText("WhiteBalanceMode.W1");
                    break;
                default:
                    result = "Not defined";
                    break;
            }

            return result;
        }
    }

    public string SharpnessText
    {
        get
        {
            string result = "";

            switch (Sharpness)
            {
                case 0:
                    result = GetLocalizedText("Sharpness.S0");
                    break;
                case 1:
                    result = GetLocalizedText("Sharpness.S1");
                    break;
                case 2:
                    result = GetLocalizedText("Sharpness.S2");
                    break;
                default:
                    result = "Not defined";
                    break;
            }

            return result;
        }
    }

    public string SaturationText
    {
        get
        {
            string result = "";

            switch (Saturation)
            {
                case 0:
                    result = GetLocalizedText("Saturation.S0");
                    break;
                case 1:
                    result = GetLocalizedText("Saturation.S1");
                    break;
                case 2:
                    result = GetLocalizedText("Saturation.S2");
                    break;
                default:
                    result = "Not defined";
                    break;
            }

            return result;
        }
    }

    public string ContrastText
    {
        get
        {
            string result = "";

            switch (Contrast)
            {
                case 0:
                    result = GetLocalizedText("Contrast.C0");
                    break;
                case 1:
                    result = GetLocalizedText("Contrast.C1");
                    break;
                case 2:
                    result = GetLocalizedText("Contrast.C2");
                    break;
                default:
                    result = "Not defined";
                    break;
            }

            return result;
        }
    }
}
