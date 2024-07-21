using System;

namespace Relay.Customers.Contracts
{
  public class ProjectCreatedMessage
  {
    public string ExtAuthId { get; set; }
    public int ProjectId { get; set; }
    public string SubscriptionId { get; set; }
    public Guid ProjectApiKey { get; set; }
    public ProjectSettings ProjectSettings { get; set; }
  }
}
