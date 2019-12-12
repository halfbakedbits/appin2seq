using System;
using TinyCsvParser.TypeConverter;

namespace appinSeq.Conversion
{
  internal class DateTimeOffsetConverter : NonNullableConverter<DateTimeOffset>
  {
    protected override bool InternalConvert(string value, out DateTimeOffset result)
    {
      return DateTimeOffset.TryParse(value, out result);
    }
  }
}