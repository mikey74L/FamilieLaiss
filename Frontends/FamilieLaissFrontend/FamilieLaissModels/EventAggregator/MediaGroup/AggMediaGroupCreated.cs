using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.EventAggregator.MediaGroup;

public class AggMediaGroupCreated
{
    public required IMediaGroupModel MediaGroup { get; init; }
}
