using System;

namespace Relay.Customers.Contracts
{
  public class ProjectSettingsUpdatedMessage
  {
    public string ExtAuthId { get; set; }
    public int ProjectId { get; set; }
    public Guid ProjectApiKey { get; set; }
    public ProjectSettings ProjectSettings { get; set; }
  }
}
