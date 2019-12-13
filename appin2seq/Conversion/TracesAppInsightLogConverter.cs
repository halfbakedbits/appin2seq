using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using appin2seq.Conversion.Framework;
using GenericParsing;
using Newtonsoft.Json;

namespace appin2seq.Conversion
{
  internal static class ObjectExtensions
  {
    public static ExpandoObject ToExpandoObject(this object obj)
    {
      // Null-check

      IDictionary<string, object> expando = new ExpandoObject();

      foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj.GetType()))
      {
        expando[property.Name] = property.GetValue(obj);
      }

      return (ExpandoObject)expando;
    }
  }

  internal class TracesAppInsightLogConverter : IAppInsightLogConverter
  {
    private readonly IMapper<GenericParser, TracesEntry> EntityMapper;

    public TracesAppInsightLogConverter(IMapper<GenericParser, TracesEntry> entityMapper)
    {
      EntityMapper = entityMapper;
    }

    public void Convert(FileInfo file, string destinationFile)
    {
      var items = new List<string>();

      using (var parser = new GenericParser(file.FullName)
      {
        FirstRowHasHeader = true, SkipEmptyRows = true, TrimResults = true
      })
      {
        while (parser.Read())
        {
          var tracesEntity = EntityMapper.Map(parser);

          var expando = new ExpandoObject();
          var d = (IDictionary<string, object>)expando;
          foreach (var p in tracesEntity.ToExpandoObject())
          {
            switch (p.Key)
            {
              case nameof(TracesEntry.Timestamp):
                d["@t"] = p.Value;

                continue;
              case nameof(TracesEntry.Message):
                d["@m"] = p.Value;

                continue;
              case nameof(TracesEntry.OperationId):
                d["@i"] = p.Value ?? Guid.NewGuid();

                continue;
              case nameof(TracesEntry.SeverityLevel):
              {
                if (p.Value != null)
                {
                  d["@l"] = MapLevel(p.Value.ToString()).ToString();
                }

                continue;
              }
              case nameof(TracesEntry.CustomDimensions):
                continue;
            }

            if (string.IsNullOrWhiteSpace(p.Value?.ToString()))
            {
              continue;
            }

            d[p.Key] = p.Value;
          }

          var customProperties = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

          if (tracesEntity?.CustomDimensions != null)
          {
            foreach (var kvp in tracesEntity.CustomDimensions)
            {
              string customKey = kvp.Key;
              if (customKey == "MessageTemplate")
              {
                customKey = "@mt";
              }

              if (customKey == "Exception")
              {
                customKey = "@x";
              }

              if (customKey == "Renderings")
              {
                customKey = "@r";
              }

              if (!customProperties.Contains(kvp.Key))
              {
                // need to track unknown custom property key
                if (d.Keys.Contains(kvp.Key))
                {
                  // need to avoid conflict
                  var customDimensionKeyConflictOption = $"aiSeq:{kvp.Key}";
                  if (!d.Keys.Contains(customDimensionKeyConflictOption))
                  {
                    d.Keys.Add(customDimensionKeyConflictOption);
                    customKey = customDimensionKeyConflictOption;
                  }
                }
                else
                {
                  customProperties.Add(kvp.Key);
                  customKey = kvp.Key;
                }
              }

              d[customKey] = kvp.Value;
            }
          }

          var serialized = JsonConvert.SerializeObject(d, Formatting.None,
            new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
          items.Add(serialized);
        }
      }

      if (destinationFile == null)
      {
        destinationFile = file.FullName + ".clef";
      }

      using (var sw = File.CreateText(destinationFile))
      {
        foreach (var i in items)
        {
          sw.WriteLine(i);
        }

        sw.Flush();
      }
    }

    public bool CanHandle(AppInsightsLogSource format)
    {
      return format == AppInsightsLogSource.Traces;
    }

    private SeqLevel MapLevel(string aiLevel)
    {
      if (int.TryParse(aiLevel, out var i))
      {
        switch (i)
        {
          case 0:
            return SeqLevel.Verbose;
          case 1:
            return SeqLevel.Debug;
          case 2:
            return SeqLevel.Information;
          case 3:
            return SeqLevel.Warning;
          case 4:
            return SeqLevel.Error;
          case 5:
            return SeqLevel.Critical;
          default:
            return SeqLevel.Unknown;
        }
      }

      if (string.IsNullOrWhiteSpace(aiLevel))
      {
        return SeqLevel.Unknown;
      }

      return Enum.TryParse(typeof(SeqLevel), aiLevel, true, out var e) ? (SeqLevel)e : SeqLevel.Unknown;
    }
  }

  internal enum SeqLevel
  {
    Unknown = -1,
    Verbose = 0,
    Debug = 1,
    Information = 2,
    Warning = 3,
    Error = 4,
    Critical = 5
  }
}
