using Google.Protobuf.WellKnownTypes;

namespace Catalog.Grpc.Shared.CategoryValue;

public partial class UpdateCategoryValueResponce
{
    public DateTimeOffset ChangeDateOffset
    {
        get => ChangeDate.ToDateTimeOffset();
        set => ChangeDate = Timestamp.FromDateTimeOffset(value);
    }
}
