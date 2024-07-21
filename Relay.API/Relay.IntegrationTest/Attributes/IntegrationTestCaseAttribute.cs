namespace Relay.IntegrationTest.Attributes
{
  using System;

  [AttributeUsage(AttributeTargets.Method)]
  public class IntegrationTestCaseAttribute : Attribute
  {
    public int Order { get; set; }
    public string Description { get; set; }

    public IntegrationTestCaseAttribute(int order, string description)
    {
      this.Order = order;
      this.Description = description;
    }
  }
}
