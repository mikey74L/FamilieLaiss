using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace InfrastructureHelper.Converter;

internal class NullableDateTimeOffsetConverter : ValueConverter<DateTimeOffset?, DateTimeOffset?>
{
    public NullableDateTimeOffsetConverter()
              : base(
                  d => d == null ? null : d.Value.ToUniversalTime(),
                  d => d == null ? null : d.Value.ToUniversalTime())
    {
    }
}
