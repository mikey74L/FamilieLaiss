using FamilieLaissInterfaces.Models.Data;

namespace FamilieLaissModels.Models.MediaItem;

public class MediaItemCategoryValueModel : IMediaItemCategoryValueModel
{
    public long Id { get; set; }

    public IMediaItemModel? MediaItem { get; set; }

    public ICategoryValueModel? CategoryValue { get; set; }

    public DateTimeOffset? CreateDate { get; set; }
}
