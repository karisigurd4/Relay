namespace Relay.IntegrationTest.Attributes
{
  using System;

  [AttributeUsage(AttributeTargets.Class)]
  public class IntegrationTestAttribute : Attribute
  {
    public int Order { get; set; }
    public string Description { get; set; }

    public IntegrationTestAttribute(int order, string description)
    {
      this.Order = order;
      this.Description = description;
    }
  }
}
