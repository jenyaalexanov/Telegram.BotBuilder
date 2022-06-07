using Microsoft.AspNetCore.Mvc;

namespace JA.Telegram.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ActionResult Index()
        {
            return Ok("Ok!");
        }
    }
}