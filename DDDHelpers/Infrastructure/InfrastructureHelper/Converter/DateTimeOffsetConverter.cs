using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace InfrastructureHelper.Converter;

internal class DateTimeOffsetConverter : ValueConverter<DateTimeOffset, DateTimeOffset>
{
    public DateTimeOffsetConverter()
        : base(
            d => d.ToUniversalTime(),
            d => d.ToUniversalTime())
    {
    }
}
