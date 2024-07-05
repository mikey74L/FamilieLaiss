#nullable disable
using FamilieLaissModels.Models.UploadPicture;

namespace FamilieLaissGraphQLDataLayer.ResponceTypes.UploadPicture;

public class GetAllUploadPicturesResponce
{
    public List<UploadPictureModel> UploadPictures { get; set; } = new();
}

public class GetUploadPicturesCountResponce
{
    public int UploadPictureCount { get; set; }
}

public class MutationResult
{
    public UploadPictureModel UploadPicture { get; set; }
}

public class MutationResultCount
{
    public int Count { get; set; }
}

public class DeleteUploadPictureResponce
{
    public MutationResult DeleteUploadPicture { get; set; }
}

public class DeleteAllUploadPicturesResponce
{
    public MutationResultCount DeleteAllUploadPictures { get; set; }
}
