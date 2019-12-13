using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace appin2seq.Conversion.Framework
{
  internal class AppInsightLogConverterFactory : IAppInsightLogConverterFactory
  {
    private readonly IEnumerable<IAppInsightLogConverter> _converters;

    public AppInsightLogConverterFactory(IEnumerable<IAppInsightLogConverter> converters)
    {
      _converters = converters;
    }

    public IAppInsightLogConverter Create(FileInfo fileInfo, AppInsightsLogSource format)
    {
      var converter = _converters.FirstOrDefault(f => f.CanHandle(format));
      if (converter == null)
      {
        return new SimpleConverter();
      }

      return converter;
    }

    private class SimpleConverter : IAppInsightLogConverter
    {
      public void Convert(FileInfo file, string destinationFile)
      {
        Console.WriteLine("Format not supported");
      }

      public bool CanHandle(AppInsightsLogSource format)
      {
        return false;
      }
    }
  }
}
