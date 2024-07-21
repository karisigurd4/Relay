namespace Relay.GameServer.Core.Utilities
{
  using Newtonsoft.Json;
  using Relay.GameServer.Core.Contracts;
  using System;
  using System.IO;

  public static class ConfigurationReader
  {
    private static GameServerConfiguration gameServerConfiguration = null;

    public static GameServerConfiguration GetGameServerConfiguration()
    {
      if (gameServerConfiguration == null)
      {
        var environmentconfigText = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory, "Configuration\\GameServerConfiguration.json"));
        gameServerConfiguration = JsonConvert.DeserializeObject<GameServerConfiguration>(environmentconfigText);
      }

      return gameServerConfiguration;
    }
  }
}
