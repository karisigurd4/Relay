namespace Relay.Contracts
{
  public class BrowseGameServersRequest
  {
    public string ProjectId { get; set; }
    public bool HideFull { get; set; }
    public bool HidePrivate { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string OrderBy { get; set; }
    public string OrderDirection { get; set; }
  }
}
