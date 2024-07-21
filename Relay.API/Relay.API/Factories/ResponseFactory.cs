namespace Relay.API
{
  using Relay.Contracts;
  using System;

  public static class ResponseFactory
  {
    public static BaseResponse CreateInternalErrorResponse(Exception e) =>
#if DEBUG
      throw e;
#else
      new BaseResponse()
      {
        OperationResult = OperationResult.Failed_InternalError,
        Message = "An internal error occurred when processing your request",
        Success = false
      };
#endif
  }
}
