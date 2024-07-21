namespace Relay.API.Controllers
{
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Cors;
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Mvc;
  using Relay.Contracts;
  using Relay.Core.Utilities;
  using Relay.Implementation.Interfaces;
  using System;

  /// <summary>
  /// Entrypoint route for retrieving information about game servers
  /// </summary>
  [EnableCors("CorsPolicy")]
  [Route("api/[controller]")]
  public class GameServerController : ControllerBase
  {
    private readonly IGameServerManager gameServerManager;

    public GameServerController()
    {
      this.gameServerManager = Container.WindsorContainer.Resolve<IGameServerManager>();
    }


    /// <summary>
    /// Finds an available game server to join via matchmaking
    /// </summary>
    [HttpPost]
    [Route("matchmaking")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<FindGameServerResponse> FindGameServer([FromBody] FindGameServerRequest request)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var response = gameServerManager.FindGameServer(request);

        Response.StatusCode = response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }

    /// <summary>
    /// Browse through active game servers per project
    /// </summary>
    [HttpPost]
    [Route("browse")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<BrowseGameServersResponse> BrowseGameServers([FromBody] BrowseGameServersRequest request)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var response = gameServerManager.BrowseGameServers(request);

        Response.StatusCode = response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }

    /// <summary>
    /// Gets game server info by code
    /// </summary>
    [HttpPost]
    [Route("code")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<GetGameServerByCodeResponse> GetGameServerByCode([FromBody] GetGameServerByCodeRequest request)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var response = gameServerManager.GetGameServerByCode(request);

        Response.StatusCode = response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }

    /// <summary>
    /// Requests to start a new hosted game server instance
    /// </summary>
    [HttpPost]
    [Route("host")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<HostGameServerResponse> HostGameServer([FromBody] HostGameServerRequest request)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var response = gameServerManager.HostGameServer(request);

        Response.StatusCode = response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }

    /// <summary>
    /// Internal use, requires key, stops a game server process
    /// </summary>
    [HttpPost]
    [Route("stop")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<StopGameServerResponse> StopGameServer([FromBody] StopGameServerRequest request)
    {
      // Hard coded api key used by game servers to manage operations on behalf 
      if (request.ApiKey != Guid.Parse("9D2F3CCE-0FAC-4981-BE02-42C4FDB34FB3"))
      {
        Response.StatusCode = StatusCodes.Status401Unauthorized;
        return null;
      }

      return ControllerUtils.SafeExecution(() =>
      {
        var response = gameServerManager.StopGameServer(request);

        Response.StatusCode = response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }

    /// <summary>
    /// Registers that a player has joined the specified game server
    /// </summary>
    /// <returns>A response containing operation execution results</returns>
    [HttpPost]
    [Route("registerPlayerJoined")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<AddGameServerPlayerResponse> AddGameServerPlayer([FromBody] AddGameServerPlayerRequest request)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var response = gameServerManager.AddGameServerPlayer(request);

        Response.StatusCode = response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }

    /// <summary>
    /// Unregisters that a player has joined the specified game server
    /// </summary>
    /// <returns>A response containing operation execution results</returns>
    [HttpPost]
    [Route("registerPlayerLeft")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<RemoveGameServerPlayerResponse> RemoveGameServerPlayer([FromBody] RemoveGameServerPlayerRequest request)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var response = gameServerManager.RemoveGameServerPlayer(request);

        Response.StatusCode = response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }

    /// <summary>
    /// Gets game server info
    /// </summary>
    /// <returns>Game server information</returns>
    [HttpPost]
    [Route("info")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<GetGameServerInfoByIdResponse> GetGameServerInfoById([FromBody] GetGameServerInfoByIdRequest request)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var response = gameServerManager.GetGameServerInfoById(request);

        Response.StatusCode = response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

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
    public ActionResult<GetGameServerStatisticsResponse> GetGameServerStatistics([FromQuery] string projectId, [FromQuery] Period period)
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var request = new GetGameServerStatisticsRequest()
        {
          ExtAuthId = TokenUtils.GetUserId(User),
          ProjectId = projectId,
          Period = period
        };

        var response = gameServerManager.GetGameServerStatistics(request);

        Response.StatusCode = response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }
  }
}
