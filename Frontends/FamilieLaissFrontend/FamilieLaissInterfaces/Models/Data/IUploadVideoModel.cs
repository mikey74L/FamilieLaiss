
using FamilieLaissGraphQlClientLibrary;
using FamilieLaissInterfaces.Attributes;
using FamilieLaissInterfaces.Enums;

namespace FamilieLaissInterfaces.Models.Data;

[GraphQlSortInputType(typeof(UploadVideoSortInput))]
[GraphQlFilterInputType(typeof(UploadVideoFilterInput))]
[GraphQlFilterGroup("UploadView", "VideoSize", 1)]
public interface IUploadVideoModel : IBaseModel<IUploadVideoModel>
{
    public long Id { get; set; }
    [GraphQlSort("UploadView", 1, false, GraphQlSortDirection.Ascending)]
    [GraphQlSort("UploadView", 2, false, GraphQlSortDirection.Descending)]
    public string? Filename { get; set; }

    [GraphQlFilter("UploadView", "VideoSize", 1, GraphQlFilterType.NumberRange)]
    public int? Height { get; set; }

    [GraphQlFilter("UploadView", "VideoSize", 2, GraphQlFilterType.NumberRange)]
    public int? Width { get; set; }

    public int? DurationHour { get; set; }
    public int? DurationMinute { get; set; }
    public int? DurationSecond { get; set; }
    public EnumUploadStatus? Status { get; set; }
    public EnumVideoType? VideoType { get; set; }

    [GraphQlSort("UploadView", 3, true, GraphQlSortDirection.Ascending)]
    [GraphQlSort("UploadView", 4, false, GraphQlSortDirection.Descending)]
    public DateTimeOffset? CreateDate { get; set; }

    public string? VideoSize { get; }

    public string? VideoLength { get; }

    public bool IsSelected { get; set; }
}