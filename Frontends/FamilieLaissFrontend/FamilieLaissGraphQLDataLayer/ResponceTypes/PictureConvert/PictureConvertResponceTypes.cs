using FamilieLaissModels.Models.PictureConvert;

namespace FamilieLaissGraphQLDataLayer.ResponceTypes.PictureConvert;

public class GetPictureConvertWaitingResponce
{
    public List<PictureConvertStatusModel> PictureConvertStatus { get; set; } = new();
}

public class GetPictureConvertExecutingResponce : GetPictureConvertWaitingResponce
{
}

public class GetPictureConvertSuccessResponce : GetPictureConvertWaitingResponce
{
}

public class GetPictureConvertErrorResponce : GetPictureConvertWaitingResponce
{
}
