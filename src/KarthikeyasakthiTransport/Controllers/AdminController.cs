using KarthikeyasakthiTransport.Filters;
using Microsoft.AspNetCore.Mvc;

namespace KarthikeyasakthiTransport.Controllers
{
    [ValidateAdminSession]
    public class AdminController : Controller
    {
        // GET: /<controller>/
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}