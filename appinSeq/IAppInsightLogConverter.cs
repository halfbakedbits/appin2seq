namespace appinSeq
{
  internal interface IAppInsightLogConverter
  {
    void ConvertTo(string destinationFile);
    bool CanHandle(AppInsightsLogSource format);
  }
}
