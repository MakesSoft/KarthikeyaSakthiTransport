using Microsoft.AspNetCore.Mvc;

namespace KarthikeyasakthiTransport.Controllers
{
    public class DivElementViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}