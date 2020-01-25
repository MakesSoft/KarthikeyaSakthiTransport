using KarthikeyasakthiTransport.Filters;
using KarthikeyasakthiTransport.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KarthikeyasakthiTransport.Controllers
{
    [ValidateAdminSession]
    public class GroupController : Controller
    {
        private DatabaseContext _context;

        public GroupController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult List()
        {
            return View(_context.Group);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Group());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Group Group)
        {
            if (ModelState.IsValid)
            {
                bool isNameExists = IsNameExists(Group);

                if (isNameExists)
                {
                    ModelState.AddModelError("GroupName", "Group name is already exists.");
                    return View(Group);
                }

                var group = new Group
                {
                    GroupName = Group.GroupName,
                    CreateDate = DateTime.Now,
                    CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"))
                };

                _context.Group.Add(group);
                _context.SaveChanges();

                TempData["GroupMessage"] = "Group Saved Successfully";
                ModelState.Clear();
            }

            return View(new Group());
        }

        private bool IsNameExists(Group Group)
        {
            var result = _context.Group.Where(x => x.GroupName.Equals(Group.GroupName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            return result == null ? false : result.GroupId == Group.GroupId ? false : true;
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View("Edit", _context.Group.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Group Group)
        {
            if (ModelState.IsValid)
            {
                bool isNameExists = IsNameExists(Group);

                if (isNameExists)
                {
                    ModelState.AddModelError("GroupName", "Group name is already exists.");
                    return View(Group);
                }

                _context.Entry(Group).Property(x => x.GroupName).IsModified = true;
                _context.SaveChanges();

                TempData["GroupUpdateMessage"] = "Group Saved Successfully";
                ModelState.Clear();
            }

            return View(new Group());
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _context.Group.Remove(_context.Group.Find(id));
            _context.SaveChanges();

            return RedirectToAction("List", "Group");
        }

        public IActionResult GetGroups(string search)
        {
            var result = search == null ? _context.Group : _context.Group.Where(x => x.GroupName.Contains(search));

            var data = from grp in result
                       select new
                       {
                           id = grp.GroupId,
                           value = grp.GroupName
                       };

            return Json(new
            {
                success = true,
                responseText = data
            });
        }
    }
}