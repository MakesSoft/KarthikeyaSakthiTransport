using KarthikeyasakthiTransport.Filters;
using Microsoft.AspNetCore.Mvc;

namespace KarthikeyasakthiTransport.Controllers
{
    [ValidateAdminSession]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}