using Google.Protobuf.WellKnownTypes;

namespace SPAGateway.Grpc.Shared;

public partial class CreateCategoryValueResponce
{
    public DateTimeOffset CreateDateOffset
    {
        get => CreateDate.ToDateTimeOffset();
        set => CreateDate = Timestamp.FromDateTimeOffset(value);
    }
}
