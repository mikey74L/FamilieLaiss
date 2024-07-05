namespace Catalog.API.GraphQL.Mutations.MediaGroup
{
    public class AddMediaGroupInput
    {
        public string NameGerman { get; set; }

        public string NameEnglish { get; set; }

        public string DescriptionGerman { get; set; }

        public string DescriptionEnglish { get; set; }

        public DateTimeOffset EventDate { get; set; }
    }
}
