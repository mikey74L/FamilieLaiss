using FamilieLaissSharedObjects.Enums;

namespace Catalog.API.GraphQL.Mutations.MediaItem;

public class AddMediaItemInput
{
    public long MediaGroupID { get; set; }

    public EnumMediaType MediaType { get; set; }

    public string NameGerman { get; set; }

    public string NameEnglish { get; set; }

    public string DescriptionGerman { get; set; }

    public string DescriptionEnglish { get; set; }

    public bool OnlyFamily { get; set; }

    public long UploadID { get; set; }

    public List<long> CategoryValues { get; set; }
}
