using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.EventAggregator.CategoryValue;

public class AggCategoryValueChanged
{
    public required ICategoryValueModel CategoryValue { get; init; }
}
