using Microsoft.AspNetCore.Mvc;

namespace datntdev.Microservices.Srv.Identity.Web.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public Task<IActionResult> Get()
        {
            return Task.FromResult<IActionResult>(Ok("Hello World from Srv.Identity"));
        }
    }
}
