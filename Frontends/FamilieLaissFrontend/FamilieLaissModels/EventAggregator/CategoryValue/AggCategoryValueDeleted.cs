using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.EventAggregator.CategoryValue;

public class AggCategoryValueDeleted
{
    public required ICategoryValueModel CategoryValue { get; init; }
}
