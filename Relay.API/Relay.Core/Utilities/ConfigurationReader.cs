using Newtonsoft.Json;
using Relay.Core.Contracts;
using System;
using System.IO;
using System.Linq;

namespace Relay.Core.Utilities
{
  public static class ConfigurationReader
  {
    private static EnvironmentConfig environmentConfiguration = null;

    public static EnvironmentConfig GetEnvironmentConfiguration()
    {
      if (environmentConfiguration == null)
      {
        var apiConfigText = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory, "Configuration\\ApiConfig.json"));
        var targetEnvironment = JsonConvert.DeserializeObject<ApiConfig>(apiConfigText).Target;

        var environmentconfigText = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory, "Configuration\\Environmentconfig.json"));
        environmentConfiguration = JsonConvert.DeserializeObject<EnvironmentConfig[]>(environmentconfigText).FirstOrDefault(x => x.Environment == targetEnvironment);
      }

      return environmentConfiguration;
    }
  }
}
