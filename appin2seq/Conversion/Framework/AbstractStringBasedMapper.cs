using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace appin2seq.Conversion.Framework
{
  internal abstract class AbstractStringBasedMapper<TSource, TTarget> : IMapper<TSource, TTarget>
  {
    public abstract TTarget Map(TSource source);

    protected DateTimeOffset? ToNullableDateTimeOffset(string s)
    {
      if (string.IsNullOrWhiteSpace(s))
      {
        return null;
      }

      return DateTimeOffset.TryParse(s, out var result) ? result : (DateTimeOffset?)null;
    }

    protected int? ToNullableInt(string s)
    {
      if (string.IsNullOrWhiteSpace(s))
      {
        return null;
      }

      return int.TryParse(s, out var result) ? result : (int?)null;
    }

    protected Dictionary<string, object> ToDictionary(string s)
    {
      if (string.IsNullOrWhiteSpace(s))
      {
        return null;
      }

      return JsonConvert.DeserializeObject<Dictionary<string, object>>(s);
    }

    protected Guid? ToNullableGuid(string s)
    {
      if (string.IsNullOrWhiteSpace(s))
      {
        return null;
      }

      return Guid.TryParse(s, out var result) ? result : (Guid?)null;
    }

    protected Guid ToGuid(string s)
    {
      if (string.IsNullOrWhiteSpace(s))
      {
        return Guid.Empty;
      }

      return Guid.TryParse(s, out var result) ? result : Guid.Empty;
    }
  }
}
