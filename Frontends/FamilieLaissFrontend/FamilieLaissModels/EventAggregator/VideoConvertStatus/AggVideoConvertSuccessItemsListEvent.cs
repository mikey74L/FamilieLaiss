using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.EventAggregator.VideoConvertStatus;

public class AggVideoConvertSuccessItemsListEvent
{
    public required IEnumerable<IVideoConvertStatusModel> Items { get; init; }
}