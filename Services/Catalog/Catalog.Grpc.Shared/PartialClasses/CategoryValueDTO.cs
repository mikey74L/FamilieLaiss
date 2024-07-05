﻿using Google.Protobuf.WellKnownTypes;

namespace Catalog.Grpc.Shared.CategoryValue;

public partial class CategoryValueDTO
{
    public DateTimeOffset CreateDateOffset
    {
        get => CreateDate.ToDateTimeOffset();
        set => CreateDate = Timestamp.FromDateTimeOffset(value);
    }

    public DateTimeOffset ChangeDateOffset
    {
        get => ChangeDate.ToDateTimeOffset();
        set => ChangeDate = Timestamp.FromDateTimeOffset(value);
    }
}
