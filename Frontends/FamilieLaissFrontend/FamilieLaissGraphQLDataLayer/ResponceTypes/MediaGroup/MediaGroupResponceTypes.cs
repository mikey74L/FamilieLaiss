#nullable disable
using FamilieLaissModels.Models.MediaGroup;

namespace FamilieLaissGraphQLDataLayer.ResponceTypes.MediaGroup;

public class GetAllMediaGroupsResponce
{
    public List<MediaGroupModel> MediaGroups { get; set; } = new();
}

public class GetMediaGroupResponce : GetAllMediaGroupsResponce
{
}

public class GetMediaItemCountForMediaGroupsResponce
{
    public List<MediaGroupMediaItemCountInfo> MediaItemCountForMediaGroups { get; set; } = new();
}

public class GetEnglishMediaGroupNameExistsResponce
{
    public bool EnglishMediaGroupNameExists { get; set; }
}

public class GetGermanMediaGroupNameExistsResponce
{
    public bool GermanMediaGroupNameExists { get; set; }
}

public class MutationResult
{
    public MediaGroupModel MediaGroup { get; set; }
}

public class AddMediaGroupResponce
{
    public MutationResult AddMediaGroup { get; set; }
}

public class UpdateMediaGroupResponce
{
    public MutationResult UpdateMediaGroup { get; set; }
}
public class DeleteMediaGroupResponce
{
    public MutationResult DeleteMediaGroup { get; set; }
}
