using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.EventAggregator.VideoConvertStatus;

public class AggVideoConvertProgressEvent
{
    public required IVideoConvertStatusModel Item { get; init; }
}