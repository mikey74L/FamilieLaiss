using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.Models.MediaGroup;

public class MediaGroupMediaItemCountInfo : IMediaGroupMediaItemCountInfo
{
    public long MediaGroupId { get; set; }
    public int Count { get; set; }
}
