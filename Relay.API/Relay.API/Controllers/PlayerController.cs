namespace Relay.API.Controllers
{
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Cors;
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Mvc;
  using Relay.Contracts;
  using Relay.Core.Utilities;
  using Relay.Implementation.Interfaces;

  /// <summary>
  /// Entrypoint route for retrieving information about players
  /// </summary>
  [EnableCors("CorsPolicy")]
  [Route("api/[controller]")]
  public class PlayerController : ControllerBase
  {
    private readonly IPlayerManager playerManager;

    public PlayerController()
    {
      this.playerManager = Container.WindsorContainer.Resolve<IPlayerManager>();
    }

    /// <summary>
    /// Registers a new player
    /// </summary>
    /// <returns>The registered player and associated registration information</returns>
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<RegisterPlayerResponse> RegisterPlayer([FromBody] RegisterPlayerRequest request)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var response = playerManager.RegisterPlayer(request);

        Response.StatusCode = !response.Success && response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }

    /// <summary>
    /// Gets player's information
    /// </summary>
    /// <returns>The player specified</returns>
    [HttpPost]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<GetPlayerResponse> GetPlayer([FromBody] GetPlayerRequest request)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var response = playerManager.GetPlayer(request);

        Response.StatusCode = !response.Success && response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }

    /// <summary>
    /// Updates a player's associated information
    /// </summary>
    /// <returns>Operation execution details</returns>
    [HttpPost]
    [Route("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<UpdatePlayerResponse> UpdatePlayer([FromBody] UpdatePlayerRequest request)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var response = playerManager.UpdatePlayer(request);

        Response.StatusCode = !response.Success && response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }

    /// <summary>
    /// Searches for players based on the specified criteria
    /// </summary>
    /// <returns>A list of players matching the specified criteria</returns>
    [HttpPost]
    [Route("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<SearchPlayersResponse> SearchPlayers([FromBody] SearchPlayersRequest request)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var response = playerManager.SearchPlayers(request);

        Response.StatusCode = !response.Success && response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }

    /// <summary>
    /// Sends a friend request from the player specified with the ApiKey to the specified player id
    /// </summary>
    /// <returns>A list of players matching the specified criteria</returns>
    [HttpPost]
    [Route("friends/request")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<SendPlayerFriendRequestResponse> SendPlayerFriendRequest([FromBody] SendPlayerFriendRequest request)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var response = playerManager.SendPlayerFriendRequest(request);

        Response.StatusCode = !response.Success && response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }

    /// <summary>
    /// Removes the friend relation between the players specified with the ApiKey and the PlayerId parameters
    /// </summary>
    /// <returns>A list of players matching the specified criteria</returns>
    [HttpPost]
    [Route("friends/remove")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<RemovePlayerFromFriendListResponse> RemoveFriend([FromBody] RemovePlayerFromFriendListRequest request)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var response = playerManager.RemovePlayerFromFriendsList(request);

        Response.StatusCode = !response.Success && response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }

    /// <summary>
    /// Gets a player's friends list
    /// </summary>
    /// <returns>A list of players in the player's friends list</returns>
    [HttpPost]
    [Route("friends/list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<GetPlayerFriendsListResponse> GetPlayerFriendsList([FromBody] GetPlayerFriendsListRequest request)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var response = playerManager.GetPlayerFriendsList(request);

        Response.StatusCode = !response.Success && response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }

    /// <summary>
    /// Increments a Score value assocaited to the player as well as the game server the player is in.
    /// </summary>
    /// <returns>Operation result</returns>
    [HttpPost]
    [Route("score")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IncrementPlayerScoreResponse> IncrementPlayerScore([FromBody] IncrementPlayerScoreRequest request)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var response = playerManager.IncrementPlayerScore(request);

        Response.StatusCode = !response.Success && response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }


    /// <summary>
    /// Sets a player's name by api key
    /// </summary>
    /// <returns>Operation result</returns>
    [HttpPost]
    [Route("name")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<SetPlayerNameResponse> SetPlayerName([FromBody] SetPlayerNameRequest request)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var response = playerManager.SetPlayerName(request);

        Response.StatusCode = !response.Success && response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }

    /// <summary>
    /// Gets game server statistics
    /// </summary>
    /// <returns>Game server statistics</returns>
    [HttpGet]
    [Authorize]
    [Route("statistics")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<GetPlayerStatisticsResponse> GetPlayerStatistics([FromQuery] string projectId, [FromQuery] Period period)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var request = new GetPlayerStatisticsRequest()
        {
          ExtAuthId = TokenUtils.GetUserId(User),
          ProjectId = projectId,
          Period = period
        };

        var response = playerManager.GetPlayerStatistics(request);

        Response.StatusCode = response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }
  }
}
