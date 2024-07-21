namespace Relay.Contracts
{
  public enum OperationResult
  {
    Success,
    Failed_InternalError,
    Fault_BadRequestParameters
  }

  public class BaseResponse
  {
    public OperationResult OperationResult { get; set; }
    public string Message { get; set; }
    public bool Success { get; set; }
  }
}
