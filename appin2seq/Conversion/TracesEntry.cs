using System;
using System.Collections.Generic;

namespace appin2seq.Conversion
{
  internal class TracesEntry
  {
    public string Message { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public int? SeverityLevel { get; set; }
    public string ItemType { get; set; }
    public Dictionary<string, object> CustomDimensions { get; set; }
    public string OperationName { get; set; }
    public Guid? OperationId { get; set; }
    public Guid? OperationParentId { get; set; }
    public Guid? ItemId { get; set; }
    public string CloudRoleName { get; set; }
    public string CloudRoleInstance { get; set; }
    public Guid? AppId { get; set; }
    public string AppName { get; set; }
    public Guid? Ikey { get; set; }
  }
}
