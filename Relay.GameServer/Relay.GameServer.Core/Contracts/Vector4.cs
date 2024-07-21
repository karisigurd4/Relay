namespace Relay.GameServer.Core.Contracts
{
  public class Vector4
  {
    public Vector4(float x, float y, float z, float w)
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
