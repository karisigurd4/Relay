namespace Relay.DataModel
{
  using System;

  public class NotificationMessage
  {
    public int Id { get; set; }
    public int ToPlayerId { get; set; }
    public int ReferenceId { get; set; }
    public NotificationMessageType Type { get; set; }
    public DateTime SentDateTime { get; set; }
    public string Data { get; set; }
    public bool ViewedFlag { get; set; }
  }
}
