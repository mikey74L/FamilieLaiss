using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.EventAggregator.CategoryValue;

public class AggCategoryValueCreated
{
    public required ICategoryValueModel CategoryValue { get; init; }
}
