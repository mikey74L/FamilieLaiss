using FamilieLaissModels.Models.VideoConvert;

namespace FamilieLaissGraphQLDataLayer.ResponceTypes.VideoConvert;

public class GetVideoConvertWaitingResponce
{
    public List<VideoConvertStatusModel> VideoConvertStatus { get; set; } = new();
}

public class GetVideoConvertExecutingResponce : GetVideoConvertWaitingResponce
{
}

public class GetVideoConvertSuccessResponce : GetVideoConvertWaitingResponce
{
}

public class GetVideoConvertErrorResponce : GetVideoConvertWaitingResponce
{
}
