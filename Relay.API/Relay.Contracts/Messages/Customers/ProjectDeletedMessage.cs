using System;

namespace Relay.Customers.Contracts
{
  public class ProjectDeletedMessage
  {
    public int ProjectId { get; set; }
    public Guid ProjectApiKey { get; set; }
  }
}
