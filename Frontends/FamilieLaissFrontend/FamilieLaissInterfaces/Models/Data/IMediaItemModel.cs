
using FamilieLaissGraphQlClientLibrary;

namespace FamilieLaissInterfaces.Models.Data
{
    public interface IMediaItemModel : IBaseModel<IMediaItemModel>
    {
        public long Id { get; set; }

        public IMediaGroupModel? MediaGroup { get; set; }

        public long? MediaGroupId { get; set; }

        public EnumMediaType? MediaType { get; set; }

        public string? NameGerman { get; set; }

        public string? NameEnglish { get; set; }

        public string? DescriptionGerman { get; set; }

        public string? DescriptionEnglish { get; set; }

        public bool? OnlyFamily { get; set; }

        public IUploadPictureModel? UploadPicture { get; set; }

        public IUploadVideoModel? UploadVideo { get; set; }

        public List<IMediaItemCategoryValueModel>? MediaItemCategoryValues { get; set; }

        public DateTimeOffset? CreateDate { get; set; }

        public DateTimeOffset? ChangeDate { get; set; }

        public string? LocalizedName { get; }

        public string? LocalizedDescription { get; }
    }
}
