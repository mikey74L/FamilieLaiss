using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.EventAggregator.MediaItem;

public class AggMediaItemCreated
{
    public required IMediaItemModel MediaItem { get; init; }
}
