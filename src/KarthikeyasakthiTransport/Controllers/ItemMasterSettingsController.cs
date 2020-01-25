using KarthikeyasakthiTransport.Filters;
using KarthikeyasakthiTransport.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KarthikeyasakthiTransport.Controllers
{
    [ValidateAdminSession]
    public class ItemMasterSettingsController : Controller
    {
        private DatabaseContext _context;

        public ItemMasterSettingsController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult List(int? id)
        {
            ViewBag.LedgerMaster = _context.LedgerMaster;

            var settings = _context.Settings.ToList().OrderByDescending(x => x.SettingsId).First().ItemSettings;

            if (settings)
            {
                var itemSettings = from itemMaster in _context.ItemMaster
                                   join category in _context.Category on itemMaster.CategoryId equals category.CategoryId
                                   where itemMaster.CategoryId == 1
                                   select new LedgerItemMaster
                                   {
                                       ItemMasterId = itemMaster.ItemMasterId,
                                       ItemMasterName = itemMaster.ItemMasterName,
                                       CategoryId = itemMaster.CategoryId,
                                       CategoryName = category.CategoryName,
                                       BarCode = itemMaster.BarCode,
                                       PurchaseRate = itemMaster.PurchaseRate,
                                       MaximumRetailPrice = itemMaster.MaximumRetailPrice
                                   };

                return View(itemSettings);
            }

            return View();
        }

        [HttpPost]
        public IActionResult Create(List<LedgerItemMaster> data)
        {
            var result = data;

            return View();
        }

        private IActionResult ReturnView(LedgerMaster ledgerMaster, bool create)
        {
            switch (ledgerMaster.LedgerMasterType)
            {
                case 1:
                    return create ? View("CreateSupplierMaster", ledgerMaster) : View("EditSupplierMaster", ledgerMaster);

                case 2:
                    return create ? View("CreateCustomerMaster", ledgerMaster) : View("EditCustomerMaster", ledgerMaster);

                default:
                    return create ? View("CreateLedgerHead", ledgerMaster) : View("EditLedgerHead", ledgerMaster);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var list = from ledger in _context.LedgerMaster
                       join subGroup in _context.SubGroup on ledger.SubGroupId equals subGroup.SubGroupId
                       where ledger.LedgerMasterId == id
                       select new LedgerMaster
                       {
                           LedgerMasterId = ledger.LedgerMasterId,
                           LedgerMasterName = ledger.LedgerMasterName,
                           LedgerMasterDescription = ledger.LedgerMasterDescription,
                           Division = ledger.Division,
                           SubGroupName = ledger.SubGroupName,
                           SubGroupId = ledger.SubGroupId,
                           LedgerMasterType = ledger.LedgerMasterType,
                           MarginPercentage = ledger.MarginPercentage,
                           SaleRate = ledger.SaleRate,
                           Address = ledger.Address,
                           City = ledger.City,
                           State = ledger.State,
                           PinCode = ledger.PinCode,
                           MobileNumber = ledger.MobileNumber,
                           EmailId = ledger.EmailId,
                           GSTNumber = ledger.GSTNumber,
                           PANNumber = ledger.PANNumber,
                           Debit = ledger.Debit,
                           Credit = ledger.Credit,
                           CreatedBy = ledger.CreatedBy,
                           CreateDate = ledger.CreateDate,
                       };

            return ReturnView(list.First(), false);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(LedgerMaster ledgerMaster)
        {
            if (ModelState.IsValid)
            {
                bool isNameExists = IsNameExists(ledgerMaster);

                if (isNameExists)
                {
                    ModelState.AddModelError("LedgerMasterName", "Ledger name is already exists.");
                    return ReturnView(ledgerMaster, false);
                }

                if (!string.IsNullOrWhiteSpace(ledgerMaster.SubGroupName) && ledgerMaster.SubGroupId == 0)
                {
                    ModelState.AddModelError("SubGroupName", "Please select valid Sub Group");
                    return ReturnView(ledgerMaster, false);
                }

                _context.Entry(ledgerMaster).Property(x => x.LedgerMasterName).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.LedgerMasterDescription).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.Division).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.SubGroupId).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.MarginPercentage).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.SaleRate).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.Address).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.City).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.State).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.PinCode).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.MobileNumber).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.EmailId).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.GSTNumber).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.PANNumber).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.Debit).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.Credit).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.CreatedBy).IsModified = true;
                _context.Entry(ledgerMaster).Property(x => x.CreateDate).IsModified = true;

                _context.SaveChanges();

                return RedirectToAction("List", new { id = ledgerMaster.LedgerMasterType });
            }

            return ReturnView(ledgerMaster, false);
        }

        private bool IsNameExists(LedgerMaster ledgerMaster)
        {
            var result = _context.LedgerMaster.Where(x => x.LedgerMasterName.Equals(ledgerMaster.LedgerMasterName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            return result == null ? false : result.LedgerMasterId == ledgerMaster.LedgerMasterId ? false : true;
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var ledgerMaster = _context.LedgerMaster.Find(id);
            _context.LedgerMaster.Remove(ledgerMaster);
            _context.SaveChanges();

            return RedirectToAction("List", new { id = ledgerMaster.LedgerMasterType });
        }
    }
}