#nullable disable
using FamilieLaissModels.Models.UploadVideo;

namespace FamilieLaissGraphQLDataLayer.ResponceTypes.UploadVideo;

public class GetAllUploadVideosResponce
{
    public List<UploadVideoModel> UploadVideos { get; set; } = new();
}

public class GetUploadVideosCountResponce
{
    public int UploadVideoCount { get; set; }
}

public class MutationResult
{
    public UploadVideoModel UploadVideo { get; set; }
}

public class MutationResultCount
{
    public int Count { get; set; }
}

public class DeleteUploadVideoResponce
{
    public MutationResult DeleteUploadVideo { get; set; }
}

public class DeleteAllUploadVideosResponce
{
    public MutationResultCount DeleteAllUploadVideos { get; set; }
}
