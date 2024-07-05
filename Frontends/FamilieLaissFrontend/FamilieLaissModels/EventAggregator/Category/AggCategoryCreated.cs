using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.EventAggregator.Category;

public class AggCategoryCreated
{
    public required ICategoryModel Category { get; init; }
}
