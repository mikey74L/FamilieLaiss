#nullable disable
using FamilieLaissModels.Models.Category;

namespace FamilieLaissGraphQLDataLayer.ResponceTypes.Category;

public class GetAllCategoriesResponce
{
    public List<CategoryModel> Categories { get; set; } = new();
}

public class GetPhotoCategoriesResponce : GetAllCategoriesResponce
{
}

public class GetVideoCategoriesResponce : GetAllCategoriesResponce
{
}

public class GetCategoryResponce : GetAllCategoriesResponce
{
}

public class GetEnglishCategoryNameExistsResponce
{
    public bool EnglishCategoryNameExists { get; set; }
}

public class GetGermanCategoryNameExistsResponce
{
    public bool GermanCategoryNameExists { get; set; }
}

public class MutationResult
{
    public CategoryModel Category { get; set; }
}

public class AddCategoryResponce
{
    public MutationResult AddCategory { get; set; }
}

public class UpdateCategoryResponce
{
    public MutationResult UpdateCategory { get; set; }
}

public class DeleteCategoryResponce
{
    public MutationResult DeleteCategory { get; set; }
}