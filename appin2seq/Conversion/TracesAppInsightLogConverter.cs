using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using appin2seq.Conversion.Framework;
using GenericParsing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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

      return (ExpandoObject) expando;
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
            if (p.Key == nameof(TracesEntry.Timestamp))
            {
              d["@t"] = p.Value;
              continue;
            }

            if (p.Key == nameof(TracesEntry.Message))
            {
              d["@m"] = p.Value;
              continue;
            }

            if (p.Key == nameof(TracesEntry.OperationId) && p.Value != null)
            {
              d["@i"] = p.Value;
              continue;
            }

            if (p.Key == nameof(TracesEntry.SeverityLevel) && p.Value != null)
            {
              d["@l"] = MapLevel(p.Value as string);
            }

            d[p.Key] = p.Value;
          }

          var customProperties = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

          if (tracesEntity?.CustomDimensions != null)
          {
            foreach (var kvp in tracesEntity.CustomDimensions)
            {
              string customKey = kvp.Key;
              if (customKey == "Message Template")
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
            new JsonSerializerSettings
            {
              NullValueHandling = NullValueHandling.Ignore,
              ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
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

    private string MapLevel(string aiLevel)
    {
      throw new NotImplementedException();
    }
  }
}
