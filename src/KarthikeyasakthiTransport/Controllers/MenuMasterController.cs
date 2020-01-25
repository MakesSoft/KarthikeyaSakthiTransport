using KarthikeyasakthiTransport.Filters;
using KarthikeyasakthiTransport.Model;
using Microsoft.AspNetCore.Mvc;

namespace MyMVC.Controllers
{
    [ValidateAdminSession]
    public class MenuMasterController : Controller
    {
        private DatabaseContext _context;

        public MenuMasterController(DatabaseContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var menuMasters = _context.MenuMaster;

            return PartialView(menuMasters);
        }
    }
}