using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.EventAggregator.PictureConvertStatus;

public class AggPictureConvertErrorItemsListEvent
{
    public required IEnumerable<IPictureConvertStatusModel> Items { get; init; }
}
