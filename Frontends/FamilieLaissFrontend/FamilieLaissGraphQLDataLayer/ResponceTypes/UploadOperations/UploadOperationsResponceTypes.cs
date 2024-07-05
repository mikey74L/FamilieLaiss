#nullable disable
namespace FamilieLaissGraphQLDataLayer.ResponceTypes.UploadOperations;

public class GetUploadIDResponce
{
    public long UploadID { get; set; }
}

public class MutationResult
{
    public bool Status { get; set; }
}

public class AddPictureUploadChunkResponce
{
    public MutationResult AddUploadPictureChunk { get; set; }
}

public class FinishPictureUploadResponce
{
    public MutationResult UploadPictureFinish { get; set; }
}

public class AddVideoUploadChunkResponce
{
    public MutationResult AddUploadVideoChunk { get; set; }
}

public class FinishVideoUploadResponce
{
    public MutationResult UploadVideoFinish { get; set; }
}
