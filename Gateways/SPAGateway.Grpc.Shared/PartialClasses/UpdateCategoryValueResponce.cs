using Google.Protobuf.WellKnownTypes;

namespace SPAGateway.Grpc.Shared;

public partial class UpdateCategoryValueResponce
{
    public DateTimeOffset ChangeDateOffset
    {
        get => ChangeDate.ToDateTimeOffset();
        set => ChangeDate = Timestamp.FromDateTimeOffset(value);
    }
}
