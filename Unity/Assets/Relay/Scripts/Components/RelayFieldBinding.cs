using System.Reflection;

namespace BitterShark.Relay
{
  public class RelayFieldBinding
  {
    public object Source { get; set; }
    public object BoundTo { get; set; }
    public FieldInfo SourceField { get; set; }
    public FieldInfo BoundField { get; set; }
  }
}
