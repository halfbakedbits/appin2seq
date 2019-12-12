using System;

namespace appinSeq.Conversion
{
  internal class TracesEntry
  {
    public string Message { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public int? SeverityLevel { get; set; }
    public string ItemType { get; set; }
    public string CustomDimensions { get; set; }
  }
}
