using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EngeneerLenRooAspNet.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        [Route("")]
        [Route("chat")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
