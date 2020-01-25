using KarthikeyasakthiTransport.Model;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace KarthikeyasakthiTransport.Controllers
{
    public class MenuMasterViewComponent : ViewComponent
    {
        private DatabaseContext _context;

        public MenuMasterViewComponent(DatabaseContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var menuMasters = _context.MenuMaster.OrderBy(o => o.OrderBy).ToList();

            return View(menuMasters);
        }
    }
}