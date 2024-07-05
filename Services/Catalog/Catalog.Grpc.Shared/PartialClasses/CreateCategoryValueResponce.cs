using Google.Protobuf.WellKnownTypes;

namespace Catalog.Grpc.Shared.CategoryValue;

public partial class CreateCategoryValueResponce
{
    public DateTimeOffset CreateDateOffset
    {
        get => CreateDate.ToDateTimeOffset();
        set => CreateDate = Timestamp.FromDateTimeOffset(value);
    }
}
