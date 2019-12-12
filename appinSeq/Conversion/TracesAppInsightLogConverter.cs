using System.IO;
using System.Text;
using TinyCsvParser;

namespace appinSeq.Conversion
{
  internal class TracesAppInsightLogConverter : IAppInsightLogConverter
  {
    public void Convert(FileInfo file, string destinationFile)
    {
      var csvParserOptions = new CsvParserOptions(true, ',', 2, false);
      var csvMapper = new CsvAppInsightsLogAnalyticsTracesMapper();

      var csvParser = new CsvParser<TracesEntry>(csvParserOptions, csvMapper);

      foreach (var entry in csvParser.ReadFromFile(file.FullName, Encoding.UTF8))
      {
      }
    }

    public bool CanHandle(AppInsightsLogSource format)
    {
      return format == AppInsightsLogSource.Traces;
    }
  }
}
