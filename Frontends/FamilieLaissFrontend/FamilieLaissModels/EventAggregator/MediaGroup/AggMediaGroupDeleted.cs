using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.EventAggregator.MediaGroup;

internal class AggMediaGroupDeleted
{
    public required IMediaGroupModel MediaGroup { get; init; }
}
