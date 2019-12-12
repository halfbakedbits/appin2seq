using appinSeq.Conversion.Framework;
using TinyCsvParser.Mapping;

namespace appinSeq.Conversion
{
  internal class CsvAppInsightsLogAnalyticsTracesMapper
    : CsvMapping<TracesEntry>
  {
    public CsvAppInsightsLogAnalyticsTracesMapper()
    {
      MapProperty(0, e => e.Timestamp, new NullableDateTimeOffsetConverter());
      MapProperty(1, e => e.Message);
      MapProperty(2, e => e.SeverityLevel);
      MapProperty(3, e => e.ItemType);
      MapProperty(4, e => e.CustomDimensions);
    }
  }
}
