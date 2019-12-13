using System.IO;

namespace appin2seq.Conversion.Framework
{
  internal interface IAppInsightLogConverterFactory
  {
    IAppInsightLogConverter Create(FileInfo fileInfo, AppInsightsLogSource format);
  }
}
