using System;
using System.IO;
using Autofac;
using CLAP;

namespace appinSeq
{
  internal class ApplicationInsightsSeqImporter
  {
    [Verb(Aliases = "convert", Description = "Convert source file to Seq compact format", IsDefault = true)]
    public void ConvertToSeqCompact(
      [Required] [Description("Source file (Application Insights CSV export format)")] [Aliases("s")]
      string sourceFile, [Required] [Aliases("f")] [Description("Format of AppInsights Log file")]
      AppInsightsLogSource format, [Aliases("d")] [Description("Destination file path")]
      string destinationFile = null)
    {
      if (string.IsNullOrWhiteSpace(sourceFile))
      {
        Console.WriteLine("Supply source file path");
      }

      while (string.IsNullOrWhiteSpace(sourceFile))
      {
        Console.Write("Source file:");
        var filePath = Console.ReadLine();
        if (File.Exists(filePath))
        {
          sourceFile = filePath;
        }
      }

      var container = new Bootstrapper().Init();

      var converter = container.Resolve<IAppInsightLogConverterFactory>()
        .Create(new FileInfo(sourceFile), format);

      converter.ConvertTo(destinationFile);
    }

    [Verb(Aliases = "import", Description = "Import source file to Seq import URL", IsDefault = false)]
    public void ImportToSeq(
      [Required] [Description("Source file (Application Insights CSV export format)")] [Aliases("source")]
      string sourceFile, [Required] [Description("Seq URL")] string seqStore)
    {
    }
  }
}