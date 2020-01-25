using KarthikeyasakthiTransport.Filters;
using KarthikeyasakthiTransport.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KarthikeyasakthiTransport.Controllers
{
    [ValidateAdminSession]
    public class SubGroupController : Controller
    {
        private DatabaseContext _context;

        public SubGroupController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult List()
        {
            var subGroupViewModel = from subgroup in _context.SubGroup
                                    join parentgroup in _context.Group on subgroup.GroupId equals parentgroup.GroupId
                                    select new SubGroupViewModel
                                    {
                                        GroupName = parentgroup.GroupName,
                                        SubGroup = subgroup
                                    };

            return View(subGroupViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new SubGroupViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SubGroupViewModel SubGroupViewModel)
        {
            if (ModelState.IsValid)
            {
                bool isNameExists = IsNameExists(SubGroupViewModel);

                if (isNameExists)
                {
                    ModelState.AddModelError("SubGroupName", "SubGroup name is already exists.");
                    return View(SubGroupViewModel);
                }

                var subGroup = new SubGroup
                {
                    SubGroupName = SubGroupViewModel.SubGroup.SubGroupName,
                    GroupId = SubGroupViewModel.SubGroup.GroupId,
                    CreateDate = DateTime.Now,
                    CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"))
                };

                _context.SubGroup.Add(subGroup);
                _context.SaveChanges();

                TempData["SubGroupMessage"] = "SubGroup Saved Successfully";
                ModelState.Clear();
            }

            return View(new SubGroupViewModel());
        }

        private bool IsNameExists(SubGroupViewModel SubGroupViewModel)
        {
            var result = _context.SubGroup.Where(x => x.SubGroupName.Equals(SubGroupViewModel.SubGroup.SubGroupName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            return result == null ? false : result.SubGroupId == SubGroupViewModel.SubGroup.SubGroupId ? false : true;
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var subGroupViewModel = from subgroup in _context.SubGroup
                                    join parentgroup in _context.Group on subgroup.GroupId equals parentgroup.GroupId
                                    where subgroup.SubGroupId == id
                                    select new SubGroupViewModel
                                    {
                                        GroupName = parentgroup.GroupName,
                                        SubGroup = subgroup
                                    };

            return View(subGroupViewModel.First());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SubGroupViewModel SubGroupViewModel)
        {
            if (ModelState.IsValid)
            {
                bool isNameExists = IsNameExists(SubGroupViewModel);

                if (isNameExists)
                {
                    ModelState.AddModelError("SubGroup_SubGroupName", "SubGroup name is already exists.");
                    return View(SubGroupViewModel);
                }

                var subGroup = new SubGroup
                {
                    SubGroupId = SubGroupViewModel.SubGroup.SubGroupId,
                    SubGroupName = SubGroupViewModel.SubGroup.SubGroupName,
                    GroupId = SubGroupViewModel.SubGroup.GroupId
                };

                _context.Entry(subGroup).Property(x => x.SubGroupName).IsModified = true;
                _context.Entry(subGroup).Property(x => x.GroupId).IsModified = true;
                _context.SaveChanges();

                TempData["SubGroupUpdateMessage"] = "SubGroup Saved Successfully";
                ModelState.Clear();
            }

            return View(new SubGroupViewModel());
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _context.SubGroup.Remove(_context.SubGroup.Find(id));
            _context.SaveChanges();

            return RedirectToAction("List", "SubGroup");
        }

        public IActionResult GetSubGroups(string search)
        {
            var result = search == null ? _context.SubGroup : _context.SubGroup.Where(x => x.SubGroupName.Contains(search));

            var data = from grp in result
                       where grp.SubGroupId > 2
                       select new
                       {
                           id = grp.SubGroupId,
                           value = grp.SubGroupName
                       };

            return Json(new
            {
                success = true,
                responseText = data
            });
        }
    }
}