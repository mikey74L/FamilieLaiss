#nullable disable
using FamilieLaissModels.Models.Category;
using FamilieLaissModels.Models.CategoryValue;

namespace FamilieLaissGraphQLDataLayer.ResponceTypes.CategoryValue;

public class GetCategoryValuesForCategoryResponce
{
    public List<CategoryModel> Categories { get; set; }
    public List<CategoryValueModel> CategoryValues { get; set; }
}

public class GetCategoryValueResponce
{
    public List<CategoryValueModel> CategoryValues { get; set; }
}

public class GetEnglishCategoryValueNameExistsResponce
{
    public bool EnglishCategoryValueNameExists { get; set; }
}

public class GetGermanCategoryValueNameExistsResponce
{
    public bool GermanCategoryValueNameExists { get; set; }
}

public class MutationResult
{
    public CategoryValueModel CategoryValue { get; set; }
}

public class AddCategoryValueResponce
{
    public MutationResult AddCategoryValue { get; set; }
}

public class UpdateCategoryValueResponce
{
    public MutationResult UpdateCategoryValue { get; set; }
}

public class DeleteCategoryValueResponce
{
    public MutationResult DeleteCategoryValue { get; set; }
}