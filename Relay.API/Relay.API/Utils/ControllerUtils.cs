namespace Relay.API
{
  using Relay.Contracts;
  using System;

  public static class ControllerUtils
  {
    public static T SafeExecution<T>(Func<T> func)
      where T : BaseResponse
    {
      try
      {
        return func();
      }
      catch (Exception e)
      {
        try
        {
          return (T)ResponseFactory.CreateInternalErrorResponse(e);
        }
        catch (Exception)
        {
          throw;
        }
      }
    }
  }
}
