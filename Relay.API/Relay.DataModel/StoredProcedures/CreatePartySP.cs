namespace Relay.DataModel
{
  public class CreatePartySPResponse
  {
    public bool Success { get; set; }
  }

  public class CreatePartySPRequest
  {
    public int PartyLeaderPlayerId { get; set; }
  }

  public static class CreatePartySP
  {
    public static string Name => "[Relay].[CreateParty]";

    public static object CreateParameters(int partyLeaderPlayerId) => new
    {
      partyLeaderPlayerId
    };
  }
}
