using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.EventAggregator.PictureConvertStatus;

public class AggPictureConvertProgressEvent
{
    public required IPictureConvertStatusModel Item { get; init; }
}
