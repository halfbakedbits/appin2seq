using CLAP;

namespace appin2seq
{
  class Program
  {
    static void Main(string[] args)
    {
      Parser.RunConsole<ApplicationInsightsSeqImporter>(args);
    }
  }
}
