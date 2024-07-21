namespace Relay.Contracts.Messages
{
  using System;

  public class ProjectSubscriptionStatusExpiringMessage
  {
    public Guid ProjectApiKey { get; set; }
    public string SubscriptionId { get; set; }
  }
}
