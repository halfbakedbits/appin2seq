using System;
using System.IO;
using CLAP;

namespace appinSeq
{
  class Program
  {
    static void Main(string[] args)
    {
      Parser.RunConsole<ApplicationInsightsSeqImporter>(args);
    }
  }

  internal class ApplicationInsightsSeqImporter
  {
    [Verb(Aliases = "convert", Description = "Convert source file to Seq compact format", IsDefault = true)]
    public void ConvertToSeqCompact(
      [Description("Source file (Application Insights CSV export format)")] [Aliases("source")]
      string sourceFile, string destinationFile = null)
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


    }

    [Verb(Aliases = "import", Description = "Import source file to Seq import URL", IsDefault = false)]
    public void ImportToSeq(
      [Required] [Description("Source file (Application Insights CSV export format)")] [Aliases("source")]
      string sourceFile, [Required] [Description("Seq URL")] string seqStore)
    {
    }
  }
}
