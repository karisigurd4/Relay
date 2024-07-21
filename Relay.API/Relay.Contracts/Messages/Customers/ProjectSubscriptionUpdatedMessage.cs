using System;

namespace Relay.Customers.Contracts
{
  public class ProjectSubscriptionUpdatedMessage
  {
    public int SubscriptionServiceCatalogId { get; set; }
    public Guid ProjectApiKey { get; set; }
    public string SubscriptionId { get; set; }
    public bool Active { get; set; }
  }
}
