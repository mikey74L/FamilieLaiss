using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.EventAggregator.VideoConvertStatus;

public class AggVideoConvertErrorItemsListEvent
{
    public required IEnumerable<IVideoConvertStatusModel> Items { get; init; }
}