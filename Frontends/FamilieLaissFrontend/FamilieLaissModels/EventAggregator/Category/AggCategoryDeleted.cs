using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.EventAggregator.Category;

public class AggCategoryValueDeleted
{
    public required ICategoryModel Category { get; init; }
}
