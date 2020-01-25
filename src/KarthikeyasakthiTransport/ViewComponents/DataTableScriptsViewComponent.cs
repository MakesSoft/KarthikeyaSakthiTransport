using Microsoft.AspNetCore.Mvc;

namespace KarthikeyasakthiTransport.Controllers
{
    public class DataTableScriptsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}