using Google.Protobuf.WellKnownTypes;

namespace Catalog.Grpc.Shared.Category;

public partial class UpdateCategoryResponce
{
    public DateTimeOffset ChangeDateOffset
    {
        get => ChangeDate.ToDateTimeOffset();
        set => ChangeDate = Timestamp.FromDateTimeOffset(value);
    }
}
