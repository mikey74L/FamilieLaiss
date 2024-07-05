namespace Catalog.API.GraphQL.Mutations.MediaItem;

public class UpdateMediaItemInput
{
    public long Id { get; set; }

    public string NameGerman { get; set; }

    public string NameEnglish { get; set; }

    public string DescriptionGerman { get; set; }

    public string DescriptionEnglish { get; set; }

    public bool OnlyFamily { get; set; }

    public List<long> CategoryValues { get; set; }
}
