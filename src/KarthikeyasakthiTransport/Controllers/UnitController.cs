using KarthikeyasakthiTransport.Filters;
using KarthikeyasakthiTransport.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KarthikeyasakthiTransport.Controllers
{
    [ValidateAdminSession]
    public class UnitController : Controller
    {
        private DatabaseContext _context;

        public UnitController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult List()
        {
            return View(_context.Unit);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Unit());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Unit Unit)
        {
            if (ModelState.IsValid)
            {
                bool isNameExists = IsNameExists(Unit);

                if (isNameExists)
                {
                    ModelState.AddModelError("UnitName", "Unit name is already exists.");
                    return View(Unit);
                }

                var unit = new Unit
                {
                    UnitName = Unit.UnitName,
                    CreateDate = DateTime.Now,
                    CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"))
                };

                _context.Unit.Add(unit);
                _context.SaveChanges();

                TempData["UnitMessage"] = "Unit Saved Successfully";
                ModelState.Clear();
            }

            return View(new Unit());
        }

        private bool IsNameExists(Unit Unit)
        {
            var result = _context.Unit.Where(x => x.UnitName.Equals(Unit.UnitName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            return result == null ? false : result.UnitId == Unit.UnitId ? false : true;
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View("Edit", _context.Unit.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Unit Unit)
        {
            if (ModelState.IsValid)
            {
                bool isNameExists = IsNameExists(Unit);

                if (isNameExists)
                {
                    ModelState.AddModelError("UnitName", "Unit name is already exists.");
                    return View(Unit);
                }

                _context.Entry(Unit).Property(x => x.UnitName).IsModified = true;
                _context.SaveChanges();

                TempData["UnitUpdateMessage"] = "Unit Saved Successfully";
                ModelState.Clear();
            }

            return View(new Unit());
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _context.Unit.Remove(_context.Unit.Find(id));
            _context.SaveChanges();

            return RedirectToAction("List", "Unit");
        }

        public IActionResult GetUnits(string search)
        {
            var result = search == null ? _context.Unit : _context.Unit.Where(x => x.UnitName.Contains(search));

            var data = from unit in result
                       select new
                       {
                           id = unit.UnitId,
                           value = unit.UnitName
                       };

            return Json(new
            {
                success = true,
                responseText = data
            });
        }
    }
}