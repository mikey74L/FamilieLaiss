using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FLBackEnd.Infrastructure.Converter;

internal class DateTimeOffsetConverter : ValueConverter<DateTimeOffset, DateTimeOffset>
{
    public DateTimeOffsetConverter()
        : base(
            d => d.ToUniversalTime(),
            d => d.ToUniversalTime())
    {
    }
}