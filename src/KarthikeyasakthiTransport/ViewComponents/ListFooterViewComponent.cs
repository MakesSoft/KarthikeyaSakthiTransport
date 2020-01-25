using Microsoft.AspNetCore.Mvc;

namespace KarthikeyasakthiTransport.Controllers
{
    public class ListFooterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}