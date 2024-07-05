namespace FamilieLaissInterfaces.Models.Data
{
    public interface IMediaGroupModel : IBaseModel<IMediaGroupModel>
    {
        public long Id { get; set; }
        public string? NameGerman { get; set; }
        public string? NameEnglish { get; set; }
        public string? DescriptionGerman { get; set; }
        public string? DescriptionEnglish { get; set; }
        public DateTimeOffset? EventDate { get; set; }

        public DateTime? EventDateForInput { get; set; }

        public DateTimeOffset? CreateDate { get; set; }
        public DateTimeOffset? ChangeDate { get; set; }

        public List<IMediaItemModel> MediaItems { get; set; }

        public string? LocalizedName { get; }

        public string? LocalizedDescription { get; }
    }
}
