using System;
using System.IO;
using GenericParsing;

namespace appinSeq.Conversion.Framework
{
  internal class TracesAppInsightLogConverter : IAppInsightLogConverter
  {
    private readonly IMapper<GenericParser, TracesEntry> _mapper;

    public TracesAppInsightLogConverter(IMapper<GenericParser, TracesEntry> mapper)
    {
      _mapper = mapper;
    }

    public void Convert(FileInfo file, string destinationFile)
    {
      using (var parser = new GenericParser(file.FullName)
      {
        FirstRowHasHeader = true, SkipEmptyRows = true, TrimResults = true
      })
      {
        while (parser.Read())
        {
          var tracesEntity = _mapper.Map(parser);

          Console.WriteLine($"{tracesEntity.Timestamp}:{tracesEntity.Message}");
        }
      }
    }

    public bool CanHandle(AppInsightsLogSource format)
    {
      return format == AppInsightsLogSource.Traces;
    }
  }
}
