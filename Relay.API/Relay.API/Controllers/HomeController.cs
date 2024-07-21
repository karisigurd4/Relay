namespace Relay.API.Controllers
{
  using MassTransit;
  using Microsoft.AspNetCore.Cors;
  using Microsoft.AspNetCore.Mvc;

  [EnableCors("CorsPolicy")]
  [Route("/")]
  public class HomeController : ControllerBase
  {
    private readonly IBus bus;

    public HomeController()
    {
      this.bus = Container.WindsorContainer.Resolve<IBus>();
    }

    [HttpGet]
    [Route("")]
    public ActionResult<string> Root()
    {
      return "OK";
    }
  }
}
