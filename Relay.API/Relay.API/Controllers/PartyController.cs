using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Relay.Contracts;
using Relay.Implementation.Interfaces;

namespace Relay.API.Controllers
{
  /// <summary>
  /// Entrypoint route for managing player parties
  /// </summary>
  [EnableCors("CorsPolicy")]
  [Route("api/[controller]")]
  public class PartyController : ControllerBase
  {
    private IPartyManager partyManager;

    public PartyController()
    {
      this.partyManager = Container.WindsorContainer.Resolve<IPartyManager>();
    }

    /// <summary>
    /// Gets the specified player's active party details
    /// </summary>
    /// <returns>A response containing operation execution results</returns>
    [HttpPost]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<GetPlayerPartyResponse> GetPlayerParty([FromBody] GetPlayerPartyRequest request)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var response = partyManager.GetPlayerParty(request);

        Response.StatusCode = response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }

    /// <summary>
    /// Assigns the party leader player
    /// </summary>
    /// <returns>A response containing operation execution results</returns>
    [HttpPost]
    [Route("assignleader")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<SetPartyLeaderPlayerResponse> SetPartyLeaderPlayer([FromBody] SetPartyLeaderPlayerRequest request)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var response = partyManager.SetPartyLeaderPlayer(request);

        Response.StatusCode = response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }

    /// <summary>
    /// Kicks a specified player from the specified party
    /// </summary>
    /// <returns>A response containing operation execution results</returns>
    [HttpPost]
    [Route("kick")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<KickPartyPlayerResponse> KickPartyPlayer([FromBody] KickPartyPlayerRequest request)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var response = partyManager.KickPartyPlayer(request);

        Response.StatusCode = response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }

    /// <summary>
    /// Invites a specified player to join the sending player's party
    /// </summary>
    /// <returns>A response containing operation execution results</returns>
    [HttpPost]
    [Route("invite")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<InvitePlayerToPartyResponse> InvitePlayerToParty([FromBody] InvitePlayerToPartyRequest request)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var response = partyManager.InvitePlayerToParty(request);

        Response.StatusCode = response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }

    /// <summary>
    /// The specified player by API key will leave its currently joined party
    /// </summary>
    /// <returns>A response containing operation execution results</returns>
    [HttpPost]
    [Route("leave")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<LeavePartyResponse> LeaveParty([FromBody] LeavePartyRequest request)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var response = partyManager.LeaveParty(request);

        Response.StatusCode = response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }
  }
}
