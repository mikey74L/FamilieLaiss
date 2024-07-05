using Google.Protobuf.WellKnownTypes;

namespace Catalog.Grpc.Shared.Category;

public partial class CreateCategoryResponce
{
    public DateTimeOffset CreateDateOffset
    {
        get => CreateDate.ToDateTimeOffset();
        set => CreateDate = Timestamp.FromDateTimeOffset(value);
    }
}
