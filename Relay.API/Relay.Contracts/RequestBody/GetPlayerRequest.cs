namespace Relay.Contracts
{
  public class GetPlayerRequest
  {
    /// <summary>
    /// Determines project associated to the player
    /// </summary>
    public string ProjectId { get; set; }

    /// <summary>
    /// [Optional] If set, response will contain player private data
    /// </summary>
    public string ApiKey { get; set; }

    /// <summary>
    /// If ApiKey parameter is null, this one is used and only public data is returned
    /// </summary>
    public int Id { get; set; }
  }
}
