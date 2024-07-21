using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Relay.Contracts;
using Relay.Implementation.Interfaces;

namespace Relay.API.Controllers
{
  [EnableCors("CorsPolicy")]
  [Route("api/[controller]")]
  public class GameServerHostsController : ControllerBase
  {
    private readonly IGameServerHostManager gameServerHostManager;

    public GameServerHostsController()
    {
      this.gameServerHostManager = Container.WindsorContainer.Resolve<IGameServerHostManager>();
    }


    /// <summary>
    /// Finds an available game server to join
    /// </summary>
    [HttpGet]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<GetGameServerHostsResponse> FindGameServer()
    {
      return ControllerUtils.SafeExecution(() =>
      {
        var response = gameServerHostManager.GetGameServerHosts(new GetGameServerHostsRequest());

        Response.StatusCode = response.OperationResult != OperationResult.Fault_BadRequestParameters ? StatusCodes.Status400BadRequest : StatusCodes.Status200OK;

        return response;
      });
    }
  }
}
