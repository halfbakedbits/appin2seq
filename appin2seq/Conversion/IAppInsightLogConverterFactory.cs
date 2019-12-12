using System.IO;

namespace appinSeq.Conversion
{
  internal interface IAppInsightLogConverterFactory
  {
    IAppInsightLogConverter Create(FileInfo fileInfo, AppInsightsLogSource format);
  }
}
