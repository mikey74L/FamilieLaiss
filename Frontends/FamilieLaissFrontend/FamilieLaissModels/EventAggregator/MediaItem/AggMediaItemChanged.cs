using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.EventAggregator.MediaItem;

public class AggMediaItemChanged
{
    public required IMediaItemModel MediaItem { get; init; }
}
