namespace Relay.DataModel
{
  using System;

  public class RegisterPlayerSPResponse
  {
    public bool Success { get; set; }
    public Guid ApiKey { get; set; }
    public int Id { get; set; }
  }

  public class RegisterPlayerSPRequest
  {
    public string ProjectId { get; set; }
    public string Name { get; set; }
  }

  public static class RegisterPlayerSP
  {
    public static string Name => "[Relay].[RegisterPlayer]";

    public static object CreateParameters(string projectId, string name) => new
    {
      ProjectId = projectId,
      name
    };
  }
}
