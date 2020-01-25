using Microsoft.AspNetCore.Mvc;

namespace KarthikeyasakthiTransport.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error()
        {
            HttpContext.Session.Clear();
            return View("Error");
        }
    }
}