using Google.Protobuf.WellKnownTypes;

namespace SPAGateway.Grpc.Shared;

public partial class UpdateCategoryResponce
{
    public DateTimeOffset ChangeDateOffset
    {
        get => ChangeDate.ToDateTimeOffset();
        set => ChangeDate = Timestamp.FromDateTimeOffset(value);
    }
}
