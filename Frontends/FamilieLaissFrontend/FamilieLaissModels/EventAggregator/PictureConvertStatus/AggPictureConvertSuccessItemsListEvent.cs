using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.EventAggregator.PictureConvertStatus;

public class AggPictureConvertSuccessItemsListEvent
{
    public required IEnumerable<IPictureConvertStatusModel> Items { get; init; }
}
