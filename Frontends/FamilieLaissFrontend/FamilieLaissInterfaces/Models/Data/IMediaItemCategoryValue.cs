namespace FamilieLaissInterfaces.Models.Data;

public interface IMediaItemCategoryValueModel
{
    public long Id { get; set; }

    public IMediaItemModel? MediaItem { get; set; }

    public ICategoryValueModel? CategoryValue { get; set; }

    public DateTimeOffset? CreateDate { get; set; }
}
