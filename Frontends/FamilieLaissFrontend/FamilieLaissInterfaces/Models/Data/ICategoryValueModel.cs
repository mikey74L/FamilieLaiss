namespace FamilieLaissInterfaces.Models.Data
{
    public interface ICategoryValueModel : IBaseModel<ICategoryValueModel>
    {
        public long Id { get; set; }

        public long? CategoryId { get; set; }

        public ICategoryModel? Category { get; set; }

        public string? NameGerman { get; set; }

        public string? NameEnglish { get; set; }

        public DateTimeOffset? CreateDate { get; set; }

        public DateTimeOffset? ChangeDate { get; set; }

        public string? LocalizedName { get; }
    }
}
