using System.IO;

namespace appinSeq.Conversion
{
  internal interface IAppInsightLogConverter
  {
    void Convert(FileInfo sourceFile, string destinationFile);
    bool CanHandle(AppInsightsLogSource format);
  }
}
