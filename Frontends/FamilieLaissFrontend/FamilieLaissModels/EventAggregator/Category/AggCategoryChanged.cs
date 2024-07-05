using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.EventAggregator.Category;

public class AggCategoryChanged
{
    public required ICategoryModel Category { get; init; }
}
