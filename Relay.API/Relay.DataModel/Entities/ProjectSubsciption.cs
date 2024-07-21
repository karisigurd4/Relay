namespace Relay.DataModel
{
  using System;

  public class ProjectSubsciption
  {
    public Guid ProjectId { get; set; }
    public string SubscriptionId { get; set; }
    public bool Active { get; set; }
    public DateTime LastCheckedStatusDate { get; set; }
    public bool CheckExpirationFlag { get; set; }
  }
}
