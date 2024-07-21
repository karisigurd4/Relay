namespace Relay.API.Controllers
{
  using Microsoft.AspNetCore.Cors;
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Mvc;
  using Relay.Contracts;
  using Relay.Implementation.Interfaces;

  /// <summary>
  /// Entrypoint route for retrieving information about notification messages for players
  /// </summary>
  [EnableCors("CorsPolicy")]
  [Route("api/[controller]")]
  public class NotificationMessageController : ControllerBase
  {
    private readonly INotificationMessageManager notificationMessageManager;

    public NotificationMessageController()
    {
      this.notificationMessageManager = Container.WindsorContainer.Resolve<INotificationMessageManager>();
    }

    /// <summary>
    /// Method that can be polled to show live indication of unread messages the player has received
    /// </summary>
    /// <returns>Number of unread notifications</returns>
    [HttpPost]
    [Route("count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<GetUnreadNotificationMessagesCountResponse> GetUnreadNotificationMessagesCount([FromBody] GetUnreadNotificationMessagesCountRequest request)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var response = notificationMessageManager.GetUnreadNotificationMessagesCount(request);

        Response.StatusCode = response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }

    /// <summary>
    /// Returns notification messages the player has received
    /// </summary>
    /// <returns>A list of notifications</returns>
    [HttpPost]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<GetNotificationMessagesResponse> GetNotificationMessages([FromBody] GetNotificationMessagesRequest request)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var response = notificationMessageManager.GetNotificationMessages(request);

        Response.StatusCode = response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }

    /// <summary>
    /// Sends an answer to a notification message.
    /// </summary>
    /// <returns>Operation execution result details</returns>
    [HttpPost]
    [Route("answer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<AnswerNotificationResponse> AnswerNotificationMessage([FromBody] AnswerNotificationRequest request)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var response = notificationMessageManager.AnswerNotification(request);

        Response.StatusCode = response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }
  }
}

