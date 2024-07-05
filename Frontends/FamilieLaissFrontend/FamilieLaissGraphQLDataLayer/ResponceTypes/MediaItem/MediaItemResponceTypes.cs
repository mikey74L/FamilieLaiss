#nullable disable
using FamilieLaissModels.Models.MediaItem;

namespace FamilieLaissGraphQLDataLayer.ResponceTypes.MediaItem;

public class GetAllItemsForMediaGroupResponce
{
    public List<MediaItemModel> MediaItems { get; set; }
}

public class GetMediaItemResponce : GetAllItemsForMediaGroupResponce
{
}

public class GetVideosForMediaGroupResponce : GetAllItemsForMediaGroupResponce
{
}

public class GetFotosForMediaGroupResponce : GetAllItemsForMediaGroupResponce
{
}

public class GetEnglishMediaItemNameExistsResponce
{
    public bool EnglishMediaItemNameExists { get; set; }
}

public class GetGermanMediaItemNameExistsResponce
{
    public bool GermanMediaItemNameExists { get; set; }
}

public class MutationResult
{
    public MediaItemModel MediaItem { get; set; }
}

public class AddMediaItemResponce
{
    public MutationResult AddMediaItem { get; set; }
}

public class UpdateMediaItemResponce
{
    public MutationResult UpdateMediaItem { get; set; }
}

public class RemoveMediaItemResponce
{
    public MutationResult RemoveMediaItem { get; set; }
}
