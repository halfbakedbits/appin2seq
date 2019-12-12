using GenericParsing;

namespace appinSeq.Conversion.Framework
{
  internal class GenericParserTracesEntityMapper : AbstractStringBasedMapper<GenericParser, TracesEntry>
  {
    public override TracesEntry Map(GenericParser source)
    {
      var tracesEntity = new TracesEntry
      {
        Timestamp = ToNullableDateTimeOffset(source["timestamp"]),
        Message = source["message"],
        SeverityLevel = ToNullableInt(source["severityLevel"]),
        ItemType = source["itemType"],
        CustomDimensions = ToDictionary(source["customDimensions"]),
        OperationName = source["operation_Name"],
        OperationId = ToNullableGuid(source["operation_Id"]),
        OperationParentId = ToNullableGuid(source["operation_ParentId"]),
        ItemId = ToNullableGuid(source["itemId"]),
        CloudRoleName = source["cloud_RoleName"],
        CloudRoleInstance = source["cloud_RoleInstance"],
        AppName = source["appName"],
        AppId = ToNullableGuid(source["appId"]),
        Ikey = ToNullableGuid(source["iKey"])
      };

      return tracesEntity;
    }
  }
}
