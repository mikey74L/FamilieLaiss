using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Attributes;
using FamilieLaissInterfaces.Enums;

namespace FamilieLaissInterfaces.Models.Data
{
    [GraphQlSortInputType(typeof(UploadPictureSortInput))]
    [GraphQlFilterInputType(typeof(UploadPictureFilterInput))]
    [GraphQlFilterGroup("UploadView", "PictureSize", 1)]
    [GraphQlFilterGroup("UploadView", "DateValues", 2)]
    public interface IUploadPictureModel : IBaseModel<IUploadPictureModel>
    {
        public long Id { get; set; }

        [GraphQlSort("UploadView", 1, false, GraphQlSortDirection.Ascending)]
        [GraphQlSort("UploadView", 2, false, GraphQlSortDirection.Descending)]
        public string? Filename { get; set; }

        [GraphQlFilter("UploadView", "PictureSize", 1, GraphQlFilterType.NumberRange)]
        public int? Height { get; set; }

        [GraphQlFilter("UploadView", "PictureSize", 2, GraphQlFilterType.NumberRange)]
        public int? Width { get; set; }

        public EnumUploadStatus? Status { get; set; }

        [GraphQlSort("UploadView", 3, true, GraphQlSortDirection.Ascending)]
        [GraphQlSort("UploadView", 4, false, GraphQlSortDirection.Descending)]
        [GraphQlFilter("UploadView", "DateValues", 1, GraphQlFilterType.DateRange)]
        public DateTimeOffset? CreateDate { get; set; }

        public IUploadPictureExifInfoModel? UploadPictureExifInfo { get; set; }

        public IGoogleGeoCodingAddressModel? GoogleGeoCodingAddress { get; set; }

        public string? PictureSize { get; }

        public bool IsSelected { get; set; }
    }
}
