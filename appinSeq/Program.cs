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
}
