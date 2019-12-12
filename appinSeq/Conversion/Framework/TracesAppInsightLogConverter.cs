using System;
using System.IO;
using System.Text;
using TinyCsvParser;
using TinyCsvParser.Tokenizer.RFC4180;

namespace appinSeq.Conversion.Framework
{
  internal class TracesAppInsightLogConverter : IAppInsightLogConverter
  {
    public void Convert(FileInfo file, string destinationFile)
    {
      var options = new Options('"', '\\', ',');
      var tokenizer = new RFC4180Tokenizer(options);
      var csvParserOptions = new CsvParserOptions(true, tokenizer);
      var csvMapper = new CsvAppInsightsLogAnalyticsTracesMapper();
      var csvParser = new CsvParser<TracesEntry>(csvParserOptions, csvMapper);

      foreach (var entry in csvParser.ReadFromFile(file.FullName, Encoding.UTF8))
      {
        if (entry.IsValid)
        {
          Console.WriteLine(entry.RowIndex + " : " + entry.Result?.Timestamp);
        }
        else
        {
          Console.WriteLine(entry.RowIndex + " : " + entry.Error);
        }
      }
    }

    public bool CanHandle(AppInsightsLogSource format)
    {
      return format == AppInsightsLogSource.Traces;
    }
  }
}
