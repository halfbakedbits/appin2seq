using System;
using System.Linq;
using System.Text;
using TinyCsvParser;

namespace appinSeq
{
  internal class TracesAppInsightLogConverter : IAppInsightLogConverter
  {
    public void ConvertTo(string destinationFile)
    {
      CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',', 2, false);
      CsvReaderOptions csvReaderOptions = new CsvReaderOptions(new[] {Environment.NewLine});
      var csvMapper = new CsvAppInsightsLogAnalyticsTracesMapper();

      var csvParser = new CsvParser<TracesEntry>(csvParserOptions, csvMapper);

      var result = csvParser
        .ReadFromFile(@"persons.csv", Encoding.ASCII)
        .ToList();
    }

    public bool CanHandle(AppInsightsLogSource format)
    {
      return format == AppInsightsLogSource.Traces;
    }
  }
}
