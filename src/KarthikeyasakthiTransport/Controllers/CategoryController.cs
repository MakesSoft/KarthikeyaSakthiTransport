using KarthikeyasakthiTransport.Filters;
using KarthikeyasakthiTransport.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KarthikeyasakthiTransport.Controllers
{
    [ValidateAdminSession]
    public class CategoryController : Controller
    {
        private DatabaseContext _context;

        public CategoryController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult List()
        {
            //return new ViewAsPdf("List", _context.Category);
            //return new ViewAsPdf("List", _context.Category)
            //{
            //    FileName = "Bill.pdf"
            //};
            return View(_context.Category);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Category());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category Category)
        {
            if (ModelState.IsValid)
            {
                bool isNameExists = IsNameExists(Category);

                if (isNameExists)
                {
                    ModelState.AddModelError("CategoryName", "Category name is already exists.");
                    return View(Category);
                }

                var category = new Category
                {
                    CategoryName = Category.CategoryName,
                    CreateDate = DateTime.Now,
                    CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"))
                };

                _context.Category.Add(category);
                _context.SaveChanges();

                TempData["CategoryMessage"] = "Category Saved Successfully";
                ModelState.Clear();
            }

            return View(new Category());
        }

        private bool IsNameExists(Category Category)
        {
            var result = _context.Category.Where(x => x.CategoryName.Equals(Category.CategoryName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            return result == null ? false : result.CategoryId == Category.CategoryId ? false : true;
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View("Edit", _context.Category.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category Category)
        {
            if (ModelState.IsValid)
            {
                bool isNameExists = IsNameExists(Category);

                if (isNameExists)
                {
                    ModelState.AddModelError("CategoryName", "Category name is already exists.");
                    return View(Category);
                }

                _context.Entry(Category).Property(x => x.CategoryName).IsModified = true;
                _context.SaveChanges();

                TempData["CategoryUpdateMessage"] = "Category Saved Successfully";
                ModelState.Clear();
                return RedirectToAction("List", "Category");
            }

            return View(Category);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _context.Category.Remove(_context.Category.Find(id));
            _context.SaveChanges();

            return RedirectToAction("List", "Category");
        }

        public IActionResult GetCategories(string search)
        {
            var result = search == null ? _context.Category : _context.Category.Where(x => x.CategoryName.Contains(search));

            var data = from category in result
                       select new
                       {
                           id = category.CategoryId,
                           value = category.CategoryName
                       };

            return Json(new
            {
                success = true,
                responseText = data
            });
        }
    }
}