using System.IO;

namespace appin2seq.Conversion.Framework
{
  internal interface IAppInsightLogConverter
  {
    void Convert(FileInfo sourceFile, string destinationFile);
    bool CanHandle(AppInsightsLogSource format);
  }
}
