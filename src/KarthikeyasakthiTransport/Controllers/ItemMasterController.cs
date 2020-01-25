using KarthikeyasakthiTransport.Filters;
using KarthikeyasakthiTransport.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KarthikeyasakthiTransport.Controllers
{
    [ValidateAdminSession]
    public class ItemMasterController : Controller
    {
        private DatabaseContext _context;

        public ItemMasterController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult List()
        {
            var list = from itemMaster in _context.ItemMaster
                       join category in _context.Category on itemMaster.CategoryId equals category.CategoryId
                       select new ItemMasterViewModel
                       {
                           ItemMaster = itemMaster,
                           CategoryName = category.CategoryName,
                       };

            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var settings = _context.Settings.ToList().OrderByDescending(x => x.SettingsId).First();

            var itemMaster = new ItemMasterViewModel
            {
                ItemMaster = new ItemMaster
                {
                    BarCode = BarCodeConfig(),
                    GSTInclude = settings.GSTInclude
                }
            };

            return View(itemMaster);
        }

        private string BarCodeConfig()
        {
            var settings = _context.Settings.ToList().OrderByDescending(x => x.SettingsId).First();
            var startNumber = settings.BarCodeStartFrom + _context.ItemMaster.Max(x => x.ItemMasterId) + 1;
            var barCodeConfig = settings.CustomBarCode + Convert.ToString(startNumber);

            return barCodeConfig;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ItemMasterViewModel ItemMasterViewModel)
        {
            if (ModelState.IsValid)
            {
                bool isNameExists = IsNameExists(ItemMasterViewModel);

                if (isNameExists)
                {
                    ModelState.AddModelError("ItemMasterName", "Item Master name is already exists.");
                    return View(ItemMasterViewModel);
                }

                if (!string.IsNullOrWhiteSpace(ItemMasterViewModel.UnitName) && ItemMasterViewModel.ItemMaster.UnitId == 0)
                {
                    ModelState.AddModelError("UnitName", "Please select valid Unit Name");
                    return View(ItemMasterViewModel);
                }

                var settings = _context.Settings.ToList().OrderByDescending(x => x.SettingsId).First();

                var itemMaster = GetItemMaster(ItemMasterViewModel);
                itemMaster.BarCode = BarCodeConfig();

                _context.ItemMaster.Add(itemMaster);
                _context.SaveChanges();

                TempData["ItemMasterMessage"] = "ItemMaster Saved Successfully";
                ModelState.Clear();
                return RedirectToAction("Create");
            }

            return View(ItemMasterViewModel);
        }

        private bool IsNameExists(ItemMasterViewModel itemMasterViewModel)
        {
            var result = _context.ItemMaster.Where(x => x.ItemMasterName.Equals(itemMasterViewModel.ItemMaster.ItemMasterName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            return result == null ? false : result.ItemMasterId == itemMasterViewModel.ItemMaster.ItemMasterId ? false : true;
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var list = from itemMaster in _context.ItemMaster
                       join category in _context.Category on itemMaster.CategoryId equals category.CategoryId
                       where itemMaster.ItemMasterId == id
                       select new ItemMasterViewModel
                       {
                           ItemMaster = itemMaster,
                           CategoryName = category.CategoryName,
                       };

            return View(list.First());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ItemMasterViewModel ItemMasterViewModel)
        {
            if (ModelState.IsValid)
            {
                bool isNameExists = IsNameExists(ItemMasterViewModel);

                if (isNameExists)
                {
                    ModelState.AddModelError("ItemMasterName", "Item Master name is already exists.");
                    return View(ItemMasterViewModel);
                }

                if (!string.IsNullOrWhiteSpace(ItemMasterViewModel.UnitName) && ItemMasterViewModel.ItemMaster.UnitId == 0)
                {
                    ModelState.AddModelError("UnitName", "Please select valid Unit Name");
                    return View(ItemMasterViewModel);
                }

                var settings = _context.Settings.ToList().OrderByDescending(x => x.SettingsId).First();

                var itemMaster = GetItemMaster(ItemMasterViewModel);

                _context.Entry(itemMaster).Property(x => x.ItemMasterName).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.ItemMasterDescription).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.ItemMasterNameTamil).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.CategoryId).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.BarCode).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.Case).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.Quantity).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.HsnCode).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.UnitId).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.MaximumRetailPrice).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.BasicRate).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.SaleRate).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.WholeSaleRate).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.PurchaseRate).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.TaxPercentage).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.GSTInclude).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.TaxAmount).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.BeforeTax).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.AfterTax).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.MarkDownPercentage).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.MarkDownAmount).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.MarkUpPercentage).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.MarkUpAmount).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.DiscountAmount).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.DiscountPercentage).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.CessPercentage).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.AdditionalCess).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.ReorderLevel).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.OpeningStock).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.CurrentStock).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.StockMaintenance).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.CreatedBy).IsModified = true;
                _context.Entry(itemMaster).Property(x => x.CreateDate).IsModified = true;

                _context.SaveChanges();

                TempData["ItemMasterMessage"] = "ItemMaster Saved Successfully";
                ModelState.Clear();
                return RedirectToAction("List");
            }

            return View(ItemMasterViewModel);
        }

        private ItemMaster GetItemMaster(ItemMasterViewModel ItemMasterViewModel)
        {
            return new ItemMaster
            {
                ItemMasterId = ItemMasterViewModel.ItemMaster.ItemMasterId,
                ItemMasterName = ItemMasterViewModel.ItemMaster.ItemMasterName,
                ItemMasterDescription = ItemMasterViewModel.ItemMaster.ItemMasterDescription,
                ItemMasterNameTamil = ItemMasterViewModel.ItemMaster.ItemMasterNameTamil,
                CategoryId = ItemMasterViewModel.ItemMaster.CategoryId,
                BarCode = ItemMasterViewModel.ItemMaster.BarCode,
                Case = ItemMasterViewModel.ItemMaster.Case,
                Quantity = ItemMasterViewModel.ItemMaster.Quantity,
                HsnCode = ItemMasterViewModel.ItemMaster.HsnCode,
                UnitId = ItemMasterViewModel.ItemMaster.UnitId,
                MaximumRetailPrice = ItemMasterViewModel.ItemMaster.MaximumRetailPrice,
                BasicRate = ItemMasterViewModel.ItemMaster.BasicRate,
                SaleRate = ItemMasterViewModel.ItemMaster.SaleRate,
                WholeSaleRate = ItemMasterViewModel.ItemMaster.WholeSaleRate,
                PurchaseRate = ItemMasterViewModel.ItemMaster.PurchaseRate,
                TaxPercentage = ItemMasterViewModel.ItemMaster.TaxPercentage,
                GSTInclude = ItemMasterViewModel.ItemMaster.GSTInclude,
                TaxAmount = ItemMasterViewModel.ItemMaster.TaxAmount,
                BeforeTax = ItemMasterViewModel.ItemMaster.BeforeTax,
                AfterTax = ItemMasterViewModel.ItemMaster.AfterTax,
                MarkDownPercentage = ItemMasterViewModel.ItemMaster.MarkDownPercentage,
                MarkDownAmount = ItemMasterViewModel.ItemMaster.MarkDownAmount,
                MarkUpPercentage = ItemMasterViewModel.ItemMaster.MarkUpPercentage,
                MarkUpAmount = ItemMasterViewModel.ItemMaster.MarkUpAmount,
                DiscountAmount = ItemMasterViewModel.ItemMaster.DiscountAmount,
                DiscountPercentage = ItemMasterViewModel.ItemMaster.DiscountPercentage,
                CessPercentage = ItemMasterViewModel.ItemMaster.CessPercentage,
                AdditionalCess = ItemMasterViewModel.ItemMaster.AdditionalCess,
                ReorderLevel = ItemMasterViewModel.ItemMaster.ReorderLevel,
                OpeningStock = ItemMasterViewModel.ItemMaster.OpeningStock,
                CurrentStock = ItemMasterViewModel.ItemMaster.CurrentStock,
                StockMaintenance = ItemMasterViewModel.ItemMaster.StockMaintenance,
                CreateDate = DateTime.Now,
                CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"))
            };
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _context.ItemMaster.Remove(_context.ItemMaster.Find(id));
            _context.SaveChanges();

            return RedirectToAction("List", "ItemMaster");
        }
    }
}