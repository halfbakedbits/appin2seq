using System;
using TinyCsvParser.TypeConverter;

namespace appinSeq.Conversion.Framework
{
  internal class NullableDateTimeOffsetConverter : NullableInnerConverter<DateTimeOffset>
  {
    public NullableDateTimeOffsetConverter(NonNullableConverter<DateTimeOffset> internalConverter) : base(
      internalConverter)
    {
    }

    public NullableDateTimeOffsetConverter() : this(new DateTimeOffsetConverter())
    {
    }
  }
}