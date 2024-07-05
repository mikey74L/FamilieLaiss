namespace Catalog.DTO.CategoryValue;

public class CheckCategoryValueNameExistsDTO
{
    /// <summary>
    /// The unique identifier for an existing category value or -1 if new
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// The unique identifier for the category the value belongs to
    /// </summary>
    public long CategoryId { get; set; }

    /// <summary>
    /// The name to check
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
