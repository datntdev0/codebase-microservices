using datntdev.Abp.Web.Security.AntiForgery;
using datntdev.Microservice.Controllers;
using Microsoft.AspNetCore.Antiforgery;

namespace datntdev.Microservice.Web.Host.Controllers
{
    public class AntiForgeryController : MicroserviceControllerBase
    {
        private readonly IAntiforgery _antiforgery;
        private readonly IAbpAntiForgeryManager _antiForgeryManager;

        public AntiForgeryController(IAntiforgery antiforgery, IAbpAntiForgeryManager antiForgeryManager)
        {
            _antiforgery = antiforgery;
            _antiForgeryManager = antiForgeryManager;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }

        public void SetCookie()
        {
            _antiForgeryManager.SetCookie(HttpContext);
        }
    }
}
