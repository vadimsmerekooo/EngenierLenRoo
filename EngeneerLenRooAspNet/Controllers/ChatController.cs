using Microsoft.AspNetCore.Mvc;

namespace EngeneerLenRooAspNet.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
