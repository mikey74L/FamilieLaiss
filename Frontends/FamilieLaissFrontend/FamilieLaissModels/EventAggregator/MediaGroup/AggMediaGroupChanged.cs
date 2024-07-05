using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.EventAggregator.MediaGroup;

public class AggMediaGroupChanged
{
    public required IMediaGroupModel MediaGroup { get; init; }
}
