using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.EventAggregator.VideoConvertStatus;

public class AggVideoConvertWaitingItemsListEvent
{
    public required IEnumerable<IVideoConvertStatusModel> Items { get; init; }
}