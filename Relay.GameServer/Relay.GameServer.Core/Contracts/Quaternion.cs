namespace Relay.GameServer.Core.Contracts
{
  public class Quaternion
  {
    public Quaternion(float x, float y, float z, float w)
    {
      this.x = x;
      this.y = y;
      this.z = z;
      this.w = w;
    }

    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
    public float w { get; set; }
  }
}
